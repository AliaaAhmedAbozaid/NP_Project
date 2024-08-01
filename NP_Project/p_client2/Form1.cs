using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace p_client2
{
    public partial class Form1 : Form
    {
        Socket server;
        NetworkStream ns;
        StreamReader sr;
        StreamWriter sw;
        string selectedImagePath;
        
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            IPAddress host = IPAddress.Parse("127.0.0.1");
            IPEndPoint hostep = new IPEndPoint(host, 9050);
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            await Task.Run(() => server.Connect(hostep));
            textBox1.Text += "Client:Socket connected to " + server.RemoteEndPoint?.ToString() + "\n";
            ns = new NetworkStream(server);
            sr = new StreamReader(ns);
            sw = new StreamWriter(ns);
            string data = await Task.Run(() => sr.ReadLine());
            textBox1.Text += "Server:" + data + "\n";
            while (true)
            {

                string s = await sr.ReadLineAsync();

                if (s == "mess")
                {
                    string content = await sr.ReadLineAsync();
                    textBox1.Text += "server : " + content + "\n";
                }
                else if (s == "img")
                {


                    byte[] sz = new byte[4];
                    await ns.ReadAsync(sz, 0, sz.Length);

                    int n = BitConverter.ToInt32(sz, 0);
                    int i = 0;
                    byte[] data2 = new byte[n];

                    while (i < n)
                    {

                        int c = await ns.ReadAsync(data2, i, n - i);
                        i += c;
                    }
                    // pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;

                    using (MemoryStream ms = new MemoryStream(data2))
                    {
                        pictureBox1.Image = Image.FromStream(ms);
                    }

                }
                else if(s=="dir"||s=="fil")
                {
                    while(true)
                    {
                        string content = await sr.ReadLineAsync();
                        textBox1.Text += "server : " + content + "\n";
                        textBox1.Text += Environment.NewLine;
                    }
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ns.Close();
            server.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            sw.WriteLine(textBox2.Text);
            sw.Flush();
            textBox1.Text += "Client: " + textBox2.Text + "\n";
            textBox2.Text = string.Empty;
        }

        private async void button3_Click(object sender, EventArgs e)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
           
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string selectedFilePath = openFileDialog.FileName;

                if (IsImageFile(selectedFilePath))
                {
                    //Console.WriteLine("DialogResult.OK");
                    
                    sw.WriteLine("IMG " + selectedFilePath);
                    sw.Flush ();    
                                 }
                else
                {
                    using (FileStream fs = new FileStream(selectedFilePath, FileMode.Open,FileAccess.Read))
                    {
                        using (StreamReader sr = new StreamReader(fs))
                        {
                            textBox1.Text += "content of file from server to client"+sr.ReadToEnd();
                        }
                    }
                    sw.WriteLine("FIL " + selectedFilePath);
                    sw.Flush();
                }
            }

        }

        private bool IsImageFile(string filePath)
        {
            // Define a list of valid image file extensions
            string[] imageExtensions = { ".jpg", ".jpeg", ".png", ".bmp", ".gif", ".tiff" };
            string fileExtension = Path.GetExtension(filePath).ToLower();

            foreach (string extension in imageExtensions)
            {
                if (fileExtension == extension)
                {
                    return true;
                }
            }
            return false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
          
            string selectedPath = "";

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                selectedPath = folderBrowserDialog.SelectedPath;
                    // Handle case where user only selected a directory
                    sw.WriteLine("DIR " + selectedPath);
                    //textBox3.Text = " ";
                   
                
            }

            sw.Flush();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
