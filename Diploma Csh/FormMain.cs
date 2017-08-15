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
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string filename;
                OpenFileDialog openFileDialog1 = new OpenFileDialog() { Filter = "Текстовые файлы(*.txt)|*.txt" };
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    filename = openFileDialog1.FileName;
                    FormProgressBar FPB = new FormProgressBar(this, filename);
                    FPB.Show();
                    this.Hide();
                }
            }
            catch(Exception ex)
            {
                FormException FE = new FormException(this, ex.Message, this);
                FE.Show();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormHelp FH = new FormHelp();
            FH.Show();
        }
    }
}
