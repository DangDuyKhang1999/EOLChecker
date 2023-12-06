namespace EOLChecker
{
    partial class DialogReplace
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            ckOption2 = new CheckBox();
            ckOption1 = new CheckBox();
            btnOK = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.BackColor = Color.FromArgb(190, 217, 217);
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            label1.ForeColor = Color.Black;
            label1.Location = new Point(5, 9);
            label1.Name = "label1";
            label1.Size = new Size(490, 67);
            label1.TabIndex = 0;
            label1.Text = "label1";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // ckOption2
            // 
            ckOption2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            ckOption2.AutoSize = true;
            ckOption2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            ckOption2.Location = new Point(296, 84);
            ckOption2.Name = "ckOption2";
            ckOption2.Size = new Size(77, 32);
            ckOption2.TabIndex = 2;
            ckOption2.Text = "CRLF";
            ckOption2.UseVisualStyleBackColor = false;
            ckOption2.CheckedChanged += ckOption2_CheckedChanged;
            // 
            // ckOption1
            // 
            ckOption1.AutoSize = true;
            ckOption1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            ckOption1.Location = new Point(132, 84);
            ckOption1.Name = "ckOption1";
            ckOption1.Size = new Size(77, 32);
            ckOption1.TabIndex = 3;
            ckOption1.Text = "CRLF";
            ckOption1.UseVisualStyleBackColor = false;
            ckOption1.CheckedChanged += ckOption1_CheckedChanged;
            // 
            // btnOK
            // 
            btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnOK.BackColor = Color.FromArgb(192, 255, 255);
            btnOK.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            btnOK.ForeColor = Color.Black;
            btnOK.Location = new Point(200, 126);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(100, 50);
            btnOK.TabIndex = 4;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = false;
            btnOK.Click += btnOK_Click;
            // 
            // DialogReplace
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(190, 217, 217);
            ClientSize = new Size(500, 184);
            Controls.Add(btnOK);
            Controls.Add(ckOption1);
            Controls.Add(ckOption2);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Name = "DialogReplace";
            Text = "DialogReplace";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private CheckBox ckOption2;
        private CheckBox ckOption1;
        private Button btnOK;
    }
}