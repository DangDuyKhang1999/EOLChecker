using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static EOLChecker.Form1;

namespace EOLChecker
{
    public partial class DialogReplace : Form
    {
        public Form1.LineEnding LineEndingUser { get; private set; } = Form1.LineEnding.None;
        public DialogReplace(Form1.LineEnding lineEndingBefore)
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            InitializeComponent();
            InitText(lineEndingBefore);
        }

        public void InitText(Form1.LineEnding lineEndingBefore)
        {
            label1.Text = $"What line ending type do you want to change from {lineEndingBefore} to?";

            switch (lineEndingBefore)
            {
                case Form1.LineEnding.CRLF:
                    ckOption1.Text = "CR";
                    ckOption2.Text = "LF";
                    break;
                case Form1.LineEnding.CR:
                    ckOption1.Text = "CRLF";
                    ckOption2.Text = "LF";
                    break;
                case Form1.LineEnding.LF:
                    ckOption1.Text = "CRLF";
                    ckOption2.Text = "CR";
                    break;
                case Form1.LineEnding.None:
                    break;
                default:
                    break;
            }
        }

        private void ckOption1_CheckedChanged(object sender, EventArgs e)
        {
            if (ckOption1.Checked)
            {
                ckOption2.Checked = false;
            }
        }

        private void ckOption2_CheckedChanged(object sender, EventArgs e)
        {
            if (ckOption2.Checked)
            {
                ckOption1.Checked = false;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {

            if (ckOption1.Checked)
            {
                string ck1Text = ckOption1.Text;
                LineEndingUser = ConvertToLineEnding(ck1Text);
            }
            else if (ckOption2.Checked)
            {
                string ck2Text = ckOption2.Text;
                LineEndingUser = ConvertToLineEnding(ck2Text);
            }
            DialogResult = DialogResult.OK;
            Close();
        }

        public LineEnding ConvertToLineEnding(string cktext)
        {
            switch (cktext)
            {
                case "CRLF":
                    return LineEnding.CRLF;
                case "CR":
                    return LineEnding.CR;
                case "LF":
                    return LineEnding.LF;
                default:
                    return LineEnding.None;
            }
        }
    }
}
