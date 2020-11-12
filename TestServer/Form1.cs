using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ViTCP;

namespace TestServer
{
    public partial class Form1 : Form
    {
        const int MyPort = 8888;

        public Form1()
        {
            InitializeComponent();
        }

        Server svr = null;

        private void Form1_Load(object sender, EventArgs e)
        {
            svr = new Server();

            svr.Listen(MyPort);//MySettings.HostPort);
            svr.OnClientConnect += Svr_OnClientConnect;
            svr.OnClientDisconnect += Svr_OnClientDisconnect;
            svr.OnReceiveData += Svr_OnReceiveData;
        }

        private void Svr_OnReceiveData(int clientNumber, byte[] message, int messageSize)
        {
            string converted =  Encoding.UTF8.GetString(message, 0, messageSize);
            Action a = () => { textBox1.AppendText(clientNumber + " - " + converted + Environment.NewLine); };
            this.Invoke(a);
            int t;
            if(int.TryParse(converted,out t))
            {
                converted = (t * 2).ToString("D5");
            }
            else
            {
                converted = converted.ToUpper();
            }
            var bb = Encoding.UTF8.GetBytes(converted);
            svr.SendMessage(clientNumber, bb);
        }

        private void Svr_OnClientDisconnect(int clientNumber)
        {

        }

        private void Svr_OnClientConnect(int clientNumber)
        {
            Action a = () => { listBox1.Items.Add(clientNumber + " - " + svr.workerSockets[clientNumber].szStationName); };
            this.Invoke(a);
        }
    }
}
