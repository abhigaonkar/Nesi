using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESI.Common.Models
{
    public class CheckPoint
    {
        public string Content { get; set; }
        public bool Passed { get; set; }

        public CheckPoint()
        {
            this.Content = "";
            this.Passed = false;
        }
    }
}
