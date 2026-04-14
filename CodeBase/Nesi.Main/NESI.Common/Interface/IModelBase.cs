using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESI.Common.Interface
{
    public interface IModelBase
    {
        IReport GetSchema();

        List<string> Metadata { get; set; }

        int Count { get; set; }

    }
}
