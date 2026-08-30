using DevComponents.DotNetBar.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WzComparerR2.PluginBase;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace MapleNecrocer;

public partial class SelectFolderForm : ThemedForm
{
    public SelectFolderForm()
    {
        InitializeComponent();
        Instance = this;
    }
    public static SelectFolderForm Instance;
    private void OpenWzForm_Load(object sender, EventArgs e)
    {
        RefreshRecentFolders();
        this.FormClosing += (s, e1) =>
        {
            this.Hide();
            e1.Cancel = true;
        };
    }

    internal void RefreshRecentFolders()
    {
        RecentFilesGrid.Rows.Clear();
        if (RecentFilesGrid.Columns.Count == 0)
        {
            RecentFilesGrid.ColumnCount = 1;
            RecentFilesGrid.Columns[0].Width = 400;
            var loadButton = new DataGridViewButtonColumn
            {
                Width = 60,
                UseColumnTextForButtonValue = true,
                Text = "Load"
            };
            RecentFilesGrid.Columns.Add(loadButton);
        }

        foreach (string path in MainForm.Instance.RecentFolders)
        {
            RecentFilesGrid.Rows.Add(path);
        }
    }

    private void SelectFolderButton_Click(object sender, EventArgs e)
    {
        using var dialog = new FolderBrowserDialog
        {
            InitialDirectory = ".\\"
        };
        if (dialog.ShowDialog(this) == DialogResult.OK &&
            MainForm.Instance.LoadMapleStoryFolder(dialog.SelectedPath))
        {
            Hide();
        }
    }

    private void RecentFilesGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0 || e.RowIndex >= RecentFilesGrid.Rows.Count)
        {
            return;
        }

        string? path = RecentFilesGrid.Rows[e.RowIndex].Cells[0].Value?.ToString();
        if (!string.IsNullOrWhiteSpace(path) && MainForm.Instance.LoadMapleStoryFolder(path))
        {
            Hide();
        }
    }
}
