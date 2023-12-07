using System.IO;
using System.Net.NetworkInformation;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace EOLChecker
{
    public partial class Form1 : Form
    {
        EOLChecker EOL_Checker;
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

        //resize after button Maximum is clicked
        private void Form1_SizeChanged(object? sender, EventArgs e)
        {
            txtBrowser.Width = btnBrowser.Location.X - 10;
            txtBrowser.Height = btnBrowser.Height;
            rtxtResult.Width = btnBrowser.Location.X - 10;
            rtxtResult.Height = this.ClientRectangle.Height - rtxtResult.Location.Y - 3;
            btnStartCheck.Height = this.ClientRectangle.Height - btnStartCheck.Location.Y - 3;
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
            rtxtResult.Text = String.Empty;
            if (String.IsNullOrEmpty(txtBrowser.Text))
            {
                MessageBox.Show("Please select the directory for inspection!!!", "Error!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnBrowser.Focus();
            }
            else if (!Directory.Exists(txtBrowser.Text))
            {
                MessageBox.Show("The directory is not valid!!!", "Error!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnBrowser.Focus();
            }
            else
            {
                EOL_Checker = new(txtBrowser);
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
                    EOL_Checker.LineEnding = selectedLineEnding;
                    bool bResult = EOL_Checker.HasCodeFiles(txtBrowser.Text);
                    if (bResult)
                    {
                        if (!EOL_Checker.PrintFilesName(rtxtResult))
                        {
                            btnReplace.Visible = false;
                            MessageBox.Show("Could not find line ending with " + EOL_Checker.LineEnding.ToString(), "Error!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void txtBrowser_Leave(object sender, EventArgs e)
        {
            if (txtBrowser.Text == String.Empty)
            {
                txtBrowser.Text = "Please select the directory for inspection!!!";
                txtBrowser.ForeColor = Color.Red;
            }
        }

        private void txtBrowser_Enter(object sender, EventArgs e)
        {
            if (txtBrowser.Text == "Please select the directory for inspection!!!")
            {
                txtBrowser.Text = String.Empty;
                txtBrowser.ForeColor = Color.Black;
            }
        }

        private void btnReplace_Click(object sender, EventArgs e)
        {
            bool bResult = false;
            string strResult = string.Empty;
            using (DialogReplace dialog = new DialogReplace(EOL_Checker.LineEnding))
            {
                DialogResult result = dialog.ShowDialog();

                // Xử lý kết quả của Form Dialog nếu cần thiết
                if (result == DialogResult.OK)
                {
                    if (dialog.LineEndingUser != LineEnding.None)
                    {
                        List<FilesReplaceInfo> filesReplaceInfos = EOL_Checker.GetFilesReplaceInfoList();
                        foreach (FilesReplaceInfo filesReplaceInfo in filesReplaceInfos)
                        {
                            bResult = EOL_Checker.ReplaceLineEnding(filesReplaceInfo, EOL_Checker.LineEnding, dialog.LineEndingUser);
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
    }
}