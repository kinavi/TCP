﻿using System;
using System.Net.Sockets;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;

namespace ConsoleServer
{
    public class ClientObject
    {
        public TcpClient client;
        public ClientObject(TcpClient tcpClient)
        {
            client = tcpClient;
        }

        private string menu = "Меню://n Categories - Получить список категорий. //n";

        public void Process()
        {
            NetworkStream stream = null;
            try
            {
                stream = client.GetStream();
                byte[] data = new byte[64]; // буфер для получаемых данных
                while (true)
                {
                    // Выводим меню
                    data = Encoding.Unicode.GetBytes(menu);
                    stream.Write(data, 0, data.Length);

                    // получаем сообщение
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    do
                    {
                        bytes = stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (stream.DataAvailable);

                    string message = builder.ToString();

                    Console.WriteLine(message);

                    //-----
                    switch(message)
                    {
                        case "Categories":
                            Categores list = new Categores("Workout", "Paint");
                            BinaryFormatter formatter = new BinaryFormatter();
                            formatter.Serialize(stream, list);
                            break;
                        default:
                            // Сообщение о не верности ввода
                            message = "Команда не верна, попробуйте еще.";
                            data = Encoding.Unicode.GetBytes(message);
                            stream.Write(data, 0, data.Length);
                            break;
                    }      
                    //-----

                    // отправляем обратно сообщение в верхнем регистре
                    //message = message.Substring(message.IndexOf(':') + 1).Trim().ToUpper();
                    //data = Encoding.Unicode.GetBytes(message);
                    //stream.Write(data, 0, data.Length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (stream != null)
                    stream.Close();
                if (client != null)
                    client.Close();
            }
        }
    }
}