using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hexapod
{
    public partial class Form1 : Form
    {
        Leg one = new Leg();

        public Form1()
        {
            InitializeComponent();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.IO.Ports.SerialPort sport = new System.IO.Ports.SerialPort(textBox1.Text,
                                                                              115200,
                                                                              System.IO.Ports.Parity.None,
                                                                              8,
                                                                              System.IO.Ports.StopBits.One);

            //Leg one = new Leg();
            /*
            Leg R3 = new Leg
            {
                ch1 = 24,
                ch2 = 25,
                ch3 = 26,

                kmin1 = 700, kmax1 = 2300,
                kmin2 = 700, kmax2 = 1800,
                kmin3 = 1000, kmax3 = 2368,
                
                k001 = 1500, k901 = 2300,
                k002 = 700, k902 = 1500,
                k003 = 2368, k903 = 1500,
            };

            Leg R2 = new Leg
            {
                ch1 = 20,
                ch2 = 21,
                ch3 = 22,

                kmin1 = 700,
                kmax1 = 2300,
                kmin2 = 700,
                kmax2 = 1800,
                kmin3 = 1000,
                kmax3 = 2368,

                k001 = 1500,
                k901 = 2300,
                k002 = 700,
                k902 = 1500,
                k003 = 2368,
                k903 = 1500,
            };
            */
            try
            {
                sport.Open();
                //sport.Write(Leg.Cmd(Decimal.ToInt32(numericUpDown1.Value), Decimal.ToInt32(numericUpDown2.Value), Decimal.ToInt32(numericUpDown3.Value)) + "\r");
                string cmd = one.Point(Decimal.ToDouble(numericUpDown1.Value), Decimal.ToDouble(numericUpDown2.Value), Decimal.ToDouble(numericUpDown3.Value));
                //string cmd = R2.Point(Decimal.ToDouble(numericUpDown1.Value), Decimal.ToDouble(numericUpDown2.Value), Decimal.ToDouble(numericUpDown3.Value));
                toolStripStatusLabel1.Text = cmd;
                
                sport.Write(cmd + "\r");
            }
            catch (Exception ex) {
                MessageBox.Show(ex.ToString());
            }

           sport.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            System.IO.Ports.SerialPort sport = new System.IO.Ports.SerialPort(textBox1.Text,
                                                                              115200,
                                                                              System.IO.Ports.Parity.None,
                                                                              8,
                                                                              System.IO.Ports.StopBits.One);
            
            try
            {
                sport.Open();
                //string cmd = one.Point(Decimal.ToDouble(numericUpDown1.Value), Decimal.ToDouble(numericUpDown2.Value), 1500);
                string cmd = Leg.Cmd(one.ch1, 1500, Program.TIME) + Leg.Cmd(one.ch2, 1500, Program.TIME) + Leg.Cmd(one.ch3, 1500, Program.TIME);
                toolStripStatusLabel1.Text = cmd;
                sport.Write(cmd + "\r");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            sport.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
