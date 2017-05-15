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
    class ComplexViewPoint
    {
        Point O;                                                                        //Координаты центра экрана
        Point T1, T2, T3, Tx, Ty1, Ty2, Tz;                                             //Координаты проекций точки
        int radius;                                                                     //Радиус точки
        Color clr;                                                                      //Цвет точки и линий связи
        String name;                                                                    //Название точки
        public int x, y, z;                                                                 //Трехмерные координаты точки

        //Конструктор, приниющий координаты центра экрана, относительного которого рисуется точка
        public ComplexViewPoint(Point O, Color clr, String name)
        {
            this.O = O;
            this.clr = clr;
            this.name = name;
            radius = 3;
        }

        //Расчет координат проекций
        public void Calculate()
        {
            Tx.X = O.X - x;
            Tx.Y = O.Y;
            Ty1.X = O.X;
            Ty1.Y = O.Y + y;
            Ty2.X = O.X + y;
            Ty2.Y = O.Y;
            Tz.X = O.X;
            Tz.Y = O.Y - z;
            T1.X = Tx.X;
            T1.Y = Ty1.Y;
            T2.X = Tx.X;
            T2.Y = Tz.Y;
            T3.X = Ty2.X;
            T3.Y = Tz.Y;
        }

        //Отрисовка точки и линий связи
        public void Draw(Graphics g)
        {
            //Инструменты для рисования
            Pen pen = new Pen(clr, 1);
            pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            SolidBrush brush = new SolidBrush(clr);
            SolidBrush brushT1T2T3 = new SolidBrush(Color.Cyan);
            Font font = new Font("Arial", 10, FontStyle.Regular);

            //Линии связи точки Т
            g.DrawLine(pen, T1, Tx);
            g.DrawLine(pen, T1, Ty1);
            g.DrawLine(pen, T2, Tx);
            g.DrawLine(pen, T2, Tz);
            g.DrawLine(pen, T3, Tz);
            g.DrawLine(pen, T3, Ty2);

            //Проекции точки Т
            g.FillEllipse(brush, O.X - radius, O.Y - radius, 2 * radius, 2 * radius);
            g.DrawEllipse(pen, O.X - radius, O.Y - radius, 2 * radius, 2 * radius);

            g.FillEllipse(brush, Tx.X - radius, Tx.Y - radius, 2 * radius, 2 * radius);
            g.DrawEllipse(pen, Tx.X - radius, Tx.Y - radius, 2 * radius, 2 * radius);

            g.FillEllipse(brush, Ty1.X - radius, Ty1.Y - radius, 2 * radius, 2 * radius);
            g.DrawEllipse(pen, Ty1.X - radius, Ty1.Y - radius, 2 * radius, 2 * radius);

            g.FillEllipse(brush, Ty2.X - radius, Ty2.Y - radius, 2 * radius, 2 * radius);
            g.DrawEllipse(pen, Ty2.X - radius, Ty2.Y - radius, 2 * radius, 2 * radius);

            g.FillEllipse(brush, Tz.X - radius, Tz.Y - radius, 2 * radius, 2 * radius);
            g.DrawEllipse(pen, Tz.X - radius, Tz.Y - radius, 2 * radius, 2 * radius);

            g.FillEllipse(brushT1T2T3, T1.X - radius, T1.Y - radius, 2 * radius, 2 * radius);
            g.DrawEllipse(pen, T1.X - radius, T1.Y - radius, 2 * radius, 2 * radius);

            g.FillEllipse(brushT1T2T3, T2.X - radius, T2.Y - radius, 2 * radius, 2 * radius);
            g.DrawEllipse(pen, T2.X - radius, T2.Y - radius, 2 * radius, 2 * radius);

            g.FillEllipse(brushT1T2T3, T3.X - radius, T3.Y - radius, 2 * radius, 2 * radius);
            g.DrawEllipse(pen, T3.X - radius, T3.Y - radius, 2 * radius, 2 * radius);

            //Отрисовка линии связи между Ty1 и Ty2
            if (y > 0)
            {
                g.DrawArc(pen, O.X - y, O.Y - y, 2 * y, 2 * y, 0, 90);
            }
            if (y < 0)
            {
                g.DrawArc(pen, O.X + y, O.Y + y, -2 * y, -2 * y, 180, 90);
            }

            //Отрисовка подписей к точкам
            g.DrawString(name + "x", font, brush, Tx);
            g.DrawString(name + "y1", font, brush, Ty1);
            g.DrawString(name + "y2", font, brush, Ty2);
            g.DrawString(name + "z", font, brush, Tz);
            g.DrawString(name + "1", font, brush, T1);
            g.DrawString(name + "2", font, brush, T2);
            g.DrawString(name + "3", font, brush, T3);
        }
    }

    class ComplexView
    {
        int width;                                                                      //Ширина рисунка
        int height;                                                                     //Высота рисунка
        int indent;                                                                     //Отступ от края рисунка
        int len;                                                                        //Длина полуоси
        Point O;                                                                        //Координаты центра чертежа
        Point X1, X2, Z1, Z2;                                                           //Точки осей координат
        Color clr = Color.Black;                                                        //Цвет фоновых элементов

        public ComplexViewPoint C, T;                                                   //Отображаемые точки

        //Задание параметров в конструкторе
        public ComplexView(PictureBox p)
        {
            width = p.Width;
            height = p.Height;
            O.X = width / 2;
            O.Y = height / 2;
            indent = 20;
            len = width / 2 - indent;
            C = new ComplexViewPoint(O, Color.Black, "C");
            T = new ComplexViewPoint(O, Color.Black, "T");
            //Начальное положение камеры
            C.x = C.y = C.z = 150;
            Calculate();
        }

        //Расчет координат точек
        public void Calculate()
        {
            //Расчет точек осей координат
            X1.X = O.X + len;
            X1.Y = O.Y;
            X2.X = O.X - len;
            X2.Y = O.Y;
            Z1.X = O.X;
            Z1.Y = O.Y + len;
            Z2.X = O.X;
            Z2.Y = O.Y - len;

            //Расчет координат проекций отображаемых точек
            C.Calculate();
            T.Calculate();
        }

        //Отрисовка чертежа
        public void Draw(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            //Инструменты для рисования
            Pen pen = new Pen(clr, 1);
            
            pen.CustomEndCap = new System.Drawing.Drawing2D.AdjustableArrowCap(3.0f, 6.0f);
            pen.CustomStartCap = new System.Drawing.Drawing2D.AdjustableArrowCap(3.0f, 6.0f);

            SolidBrush brush = new SolidBrush(clr);
            Font font = new Font("Arial", 10, FontStyle.Regular);

            //Отрисовка осей
            g.DrawLine(pen, X1, X2);
            g.DrawLine(pen, Z1, Z2);
            g.DrawString("O", font, brush, O);
            g.DrawString("X", font, brush, X2);
            g.DrawString("Z", font, brush, Z2);
            g.DrawString("Y", font, brush, X1);
            g.DrawString("Y", font, brush, Z1);

            //Отрисовка точек
            T.Draw(g);
            C.Draw(g);
        }
    }
}
