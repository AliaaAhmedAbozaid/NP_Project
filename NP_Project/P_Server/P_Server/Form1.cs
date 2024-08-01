using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace P_Server
{
    public partial class Form1 : Form
    {
        Socket server;
        Socket client;
        NetworkStream ns;
        StreamReader sr;
        StreamWriter sw;
        string path;
        private string selectedPath;
        byte[] dataimg = new byte[1024];
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {

            // Create socket and strt listening
            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9050);
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            server.Bind(ipep);
            server.Listen(10);
            textBox1.Text += "Server: I'm Listening on 127.0.0.1:9050\n";

            // Accept alient connection asyncronusly
            client = await Task.Run(() => server.Accept());
            IPEndPoint clientep = client.RemoteEndPoint as IPEndPoint ?? throw new Exception("Couldn't extract the ip endpoint for the client");
            textBox1.Text += "Client: I'm Connected, my ipep = " + clientep.ToString() + "\n";

            // Connect the ns 
            ns = new NetworkStream(client);
            sr = new StreamReader(ns);
            sw = new StreamWriter(ns);
            // Send Welcome Message
            string welcome = "Welcome to my test server!";
            sw.WriteLine(welcome);
            sw.Flush();

            // Recieve messages from the client
            while (true)
            {
                string data = await Task.Run(() => sr.ReadLine());
                if (data.StartsWith("DIR"))
                {
                     path = data.Substring(4);
                    textBox1.Text += path;
                    DirectoryInfo d = new DirectoryInfo(path);
                    string name = d.FullName;
                    string[] dirs = Directory.GetDirectories(name);
                    string[] fils = Directory.GetFiles(name);
                    sw.WriteLine("dir");
                    sw.Flush ();
                    foreach (string dd in dirs)
                    {
                        DirectoryInfo dx = new DirectoryInfo(dd);

                        sw.WriteLine(dx.Name);
                        
                    }
                    sw.Flush();
                    foreach (string f in fils)
                    {

                        FileInfo dx = new FileInfo(f);
                        sw.WriteLine(dx.Name);
                    }
                    sw.Flush();
                }
                 
                else if(data.StartsWith("FIL"))
                {
                    path = data.Substring(4);
                    textBox1.Text += path;
                    FileInfo dx = new FileInfo(path);
                    FileStream Dest = new FileStream(dx.Name+".QZip.BCompressed", FileMode.Create, FileAccess.Write);
                    GZipStream gz = new GZipStream(Dest, CompressionMode.Compress);
                    FileStream fs = new FileStream(dx.Name, FileMode.Open, FileAccess.Read);
                    BinaryWriter sr = new BinaryWriter(gz);
                    BinaryReader dd = new BinaryReader(fs);
                    byte[] _da = dd.ReadBytes((int)dd.BaseStream.Length);
                    sr.Write(_da); sr.Flush(); sr.Close();
                    sw.WriteLine(gz);
                  
                    //FileStream fs = new FileStream("asd2.txt", FileMode.Create, FileAccess.Write);
                    //FileStream fd = new FileStream(path, FileMode.Open, FileAccess.Read);
                    //StreamWriter writer = new StreamWriter(fs);
                    //StreamReader reader = new StreamReader(fd);
                    //writer.Write(reader.ReadToEnd());
                    //writer.Flush();
                }
                else if (data.StartsWith("IMG"))

                {
                    path = data.Substring(4);
                    textBox1.Text += path;

                        sw.WriteLine("img");
                        sw.Flush();
                        byte[] img = File.ReadAllBytes(path);
                        byte[] size = BitConverter.GetBytes(img.Length);
                        ns.Write(size, 0, size.Length); 
                        ns.Flush();
                        ns.Write(img, 0, img.Length); 
                        ns.Flush();

                }
                else
                {
                    textBox1.Text += "Client: " + data + "\n";
                    textBox1.Text += Environment.NewLine;

                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            sw.Close();
            sr.Close();
            ns.Close();
            client.Close();
            server.Close();
        }

       
        private void button2_Click(object sender, EventArgs e)
        {
            sw.WriteLine("mess");
            sw.Flush();
            sw.WriteLine(textBox2.Text);
            sw.Flush();
            textBox1.Text += "Server: " + textBox2.Text + "\n";
            textBox2.Text = string.Empty;
        }

    }
}
