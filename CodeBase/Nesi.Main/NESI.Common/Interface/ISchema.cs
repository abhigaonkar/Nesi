using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESI.Common.Interface
{
    public interface ISchema
    {

        string name { get; set; }
        string type { get; set; }
        string validation { get; set; }
        bool display { get; set; }
        string header { get; set; }
        string width { get; set; }
        string summary { get; set; }
        int ordinal { get; set; }


    }
}
