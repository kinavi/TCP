using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleServer
{
    public enum TypeMasseng { GetMenu, GetCategories };

    [Serializable]
    public class ClinetMessage
    {
        public TypeMasseng type { get; set; }

        public ClinetMessage(TypeMasseng typeMasseng)
        {
            type = typeMasseng;
        }
    }
}
