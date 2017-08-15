using System;
using System.IO;
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
    public partial class FormProgressBar : Form
    {
        bool _flagexc;
        bool _flagbut;
        int FileStringCount;//счётчик прочитанных строк
        FormMain _mainform;
        string _path;
        double[] Total;
        double[] Indexes;
        string PremadeExcText = "некорректные данные в строке ";

        public FormProgressBar(FormMain F1, string filename)
        {
            InitializeComponent();
            _mainform = F1;
            _flagexc = false;
            _flagbut = false;
            FileStringCount = 0;
            _path = filename;
        }
        void PBIncrement()//увеличение прогрессбара, обновление текста
        {
            progressBar1.Value++;
            label1.Text = "Прогресс выполнения обработки: " + Math.Ceiling(100.0 * progressBar1.Value / progressBar1.Maximum) + "%";
            label1.Refresh();            
        }

        private void filltable(ref double[][] array, int size, String currf, int stringcount)//передавать заполняемую строку
        {//заполнение таблицы // заполняем таблицу array размера size, элементы называются nameoftype
            Char delimiter = ' ';//деление строки
            String[] substrings = currf.Split(delimiter);
            int k = 0;//проверка данных
            for (int i = 1; i <= size - 1; k += i, i++) ;
            if (substrings.Count() != k) throw new Exception(PremadeExcText + stringcount);
            int iter = 0;//отсчёт чисел в строке
            for (int i = 0; i < size; i++)
            {
                for (int j = i; j < size; j++)//увел-м ещё и iter
                {
                    if (i == j)
                    {
                        array[i][j] = 1;
                    }
                    else
                    {
                        //ввод соотношений между элементами            
                        int check;//проверка считываемого числа  
                        if (!int.TryParse(substrings[iter], out check) || check == 0 || Math.Abs(check) > 9)
                        {
                            throw new Exception(PremadeExcText + stringcount);
                        }
                        array[i][j] = double.Parse(substrings[iter]);
                        iter++;
                        if (array[i][j] < 0)
                        {
                            array[j][i] = -1 * array[i][j];
                            array[i][j] = -1 / array[i][j];
                        }
                        else
                        {
                            array[j][i] = 1 / array[i][j];
                        }
                    }
                }
            }
            return;
        }

        private void heavymath(ref double[][] array, int size, double[] result)
        {//вычисления //работаем с таблицей array размера size, результат записываем в result //метод парных сравнений
            for (int i = 0; i < size; i++)
            {
                double k = 0;
                for (int j = 0; j < size; j++)
                {
                    k += array[j][i];//сумма по столбцу
                }
                result[i] = 1 / k;
            }
            double summ = 0;
            for (int i = 0; i < size; i++) summ += result[i];
            for (int i = 0; i < size; i++) result[i] /= summ;
        }

        private double findinds(ref double[][] array, double[] col, int size)
        {//поиск индекса согласованности
            double result = 0;
            double[] tempcol = new double[size];//0
            for (int i = 0; i < size; i++)
            {
                tempcol[i] = 0;
                for (int j = 0; j < size; j++)
                {
                    tempcol[i] += array[i][j] * col[j];
                }
                result += (tempcol[i] / col[i]) / size;
            }
            result = (result - size) / (size - 1) * ((result - size) < 0 ? -1 : 1);//с учётом модуля
            return result;
        }

        private void func()
        {
            /////////////////////
            //исключения ловятся ниже
            using (StreamReader fs = new StreamReader(_path))//подкл в поток
            {
                
                String readfromfile;
                Char delimiter = ' ';//деление строки
                String[] substrings;
                int numbOFgroups, numbOFprojs;//к-во групп критериев, работ
                int[] numbOFcrits;//и критериев в каждой группе
                int totalcrits = 0; //общее к-во критериев

                //ввод к-ва работ и групп критериев
                readfromfile = fs.ReadLine();
                FileStringCount++;
                readfromfile = readfromfile.Split('/')[0];//считывание до комментария
                substrings = readfromfile.Split(delimiter);

                if (substrings.Count() != 2 || !int.TryParse(substrings[0], out numbOFprojs) || !int.TryParse(substrings[1], out numbOFgroups) || numbOFprojs <= 0 || numbOFgroups <= 0)//несоответсивие кол-ва эл-в / типа данных / значения
                {
                    throw new Exception(PremadeExcText + FileStringCount);//с проверкой данных
                }
                
                //ввод к-ва критериев в группах
                readfromfile = fs.ReadLine();
                FileStringCount++;
                readfromfile = readfromfile.Split('/')[0];//считывание до комментария
                substrings = readfromfile.Split(delimiter);
                if (substrings.Count() != numbOFgroups)
                {
                    throw new Exception(PremadeExcText + FileStringCount);
                }
                numbOFcrits = new int[numbOFgroups];
                for (int i = 0; i < numbOFgroups; i++)
                {
                    //сам ввод
                    if (!int.TryParse(substrings[i], out numbOFcrits[i]) || numbOFcrits[i] <= 0)
                    {
                        throw new Exception(PremadeExcText + FileStringCount);
                    }
                    totalcrits += numbOFcrits[i];
                }

                progressBar1.Maximum = 3 + numbOFgroups * 2 + totalcrits * 3 + 1;//установка прогрессбара
                progressBar1.Value = 1;
                PBIncrement();
                double[] indsogl = new double[1 + numbOFgroups + totalcrits];//массив индексов согласованности

                //ВВОД ТАБЛИЦ

                //группы критериев
                double[][] groups = new double[numbOFgroups][];
                for (int i = 0; i < numbOFgroups; i++) groups[i] = new double[numbOFgroups];
                //заполнение таблицы оценки групп критериев
                readfromfile = fs.ReadLine();
                FileStringCount++;
                readfromfile = readfromfile.Split('/')[0];//считывание до комментария
                this.filltable(ref groups, numbOFgroups, readfromfile, FileStringCount);
                PBIncrement();

                //критрерии в группах
                //трёхмерный массив - массив размера количества групп с двумерными массивами таблиц сравнений критериев размеров количеств критериев
                double[][][] crits = new double[numbOFgroups][][];//кол-во таблиц с критериями
                for (int i = 0; i < numbOFgroups; i++)
                {
                    crits[i] = new double[numbOFcrits[i]][];//двумерные таблицы критериев
                    for (int j = 0; j < numbOFcrits[i]; j++)
                    {
                        crits[i][j] = new double[numbOFcrits[i]];  //
                    }
                }
                //заполнение
                for (int i = 0; i < numbOFgroups; i++)
                {
                    //заполнение таблицы оценки критериев группы
                    readfromfile = fs.ReadLine();
                    FileStringCount++;
                    readfromfile = readfromfile.Split('/')[0];//считывание до комментария
                    this.filltable(ref crits[i], numbOFcrits[i], readfromfile, FileStringCount);
                    PBIncrement();
                }

                //работы
                //трёхмерный массив - массив размера количеств критериев с двумерными массивами таблиц сравнений работ по каждому критерию размеров количеств работ
                double[][][] projects = new double[totalcrits][][];
                for (int i = 0; i < totalcrits; i++)
                {
                    projects[i] = new double[numbOFprojs][];
                    for (int j = 0; j < numbOFprojs; j++)
                    {
                        projects[i][j] = new double[numbOFprojs];
                    }
                }
                //заполнение
                int iterator = 0;//подсчёт пройденных критериев
                for (int i = 0; i < numbOFgroups; i++)
                {
                    for (int j = 0; j < numbOFcrits[i]; j++, iterator++)
                    {
                        //заполнение таблицы оценки работ по критерию j + 1 группы i + 1 
                        readfromfile = fs.ReadLine();
                        FileStringCount++;
                        readfromfile = readfromfile.Split('/')[0];//считывание до комментария
                        this.filltable(ref projects[iterator], numbOFprojs, readfromfile, FileStringCount);
                        PBIncrement();
                    }
                }

                //// В Ы Ч И С Л Е Н И Я ////

                //для групп
                double[] columnGR = new double[numbOFgroups];//массив результатов
                this.heavymath(ref groups, numbOFgroups, columnGR);

                //для критериев
                double[][] columnCRIT = new double[numbOFgroups][];
                for (int i = 0; i < numbOFgroups; i++)
                {
                    columnCRIT[i] = new double[numbOFcrits[i]];
                    this.heavymath(ref crits[i], numbOFcrits[i], columnCRIT[i]);
                    PBIncrement();
                }

                //для работ
                double[][] columnPROJ = new double[totalcrits][];//массив размера к-ва критериев с массивом работ, содержащим вклад данного критерия
                for (int i = 0; i < totalcrits; i++)
                {
                    columnPROJ[i] = new double[numbOFprojs];
                    this.heavymath(ref projects[i], numbOFprojs, columnPROJ[i]);
                    PBIncrement();
                }

                for (int i = 0; i < 1 + numbOFgroups + totalcrits; i++)
                {//подсчёт индексов согласованности
                    if (i == 0)
                    {
                        indsogl[i] = this.findinds(ref groups, columnGR, numbOFgroups);
                    }
                    else if (i < 1 + numbOFgroups)
                    {
                        indsogl[i] = this.findinds(ref crits[i - 1], columnCRIT[i - 1], numbOFcrits[i - 1]);
                    }
                    else
                    {
                        indsogl[i] = this.findinds(ref projects[i - (1 + numbOFgroups)], columnPROJ[i - (1 + numbOFgroups)], numbOFprojs);
                    }
                }
                PBIncrement();

                double[] TotalResult = new double[numbOFprojs];//получаем и нормируем сортировку работ по суммарному вкладу критериев
                                                               //умножаем оценку вклада критерия в работу на сравнительную оценку критерия критерия и актуальность его группы
                for (int i = 0; i < numbOFprojs; i++) { TotalResult[i] = 0; }

                iterator = 0;
                for (int i = 0; i < numbOFgroups; i++)
                {
                    for (int j = 0; j < numbOFcrits[i]; j++, iterator++)
                    {
                        for (int k = 0; k < numbOFprojs; k++)
                        {
                            TotalResult[k] += columnPROJ[iterator][k] * columnCRIT[i][j] * columnGR[i] / totalcrits;//для каждой работы суммируем вклад каждого критерия в данную работу, умноженный на оценку данного критерия внутри его группы и на оценку самой группы, делим на к-во критериев
                        }
                        PBIncrement();
                    }
                }
                this.Total = TotalResult;
                this.Indexes = indsogl;
                fs.Close();
            }
            //получаем массив, в котором каждому критерию сопоставлен его общий актуальный вклад в каждую работу
            //"в работе А критерий Б затронут на ..% из всех работ, актуальный сравнительный вес критерия Б составляет В"

            ////////////////////////////////////
        }

        private void FormProgressBar_Load(object sender, EventArgs e)
        {
            this.CancelButton = button2;//при нажатии ESC
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button2.Enabled = false;
            this._flagbut = true;
            try
            {
                func();
            }
            catch (Exception ex)
            {
                this.Hide();
                FormException FE = new FormException(this._mainform, ex.Message, this);
                FE.Show();
                this._flagexc = true;                
            }
            if (this._flagexc == false)//иначе успевает создаться окно результатов
            {
                FormResult Finally = new FormResult(this._mainform, this.Total, this.Indexes);
                Finally.Show();
                this.Close();
            }         
        }

        private void button2_Click(object sender, EventArgs e)//"Назад"
        {
            this.Close();
        }

        private void FormProgressBar_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this._flagbut == false && this._flagexc== false )
            {
                this._mainform.Show();
            }
        }

    }
}
