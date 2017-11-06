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

            try {
                sport.Open();
                sport.Write(Leg.Cmd(Decimal.ToInt32(numericUpDown1.Value), Decimal.ToInt32(numericUpDown2.Value), Decimal.ToInt32(numericUpDown3.Value)));
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
                sport.Write("#0P700T2000#3P700T2000#5P700T7000\r");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            sport.Close();
        }
    }
}
