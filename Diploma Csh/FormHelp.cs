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
    public partial class FormHelp : Form
    {
        public FormHelp()
        {
            InitializeComponent();
            label1.Text = "";
            label1.Text += "Необходимый для корректной работы вид файла:\n\n";

            label1.Text += "Часть строки, не относящаяся к комментарию, содержит только числа.\n";
            label1.Text += "Числа в строках разделены пробелами, после последнего в строке числа может\n";
            label1.Text += "следовать комментарий к строке, отделённый знаком '/' (без пробела перед ним).\n";
            label1.Text += "Строки следуют без пропусков.\n\n";

            label1.Text += "В первой строке указаны количество исследуемых работ и групп критериев.\n";
            label1.Text += "Во второй строке указано количество критериев в каждой группе.\n";
            label1.Text += "Остальные строки заполняются из таблиц, полученных в ходе экспертной оценки,\n";
            label1.Text += "в следующем порядке(отдельная строка для каждой таблицы):\n";
            label1.Text += "Таблица оценки группы критериев.\n";
            label1.Text += "Таблицы оценки для каждой группы.\n";
            label1.Text += "Таблицы оценки работ по каждому критерию всех групп по порядку.\n\n";

            label1.Text += "Для заполнения строки, полученной из таблицы, в строку заносятся по порядку\n";
            label1.Text += "оценки элементов(1 и 2), (1 и 3), ... , (1 и n), (2 и 3), ... , (n-1 и n) таблицы.В случае,\n";
            label1.Text += "если элемент с большим порядковым номером признан более значимым, чем\n";
            label1.Text += "элемент с меньшим порядковым номером, оценка записывается со знаком '-'.\n\n";

            label1.Text += "Далее могут следовать комментарии к данным.\n";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
