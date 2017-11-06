using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hexapod
{
    public class Leg
    {
        public int knee1, knee2, knee3;

        private int hip, shin;

        public static string Cmd(int ch, int p, int t)
        {
            string cmd = "#" + ch + "P" + p + "T" + t +"\r";
            return cmd;
        }

        public static string Point (float hpelvis, float hfoot, float dist)
        {

            return "";

        }

        public static void Step (int h, int l)
        {

        }
    }

    static class Program
    {
        
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Leg one = new Leg();

            //Leg.Cmd(0,1500,5000);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
