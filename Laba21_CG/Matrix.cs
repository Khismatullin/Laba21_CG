using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Laba21_CG
{
    class Matrix
    {
        public double[,] m;
        public Matrix(double[,] arr)
        {
            m = new double[arr.GetLength(0), arr.GetLength(1)];
            Array.Copy(arr, m, arr.Length);
        }
        public static Matrix operator *(Matrix a, Matrix b)
        {
            double[,] res = new double[a.m.GetLength(0), b.m.GetLength(1)];
            for (int m = 0; m < res.GetLength(0); m++)
                for (int q = 0; q < res.GetLength(1); q++)
                    for (int n = 0; n < a.m.GetLength(1); n++)
                        res[m, q] += a.m[m, n] * b.m[n, q];
            return new Matrix(res);
        }
    }

    class Point4D
    {
        public double[] m;
        public Point4D(double[] arr)
        {
            m = new double[4];
            Array.Copy(arr, m, 4);
        }
        public static Point4D operator *(Point4D p, Matrix m)
        {
            double[] res = new double[4];
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    res[i] += p.m[j] * m.m[j, i];
            return new Point4D(res);
        }
        public Point To2D()
        {
            double w = m[3];
            Point tmp = new Point((int)(m[0] / w), (int)(m[1] / w));
            return tmp;
        }
    }
}
