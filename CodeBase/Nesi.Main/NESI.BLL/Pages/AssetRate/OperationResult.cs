using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESI.BLL.Pages.AssetRate
{
    public class OperationResult
    {
        public int okay { get; set; } // 1: done; 0: failed.
        public string message { get; set; } // general msg
        public List<ErrorRecord> errors {get;set;} // detail info
    }
}
