using System.Diagnostics;
using System.IO;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace EOLChecker
{
    public partial class Form1 : Form
    {
        EOLChecker eolChecker = null!;
        public enum LineEnding
        {
            CRLF, // Carriage Return + Line Feed (Windows)
            CR,    // Carriage Return (Mac)
            LF,   // Line Feed (Unix/Linux)
            None
        }
        public Form1()
        {
            // Width = 700
            // Height = 338
            InitializeComponent();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            this.StartPosition = FormStartPosition.CenterScreen;
            ckEOLOption.ItemCheck += CheckedListBox_ItemCheck;
            Resize += Form1_SizeChanged;
        }

        private void Form1_SizeChanged(object? sender, EventArgs e)
        {
            int newWidth = btnBrowser.Location.X - 6;
            int newHeight = this.ClientRectangle.Height - tableLayoutPanel.Location.Y - 2;

            if (newWidth > 0 && newHeight > 0)
            {
                txtBrowser.Width = newWidth;
                txtBrowser.Height = btnBrowser.Height;

                tableLayoutPanel.Width = newWidth;
                tableLayoutPanel.Height = newHeight;

                btnStartCheck.Height = this.ClientRectangle.Height - btnStartCheck.Location.Y - 3;

                foreach (Control control in tableLayoutPanel.Controls)
                {
                    if (control is Panel panel)
                    {
                        panel.Width = tableLayoutPanel.ClientSize.Width;

                        RichTextBox rtb = panel.Controls.OfType<RichTextBox>().FirstOrDefault() ?? new RichTextBox();
                        Button button = panel.Controls.OfType<Button>().FirstOrDefault() ?? new Button();

                        if (rtb != null && button != null)
                        {
                            int newRtbWidth = panel.Width - button.Width - 12;

                            if (newRtbWidth > 0)
                            {
                                rtb.Width = newRtbWidth;
                            }
                        }
                    }
                }
            }
        }


        private void BtnBrowser_Click(object sender, EventArgs e)
        {
            txtBrowser.Text = String.Empty;
            txtBrowser.ForeColor = Color.Black;
            FolderBrowserDialog folderBrowserDialog = new();

            DialogResult result = folderBrowserDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                string selectedFolderPath = folderBrowserDialog.SelectedPath;

                txtBrowser.Text = selectedFolderPath;
            }
        }

        private void BtnStartCheck_Click_1(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtBrowser.Text))
            {
                MessageBox.Show("Please select the directory for inspection!!!", "Error!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnBrowser.Focus();
                return;
            }

            if (!Directory.Exists(txtBrowser.Text))
            {
                MessageBox.Show("The directory is not valid!!!", "Error!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnBrowser.Focus();
                return;
            }

            eolChecker = new EOLChecker(txtBrowser);

            LineEnding selectedLineEnding = LineEnding.None;

            for (int i = 0; i < ckEOLOption.Items.Count; i++)
            {
                bool isChecked = ckEOLOption.GetItemChecked(i);
                if (isChecked)
                {
                    selectedLineEnding = (LineEnding)i;
                }
            }

            if (selectedLineEnding != LineEnding.None)
            {
                eolChecker.LineEnding = selectedLineEnding;
                bool bResult = eolChecker.HasCodeFiles(txtBrowser.Text);

                if (bResult)
                {
                    if (!InitializeTableLayoutResult())
                    {
                        btnReplace.Visible = false;
                        MessageBox.Show("Could not find line ending with " + eolChecker.LineEnding.ToString(), "Information!!!", MessageBoxButtons.OK, MessageBoxIcon.Information); 
                        btnBrowser.Focus();
                    }
                    else
                    {
                        btnReplace.Visible = true;
                    }
                }
                else
                {
                    MessageBox.Show("The directory does not have any .h, .cpp, or .cs files!!!", "Information!!!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnBrowser.Focus();
                }
            }
            else
            {
                MessageBox.Show("Please select the line break style!!!\n (Choose CRLF, LF, or CR in the checkboxes.)", "Error!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void CheckedListBox_ItemCheck(object? sender, ItemCheckEventArgs e)
        {
            // Uncheck all other items when an item is checked
            for (int i = 0; i < ckEOLOption.Items.Count; i++)
            {
                if (i != e.Index)
                {
                    ckEOLOption.SetItemChecked(i, false);
                }
            }
        }

        private void TxtBrowser_Leave(object sender, EventArgs e)
        {
            if (txtBrowser.Text == String.Empty)
            {
                txtBrowser.Text = "Please select the directory for inspection!!!";
                txtBrowser.ForeColor = Color.Red;
            }
        }

        private void TxtBrowser_Enter(object sender, EventArgs e)
        {
            if (txtBrowser.Text == "Please select the directory for inspection!!!")
            {
                txtBrowser.Text = String.Empty;
                txtBrowser.ForeColor = Color.Black;
            }
        }

        private void BtnReplace_Click(object sender, EventArgs e)
        {
            bool bResult = false;
            string strResult = string.Empty;
            using (DialogReplace dialog = new(eolChecker.LineEnding))
            {
                DialogResult result = dialog.ShowDialog();

                // Xử lý kết quả của Form Dialog nếu cần thiết
                if (result == DialogResult.OK)
                {
                    if (dialog.LineEndingUser != LineEnding.None)
                    {
                        List<FilesReplaceInfo> filesReplaceInfos = eolChecker.GetFilesReplaceInfoList();
                        foreach (FilesReplaceInfo filesReplaceInfo in filesReplaceInfos)
                        {
                            bResult = EOLChecker.ReplaceLineEnding(filesReplaceInfo, eolChecker.LineEnding, dialog.LineEndingUser);
                            if (bResult)
                            {
                                strResult = strResult + Environment.NewLine + filesReplaceInfo.FilePathReplace;
                            }
                        }
                    }
                }
            }
            if (bResult)
            {
                MessageBox.Show(strResult, "Replace successfully!!!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnReplace.Visible = false;
            }
        }

        private bool InitializeTableLayoutResult()
        {
            if (eolChecker == null)
            {
                return false;
            }

            bool bIsResult = false;
            List<FilesReplaceInfo> filesReplaceInfos = eolChecker.GetFilesReplaceInfoList();
            int i = 0;
            tableLayoutPanel.RowStyles.Clear();

            tableLayoutPanel.Controls.Clear();

            if (filesReplaceInfos.Count != 0)
            {
                int rtItemWidth = 0;
                if (filesReplaceInfos.Count < 6)
                {
                    rtItemWidth = tableLayoutPanel.ClientSize.Width - 88;
                }
                else
                {
                    rtItemWidth = tableLayoutPanel.ClientSize.Width - 106;
                }

                bIsResult = true;
                
                foreach (FilesReplaceInfo filesReplaceInfo in filesReplaceInfos)
                {
                    TableLayoutPanel itemPanel = new()
                    {
                        Size = new Size(tableLayoutPanel.ClientSize.Width, 60),
                        ColumnCount = 2,
                        BackColor = Color.FromArgb(140, 194, 183)
                    };


                    //RichTextBox
                    string linecode = "   line: ";
                    foreach (int line in filesReplaceInfo.ArrayLineCode)
                    {
                        linecode += line + ", ";
                    }

                    RichTextBox rtItem = new()
                    {
                        Size = new Size(rtItemWidth, 54),
                        Location = new Point(0, 0),
                        BorderStyle = BorderStyle.None
                    };

                    Font fontPath = new(rtItem.Font, FontStyle.Bold);
                    Font fontLine = new(rtItem.Font, FontStyle.Italic);

                    // Sử dụng fontPath cho fileInfo.StrFilePath
                    rtItem.SelectionStart = rtItem.TextLength;
                    rtItem.SelectionLength = 0;
                    rtItem.SelectionFont = fontPath;
                    rtItem.SelectionColor = Color.FromArgb(162, 87, 114);
                    rtItem.SelectedText = filesReplaceInfo.FilePathReplace + Environment.NewLine;

                    // Sử dụng fontLine cho linecode
                    rtItem.SelectionStart = rtItem.TextLength;
                    rtItem.SelectionLength = 0;
                    rtItem.SelectionFont = fontLine;
                    rtItem.SelectionColor = Color.Black;
                    rtItem.SelectedText = linecode;

                    //Button
                    Button button = new()
                    {
                        Text = "Open" + Environment.NewLine + "Folder",
                        Size = new Size(70, 54),
                        Location = new Point(599, 0),
                        TextAlign = ContentAlignment.MiddleCenter,
                        ForeColor = Color.Black,
                        BackColor = Color.White
                    };
                    button.FlatAppearance.BorderColor = Color.FromArgb(137, 190, 179);
                    button.FlatStyle = FlatStyle.Flat;

                    button.Click += (sender, e) =>
                    {
                        if (sender is not null)
                        {
                            ButtonOpen_Click(sender, e, filesReplaceInfo.FilePathReplace);
                        }
                    };


                    itemPanel.Controls.Add(rtItem, 0, 0);
                    itemPanel.Controls.Add(button, 1, 0);

                    tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent));

                    tableLayoutPanel.Controls.Add(itemPanel, 0, i);
                    i++;
                    fontPath.Dispose();
                    fontLine.Dispose();
                }

            }
            return bIsResult;
        }

        private static void ButtonOpen_Click(object sender, EventArgs e, string filePath)
        {
            if (sender is null)
            {
                throw new ArgumentNullException(nameof(sender));
            }

            if (e is null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException($"'{nameof(filePath)}' cannot be null or empty.", nameof(filePath));
            }

            string directoryPath = Path.GetDirectoryName(filePath) ?? string.Empty;

            if (!string.IsNullOrEmpty(directoryPath) && Directory.Exists(directoryPath))
            {
                // Get file name from path
                string fileName = Path.GetFileName(filePath);

                // Open the folder and highlight the file
                _ = Process.Start("explorer.exe", "/select, " + Path.Combine(directoryPath, fileName));
            }
            else
            {
                MessageBox.Show("The folder does not exist!");
            }

        }
    }
}