using System.Net.Sockets;
using System.Net;
using System.Text;

namespace WinFormsApp2
{
    public partial class SyncService : Form
    {
        private IPEndPoint serverIPEndPoint;                    //IP��ַ�Ͷ˿�
        private int listenPort;                                 //�����˿�
        private Socket listenSocket;                            //�����׽���
        private Socket clientSocket;                            //�ͻ����׽���
        private Thread threadAccept;                            //�ȴ��ͻ��������߳�
        public SyncService()
        {
            InitializeComponent();
        }
        
        private void bListen_Click(object sender, EventArgs e)
        {
            listenPort = Int32.Parse(tBPort.Text);      //��������˿ں�
            serverIPEndPoint = new IPEndPoint(IPAddress.Any, listenPort);//ʵ������ַ�˿���
            listenSocket = new Socket(AddressFamily.InterNetwork,
            SocketType.Stream, ProtocolType.Tcp);       //ʵ����һ��TCP��ʽ�׽���
            listenSocket.Bind(serverIPEndPoint);        //���׽��ְ󶨵����ض˿�
            listenSocket.Listen(50);                    //���׽�������Ϊ����״̬
            threadAccept = new Thread(new ThreadStart(AcceptThread));//�ȴ��ͻ��������߳�
            threadAccept.Start();                       //�������������߳�
        }

        private void AcceptThread()
        {
            clientSocket = listenSocket.Accept();//ͬ���ȴ��ͻ������ӣ������µ��׽�����ͻ��˽���ͨ��
            if (clientSocket.Connected)                      //�����ͻ������ӳɹ�
            {
                while (true)
                {
                    Byte[] receiveByte = new Byte[1024];      //��Ž��յ���Ϣ�Ļ���
                    clientSocket.Receive(receiveByte, receiveByte.Length, 0);
                    //ͬ�����տͻ�����Ϣ
                    //���ͻ�����Ϣת��Ϊ�ַ���
                    string receiveString = Encoding.Default.GetString(receiveByte);
                    ShowMessage(receiveString);               //������յ���Ϣ
                }
            }
        }

        //delegate void ShowMessageCallback(string message);  //��ʾ���յ���Ϣί��
                                                            //��ʾ���յ���Ϣ
        void ShowMessage(string message)
        {
            if (this.InvokeRequired) this.Invoke(ShowMessage, new
              object[] { message });
            else tBReceive.AppendText(message + "\n");
        }
    }
}