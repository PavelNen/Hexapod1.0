using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Serialization;

namespace Hexapod
{

    public class WrongCoordException : ApplicationException
    {
        public WrongCoordException() { }

        public WrongCoordException(string message) : base(message) { }

        public WrongCoordException(string message, Exception inner) : base(message, inner) { }

        protected WrongCoordException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    public class Leg
    {
        public short ch1, ch2, ch3;
        public short kmin1, kmax1, kmin2, kmax2, kmin3, kmax3;
        public short k001, k901, k002, k902, k003, k903;
        public double shangle;
        
        public Leg()
        {
            ch1 = 5; ch2 = 3; ch3 = 0;
            kmin1 = 600; kmax1 = 2400;
            kmin2 = 1050; kmax2 = 2400;
            kmin3 = 700; kmax3 = 2400;

            k001 = 1500; k901 = 2400;
            k002 = 1500; k902 = 2400;
            k003 = 2400; k903 = 1500;

            shangle = Math.PI / 3;
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

        private static int Ang2pos (double ang, short kst, short k00, short k90, short kmin, short kmax)
        {
            int p = kst + Convert.ToInt32( ang * Krad(k00, k90));
            if (p < kmin) { p = kmin; }
            if ( p > kmax ) { p = kmax; }
            return p;
        }
        
        private static bool ValidCoord (double hpelvis, double hfoot, double dist, double along)
        {
            double l1 = Program.HIP;
            double l2 = Program.SHIN;

            bool e = true;

            double vmin = Math.Abs(l1 - l2);
            double vmax = l1 + l2;
            double x = Math.Sqrt(Math.Pow((hpelvis - hfoot), 2) + Math.Pow(dist,2) + Math.Pow(along,2));

            if (x.CompareTo(vmin) < 0 || x.CompareTo(vmax) > 0) {
                e = false;
            }

            return e;
        }
        
        public string Point (double hpelvis, double hfoot, double dist, double along)
        {
            //hplevis - высота корпуса над полом; hfoot - высота стопы над полом;
            //dist - расстояние от нуля до стопы поперёк робота; along - рассотяние от нуля до стопы вдоль робота ;
            //shangle - поправка нулевой оси;

            if (ValidCoord(hpelvis, hfoot, dist, along) == false)
            {
                throw new WrongCoordException("Невозможные координаты");
            }
            //Конвертирование линейных координат в углы в узлах
            double l1 = Program.HIP;
            double l2 = Program.SHIN;

            double lpro = Math.Sqrt(Math.Pow(dist, 2) + Math.Pow(along, 2)); //Длина проекции ноги на пол

            double xsqr = Math.Pow((hpelvis - hfoot), 2) + Math.Pow(lpro, 2); //Это квадрат длины!
            double x = Math.Sqrt(xsqr);

            double b = Math.Acos((l1 * l1 + l2 * l2 - xsqr) / (2 * l1 * l2)); //Угол между бедром и голенью

            double siny = l2 * Math.Sin(b) / x;
            double y = Math.Acos((l1 * l1 + xsqr - l2 * l2) / (2 * l1 * x));

            double a;
            if (hpelvis == hfoot)
            {
                a = Math.PI/2 - y;
            }
            else if (hpelvis > hfoot)
            {
                a = Math.PI - y - Math.Atan(lpro / (hpelvis - hfoot)); //Угол бедра относительно вертикальной оси
            }else
            {
                a = Math.PI / 2 - y - Math.Atan((hfoot - hpelvis) / lpro); //Угол бедра относительно вертикальной оси
            }

            double c = Math.Atan(along / dist);

            int p1 = Ang2pos(shangle + c, k001, k001, k901, kmin1, kmax1);
            if (p1 < kmin1) { p1 = kmin1; }
            if (p1 > kmax1) { p1 = kmax1; }
            int p2 = Ang2pos(a, k002, k002, k902, kmin2, kmax2);
            if (p2 < kmin2) { p2 = kmin2; }
            if (p2 > kmax2) { p2 = kmax2; }
            int p3 = Ang2pos(Math.PI - b, k003, k003, k903, kmin3, kmax3);
            if (p3 < kmin3) { p3 = kmin3; }
            if (p3 > kmax3) { p3 = kmax3; }

            string order = Cmd(ch1, p1, Program.TIME) + Cmd(ch2, p2, Program.TIME) + Cmd(ch3, p3, Program.TIME) + "\r";
            //string order = "a = ";
            //order += a/Math.PI*180;
            //order += ", b = ";order += b / Math.PI * 180; order += ", y = ";order += y / Math.PI * 180;
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
        public const short TIME = 1000;
        
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
