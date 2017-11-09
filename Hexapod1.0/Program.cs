using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hexapod
{
    public class Leg
    {
        public short ch1, ch2, ch3;
        public short kmin1, kmax1, kmin2, kmax2, kmin3, kmax3;
        public short k001, k901, k002, k902, k003, k903;
        
        public Leg()
        {
            ch1 = 5; ch2 = 3; ch3 = 0;
            kmin1 = 700; kmax1 = 2400;
            kmin2 = 1050; kmax2 = 2400;
            kmin3 = 700; kmax3 = 2400;

            k001 = 1500; k901 = 2400;
            k002 = 1500; k902 = 2400;
            k003 = 2400; k903 = 1500;
        }

        private static double Krad (short k00, short k90)
        {
            //Определение P/rad для сервоприводов
            return (k90 - k00) / (Math.PI / 2);
        }

        public static string Cmd(int ch, int p, int t)
        {
            //Формирование команды без завершающего CR
            string cmd = "#" + ch + "P" + p + "T" + t;
            return cmd;
        }

        private static int Ang2pos (double ang, short kst, short k00, short k90)
        {
            int p = Convert.ToInt32( kst + ang * Krad(k00, k90));
            if (p < 600) { p = 600; }
            if ( p > 2500 ) { p = 2500; }
            return p;
        }

        public string Point (double hpelvis, double hfoot, double dist)
        {
            //Конвертирование линейных координат в углы в узлах
            double l1 = Program.HIP;
            double l2 = Program.SHIN;

            double xsqr = Math.Pow((hpelvis - hfoot), 2) + Math.Pow(dist, 2); //Это квадрат длины!
            double b = Math.Acos((l1 * l1 + l2 * l2 - xsqr) / (2 * l1 * l2)); //Угол между бедром и голенью

            double y = Math.Asin(l2 * Math.Sin(b) / Math.Sqrt(xsqr));

            double a;
            if (hpelvis == hfoot)
            {
                a = Math.PI / 2;
            }
            else if (hpelvis > hfoot)
            {
                a = Math.PI - y - Math.Atan(dist / (hpelvis - hfoot)); //Угол бедра относительно вертикальной оси
            }else
            {
                a = Math.PI - y - 1 / Math.Atan(dist / (hpelvis - hfoot)); //Угол бедра относительно вертикальной оси
            }

            short p1 = 1500;
            if (p1 < kmin1) { p1 = kmin1; }
            if (p1 > kmax1) { p1 = kmax1; }
            int p2 = Ang2pos(a, 1500, k002, k902);
            if (p2 < kmin2) { p2 = kmin2; }
            if (p2 > kmax2) { p2 = kmax2; }
            int p3 = Ang2pos(Math.PI - b, 2400, k003, k903);
            if (p3 < kmin3) { p3 = kmin3; }
            if (p3 > kmax3) { p3 = kmax3; }

            string order = Cmd(ch1, p1, Program.TIME) + Cmd(ch2, p2, Program.TIME) + Cmd(ch3, p3, Program.TIME) + "\r";
            //string order = "a = ";
            //order += a/Math.PI*180;
           // order += ", b = ";order += b / Math.PI * 180; order += ", y = ";order += y / Math.PI * 180;
            return order;

        }

        public static void Step (int h, int l)
        {

        }
    }

    static class Program
    {
        public const double HIP = 5.5; //Бедро
        public const double SHIN = 13; //Голень
        public const short TIME = 3000;
        
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Leg.Cmd(0,1500,5000);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
