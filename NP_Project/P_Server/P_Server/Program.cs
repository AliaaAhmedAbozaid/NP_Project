using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace P_Server
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
/*
 // Setup file names
string fileName = "binarystreams.exe", // this source file
fileNameCopy = "fileCopy.cs";
// Does this source file exist?
Console.WriteLine("{0} does {1} exist",
fileName, File.Exists(fileName) ? "" : "not");
// Copy this source file
File.Copy(fileName, fileNameCopy);
// Does the copy exist?
Console.WriteLine("{0} does {1} exist", fileNameCopy,
File.Exists(fileNameCopy) ? "" : "not");
// Delete the copy again
Console.WriteLine("Deleting {0}", fileNameCopy);
File.Delete(fileNameCopy);
// Does the deleted file exist
Console.WriteLine("{0} does {1} exist", fileNameCopy,
File.Exists(fileNameCopy) ? "" : "not");
// Read all lines in source file and echo 
// one of them to the console
string[] lines = File.ReadAllLines(fileName);
Console.WriteLine("Line {0}: {1}", 6, lines[6]); }}
___________
string fileName = "directory-info.cs"; // The current source file
// Get the DirectoryInfo of the current directory
// from the FileInfo of the current source file 
FileInfo fi = new FileInfo(fileName); // This source file
DirectoryInfo di = fi.Directory;
Console.WriteLine("File {0} is in directory \n {1}", fi, di);
// Get the files and directories in the parent directory.
FileInfo[] files = di.Parent.GetFiles();
DirectoryInfo[] dirs = di.Parent.GetDirectories();
// Show the name of files and directories on the console
Console.WriteLine("\nListing directory {0}:", di.Parent.Name);
foreach (DirectoryInfo d in dirs)
Console.WriteLine(d.Name);
foreach (FileInfo f in files)
Console.WriteLine(f.Name);
__________
string fileName = "binarystreams.exe"; // The current source file
FileInfo fi = new FileInfo(fileName); // This source file
string thisFile = fi.FullName,
thisDir = Directory.GetParent(thisFile).FullName,
parentDir = Directory.GetParent(thisDir).FullName;
Console.WriteLine("This file: {0}", thisFile);
Console.WriteLine("This Directory: {0}", thisDir);
Console.WriteLine("Parent directory: {0}", parentDir);
string[] files = Directory.GetFiles(parentDir);
string[] dirs = Directory.GetDirectories(parentDir);
Console.WriteLine("\nListing directory {0}:", parentDir);
foreach (string d in dirs)
Console.WriteLine(d);
foreach (string f in files)
Console.WriteLine(f); } }
++++++++++++++++++++++++++++++++
[Serializable]
public class ClassToSerialize
{
 public int age = 100;

[NonSerialized]
 public string name = "bipin";
}
_________
public void SerializeNow()
 {
 ClassToSerialize c = new ClassToSerialize();
 
 Stream s = File.Create("temp.dat");
 BinaryFormatter b = new BinaryFormatter();
 b.Serialize(s, c);
 s.Close();
 }
 public void DeSerializeNow()
 {
 ClassToSerialize c = new ClassToSerialize();
 
 Stream s = File.OpenRead("temp.dat");
 BinaryFormatter b = new BinaryFormatter();
c = (ClassToSerialize)b.Deserialize(s);
 Console.WriteLine(c.name);
 s.Close();
 }
+++++++++++++
Thread thread1 = new Thread(new
ThreadStart(DisplayThread1));
 Thread thread2 = new Thread(new
ThreadStart(DisplayThread2));
 // start them 
 thread1.Start();
 thread2.Start();}
+++++++++++++++
void DisplayThread2() {
 while (_stopThreads == false)
 {
 lock (this) {
 Console.WriteLine("Display Thread 2");
 _threadOutput = "Hello Thread2";
 Thread.Sleep(1000);  
Console.WriteLine("Thread 2 Output --> {0}", _threadOutput);
 }  
 } 
____________
AutoResetEvent _blockThread1 = new AutoResetEvent(false);
 AutoResetEvent _blockThread2 = new AutoResetEvent(true);
+++++++++++++++++
ns.Close();
server.Shutdown(SocketShutdown.Both);
server.Close(); 
+++++++++++++++++++=UDP server
int recv;
byte[] data = new byte[1024];
IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 9050);
Socket newsock = new Socket(AddressFamily.InterNetwork,
SocketType.Dgram, ProtocolType.Udp);
newsock.Bind(ipep);
Console.WriteLine("Waiting for a client...");
IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
EndPoint Remote = (EndPoint)(sender);
recv = newsock.ReceiveFrom(data, ref Remote);
Console.WriteLine("Message received from {0}:", 
Remote.ToString());
Console.WriteLine(Encoding.ASCII.GetString(data, 0, 
recv));
string welcome = "Welcome to my test server";
data = Encoding.ASCII.GetBytes(welcome);
newsock.SendTo(data, data.Length, SocketFlags.None, 
Remote);
while (true)
{
data = new byte[1024];
recv = newsock.ReceiveFrom(data, ref Remote);
Console.WriteLine(Encoding.ASCII.GetString(data, 0, 
recv));
newsock.SendTo(data, recv, SocketFlags.None, Remote);
}
}
__________________________UDB client
byte[] data = new byte[1024];
string input, stringData;
IPEndPoint ipep = new IPEndPoint(
IPAddress.Parse("127.0.0.1"), 9050);
Socket server = new Socket(AddressFamily.InterNetwork,
SocketType.Dgram, ProtocolType.Udp);
string welcome = "Hello, are you there?";
data = Encoding.ASCII.GetBytes(welcome);
server.SendTo(data, data.Length, SocketFlags.None, ipep);
IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
EndPoint Remote = (EndPoint)sender;
data = new byte[1024];
int recv = server.ReceiveFrom(data, ref Remote);
Console.WriteLine("Message received from {0}:", 
Remote.ToString());
Console.WriteLine(Encoding.ASCII.GetString(data, 0, recv));
while (true)
{
input = Console.ReadLine();
if (input == "exit")
break;
server.SendTo(Encoding.ASCII.GetBytes(input), Remote);
data = new byte[1024];
recv = server.ReceiveFrom(data, ref Remote);
stringData = Encoding.ASCII.GetString(data, 0, recv);
Console.WriteLine(stringData);
}
Console.WriteLine("Stopping client");
server.Close();
}
}

+++++++++++++++++++++++++
private void CompressBinaryFile_Click(object sender, EventArgs e)
        {
            if (fileDialog.ShowDialog(this) == DialogResult.OK)
            {
                FileStream Dest = new FileStream(fileDialog.FileName +
                ".QZip.BCompressed", FileMode.Create, FileAccess.Write);
                GZipStream gz = new GZipStream(Dest, CompressionMode.Compress);
                BinaryWriter sr = new BinaryWriter(gz);
                FileStream fs = new FileStream(fileDialog.FileName,
                FileMode.Open, FileAccess.Read);
                BinaryReader dd = new BinaryReader(fs);
                byte[] _da = dd.ReadBytes((int)dd.BaseStream.Length);
                progressBar1.Maximum = (int)_da.Length;
                progressBar1.Value = (int)_da.Length;
                sr.Write(_da); sr.Flush(); sr.Close();
                progressBar2.Maximum = progressBar1.Maximum;
                progressBar2.Value = (int.Parse(new
                FileStream(fileDialog.FileName + ".QZip.BCompressed",
                FileMode.Open, FileAccess.Read).Length.ToString()));
            }
        }
++++++++++++++++++++++++++
private void RestoreCompressedImageToPictureBox_Click(object sender, EventArgs e)
        {
            if (fileDialog.ShowDialog(this) == DialogResult.OK)
            {
                FileStream fs = new FileStream(fileDialog.FileName,
                FileMode.Open, FileAccess.Read);
                GZipStream gz = new GZipStream(fs,
                CompressionMode.Decompress);
                FileStream fs_out =
                new FileStream(fileDialog.FileName +
                ".DECOMPRESSED", FileMode.Create, FileAccess.Write);
                var decompressed = new MemoryStream();
                gz.CopyTo(decompressed);
                BinaryWriter bw = new BinaryWriter(fs_out);
                bw.Write(decompressed.ToArray(), 0, (int)decompressed.Length);
                bw.Close();
                try
                {
                    pictureBox1.Image =
                    Image.FromFile(fileDialog.FileName + ".DECOMPRESSED");
                }
                catch { }
            }
+++++++++++++++++++++++++
//Compress Text File
        private void CompressTextFile_Click(object sender, EventArgs e)
        {
                if (fileDialog.ShowDialog(this) == DialogResult.OK)
                {
                    GZipStream gz = new GZipStream(
                    new FileStream(fileDialog.FileName + ".QZip.Compressed",
                    FileMode.Create, FileAccess.Write), CompressionMode.Compress);
                    FileStream fs = new FileStream(fileDialog.FileName, FileMode.Open,
                    FileAccess.Read);
                    progressBar1.Text = fs.Length.ToString();
                    progressBar1.Maximum = (int)fs.Length;
                    progressBar1.Value = (int)fs.Length;
                    StreamWriter sw = new StreamWriter(gz);
                    sw.Write(new StreamReader(fs).ReadToEnd());
                    sw.Flush(); sw.Close();
                    progressBar2.Text = new FileStream(fileDialog.FileName +
                    ".QZip.Compressed", FileMode.Open, FileAccess.Read).Length.ToString();
                    progressBar2.Maximum = (int)fs.Length;
                    progressBar2.Value = (int.Parse(progressBar2.Text));
                }
        }
++++++++++++++++++++++
//Restore File 

        private void button2_Click(object sender, EventArgs e)
        {
            if (fileDialog.ShowDialog(this) == DialogResult.OK)
            {
                GZipStream gz = new GZipStream(new
                FileStream(fileDialog.FileName, FileMode.Open, FileAccess.Read),
                CompressionMode.Decompress);
                StreamReader sr = new StreamReader(gz);
                FileStream fs = new FileStream(fileDialog.FileName
                + ".Decompressed", FileMode.Create, FileAccess.Write);
                StreamWriter ff = new StreamWriter(fs);
                ff.Write(sr.ReadToEnd());
                ff.Flush();
                ff.Close();
            }

        }

+++++++++++++++++++++++++++++++++++++++++++++++++
 private void CompressBinaryFile_Click(object sender, EventArgs e)
        {
            if (fileDialog.ShowDialog(this) == DialogResult.OK)
            {
                FileStream Dest = new FileStream(fileDialog.FileName +
                ".QZip.BCompressed", FileMode.Create, FileAccess.Write);
                GZipStream gz = new GZipStream(Dest, CompressionMode.Compress);
                BinaryWriter sr = new BinaryWriter(gz);
                FileStream fs = new FileStream(fileDialog.FileName,
                FileMode.Open, FileAccess.Read);
                BinaryReader dd = new BinaryReader(fs);
                byte[] _da = dd.ReadBytes((int)dd.BaseStream.Length);
                progressBar1.Maximum = (int)_da.Length;
                progressBar1.Value = (int)_da.Length;
                sr.Write(_da); sr.Flush(); sr.Close();
                progressBar2.Maximum = progressBar1.Maximum;
                progressBar2.Value = (int.Parse(new
                FileStream(fileDialog.FileName + ".QZip.BCompressed",
                FileMode.Open, FileAccess.Read).Length.ToString()));
            }
        }

++++++++++++++++++++++++++++++++++++++++++++++++++++++++
private void RestoreCompressedImageToPictureBox_Click(object sender, EventArgs e)
        {
            if (fileDialog.ShowDialog(this) == DialogResult.OK)
            {
                FileStream fs = new FileStream(fileDialog.FileName,
                FileMode.Open, FileAccess.Read);
                GZipStream gz = new GZipStream(fs,
                CompressionMode.Decompress);
                FileStream fs_out =
                new FileStream(fileDialog.FileName +
                ".DECOMPRESSED", FileMode.Create, FileAccess.Write);
                var decompressed = new MemoryStream();
                gz.CopyTo(decompressed);
                BinaryWriter bw = new BinaryWriter(fs_out);
                bw.Write(decompressed.ToArray(), 0, (int)decompressed.Length);
                bw.Close();
                try
                {
                    pictureBox1.Image =
                    Image.FromFile(fileDialog.FileName + ".DECOMPRESSED");
                }
                catch { }
            }
 */