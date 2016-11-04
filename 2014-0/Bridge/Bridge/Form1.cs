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

namespace Bridge
{
    public partial class Form1 : Form
    {
        private List<PictureBox> Fichas;
        private Socket Server;
        private Socket Client;
        private IPEndPoint EPServer;
        private IPEndPoint EPClient;
        private Socket ListenServer;
        private Thread tListen;
        public Form1()
        {
            InitializeComponent();
            Fichas = new List<PictureBox>();
            Fichas.Add(pictureBox1);
            Fichas.Add(pictureBox2);
            Fichas.Add(pictureBox3);
            Fichas.Add(pictureBox4);
            Fichas.Add(pictureBox5);
            Fichas.Add(pictureBox6);
            Fichas.Add(pictureBox7);
            Fichas.Add(pictureBox8);
            Fichas.Add(pictureBox9);
            Fichas.Add(pictureBox10);
            Fichas.Add(pictureBox11);
            Fichas.Add(pictureBox12);
            Fichas.Add(pictureBox13);
            Fichas.Add(pictureBox14);
            Fichas.Add(pictureBox15);
            Fichas.Add(pictureBox16);
            Fichas.Add(pictureBox17);
            Fichas.Add(pictureBox18);
            Fichas.Add(pictureBox19);
            Fichas.Add(pictureBox20);
            Fichas.Add(pictureBox21);
            Fichas.Add(pictureBox22);
            Fichas.Add(pictureBox23);
            Fichas.Add(pictureBox24);
            Fichas.Add(pictureBox25);
            NewGame();
            EPServer = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1234);
            EPClient = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1234);
            tListen = new Thread(Listen);
        }
        
        private void NewGame()
        {
            for(int i = 0; i < Fichas.Count; i++)
            {
                Fichas.ElementAt(i).Image = Bridge.Properties.Resources.brown;
                Fichas.ElementAt(i).Tag = 0;
            }  
        }

        private bool Ganar(int Player)
        {
            int c;
            if (Player == 1)
            {
                c = 0;
                for(int i = 0; i < Fichas.Count; i++)
                {
                    if (((int)Fichas.ElementAt(i).Tag) == 1)
                    {
                        c++;
                    }
                    if (c == 5)
                        return true;
                    if(i%4==0 && i != 0)
                    {
                        c = 0;
                    }
                }
            }
            else
            {
                if (((int)pictureBox1.Tag) == 2 && ((int)pictureBox6.Tag) == 2 && ((int)pictureBox11.Tag) == 2 && ((int)pictureBox16.Tag) == 2
                    && ((int)pictureBox21.Tag) == 2)
                    return true;
                if (((int)pictureBox2.Tag) == 2 && ((int)pictureBox7.Tag) == 2 && ((int)pictureBox12.Tag) == 2 && ((int)pictureBox17.Tag) == 2
                    && ((int)pictureBox22.Tag) == 2)
                    return true;
                if (((int)pictureBox3.Tag) == 2 && ((int)pictureBox8.Tag) == 2 && ((int)pictureBox13.Tag) == 2 && ((int)pictureBox18.Tag) == 2
                    && ((int)pictureBox23.Tag) == 2)
                    return true;
                if (((int)pictureBox4.Tag) == 2 && ((int)pictureBox9.Tag) == 2 && ((int)pictureBox14.Tag) == 2 && ((int)pictureBox19.Tag) == 2
                    && ((int)pictureBox24.Tag) == 2)
                    return true;
                if (((int)pictureBox5.Tag) == 2 && ((int)pictureBox10.Tag) == 2 && ((int)pictureBox15.Tag) == 2 && ((int)pictureBox20.Tag) == 2
                    && ((int)pictureBox25.Tag) == 2)
                    return true;
            }
            return false;
        }

        private void Listen()
        {
            while(!Ganar(1) && !Ganar(2))
            {
                byte[] buff = new byte[1024];
                if (radioButton1.Checked)
                    ListenServer.Receive(buff);
                else
                    Client.Receive(buff);
                string s = "";
                if (buff != null)
                {
                    s = Encoding.ASCII.GetString(buff);
                    if (s.ElementAt(0) == '*')//Pinto Enemigo
                    {
                        s = s.Substring(1, s.Length - 1);
                        int i = Convert.ToInt32(s);
                        if (radioButton1.Checked)
                        {
                            Fichas.ElementAt(i).Image = Bridge.Properties.Resources.RED;
                            Fichas.ElementAt(i).Tag = 2;
                        }
                        else
                        {
                            Fichas.ElementAt(i).Image = Bridge.Properties.Resources.Blue;
                            Fichas.ElementAt(i).Tag = 1;
                        }      
                    }
                }
            }
        }

        private void Clickear(PictureBox PB, int i)
        {
            if (((int)PB.Tag) != 0)
                return;
            if (radioButton1.Checked)
            {
                PB.Image = Bridge.Properties.Resources.Blue;
                PB.Tag = 1;
                string s = "*" + i.ToString();
                byte[] buff = Encoding.ASCII.GetBytes(s);
                ListenServer.Send(buff);
            }
            else
            {
                PB.Image = Bridge.Properties.Resources.RED;
                PB.Tag = 2;
                string s = "*" + i.ToString();
                byte[] buff = Encoding.ASCII.GetBytes(s);
                Client.Send(buff);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Clickear(pictureBox1,0);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Clickear(pictureBox2,1);
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Clickear(pictureBox3,2);
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Clickear(pictureBox4,3);
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            Clickear(pictureBox5,4);
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            Clickear(pictureBox6,5);
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            Clickear(pictureBox7,6);
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            Clickear(pictureBox8,7);
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            Clickear(pictureBox9,8);
        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            Clickear(pictureBox10,9);
        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {
            Clickear(pictureBox11,10);
        }

        private void pictureBox12_Click(object sender, EventArgs e)
        {
            Clickear(pictureBox12,11);
        }

        private void pictureBox13_Click(object sender, EventArgs e)
        {
            Clickear(pictureBox13,12);
        }

        private void pictureBox14_Click(object sender, EventArgs e)
        {
            Clickear(pictureBox14,13);
        }

        private void pictureBox15_Click(object sender, EventArgs e)
        {
            Clickear(pictureBox15,14);
        }

        private void pictureBox16_Click(object sender, EventArgs e)
        {
            Clickear(pictureBox16,15);
        }

        private void pictureBox17_Click(object sender, EventArgs e)
        {
            Clickear(pictureBox17,16);
        }

        private void pictureBox18_Click(object sender, EventArgs e)
        {
            Clickear(pictureBox18,17);
        }

        private void pictureBox19_Click(object sender, EventArgs e)
        {
            Clickear(pictureBox19,18);
        }

        private void pictureBox20_Click(object sender, EventArgs e)
        {
            Clickear(pictureBox20,19);
        }

        private void pictureBox21_Click(object sender, EventArgs e)
        {
            Clickear(pictureBox21,20);
        }

        private void pictureBox22_Click(object sender, EventArgs e)
        {
            Clickear(pictureBox22,21);
        }

        private void pictureBox23_Click(object sender, EventArgs e)
        {
            Clickear(pictureBox23,22);
        }

        private void pictureBox24_Click(object sender, EventArgs e)
        {
            Clickear(pictureBox24,23);
        }

        private void pictureBox25_Click(object sender, EventArgs e)
        {
            Clickear(pictureBox25,24);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                Server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                Server.Bind(EPServer);
                Server.Listen(1);
                ListenServer = Server.Accept();
                MessageBox.Show("Cliente Conectado con Exito");
            }
            else
            {
                Client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                Client.Connect(EPClient);
            }
            tListen.Start();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            tListen.Abort();
        }
    }
}
