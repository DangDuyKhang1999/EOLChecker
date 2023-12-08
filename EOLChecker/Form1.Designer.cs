using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace EOLChecker
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnBrowser = new Button();
            txtBrowser = new TextBox();
            tableLayoutPanel = new TableLayoutPanel();
            btnStartCheck = new Button();
            ckEOLOption = new CheckedListBox();
            btnReplace = new Button();
            SuspendLayout();
            // 
            // btnBrowser
            // 
            btnBrowser.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnBrowser.BackColor = Color.FromArgb(137, 190, 179);
            btnBrowser.FlatAppearance.BorderColor = Color.FromArgb(137, 190, 179);
            btnBrowser.FlatStyle = FlatStyle.Flat;
            btnBrowser.ForeColor = Color.Black;
            btnBrowser.Location = new Point(694, 3);
            btnBrowser.Name = "btnBrowser";
            btnBrowser.Size = new Size(103, 40);
            btnBrowser.TabIndex = 1;
            btnBrowser.Text = "Browser";
            btnBrowser.UseVisualStyleBackColor = false;
            btnBrowser.Click += BtnBrowser_Click;
            // 
            // txtBrowser
            // 
            txtBrowser.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            txtBrowser.BackColor = Color.FromArgb(190, 217, 217);
            txtBrowser.BorderStyle = BorderStyle.None;
            txtBrowser.ForeColor = Color.Red;
            txtBrowser.Location = new Point(3, 3);
            txtBrowser.Multiline = true;
            txtBrowser.Name = "txtBrowser";
            txtBrowser.Size = new Size(689, 40);
            txtBrowser.TabIndex = 2;
            txtBrowser.Text = "Please select the directory for inspection!!!";
            txtBrowser.Enter += TxtBrowser_Enter;
            txtBrowser.Leave += TxtBrowser_Leave;
            // 
            // tableLayoutPanel
            // 
            tableLayoutPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            tableLayoutPanel.AutoScroll = true;
            tableLayoutPanel.BackColor = Color.FromArgb(190, 217, 217);
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel.ForeColor = Color.FromArgb(161, 204, 209);
            tableLayoutPanel.Location = new Point(3, 45);
            tableLayoutPanel.Name = "tableLayoutPanel";
            tableLayoutPanel.Size = new Size(689, 400);
            tableLayoutPanel.TabIndex = 6;
            // 
            // btnStartCheck
            // 
            btnStartCheck.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnStartCheck.BackColor = Color.FromArgb(137, 190, 179);
            btnStartCheck.FlatAppearance.BorderColor = Color.FromArgb(137, 190, 179);
            btnStartCheck.FlatStyle = FlatStyle.Flat;
            btnStartCheck.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnStartCheck.ForeColor = Color.Black;
            btnStartCheck.Location = new Point(694, 385);
            btnStartCheck.Name = "btnStartCheck";
            btnStartCheck.Size = new Size(103, 60);
            btnStartCheck.TabIndex = 3;
            btnStartCheck.Text = "Check";
            btnStartCheck.UseVisualStyleBackColor = false;
            btnStartCheck.Click += BtnStartCheck_Click_1;
            // 
            // ckEOLOption
            // 
            ckEOLOption.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            ckEOLOption.BackColor = Color.FromArgb(137, 190, 179);
            ckEOLOption.BorderStyle = BorderStyle.None;
            ckEOLOption.ForeColor = Color.Black;
            ckEOLOption.FormattingEnabled = true;
            ckEOLOption.Items.AddRange(new object[] { LineEnding.CRLF, LineEnding.CR, LineEnding.LF });
            ckEOLOption.Location = new Point(694, 45);
            ckEOLOption.Name = "ckEOLOption";
            ckEOLOption.Size = new Size(103, 66);
            ckEOLOption.TabIndex = 4;
            // 
            // btnReplace
            // 
            btnReplace.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnReplace.BackColor = Color.FromArgb(137, 190, 179);
            btnReplace.FlatAppearance.BorderColor = Color.FromArgb(137, 190, 179);
            btnReplace.FlatStyle = FlatStyle.Flat;
            btnReplace.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnReplace.ForeColor = Color.Black;
            btnReplace.Location = new Point(694, 339);
            btnReplace.Name = "btnReplace";
            btnReplace.Size = new Size(103, 40);
            btnReplace.TabIndex = 5;
            btnReplace.Text = "Replace";
            btnReplace.UseVisualStyleBackColor = false;
            btnReplace.Visible = false;
            btnReplace.Click += BtnReplace_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(238, 245, 255);
            ClientSize = new Size(800, 449);
            Controls.Add(btnReplace);
            Controls.Add(ckEOLOption);
            Controls.Add(btnStartCheck);
            Controls.Add(tableLayoutPanel);
            Controls.Add(txtBrowser);
            Controls.Add(btnBrowser);
            Name = "Form1";
            Text = "EOL Checker";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnBrowser;
        private TextBox txtBrowser;
        //private RichTextBox rtxtResult;
        private TableLayoutPanel tableLayoutPanel;
        private Button btnStartCheck;
        private CheckedListBox ckEOLOption;
        private Button btnReplace;
    }
}