using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ConsoleServer
{
    [Serializable]
    public class Categores
    {
        public List<String> ListNames { get; set; }
        //public string Name { get; set; }

        public Categores(params string[] list)
        {
            ListNames = new List<string>();

            foreach (String s in list)
            {
                ListNames.Add(s);
            }
        }
    }
}
