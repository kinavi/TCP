using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProGaudi.Tarantool.Client;
using System.Threading.Tasks;
using ProGaudi.Tarantool.Client.Model;
using ProGaudi.Tarantool.Client.Model.Enums;

namespace TarantoolTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Test().GetAwaiter().GetResult();
        }

        static async Task Test()
        {
            using (var box = await Box.Connect("localhost:3301"))
            {
                var schema = box.GetSchema();

                var space = await schema.GetSpace("categories");
                var primaryIndex = await space.GetIndex("id");

                var data = await primaryIndex.Select<TarantoolTuple<int>, TarantoolTuple<int, string>>(TarantoolTuple.Create<int>(1), new SelectOptions
                {
                    Iterator = Iterator.Eq
                });

                foreach (var item in data.Data)
                {
                    Console.WriteLine(item);
                }
                Console.ReadKey();
            }
        }
    }
}
