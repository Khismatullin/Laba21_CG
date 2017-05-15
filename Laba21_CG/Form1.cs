using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Laba21_CG
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            Cx = Cy = Cz = 75;
            x = y = z = 50;

            //Инициализация пространственного чертежа
            dimView = new DimView(this.pictureBoxDimensional);
            this.pictureBoxDimensional.Paint += dimView.Draw;

            //Инициализация комплексного чертежа
            complexView = new ComplexView(this.pictureBoxComplex);
            this.pictureBoxComplex.Paint += complexView.Draw;

        }

        //Объекты для привязки формы к классам, реализующим чертежы
        DimView dimView;
        ComplexView complexView;

        //Трехмерные координаты точки и камеры
        int x, y, z;
        int Cx, Cy, Cz;

        //Реакция на изменение значения координат
        private void trackBarX_ValueChanged(object sender, EventArgs e)
        {
            dimView.x = complexView.T.x = x = trackBarX.Value;
            
            complexView.Calculate();
            dimView.Calculate();

            //Вывод нового значения на экран
            labelTCoord.Text = String.Format("( {0}, {1}, {2} )", x, y, z);
            labelCCoord.Text = String.Format("( {0}, {1}, {2} )", Cx, Cy, Cz);
            pictureBoxDimensional.Refresh();
            pictureBoxComplex.Refresh();
        }

        private void trackBarY_ValueChanged(object sender, EventArgs e)
        {
            dimView.y = complexView.T.y = y = trackBarY.Value;
            
            complexView.Calculate();
            dimView.Calculate();

            //Вывод нового значения на экран
            labelTCoord.Text = String.Format("( {0}, {1}, {2} )", x, y, z);
            labelCCoord.Text = String.Format("( {0}, {1}, {2} )", Cx, Cy, Cz);
            pictureBoxDimensional.Refresh();
            pictureBoxComplex.Refresh();
        }

        private void trackBarZ_ValueChanged(object sender, EventArgs e)
        {
            dimView.z = complexView.T.z = z = trackBarZ.Value;
            
            complexView.Calculate();
            dimView.Calculate();

            //Вывод нового значения на экран
            labelTCoord.Text = String.Format("( {0}, {1}, {2} )", x, y, z);
            labelCCoord.Text = String.Format("( {0}, {1}, {2} )", Cx, Cy, Cz);
            pictureBoxDimensional.Refresh();
            pictureBoxComplex.Refresh();
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void trackBarX1_ValueChanged(object sender, EventArgs e)
        {
            dimView.Cx = complexView.C.x = Cx = trackBarX1.Value;
            
            complexView.Calculate();
            dimView.Calculate();

            //Вывод нового значения на экран
            labelTCoord.Text = String.Format("( {0}, {1}, {2} )", x, y, z);
            labelCCoord.Text = String.Format("( {0}, {1}, {2} )", Cx, Cy, Cz);
            pictureBoxDimensional.Refresh();
            pictureBoxComplex.Refresh();
        }

        private void trackBarY1_ValueChanged(object sender, EventArgs e)
        {
            dimView.Cy = complexView.C.y = Cy = trackBarY1.Value;
            
            complexView.Calculate();
            dimView.Calculate();

            //Вывод нового значения на экран
            labelTCoord.Text = String.Format("( {0}, {1}, {2} )", x, y, z);
            labelCCoord.Text = String.Format("( {0}, {1}, {2} )", Cx, Cy, Cz);
            pictureBoxDimensional.Refresh();
            pictureBoxComplex.Refresh();
        }

        private void trackBarZ1_ValueChanged(object sender, EventArgs e)
        {
            dimView.Cz = complexView.C.z = Cz = trackBarZ1.Value;
            
            complexView.Calculate();
            dimView.Calculate();

            //Вывод нового значения на экран
            labelTCoord.Text = String.Format("( {0}, {1}, {2} )", x, y, z);
            labelCCoord.Text = String.Format("( {0}, {1}, {2} )", Cx, Cy, Cz);
            pictureBoxDimensional.Refresh();
            pictureBoxComplex.Refresh();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            trackBarX.Maximum = 100;
            trackBarY.Maximum = 100;
            trackBarZ.Maximum = 100;
            trackBarX.Value = x;
            trackBarY.Value = y;
            trackBarZ.Value = z;

            trackBarX1.Maximum = 150;
            trackBarY1.Maximum = 150;
            trackBarZ1.Maximum = 150;
            trackBarX1.Value = Cx;
            trackBarY1.Value = Cy;
            trackBarZ1.Value = Cz;
        }

        private void radioButtonOrth_Click(object sender, EventArgs e)
        {
            radioButtonOrth.Checked = true;
            radioButtonCentral.Checked = false;
            dimView.isCentral = false;
            dimView.Calculate();
            pictureBoxDimensional.Refresh();
        }

        private void radioButtonCentral_Click(object sender, EventArgs e)
        {
            radioButtonOrth.Checked = false;
            radioButtonCentral.Checked = true;
            dimView.isCentral = true;
            dimView.Calculate();
            pictureBoxDimensional.Refresh();
        }
    }
}
