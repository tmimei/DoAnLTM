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

namespace Client_Server
{
    public partial class Client : Form
    {
        public Client()
        {
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            IPEndPoint ipe = new IPEndPoint(IPAddress.Parse(tbIP.Text), int.Parse(tbPort.Text)); // địa chỉ server;
            Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            if (btnConnect.Text == "Connect")
            {
                CheckForIllegalCrossThreadCalls = false;
                if (tbName.Text != "" && tbIP.Text != "" && tbPort.Text != "")
                {
                    try
                    {
                        client.Connect(ipe);
                    }
                    catch
                    {
                        MessageBox.Show("Kết nối với Server không thành công", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    btnConnect.Text = "Disconnect";
                    tbIP.ReadOnly = true;
                    tbPort.ReadOnly = true;
                    tbName.ReadOnly = true;
                }
                else MessageBox.Show("Vui lòng nhập đầy đủ IP, Port Server và Name", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else //btnConnect.Text = "Disconnect"
            {
                tbIP.Text = string.Empty;
                tbPort.Text = string.Empty;
                tbName.Text = string.Empty;
                listMess.Items.Clear();
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            //IPEndPoint ipe = new IPEndPoint(IPAddress.Parse(tbIP.Text), int.Parse(tbPort.Text));
            Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            string mess = tbName.Text + ": " + tbPath.Text; // format = name: path (đường dẫn)
            byte[] data = Encoding.UTF8.GetBytes(mess);
            client.Send(data);
        }

        void receive()
        {
            Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            byte[] data = new byte[1024 * 5000];
            int receivedBytes = client.Receive(data);
            string str = UTF32Encoding.UTF32.GetString(data, 0, receivedBytes);
            listMess.Items.Add(str);
        }
    }
}
