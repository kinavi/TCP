using System;
using System.Net.Sockets;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using ProGaudi.Tarantool.Client;
using System.Threading.Tasks;
using ProGaudi.Tarantool.Client.Model;
using ProGaudi.Tarantool.Client.Model.Enums;

namespace ConsoleServer
{
    public class ClientObject
    {
        public TcpClient client;
        public ClientObject(TcpClient tcpClient)
        {
            client = tcpClient;
        }

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
                    Test().GetAwaiter().GetResult();

                    clinetMessage = (ClinetMessage)formatter.Deserialize(stream);

                    switch (clinetMessage.type)
                    {
                        case TypeMasseng.GetMenu:
                            //// Выводим меню
                            data = Encoding.Unicode.GetBytes(menu);
                            stream.Write(data, 0, data.Length);
                            break;
                        case TypeMasseng.GetCategories:
                            Categores list = new Categores("Workout", "Paint");
                            //BinaryFormatter formatter = new BinaryFormatter();
                            formatter.Serialize(stream, list);
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

        static async Task Test()
        {
            //var box = await Box.Connect("127.0.0.1:3301");
            //var schema = box.GetSchema();
            //var space = await schema.GetSpace("examples");
            //await space.Insert((99999, "BB"));
            using (var box = await Box.Connect("operator:123123@localhost:3301"))
            {
                var schema = box.GetSchema();

                var space = await schema.GetSpace("users");
                var primaryIndex = await space.GetIndex("primary_id");

                var data = await primaryIndex.Select<TarantoolTuple<string>,
                    TarantoolTuple<string, string, string, string, long>>(
                    TarantoolTuple.Create(String.Empty), new SelectOptions
                    {
                        Iterator = Iterator.All
                    });

                foreach (var item in data.Data)
                {
                    Console.WriteLine(item);
                }
            }
        }
    }
}