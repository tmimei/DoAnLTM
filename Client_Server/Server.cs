using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Threading;
using System.IO;

namespace Client_Server
{
    public partial class Server : Form
    {
        public Server()
        {
            InitializeComponent();
        }

        IPEndPoint ipe = new IPEndPoint(IPAddress.Any, 8080);
        Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        List<Socket> clientlist = new List<Socket>();

        private void Server_Load(object sender, EventArgs e)
        {
            listMess.Items.Add("Server is ready...");
            connect();
        }

        void connect() // hàm dùng để kết nối với các client
        {
            server.Bind(ipe);
            Thread listen = new Thread(() =>
            {
                try
                {
                    while (true)
                    {
                        server.Listen(100);
                        Socket client = server.Accept();
                        //listView1.Items.Add("Client is connected");
                        clientlist.Add(client);
                        string str = "Client mới kết nối từ: " + client.RemoteEndPoint.ToString() + "\n";
                        listMess.Items.Add(new ListViewItem(str));
                        Thread recieve_thr = new Thread(recieve);
                        recieve_thr.Start(client);
                    }
                }
                catch
                {
                    IPEndPoint ipe = new IPEndPoint(IPAddress.Any, 8080);
                    Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                }

            });
            listen.Start();
        }

        void recieve(object obj)
        {
            Socket client = obj as Socket;
            Byte[] data = new byte[1024 * 5000];
            int receivedBytes = client.Receive(data);
                
            string str = Encoding.ASCII.GetString(data, 0, receivedBytes);
            string content = client.RemoteEndPoint.ToString() + ": " + str;
            listMess.Items.Add(new ListViewItem(content));

            if (!File.Exists(str))
            {
                string reply = "Server không tồn tại file có đường dẫn: " + str;
                byte[] data2 = Encoding.UTF8.GetBytes(reply);
                server.Send(data2);
            }
            else
            {
                server.SendFile(str);
            }
        }
    }
}
