using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace Pong
{
    public partial class Form1 : Form
    {
        private int xB;
        private int yB;
        private int dB;
        private Thread MoveBall;
        private UdpClient udpServer;
        private string dirGrupo;
        private IPAddress multicastAddress;
        private Thread tListen;
        private IPEndPoint EPServer;
        private UdpClient udpClient;
        private IPEndPoint EPClient;
        public Form1()
        {
            InitializeComponent();
            xB = pictureBox1.Location.X;
            yB = pictureBox1.Location.Y;
            dB = 6;
            CheckForIllegalCrossThreadCalls = false;
            MoveBall = new Thread(Mover);
            tListen = new Thread(Listen);
            dirGrupo = "230.0.0.0";
            EPServer = new IPEndPoint(IPAddress.Broadcast, 1234);
            EPClient = new IPEndPoint(IPAddress.Any, 1234);
            multicastAddress = IPAddress.Parse(dirGrupo);
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void Mover()
        {
            while (true)
            {
                if (dB == 3)//Bola derecha
                {
                    pictureBox1.Location = new Point(pictureBox1.Location.X + 5, pictureBox1.Location.Y);
                    if (((pictureBox1.Location.Y + pictureBox1.Height) > (panel2.Location.Y + 5 + panel2.Height / 2)
                        && (pictureBox1.Location.Y + pictureBox1.Height) <= (panel2.Location.Y + panel2.Height))
                        && (pictureBox1.Location.X + pictureBox1.Width) >= panel2.Location.X)
                        dB = 7;
                    if (((pictureBox1.Location.Y + pictureBox1.Height) >= (panel2.Location.Y)
                        && (pictureBox1.Location.Y + pictureBox1.Height) <= (panel2.Location.Y + panel2.Height - 5))
                        && (pictureBox1.Location.X + pictureBox1.Width) >= panel2.Location.X)
                        dB = 8;
                    if (((pictureBox1.Location.Y + pictureBox1.Height) <= (panel2.Location.Y + panel2.Height / 2 + 5)
                        && (pictureBox1.Location.Y + pictureBox1.Height) > (panel2.Location.Y + panel2.Height / 2 - 5))
                        && (pictureBox1.Location.X + pictureBox1.Width) >= panel2.Location.X)
                        dB = 4;
                }
                if (dB == 4)
                {
                    pictureBox1.Location = new Point(pictureBox1.Location.X - 5, pictureBox1.Location.Y);
                    if (((pictureBox1.Location.Y + pictureBox1.Height) < (panel1.Location.Y + 5 + panel1.Height / 2)
                        && (pictureBox1.Location.Y + pictureBox1.Height) >= (panel1.Location.Y))
                        && ((pictureBox1.Location.X) <= (panel1.Location.X + panel1.Width) && (pictureBox1.Location.X) >= (panel1.Location.X)))
                        dB = 5;
                    if (((pictureBox1.Location.Y + pictureBox1.Height) <= (panel1.Location.Y + panel1.Height)
                        && (pictureBox1.Location.Y + pictureBox1.Height) > (panel1.Location.Y + panel1.Height / 2 + 5))
                        && ((pictureBox1.Location.X) <= (panel1.Location.X + panel1.Width) && (pictureBox1.Location.X) >= (panel1.Location.X)))
                        dB = 6;
                    if (((pictureBox1.Location.Y + pictureBox1.Height) >= (panel1.Location.Y + panel1.Height / 2 - 5)
                       && (pictureBox1.Location.Y + pictureBox1.Height) < (panel1.Location.Y + panel1.Height / 2 + 5))
                       && ((pictureBox1.Location.X) <= (panel1.Location.X + panel1.Width) && (pictureBox1.Location.X) >= (panel1.Location.X)))
                        dB = 3;
                }
                if (dB == 5)//Bola diagonal superior derecha
                {
                    pictureBox1.Location = new Point(pictureBox1.Location.X + 5, pictureBox1.Location.Y - 5);
                    if ((pictureBox1.Location.Y) <= 0)
                        dB = 6;
                    if (((pictureBox1.Location.Y + pictureBox1.Height) > (panel2.Location.Y + 5 + panel2.Height / 2) 
                        && (pictureBox1.Location.Y + pictureBox1.Height) <= (panel2.Location.Y + panel2.Height))
                        && (pictureBox1.Location.X+pictureBox1.Width) >= panel2.Location.X)
                        dB = 7;
                    if (((pictureBox1.Location.Y + pictureBox1.Height) >= (panel2.Location.Y)
                        && (pictureBox1.Location.Y + pictureBox1.Height) <= (panel2.Location.Y + panel2.Height-5))
                        && (pictureBox1.Location.X + pictureBox1.Width) >= panel2.Location.X)
                        dB = 8;
                    if (((pictureBox1.Location.Y + pictureBox1.Height) <= (panel2.Location.Y + panel2.Height / 2 + 5)
                        && (pictureBox1.Location.Y + pictureBox1.Height) > (panel2.Location.Y + panel2.Height/2 - 5))
                        && (pictureBox1.Location.X + pictureBox1.Width) >= panel2.Location.X)
                        dB = 4;
                }
                if (dB == 6)//Bola diagonal inferior derecha
                {
                    pictureBox1.Location = new Point(pictureBox1.Location.X + 5, pictureBox1.Location.Y + 5);
                    if ((pictureBox1.Location.Y + pictureBox1.Height) >= 200)
                        dB = 5;
                    if (((pictureBox1.Location.Y + pictureBox1.Height) > (panel2.Location.Y + 5 + panel2.Height / 2)
                        && (pictureBox1.Location.Y + pictureBox1.Height) <= (panel2.Location.Y + panel2.Height))
                        && (pictureBox1.Location.X + pictureBox1.Width) >= panel2.Location.X)
                        dB = 7;
                    if (((pictureBox1.Location.Y + pictureBox1.Height) <= (panel2.Location.Y + panel2.Height / 2 + 5)
                        && (pictureBox1.Location.Y + pictureBox1.Height) > (panel2.Location.Y + panel2.Height / 2 - 5))
                        && (pictureBox1.Location.X + pictureBox1.Width) >= panel2.Location.X)
                        dB = 4;
                }
                if (dB == 7)//Bola diagonal inferior izquierda
                {
                    pictureBox1.Location = new Point(pictureBox1.Location.X - 5, pictureBox1.Location.Y + 5);
                    if ((pictureBox1.Location.Y + pictureBox1.Height) >= 200)
                        dB = 8;
                    if (((pictureBox1.Location.Y + pictureBox1.Height) >= (panel1.Location.Y + panel1.Height / 2 - 5)
                       && (pictureBox1.Location.Y + pictureBox1.Height) < (panel1.Location.Y + panel1.Height / 2 + 5))
                       && ((pictureBox1.Location.X) <= (panel1.Location.X + panel1.Width) && (pictureBox1.Location.X) >= (panel1.Location.X)))
                        dB = 3;
                    if (((pictureBox1.Location.Y + pictureBox1.Height) <= (panel1.Location.Y + panel1.Height)
                        && (pictureBox1.Location.Y + pictureBox1.Height) > (panel1.Location.Y + panel1.Height / 2 + 5))
                        && ((pictureBox1.Location.X) <= (panel1.Location.X + panel1.Width) && (pictureBox1.Location.X) >= (panel1.Location.X)))
                        dB = 6;
                }
                if (dB == 8)//Bola diagonal superior izquierda
                {
                    pictureBox1.Location = new Point(pictureBox1.Location.X - 5, pictureBox1.Location.Y - 5);
                    if ((pictureBox1.Location.Y + pictureBox1.Height) <= 0)
                        dB = 7;
                    if (((pictureBox1.Location.Y + pictureBox1.Height) > (panel1.Location.Y + 5 + panel1.Height / 2)
                        && (pictureBox1.Location.Y + pictureBox1.Height) <= (panel1.Location.Y + panel1.Height))
                        && (pictureBox1.Location.X) >= (panel2.Location.X + panel2.Width))
                        dB = 7;
                    if (((pictureBox1.Location.Y + pictureBox1.Height) < (panel1.Location.Y + 5 + panel1.Height / 2)
                        && (pictureBox1.Location.Y + pictureBox1.Height) >= (panel1.Location.Y))
                        && ((pictureBox1.Location.X) <= (panel1.Location.X + panel1.Width) && (pictureBox1.Location.X) >= (panel1.Location.X)))
                        dB = 5;
                    if (((pictureBox1.Location.Y + pictureBox1.Height) <= (panel1.Location.Y+panel1.Height)
                        && (pictureBox1.Location.Y + pictureBox1.Height) > (panel1.Location.Y + panel1.Height/2 + 5))
                        && ((pictureBox1.Location.X) <= (panel1.Location.X + panel1.Width) && (pictureBox1.Location.X) >= (panel1.Location.X)))
                        dB = 6;
                    if (((pictureBox1.Location.Y + pictureBox1.Height) >= (panel1.Location.Y + panel1.Height/2 -5)
                       && (pictureBox1.Location.Y + pictureBox1.Height) < (panel1.Location.Y + panel1.Height / 2 + 5))
                       && ((pictureBox1.Location.X) <= (panel1.Location.X + panel1.Width) && (pictureBox1.Location.X) >= (panel1.Location.X)))
                        dB = 3;
                }
                Thread.Sleep(50);
                if (pictureBox1.Location.X + pictureBox1.Width > panel2.Location.X + panel2.Width)
                {
                    pictureBox1.Location = new Point(xB, yB);
                    dB = 6;
                    label1.Text = (Convert.ToInt32(label1.Text) + 1).ToString();
                }
                if (pictureBox1.Location.X  < panel1.Location.X-10)
                {
                    pictureBox1.Location = new Point(xB, yB);
                    dB = 6;
                    label2.Text = (Convert.ToInt32(label2.Text) + 1).ToString();
                }
                string s = "*" + pictureBox1.Location.X.ToString() + "," + pictureBox1.Location.Y.ToString();
                byte[] buff = Encoding.ASCII.GetBytes(s);
                udpServer.Send(buff,buff.Length,EPServer);
            }
            
        }

        private void Listen()
        {
            while(Convert.ToInt32(label1.Text)<10 && Convert.ToInt32(label1.Text) < 10)
            {
                byte[] buff = new byte[1024];
                buff = udpClient.Receive(ref EPClient);
                if (buff == null)
                    continue;
                string s = Encoding.ASCII.GetString(buff);
                if (s.ElementAt(0) == '*')
                {
                    s = s.Substring(1, s.Length - 1);
                    string[] split = s.Split(',');
                    int x, y;
                    x = Convert.ToInt32(split[0]);
                    y = Convert.ToInt32(split[1]);
                    pictureBox1.Location = new Point(x, y);
                }
                if (s.ElementAt(0) == 'p')
                {
                    s = s.Substring(1, s.Length - 1);
                    string[] split = s.Split(',');
                    int x, y;
                    x = Convert.ToInt32(split[0]);
                    y = Convert.ToInt32(split[1]);
                    panel1.Location = new Point(x, y);
                }
                if (s.ElementAt(0) == 'P')
                {
                    s = s.Substring(1, s.Length - 1);
                    string[] split = s.Split(',');
                    int x, y;
                    x = Convert.ToInt32(split[0]);
                    y = Convert.ToInt32(split[1]);
                    panel2.Location = new Point(x, y);
                }
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue =='W')
            {
                if (panel1.Location.Y - 5 >= 0)
                    panel1.Location = new Point(panel1.Location.X, panel1.Location.Y - 5);
                string s = "p" + panel1.Location.X.ToString() + "," + panel1.Location.Y.ToString();
                byte[] buff = Encoding.ASCII.GetBytes(s);
                udpServer.Send(buff, buff.Length, EPServer);
            }
            if (e.KeyValue == 'S')
            {
                if (((panel1.Location.Y + 5) + panel1.Height) <= this.Height)
                    panel1.Location = new Point(panel1.Location.X, panel1.Location.Y + 5);
                string s = "p" + panel1.Location.X.ToString() + "," + panel1.Location.Y.ToString();
                byte[] buff = Encoding.ASCII.GetBytes(s);
                udpServer.Send(buff, buff.Length, EPServer);
            }
            if (e.KeyValue=='O')
            {
                if (panel2.Location.Y - 5 >= 0)
                    panel2.Location = new Point(panel2.Location.X, panel2.Location.Y - 5);
                string s = "P" + panel2.Location.X.ToString() + "," + panel2.Location.Y.ToString();
                byte[] buff = Encoding.ASCII.GetBytes(s);
                udpServer.Send(buff, buff.Length, EPServer);
            }
            if (e.KeyValue=='L')
            {
                if (((panel2.Location.Y + 5) + panel2.Height) <= this.Height)
                    panel2.Location = new Point(panel2.Location.X, panel2.Location.Y + 5);
                string s = "P" + panel2.Location.X.ToString() + "," + panel2.Location.Y.ToString();
                byte[] buff = Encoding.ASCII.GetBytes(s);
                udpServer.Send(buff, buff.Length, EPServer);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            MoveBall.Abort();
            tListen.Abort();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                udpServer = new UdpClient();
                udpServer.JoinMulticastGroup(multicastAddress);
                MoveBall.Start();
            }
            else
            {
                udpClient = new UdpClient();
                udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                udpClient.Client.Bind(EPClient);
                udpClient.JoinMulticastGroup(multicastAddress);
                tListen.Start();
            }
        }
    }
}
