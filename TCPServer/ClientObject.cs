using System;
using System.Net.Sockets;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using ProGaudi.Tarantool.Client;
using System.Threading.Tasks;
using ProGaudi.Tarantool.Client.Model;
using ProGaudi.Tarantool.Client.Model.Enums;
using System.Collections.Generic;

namespace ConsoleServer
{
    public class ClientObject
    {
        public TcpClient client;
        public ClientObject(TcpClient tcpClient)
        {
            client = tcpClient;
        }

        private List<Category> categories = new List<Category>();

        private string menu = "\nМеню:\n 1 - Получить список категорий. \n";

        public void Process()
        {
            NetworkStream stream = null;
            BinaryFormatter formatter = new BinaryFormatter();
            ClinetMessage clinetMessage;

            try
            {
                stream = client.GetStream();
                byte[] data = new byte[64]; // буфер для получаемых данных
                while (true)
                {
                    clinetMessage = (ClinetMessage)formatter.Deserialize(stream);

                    switch (clinetMessage.type)
                    {
                        case TypeMasseng.GetMenu:
                            //// Выводим меню
                            data = Encoding.Unicode.GetBytes(menu);
                            stream.Write(data, 0, data.Length);
                            break;
                        case TypeMasseng.GetCategories:
                            RequestAllCategories().GetAwaiter().GetResult();
                            //Categores list = new Categores("Workout", "Paint");
                            ////BinaryFormatter formatter = new BinaryFormatter();
                            formatter.Serialize(stream, categories);
                            break;
                        default:
                            // Сообщение о не верности ввода
                            string error = "Команда не верна, попробуйте еще.";
                            data = Encoding.Unicode.GetBytes(error);
                            stream.Write(data, 0, data.Length);
                            break;
                    }
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

        async Task RequestAllCategories()
        {
            using (var box = await Box.Connect("localhost:3301"))
            {
                categories.Clear();

                var schema = box.GetSchema();

                var space = await schema.GetSpace("categories");
                var primaryIndex = await space.GetIndex("id");

                var data = await primaryIndex.Select<TarantoolTuple<int>, TarantoolTuple<int, string>>(TarantoolTuple.Create<int>(1), new SelectOptions
                {
                    Iterator = Iterator.All
                });

                if (categories.Count == 0)
                {
                    categories = new List<Category>();

                    foreach (var item in data.Data)
                    {
                        categories.Add(new Category { id = item.Item1, Name = item.Item2 });
                    }
                }
            }
        }
    }
}