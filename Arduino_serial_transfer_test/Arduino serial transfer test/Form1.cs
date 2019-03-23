using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Arduino_serial_transfer_test
{
    public partial class Form1 : Form
    {
       
        string message = "";
        string data = "0,0 2,1 0,2 0,3 0,4 0,5 0,6 0,7";
        public Form1()
        {
            InitializeComponent();
            serialPort1.Open();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }


        private void button1_Click(object sender, EventArgs e)
        {

            string sendpacket = "";
            int n = 0;
            while(n < data.Length)
            {
                if(data[n] == ' ')
                {
                    debug(n.ToString());
                    sendpacket += data[n];
                    serialPort1.Write("1");
                    string svar = "";
                    while ((svar = readSerial()) == null) { }
                    debug("Got an answer");
                    serialPort1.Write(sendpacket);
                    sendpacket = "";
                }
                else
                {
                    sendpacket += data[n];
                }
                n++;
            }
            serialPort1.Write("0");

        }

        void debug(string text)
        {
            text = "Dator: " + text + "\n";
            tbxDebug.AppendText(text);
            File.AppendAllText(Path.Combine("test.txt"), text);
            this.Update();
            this.Refresh();
        }

        string readSerial()
        {
            return message;
        }

        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            string temp = serialPort1.ReadLine();
            File.AppendAllText(Path.Combine("test.txt"), "\r\n" + serialPort1.BytesToRead.ToString() + "\r\n");
            char lead = temp[0];
            temp = temp.Substring(1);
            if (lead == 'G')
            {
                File.AppendAllText(Path.Combine("test.txt"), temp + "\n");
                tbxDebug.Invoke(new Action(() => tbxDebug.AppendText("Arduino: " + temp + "\n")));
                this.Invoke(new Action(() => this.Update()));
                this.Invoke(new Action(() => this.Refresh()));
                message = null;
            }
            else if (lead == 'A')
            {
                message =  temp;
            }
            else
            {
                string text = "Felaktig lead: " + lead + temp + "\n";
                File.AppendAllText(Path.Combine("test.txt"), text);
                tbxDebug.Invoke(new Action(() => tbxDebug.AppendText(text)));
                this.Invoke(new Action(() => this.Update()));
                this.Invoke(new Action(() => this.Refresh()));
                message = null;
            }

        }
    }
}


