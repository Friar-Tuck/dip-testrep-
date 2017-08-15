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
    public partial class FormResult : Form
    {
        FormMain _mainform;//родительская форма
        double[] Total;
        double[] Indexes;//чем меньше, тем лучше. в идеале - меньше 0.1
        string Res = "";//строка с коэффициентами работ
        string Inds = "";//строка с индексами согласованности
        bool statusflag = true;
        public FormResult(FormMain F1, double[] T, double[] I)
        {
            InitializeComponent();
            this._mainform = F1;
            this.Total = T;
            this.Indexes = I;        
            string addspaces;
            for(int i = 0; i < T.Length; i++)
            {
                addspaces = (i < 9) ? ":     " : ((i < 99) ? ":    " : ":   ");
                this.Res += "Коэффициент работы номер " + (i+1) + addspaces + Math.Round(this.Total[i],5) + Environment.NewLine;
            }
            for (int i = 0; i < I.Length; i++)
            {
                addspaces = (i < 9) ? ":     " : ((i < 99) ? ":    " : ":   ");
                this.Inds += "Индекс согласованности таблицы номер " + (i + 1) + addspaces + Math.Round(this.Indexes[i], 5) + Environment.NewLine;
            }
            this.Inds += Environment.NewLine+ "Таблице под номером  i соответствует строка под номером i+2" + Environment.NewLine + "в обработанном файле";
            textBox1.Text = Res;
        }

        private void button1_Click(object sender, EventArgs e)//возврат
        {
            this.Close();
            this._mainform.Show();
        }

        private void FormResult_FormClosing(object sender, FormClosingEventArgs e)
        {
            this._mainform.Show();
        }

        private void button2_Click(object sender, EventArgs e)//индексы/результаты
        {
            if (statusflag == true)
            {
                statusflag = false;
                textBox1.Text = Inds;
                button2.Text = "Показать полученные коэффициенты";
            } else {
                statusflag = true;
                textBox1.Text = Res;
                button2.Text = "Показать индексы согласованности";
            }
        }
    }
}
