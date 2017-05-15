using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Laba21_CG
{
    class DimView
    {
        int width;                                                                      //Ширина
        int height;                                                                     //Высота	
        int len, lenX, lenY, lenZ;                                                      //Длина полуоси
        int radius;                                                                     //Радиус точки
        Point O, X, Y, Z;                                                               //Координатная система
        Point Tx, Ty, Tz, T1, T2, T3, T;                                                //Точка и ее проекции
        Matrix A;                                                                       //Матрица сложных преобразований
        public int Cx, Cy, Cz;                                                          //Трехмерные координаты камеры
        public int x, y, z;                                                             //Трехмерные координаты точки

        Color bgClr = Color.Black;                                                    //Цвет фоновых элементов
        Color ptClr = Color.Black;                                                 //Цвет точек

        public bool isCentral;                                                          //Является ли проецирование центральным

        //Инициализация чертежа
        public DimView(PictureBox p)
        {
            width = p.Width;
            height = p.Height;
            O.X = width / 2;
            O.Y = height / 2;
            len = 100;
            radius = 3;
            isCentral = false;
            Cx = Cy = Cz = 150;
            Calculate();
        }

        //Расчет матрицы сложного преобразования
        void CalculateMartix()
        {
            if (isCentral) A = GetMatrixRz() * GetMatrixRx() * GetMatrixMx() * GetMatrixC() * GetMatrixPz() * GetMatrixT();
            else A = GetMatrixRz() * GetMatrixRx() * GetMatrixMx() * GetMatrixPz() * GetMatrixT();
        }

        //Расчет координат точек
        public void Calculate()
        {
            CalculateMartix();
            Point4D _O = new Point4D(new double[] { 0, 0, 0, 1 });
            Point4D _X = new Point4D(new double[] { 0.5, 0, 0, 1 });
            Point4D _Y = new Point4D(new double[] { 0, 0.5, 0, 1 });
            Point4D _Z = new Point4D(new double[] { 0, 0, 0.5, 1 });
            Point4D _Tx = new Point4D(new double[] { x, 0, 0, 1 });
            Point4D _Ty = new Point4D(new double[] { 0, y, 0, 1 });
            Point4D _Tz = new Point4D(new double[] { 0, 0, z, 1 });
            Point4D _T1 = new Point4D(new double[] { x, y, 0, 1 });
            Point4D _T2 = new Point4D(new double[] { x, 0, z, 1 });
            Point4D _T3 = new Point4D(new double[] { 0, y, z, 1 });
            Point4D _T = new Point4D(new double[] { x, y, z, 1 });

            //код, чтобы оси всегда отображались правильно при любом приближении.
            O = (_O * A).To2D();
            if (Cx != 0 || Cy != 0 || Cz != 0)
            {
                _X = (_X * A);
                _Y = (_Y * A);
                _Z = (_Z * A);
                _X.m[0] /= _X.m[3]; _X.m[1] /= _X.m[3]; _X.m[3] = 1;
                _Y.m[0] /= _Y.m[3]; _Y.m[1] /= _Y.m[3]; _Y.m[3] = 1;
                _Z.m[0] /= _Z.m[3]; _Z.m[1] /= _Z.m[3]; _Z.m[3] = 1;

                X.X = (int)((_X.m[0] - O.X) * 300 + O.X);
                X.Y = (int)((_X.m[1] - O.Y) * 300 + O.Y);
                Y.X = (int)((_Y.m[0] - O.X) * 300 + O.X);
                Y.Y = (int)((_Y.m[1] - O.Y) * 300 + O.Y);
                Z.X = (int)((_Z.m[0] - O.X) * 300 + O.X);
                Z.Y = (int)((_Z.m[1] - O.Y) * 300 + O.Y);
            }
            Tx = (_Tx * A).To2D();
            Ty = (_Ty * A).To2D();
            Tz = (_Tz * A).To2D();
            T1 = (_T1 * A).To2D();
            T2 = (_T2 * A).To2D();
            T3 = (_T3 * A).To2D();
            T = (_T * A).To2D();
        }

        //Вывод чертежа на экран
        public void Draw(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            //Перо для рисования осей со стрелкой на конце
            Pen axisPen = new Pen(bgClr, 1);
            AdjustableArrowCap cap = new AdjustableArrowCap(4, 4);
            cap.MiddleInset = 3;
            axisPen.CustomEndCap = cap;

            //Перо для рисования осей
            Pen osiPen = new Pen(bgClr, 1);
            osiPen.CustomEndCap = new System.Drawing.Drawing2D.AdjustableArrowCap(3.0f, 6.0f);

            //Перо для рисования окружностей
            Pen drawellipsePen = new Pen(Color.Black, 1);

            //Перо для заливки окружностей
            Pen fillellipsePen = new Pen(Color.Black, 1);

            //Инструменты для рисования
            SolidBrush brush = new SolidBrush(bgClr);
            SolidBrush brushT = new SolidBrush(ptClr);
            SolidBrush brushTochekT1T2T3 = new SolidBrush(Color.Cyan);
            SolidBrush brushTochkiT = new SolidBrush(Color.Red);
            SolidBrush brushTochek = new SolidBrush(Color.Black);
            Font font = new Font("Arial", 10, FontStyle.Regular);
            Pen pen = new Pen(ptClr, 1);
            pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;

            //смещение для надписей
            int offset = 5;

            string errorMsg = "";
            if (CheckErrors(ref errorMsg))
            {
                g.DrawString(errorMsg, font, brush, 10, height / 2);
                return;
            }

            //Отрисовка координатной системы
            g.DrawLine(osiPen, O, X);
            g.DrawLine(osiPen, O, Y);
            g.DrawLine(osiPen, O, Z);
            g.DrawString("O", font, brush, O);
            g.DrawString("X", font, brush, X);
            g.DrawString("Y", font, brush, Y);
            g.DrawString("Z", font, brush, Z);

            //Отрисовка линий связи
            g.DrawLine(pen, Tx, T1);
            g.DrawLine(pen, Ty, T1);
            g.DrawLine(pen, Tx, T2);
            g.DrawLine(pen, Tz, T2);
            g.DrawLine(pen, Tz, T3);
            g.DrawLine(pen, Ty, T3);
            g.DrawLine(pen, T1, T);
            g.DrawLine(pen, T2, T);
            g.DrawLine(pen, T3, T);

            //Отрисовка точек
            g.FillEllipse(brushTochek, O.X - radius, O.Y - radius, 2 * radius, 2 * radius);
            g.DrawEllipse(drawellipsePen, O.X - radius, O.Y - radius, 2 * radius, 2 * radius);

            g.FillEllipse(brushTochek, Tx.X - radius, Tx.Y - radius, 2 * radius, 2 * radius);
            g.DrawEllipse(drawellipsePen, Tx.X - radius, Tx.Y - radius, 2 * radius, 2 * radius);

            g.FillEllipse(brushTochek, Ty.X - radius, Ty.Y - radius, 2 * radius, 2 * radius);
            g.DrawEllipse(drawellipsePen, Ty.X - radius, Ty.Y - radius, 2 * radius, 2 * radius);

            g.FillEllipse(brushTochek, Tz.X - radius, Tz.Y - radius, 2 * radius, 2 * radius);
            g.DrawEllipse(drawellipsePen, Tz.X - radius, Tz.Y - radius, 2 * radius, 2 * radius);

            g.FillEllipse(brushTochekT1T2T3, T1.X - radius, T1.Y - radius, 2 * radius, 2 * radius);
            g.DrawEllipse(drawellipsePen, T1.X - radius, T1.Y - radius, 2 * radius, 2 * radius);

            g.FillEllipse(brushTochekT1T2T3, T2.X - radius, T2.Y - radius, 2 * radius, 2 * radius);
            g.DrawEllipse(drawellipsePen, T2.X - radius, T2.Y - radius, 2 * radius, 2 * radius);

            g.FillEllipse(brushTochekT1T2T3, T3.X - radius, T3.Y - radius, 2 * radius, 2 * radius);
            g.DrawEllipse(drawellipsePen, T3.X - radius, T3.Y - radius, 2 * radius, 2 * radius);

            g.FillEllipse(brushTochkiT, T.X - radius, T.Y - radius, 2 * radius, 2 * radius);
            g.DrawEllipse(drawellipsePen, T.X - radius, T.Y - radius, 2 * radius, 2 * radius);

            //Отрисовка подписей к точкам
            g.DrawString("Tx", font, brush, Tx.X - offset,Tx.Y + offset);
            g.DrawString("Ty", font, brush, Ty.X - offset, Ty.Y + offset);
            g.DrawString("Tz", font, brush, Tz.X + offset, Tz.Y - (offset*2));
            g.DrawString("T1", font, brush, T1.X - offset, T1.Y + offset);
            g.DrawString("T2", font, brush, T2.X + offset, T2.Y + offset);
            g.DrawString("T3", font, brush, T3.X + offset, T3.Y + offset);
            g.DrawString("T", font, brush, T.X + offset, T.Y + offset);

            
        }

        bool CheckErrors(ref string errorMsg)
        {
            if (Cx == 0 && Cy == 0 && Cz == 0)
            {
                errorMsg = "Наблюдатель находится в начале координат !";
                return true;
            }

            if (isCentral && Cx == x && Cy == y && Cz == z)
            {
                errorMsg = "Наблюдатель совпадает с точкой T !";
                return true;
            }
            if (isCentral && (-Cx * (x - Cx) - Cy * (y - Cy) - Cz * (z - Cz) <= 0))
            {
                errorMsg = "Наблюдатель расположен ближе, чем точка Т !";
                return true;
            }
            if (CheckCrd(Tx) || CheckCrd(Ty) || CheckCrd(Tz) || CheckCrd(T1) || CheckCrd(T2) || CheckCrd(T3) || CheckCrd(T))
            {
                errorMsg = "Точка вышла за область отображения";
                return true;
            }
            return false;
        }
        bool CheckCrd(Point T)
        {
            if (T.X > width - 4 || T.X < 0 || T.Y > height - 4 || T.Y < 0) return true;
            else return false;
        }
        Matrix GetMatrixRz()
        {
            double cosa, sina;
            if (Cx == 0 && Cy == 0)
            {
                cosa = 1;
                sina = 0;
            }
            else
            {
                cosa = Cy / Math.Sqrt(Cx * Cx + Cy * Cy);
                sina = Cx / Math.Sqrt(Cx * Cx + Cy * Cy);
            }
            Matrix Rz = new Matrix(new double[,] { { cosa,  sina,  0,     0 },
                                                   { -sina, cosa,  0,     0 },
                                                   { 0,     0,     1,     0 },
                                                   { 0,     0,     0,     1 } });
            return Rz;
        }
        Matrix GetMatrixRx()
        {
            double sinb = Math.Sqrt(Cx * Cx + Cy * Cy) / Math.Sqrt(Cz * Cz + Cy * Cy + Cx * Cx);
            double cosb = Cz / Math.Sqrt(Cz * Cz + Cy * Cy + Cx * Cx);
            Matrix Rx = new Matrix(new double[,] { { 1,     0,     0,     0 },
                                                   { 0,     cosb,  sinb,  0 },
                                                   { 0,     -sinb, cosb,  0 },
                                                   { 0,     0,     0,     1 } });
            return Rx;
        }
        Matrix GetMatrixMx()
        {
            Matrix Mx = new Matrix(new double[,] { { -1, 0,  0,  0 },
                                                   { 0,  1,  0,  0 },
                                                   { 0,  0,  1,  0 },
                                                   { 0,  0,  0,  1 } });
            return Mx;
        }
        Matrix GetMatrixPz()
        {
            Matrix Pz = new Matrix(new double[,] { { 1, 0, 0, 0 },
                                                   { 0, 1, 0, 0 },
                                                   { 0, 0, 0, 0 },
                                                   { 0, 0, 0, 1 } });
            return Pz;
        }
        Matrix GetMatrixT()
        {
            Matrix T = new Matrix(new double[,] { { 1,   0,   0,   0 },
                                                  { 0,   1,   0,   0 },
                                                  { 0,   0,   1,   0 },
                                                  { width / 2, height / 2, 0,   1 } });
            return T;
        }
        Matrix GetMatrixC()
        {
            double c = Math.Sqrt(Cx * Cx + Cy * Cy + Cz * Cz);
            Matrix C = new Matrix(new double[,] { { 1,   0,   0,   0      },
                                                  { 0,   1,   0,   0      },
                                                  { 0,   0,   1,   -1 / c },
                                                  { 0,   0,   0,   1      } });
            return C;
        }
    }
}
