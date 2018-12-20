using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamvvm;

namespace MyTCPLib
{
    [Serializable]
    public class Category : BaseModel
    {
        public int id { get; set; }
        public string Name { get; set; }
    }
}
