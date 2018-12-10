using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;


namespace ConsoleServer
{
    class ServerProgram
    {
        const int port = 8888;
        static TcpListener listener;

        static void Main(string[] args)
        {
            

            try
            {
                listener = new TcpListener(IPAddress.Parse("127.0.0.1"), port);
                listener.Start();
                Console.WriteLine("Ожидание подключений...");

                while (true)
                {
                    TcpClient client = listener.AcceptTcpClient();
                     ClientObject clientObject = new ClientObject(client);

                    // создаем новый поток для обслуживания нового клиента
                    Thread clientThread = new Thread(new ThreadStart(clientObject.Process));
                    clientThread.Start();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (listener != null)
                    listener.Stop();
            }
        }
        
    }
}




//using System;
//using System.Net.Sockets;
//using System.Text;
//using System.Net;
//using System.Threading;

//namespace ConsoleServer
//{

//    class ServerProgram
//    {
//        const int port = 8888;
//        static TcpListener listener;
//        static void Main(string[] args)
//        {
//            try
//            {
//                listener = new TcpListener(IPAddress.Parse("127.0.0.1"), port);
//                listener.Start();
//                Console.WriteLine("Ожидание подключений...");

//                while (true)
//                {
//                    TcpClient client = listener.AcceptTcpClient();
//                    ClientObject clientObject = new ClientObject(client);

//                    // создаем новый поток для обслуживания нового клиента
//                    Thread clientThread = new Thread(new ThreadStart(clientObject.Process));
//                    clientThread.Start();
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine(ex.Message);
//            }
//            finally
//            {
//                if (listener != null)
//                    listener.Stop();
//            }
//        }
//    }

//    public class ClientObject
//    {
//        public TcpClient client;
//        public ClientObject(TcpClient tcpClient)
//        {
//            client = tcpClient;
//        }

//        public void Process()
//        {
//            NetworkStream stream = null;
//            try
//            {
//                stream = client.GetStream();
//                byte[] data = new byte[64]; // буфер для получаемых данных
//                while (true)
//                {
//                    // получаем сообщение
//                    StringBuilder builder = new StringBuilder();
//                    int bytes = 0;
//                    do
//                    {
//                        bytes = stream.Read(data, 0, data.Length);
//                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
//                    }
//                    while (stream.DataAvailable);

//                    string message = builder.ToString();

//                    Console.WriteLine(message);
//                    // отправляем обратно сообщение в верхнем регистре
//                    message = message.Substring(message.IndexOf(':') + 1).Trim().ToUpper();
//                    data = Encoding.Unicode.GetBytes(message);
//                    stream.Write(data, 0, data.Length);
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine(ex.Message);
//            }
//            finally
//            {
//                if (stream != null)
//                    stream.Close();
//                if (client != null)
//                    client.Close();
//            }
//        }
//    }
//}

////using System;
////using System.Text;
////using System.Net;
////using System.Net.Sockets;

////namespace SocketTcpServer
////{
////    class ServerProgram
////    {
////        static int port = 8005; // порт для приема входящих запросов
////        static void Main(string[] args)
////        {
////            // получаем адреса для запуска сокета
////            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);

////            // создаем сокет
////            Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
////            try
////            {
////                // связываем сокет с локальной точкой, по которой будем принимать данные
////                listenSocket.Bind(ipPoint);

////                // начинаем прослушивание
////                listenSocket.Listen(10);

////                Console.WriteLine("Сервер запущен. Ожидание подключений...");

////                while (true)
////                {
////                    Socket handler = listenSocket.Accept();
////                    // получаем сообщение
////                    StringBuilder builder = new StringBuilder();
////                    int bytes = 0; // количество полученных байтов
////                    byte[] data = new byte[256]; // буфер для получаемых данных

////                    do
////                    {
////                        bytes = handler.Receive(data);
////                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
////                    }
////                    while (handler.Available > 0);

////                    Console.WriteLine(DateTime.Now.ToShortTimeString() + ": " + builder.ToString());

////                    // отправляем ответ
////                    string message = "ваше сообщение доставлено";
////                    data = Encoding.Unicode.GetBytes(message);
////                    handler.Send(data);
////                    // закрываем сокет
////                    handler.Shutdown(SocketShutdown.Both);
////                    handler.Close();
////                }
////            }
////            catch (Exception ex)
////            {
////                Console.WriteLine(ex.Message);
////            }
////        }
////    }
////}