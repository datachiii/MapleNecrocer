using MapleNecrocer;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using Xunit;

namespace MapleNecrocer.Tests;

[Collection(ThemeTestCollection.Name)]
public sealed class ThemeManagerTests
{
    [Fact]
    public void InitializeTheme_LoadsStoredModeWithoutSaving()
    {
        string directory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        string path = Path.Combine(directory, "theme.txt");
        try
        {
            var store = new ThemePreferenceStore(path);
            Assert.True(store.Save(ThemeMode.Light));

            Program.InitializeTheme(store);

            Assert.Equal(ThemeMode.Light, ThemeManager.CurrentMode);
            Assert.Equal("Light", File.ReadAllText(path));
        }
        finally
        {
            if (Directory.Exists(directory))
            {
                Directory.Delete(directory, recursive: true);
            }
        }
    }

    [Fact]
    public void OptionForm_DarkModeCheckboxReflectsChangesAndPersistsMode()
    {
        StaTest.Run(() =>
        {
            string directory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
            string path = Path.Combine(directory, "theme.txt");
            try
            {
                ThemeManager.Initialize(ThemeMode.Dark, new ThemePreferenceStore(path));
                using var form = new OptionForm();
                form.Show();
                Application.DoEvents();

                CheckBox checkBox = form.Controls
                    .Find("darkModeCheckBox", searchAllChildren: true)
                    .OfType<CheckBox>()
                    .Single();
                Assert.True(checkBox.Checked);

                checkBox.Checked = false;

                Assert.Equal(ThemeMode.Light, ThemeManager.CurrentMode);
                Assert.Equal("Light", File.ReadAllText(path));
            }
            finally
            {
                if (Directory.Exists(directory))
                {
                    Directory.Delete(directory, recursive: true);
                }
            }
        });
    }

    [Fact]
    public void OptionForm_ShownWhileMuted_SynchronizesCheckboxWithoutCallingBass()
    {
        StaTest.Run(() =>
        {
            bool originalMute = Sound.isMute;
            try
            {
                Sound.isMute = true;
                using var form = new OptionForm();
                MethodInfo shownHandler = typeof(OptionForm).GetMethod(
                    "OptionForm_Shown",
                    BindingFlags.Instance | BindingFlags.NonPublic)!;

                Exception? exception = Record.Exception(() =>
                    shownHandler.Invoke(form, [form, EventArgs.Empty]));

                Assert.Null(exception);
                CheckBox muteCheckBox = form.Controls
                    .Find("checkBox1", searchAllChildren: true)
                    .OfType<CheckBox>()
                    .Single();
                Assert.True(muteCheckBox.Checked);
            }
            finally
            {
                Sound.isMute = originalMute;
            }
        });
    }

    [Fact]
    public void EveryApplicationForm_InheritsThemedForm()
    {
        Type[] unthemedForms = typeof(MainForm).Assembly
            .GetTypes()
            .Where(type =>
                !type.IsAbstract &&
                type != typeof(ThemedForm) &&
                type.Namespace == typeof(MainForm).Namespace &&
                typeof(Form).IsAssignableFrom(type) &&
                !typeof(ThemedForm).IsAssignableFrom(type))
            .ToArray();

        Assert.Empty(unthemedForms);
    }

    [Fact]
    public void Apply_StylesRepresentativeControlsAndDataGrid()
    {
        StaTest.Run(() =>
        {
            ThemeManager.Initialize(ThemeMode.Dark);
            using var form = new Form();
            using var textBox = new TextBox();
            using var comboBox = new ComboBox();
            using var button = new Button();
            using var tabControl = new TabControl();
            using var tabPage = new TabPage();
            using var grid = new DataGridView();
            tabControl.TabPages.Add(tabPage);
            form.Controls.AddRange([textBox, comboBox, button, tabControl, grid]);

            ThemeManager.Apply(form);

            ThemePalette palette = ThemePalette.For(ThemeMode.Dark);
            Assert.Equal(palette.WindowBackground, form.BackColor);
            Assert.Equal(palette.InputBackground, textBox.BackColor);
            Assert.Equal(palette.InputBackground, comboBox.BackColor);
            Assert.Equal(palette.Foreground, comboBox.ForeColor);
            Assert.Equal(DrawMode.OwnerDrawFixed, comboBox.DrawMode);
            Assert.Equal(FlatStyle.Flat, comboBox.FlatStyle);
            Assert.Equal(palette.ControlBackground, button.BackColor);
            Assert.False(button.UseVisualStyleBackColor);
            Assert.Equal(TabDrawMode.OwnerDrawFixed, tabControl.DrawMode);
            Assert.Equal(palette.ControlBackground, tabPage.BackColor);
            Assert.Equal(palette.WindowBackground, grid.BackgroundColor);
            Assert.Equal(palette.SelectionBackground, grid.DefaultCellStyle.SelectionBackColor);
            Assert.False(grid.EnableHeadersVisualStyles);
        });
    }

    [Fact]
    public void Apply_WhenOneControlRejectsAColor_StillThemesItsSibling()
    {
        StaTest.Run(() =>
        {
            ThemeManager.Initialize(ThemeMode.Dark);
            using var form = new Form();
            using var throwingControl = new ThrowingControl();
            using var textBox = new TextBox();
            form.Controls.AddRange([throwingControl, textBox]);

            ThemeManager.Apply(form);

            Assert.Equal(ThemePalette.For(ThemeMode.Dark).InputBackground, textBox.BackColor);
        });
    }

    [Fact]
    public void SetMode_UpdatesAnAlreadyOpenThemedForm()
    {
        StaTest.Run(() =>
        {
            ThemeManager.Initialize(ThemeMode.Dark);
            using var form = new ThemedForm();
            form.Show();

            Assert.Equal(ThemePalette.For(ThemeMode.Dark).WindowBackground, form.BackColor);

            Assert.True(ThemeManager.SetMode(ThemeMode.Light));

            Assert.Equal(ThemePalette.For(ThemeMode.Light).WindowBackground, form.BackColor);
            form.Close();
        });
    }

    [Fact]
    public void ControlAdded_AppliesCurrentModeToDynamicControls()
    {
        StaTest.Run(() =>
        {
            ThemeManager.Initialize(ThemeMode.Dark);
            using var form = new ThemedForm();
            using var panel = new Panel();
            form.Controls.Add(panel);
            form.Show();

            using var lateTextBox = new TextBox();
            panel.Controls.Add(lateTextBox);

            Assert.Equal(ThemePalette.For(ThemeMode.Dark).InputBackground, lateTextBox.BackColor);
            form.Close();
        });
    }

    [Fact]
    public void SetMode_WhenSaveFails_KeepsInMemoryMode()
    {
        StaTest.Run(() =>
        {
            string directory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(directory);
            try
            {
                ThemeManager.Initialize(ThemeMode.Dark, new ThemePreferenceStore(directory));

                Assert.False(ThemeManager.SetMode(ThemeMode.Light));
                Assert.Equal(ThemeMode.Light, ThemeManager.CurrentMode);
            }
            finally
            {
                Directory.Delete(directory);
            }
        });
    }

    private sealed class ThrowingControl : Control
    {
        public override Color ForeColor
        {
            get => base.ForeColor;
            set => throw new InvalidOperationException("Test control rejects theme colors.");
        }
    }
}
