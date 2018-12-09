using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.Net.Sockets;
using System.Text;
using ConsoleServer;

namespace ConsoleClient
{
    class ClientProgram
    {
        const int port = 8888;
        const string address = "127.0.0.1";
        static void Main(string[] args)
        {
            Console.Write("Введите свое имя:");
            string userName = Console.ReadLine();
            TcpClient client = null;
            try
            {
                client = new TcpClient(address, port);
                NetworkStream stream = client.GetStream();

                while (true)
                {
                    //Сначала авторизация

                    //получаем menu
                    byte[] data = new byte[64]; // буфер для получаемых данных
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    do
                    {
                        bytes = stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (stream.DataAvailable);

                    string message = builder.ToString();
                    Console.WriteLine("Сервер: {0}", message);

                    Console.Write(userName + ": ");
                    // ввод сообщения
                    message = Console.ReadLine();
                    message = String.Format("{0}: {1}", userName, message);
                    // преобразуем сообщение в массив байтов
                    data = Encoding.Unicode.GetBytes(message);

                    


                    // отправка сообщения
                    stream.Write(data, 0, data.Length);

                    //----
                    //Catec
                    // newPerson = (Person)formatter.Deserialize(fs);
                    BinaryFormatter formatter = new BinaryFormatter();
                    Categores one = (Categores)formatter.Deserialize(stream);

                    Console.WriteLine("Категории");
                    foreach (string s in one.ListNames)
                    {
                        Console.WriteLine(s);
                    }

                    
                    // ----

                    // получаем ответ
                    //data = new byte[64]; // буфер для получаемых данных
                    //StringBuilder builder = new StringBuilder();
                    //int bytes = 0;
                    //do
                    //{
                    //    bytes = stream.Read(data, 0, data.Length);
                    //    builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    //}
                    //while (stream.DataAvailable);

                    //message = builder.ToString();
                    //Console.WriteLine("Сервер: {0}", message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                client.Close();
            }
        }
    }
}

//using System;
//using System.Net.Sockets;
//using System.Text;

//namespace ConsoleClient
//{
//    class ClientProgram
//    {
//        const int port = 8888;
//        const string address = "127.0.0.1";
//        static void Main(string[] args)
//        {
//            Console.Write("Введите свое имя:");
//            string userName = Console.ReadLine();
//            TcpClient client = null;
//            try
//            {
//                client = new TcpClient(address, port);
//                NetworkStream stream = client.GetStream();

//                while (true)
//                {
//                    Console.Write(userName + ": ");
//                    // ввод сообщения
//                    string message = Console.ReadLine();
//                    message = String.Format("{0}: {1}", userName, message);
//                    // преобразуем сообщение в массив байтов
//                    byte[] data = Encoding.Unicode.GetBytes(message);
//                    // отправка сообщения
//                    stream.Write(data, 0, data.Length);

//                    // получаем ответ
//                    data = new byte[64]; // буфер для получаемых данных
//                    StringBuilder builder = new StringBuilder();
//                    int bytes = 0;
//                    do
//                    {
//                        bytes = stream.Read(data, 0, data.Length);
//                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
//                    }
//                    while (stream.DataAvailable);

//                    message = builder.ToString();
//                    Console.WriteLine("Сервер: {0}", message);
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine(ex.Message);
//            }
//            finally
//            {
//                client.Close();
//            }
//        }
//    }
//}

////using System;
////using System.Text;
////using System.Net;
////using System.Net.Sockets;

////namespace SocketTcpClient
////{
////    class ClientProgram
////    {
////        // адрес и порт сервера, к которому будем подключаться
////        static int port = 8005; // порт сервера
////        static string address = "127.0.0.1"; // адрес сервера
////        static void Main(string[] args)
////        {
////            try
////            {
////                IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(address), port);

////                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
////                // подключаемся к удаленному хосту
////                socket.Connect(ipPoint);
////                Console.Write("Введите сообщение:");
////                string message = Console.ReadLine();
////                byte[] data = Encoding.Unicode.GetBytes(message);
////                socket.Send(data);

////                // получаем ответ
////                data = new byte[256]; // буфер для ответа
////                StringBuilder builder = new StringBuilder();
////                int bytes = 0; // количество полученных байт

////                do
////                {
////                    bytes = socket.Receive(data, data.Length, 0);
////                    builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
////                }
////                while (socket.Available > 0);
////                Console.WriteLine("ответ сервера: " + builder.ToString());

////                // закрываем сокет
////                socket.Shutdown(SocketShutdown.Both);
////                socket.Close();
////            }
////            catch (Exception ex)
////            {
////                Console.WriteLine(ex.Message);
////            }
////            Console.Read();
////        }
////    }
////}