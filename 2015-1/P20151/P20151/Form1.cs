using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace P20151
{
    public partial class Form1 : Form
    {
        private Socket Server;
        private Socket EnviarServidor;
        private IPEndPoint EPServidor;
        private IPEndPoint EPCliente;
        private Socket Client;
        private PictureBox Pb;
        private bool Comenzar;
        private Thread Escucha;
        private Random r;
        public Form1()
        {
            InitializeComponent();
            pictureBox3.SendToBack();
            Comenzar = false;
            CheckForIllegalCrossThreadCalls = false;
            Escucha = new Thread(Listen);
            r = new Random();
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void panel1_Move(object sender, EventArgs e)
        {
            
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (Comenzar)
            {
                Pb.Location = new Point(e.X, e.Y);
                if (radioButton1.Checked)//servidor
                {
                    byte[] buff = new byte[1024];
                    string s = "";
                    s += "*" + e.X.ToString() + "," + e.Y.ToString();
                    buff = Encoding.ASCII.GetBytes(s);
                    EnviarServidor.Send(buff);
                }
                else
                {
                    byte[] buff = new byte[1024];
                    string s = "";
                    s += "*" + e.X.ToString() + "," + e.Y.ToString();
                    buff = Encoding.ASCII.GetBytes(s);
                    Client.Send(buff);
                }
            }
                
        }

        private void Listen()
        {
            while (label2.Text.CompareTo("5") != 0 && label4.Text.CompareTo("5") != 0)
            {
                byte[] buff = new byte[1024];
                string pos = "";
                if (radioButton1.Checked)
                    EnviarServidor.Receive(buff);
                else
                    Client.Receive(buff);
                if (buff != null)
                {
                    pos = Encoding.ASCII.GetString(buff);
                    if (pos.ElementAt(0) == '*')//Moviendo Mouse enemigo
                    {
                        pos = pos.Substring(1, pos.Length - 1);
                        string[] split = pos.Split(',');
                        if (radioButton1.Checked)//Muevo al cliente
                        {
                            pictureBox2.Location = new Point(Convert.ToInt32(split[0]), Convert.ToInt32(split[1]));
                            this.Refresh();
                        }
                        else//Muevo al servidor
                        {
                            pictureBox1.Location = new Point(Convert.ToInt32(split[0]), Convert.ToInt32(split[1]));
                            this.Refresh();
                        }
                    }
                    if (pos.ElementAt(0) == '+')
                    {
                        pos = pos.Substring(1, pos.Length - 1);
                        string[] split = pos.Split(',');
                        pictureBox3.Location = new Point(Convert.ToInt32(split[0]), Convert.ToInt32(split[1]));
                        this.Refresh();
                    }
                    if (pos.ElementAt(0) == 'P')
                    {
                        label2.Text = ((int)Convert.ToInt32(label4.Text) + 1).ToString();
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Comenzar = true;
            if (radioButton1.Checked)//Soy servidor
            {
                EPServidor = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1234);
                Server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                Server.Bind(EPServidor);
                Server.Listen(1);
                EnviarServidor = Server.Accept();
                MessageBox.Show("Cliente Conectado con Exito");
                Pb = pictureBox1;
                int x = r.Next(2, 428);
                int y = r.Next(0, 290);
                pictureBox3.Location = new Point(x, y);
                byte[] buff = new byte[1024];
                string s = "+" + x + "," + y;
                buff = Encoding.ASCII.GetBytes(s);
                EnviarServidor.Send(buff);
            }
            else
            {
                EPCliente = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1234);
                Client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                Client.Connect(EPCliente);
                Pb = pictureBox2;
            }
            Escucha.Start();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Escucha.Abort();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)//servidor
            {
                byte[] buff = new byte[1024];
                string s = "";
                s += "P";
                buff = Encoding.ASCII.GetBytes(s);
                label4.Text = ((int)Convert.ToInt32(label4.Text) + 1).ToString();
                EnviarServidor.Send(buff);
            }
            else
            {
                byte[] buff = new byte[1024];
                string s = "";
                s += "P";
                buff = Encoding.ASCII.GetBytes(s);
                label4.Text = ((int)Convert.ToInt32(label4.Text) + 1).ToString();
                Client.Send(buff);
            }
        }
    }
}
