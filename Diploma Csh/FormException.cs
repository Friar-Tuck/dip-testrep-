using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Diploma_Csh
{
    public partial class FormException : Form
    {
        FormMain _mainform;
        Form _errorform;
        public FormException(FormMain F1, string text, Form F2)
        {
            InitializeComponent();
            this._mainform = F1;
            this._errorform = F2;
            textBox1.Text = "Ошибка:" + Environment.NewLine + text;       
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this._errorform != this._mainform)
            {
                this._errorform.Close();
            }
            this._mainform.Show();
            this.Close();
        }

        private void FormException_FormClosing(object sender, FormClosingEventArgs e)
        {
            this._mainform.Show();
        }
    }
}
