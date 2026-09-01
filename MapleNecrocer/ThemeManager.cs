using Manina.Windows.Forms;
using System.Drawing;
using System.Windows.Forms;

namespace MapleNecrocer;

internal enum ThemeRole
{
    Default,
    AvatarInventory,
}

internal static class ThemeManager
{
    private static ThemePreferenceStore? preferenceStore;

    internal static ThemeMode CurrentMode { get; private set; } = ThemeMode.Dark;
    internal static ThemePalette CurrentPalette => ThemePalette.For(CurrentMode);

    internal static void Initialize(
        ThemeMode mode,
        ThemePreferenceStore? store = null)
    {
        CurrentMode = mode;
        preferenceStore = store;
    }

    internal static bool SetMode(ThemeMode mode)
    {
        CurrentMode = mode;
        Form[] openForms = Application.OpenForms.Cast<Form>().ToArray();
        foreach (Form form in openForms)
        {
            Apply(form);
        }

        return preferenceStore?.Save(mode) ?? true;
    }

    internal static void SetRole(Control control, ThemeRole role)
    {
        control.Tag = role;
    }

    internal static void Apply(Control root)
    {
        ApplyTree(root, CurrentPalette);
    }

    private static void ApplyTree(Control control, ThemePalette palette)
    {
        control.ControlAdded -= Control_ControlAdded;
        control.ControlAdded += Control_ControlAdded;
        control.HandleCreated -= Control_HandleCreated;
        control.HandleCreated += Control_HandleCreated;

        try
        {
            switch (control)
            {
                case ImageListView imageListView:
                    AvatarItemBrowserStyle.Apply(imageListView, CurrentMode);
                    break;
                case AvatarFormDraw avatarFormDraw:
                    AvatarSearchStyle.ApplyPreviewSurface(avatarFormDraw, CurrentMode);
                    break;
                case DataGridView grid when grid.Tag is ThemeRole.AvatarInventory:
                    ApplyAvatarInventory(grid, palette);
                    break;
                case DataGridView grid:
                    ApplyGrid(grid, palette);
                    break;
                case TextBoxBase textBox:
                    textBox.BackColor = palette.InputBackground;
                    textBox.ForeColor = palette.Foreground;
                    break;
                case ComboBox comboBox:
                    comboBox.BackColor = palette.InputBackground;
                    comboBox.ForeColor = palette.Foreground;
                    comboBox.DrawMode = DrawMode.OwnerDrawFixed;
                    comboBox.FlatStyle = FlatStyle.Flat;
                    comboBox.DrawItem -= ComboBox_DrawItem;
                    comboBox.DrawItem += ComboBox_DrawItem;
                    break;
                case ListBox listBox:
                    listBox.BackColor = palette.InputBackground;
                    listBox.ForeColor = palette.Foreground;
                    break;
                case ListView listView:
                    listView.BackColor = palette.InputBackground;
                    listView.ForeColor = palette.Foreground;
                    break;
                case TreeView treeView:
                    treeView.BackColor = palette.InputBackground;
                    treeView.ForeColor = palette.Foreground;
                    break;
                case Button button:
                    button.UseVisualStyleBackColor = false;
                    button.FlatStyle = FlatStyle.Flat;
                    button.FlatAppearance.BorderColor = palette.Border;
                    button.BackColor = palette.ControlBackground;
                    button.ForeColor = palette.Foreground;
                    if (button.FindForm() is AvatarForm &&
                        int.TryParse(button.Tag?.ToString(), out _))
                    {
                        AvatarButtonStyle.Apply(button, CurrentMode);
                    }
                    break;
                case TabControl tabControl:
                    tabControl.DrawMode = TabDrawMode.OwnerDrawFixed;
                    tabControl.DrawItem -= TabControl_DrawItem;
                    tabControl.DrawItem += TabControl_DrawItem;
                    tabControl.Paint -= TabControl_Paint;
                    tabControl.Paint += TabControl_Paint;
                    tabControl.BackColor = palette.WindowBackground;
                    tabControl.ForeColor = palette.Foreground;
                    break;
                case CheckBox:
                case RadioButton:
                case Label:
                    control.ForeColor = palette.Foreground;
                    break;
                case TabPage or Panel or GroupBox:
                    control.BackColor = palette.ControlBackground;
                    control.ForeColor = palette.Foreground;
                    break;
                case Form form:
                    form.BackColor = palette.WindowBackground;
                    form.ForeColor = palette.Foreground;
                    NativeWindowTheme.Apply(form, CurrentMode);
                    break;
                default:
                    control.ForeColor = palette.Foreground;
                    break;
            }
        }
        catch
        {
        }

        NativeControlTheme.Apply(control, CurrentMode);

        foreach (Control child in control.Controls)
        {
            ApplyTree(child, palette);
        }

        try
        {
            control.Invalidate();
        }
        catch
        {
        }
    }

    private static void ApplyGrid(DataGridView grid, ThemePalette palette)
    {
        grid.EnableHeadersVisualStyles = false;
        grid.BackgroundColor = palette.WindowBackground;
        grid.GridColor = palette.Grid;
        grid.DefaultCellStyle.BackColor = palette.InputBackground;
        grid.DefaultCellStyle.ForeColor = palette.Foreground;
        grid.DefaultCellStyle.SelectionBackColor = palette.SelectionBackground;
        grid.DefaultCellStyle.SelectionForeColor = palette.SelectionForeground;
        grid.RowsDefaultCellStyle.BackColor = palette.InputBackground;
        grid.RowsDefaultCellStyle.ForeColor = palette.Foreground;
        grid.AlternatingRowsDefaultCellStyle.BackColor = palette.ControlBackground;
        grid.AlternatingRowsDefaultCellStyle.ForeColor = palette.Foreground;
        grid.ColumnHeadersDefaultCellStyle.BackColor = palette.ControlBackground;
        grid.ColumnHeadersDefaultCellStyle.ForeColor = palette.Foreground;
        grid.RowHeadersDefaultCellStyle.BackColor = palette.ControlBackground;
        grid.RowHeadersDefaultCellStyle.ForeColor = palette.Foreground;
    }

    private static void ApplyAvatarInventory(DataGridView grid, ThemePalette palette)
    {
        ApplyGrid(grid, palette);
        grid.ColumnAdded -= AvatarInventory_ColumnAdded;
        grid.ColumnAdded += AvatarInventory_ColumnAdded;
        grid.BackgroundColor = palette.AvatarCanvasBackground;
        grid.DefaultCellStyle.BackColor = palette.AvatarCanvasBackground;
        grid.RowsDefaultCellStyle.BackColor = palette.AvatarCanvasBackground;
        grid.AlternatingRowsDefaultCellStyle.BackColor = palette.AvatarCanvasBackground;
        grid.DefaultCellStyle.SelectionBackColor = palette.AvatarCanvasBackground;
        grid.DefaultCellStyle.SelectionForeColor = palette.Foreground;

        foreach (DataGridViewColumn column in grid.Columns)
        {
            Color background = column is DataGridViewImageColumn
                ? palette.AvatarItemBackground
                : palette.AvatarCanvasBackground;
            column.DefaultCellStyle.BackColor = background;
            column.DefaultCellStyle.ForeColor = palette.Foreground;
            column.DefaultCellStyle.SelectionBackColor = background;
            column.DefaultCellStyle.SelectionForeColor = palette.Foreground;

            if (column is DataGridViewButtonColumn buttonColumn)
            {
                buttonColumn.FlatStyle = FlatStyle.Flat;
            }
        }
    }

    private static void AvatarInventory_ColumnAdded(
        object? sender,
        DataGridViewColumnEventArgs e)
    {
        if (sender is DataGridView grid)
        {
            ApplyAvatarInventory(grid, CurrentPalette);
        }
    }

    private static void Control_ControlAdded(object? sender, ControlEventArgs e)
    {
        Apply(e.Control);
    }

    private static void Control_HandleCreated(object? sender, EventArgs e)
    {
        if (sender is Control control)
        {
            NativeControlTheme.Apply(control, CurrentMode);
        }
    }

    private static void ComboBox_DrawItem(object? sender, DrawItemEventArgs e)
    {
        if (sender is not ComboBox comboBox)
        {
            return;
        }

        ThemePalette palette = CurrentPalette;
        bool selected = e.State.HasFlag(DrawItemState.Selected);
        Color background = selected
            ? palette.SelectionBackground
            : palette.InputBackground;
        Color foreground = selected
            ? palette.SelectionForeground
            : palette.Foreground;

        using var brush = new SolidBrush(background);
        e.Graphics.FillRectangle(brush, e.Bounds);

        string text = e.Index >= 0 && e.Index < comboBox.Items.Count
            ? comboBox.GetItemText(comboBox.Items[e.Index])
            : comboBox.Text;
        Rectangle textBounds = Rectangle.Inflate(e.Bounds, -2, 0);
        TextRenderer.DrawText(
            e.Graphics,
            text,
            comboBox.Font,
            textBounds,
            foreground,
            TextFormatFlags.Left |
            TextFormatFlags.VerticalCenter |
            TextFormatFlags.EndEllipsis);
    }

    private static void TabControl_DrawItem(object? sender, DrawItemEventArgs e)
    {
        if (sender is not TabControl tabControl ||
            e.Index < 0 ||
            e.Index >= tabControl.TabPages.Count)
        {
            return;
        }

        ThemePalette palette = CurrentPalette;
        Color background = e.Index == tabControl.SelectedIndex
            ? palette.ControlBackground
            : palette.WindowBackground;
        using var brush = new SolidBrush(background);
        e.Graphics.FillRectangle(brush, e.Bounds);
        TextRenderer.DrawText(
            e.Graphics,
            tabControl.TabPages[e.Index].Text,
            tabControl.Font,
            e.Bounds,
            palette.Foreground,
            TextFormatFlags.HorizontalCenter |
            TextFormatFlags.VerticalCenter |
            TextFormatFlags.EndEllipsis);
    }

    private static void TabControl_Paint(object? sender, PaintEventArgs e)
    {
        if (sender is TabControl tabControl)
        {
            using var brush = new SolidBrush(CurrentPalette.WindowBackground);
            e.Graphics.FillRectangle(brush, tabControl.ClientRectangle);
        }
    }
}
