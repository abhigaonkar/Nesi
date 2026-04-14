using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESI.Common.Interface
{
    public interface IReport
    {
        string name { get; set; }
        Schema[] columns { get; set; }

        string createEndPoint { get; set; }


        string editEndPoint { get; set; }


        string deleteEndPoint { get; set; }


        bool editable { get; set; }

        string bulkEditEndPoint { get; set; }



        string searchEndPoint { get; set; }

        bool isPrivate { get; set; }


        string distinctEndPoint { get; set; }


        string exportToExcelEndPoint { get; set; }



        string exportToPdfEndPoint { get; set; }

        bool isSelectable { get; set; }
    }
}
