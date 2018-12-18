using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.Net.Sockets;
using System.Text;
using MyTCPLib;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ConsoleClient
{
    class ClientProgram
    {
        const int port = 8888;
        const string address = "127.0.0.1";

        //private List<Category> categories;

        static void Main(string[] args)
        {
            Console.Write("Введите свое имя:");
            string userName = Console.ReadLine();
            TcpClient client = null;
            try
            {
                client = new TcpClient(address, port);
                NetworkStream stream = client.GetStream();
                BinaryFormatter formatter = new BinaryFormatter();
                ClinetMessage messenge;

                string input;

                while (true)
                {
                    Console.Write(userName + ": ");
                    input = Console.ReadLine();

                    switch (input)
                    {
                        case "1":
                            formatter = new BinaryFormatter();

                            messenge = new ClinetMessage(TypeMasseng.GetCategories);
                            formatter.Serialize(stream, messenge);

                            while(true)
                            {
                                if (stream.DataAvailable)
                                {
                                    Console.WriteLine("Сервер говорит!");
                                    ObservableCollection<Category> categories = (ObservableCollection<Category>)formatter.Deserialize(stream);
                                    Console.WriteLine("Категории: ");
                                    foreach (Category c in categories)
                                    {
                                        Console.WriteLine("{0}. - {1}",c.id,c.Name);
                                    }
                                    break;
                                }
                            }
                            break;
                        default:
                            Console.WriteLine("Ошибка ввобда");
                            //Отправка запроса на меню
                            messenge = new ClinetMessage(TypeMasseng.GetMenu);
                            formatter.Serialize(stream, messenge);
                            
                            while(true)
                            {
                                if (stream.DataAvailable)
                                {
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
                                    Console.WriteLine("Сервер говорит! {0}", message);
                                    break;
                                }
                            }
                            break;
                    }
                    
                    //// ввод сообщения
                    //string Inmessage = Console.ReadLine();
                    ////Inmessage = String.Format("{0}: {1}", userName, Inmessage);
                    //// преобразуем сообщение в массив байтов
                    //data = Encoding.Unicode.GetBytes(Inmessage);

                    //// отправка сообщения
                    //stream.Write(data, 0, data.Length);
                    //if(Inmessage=="1")
                    //{
                        
                    //}

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