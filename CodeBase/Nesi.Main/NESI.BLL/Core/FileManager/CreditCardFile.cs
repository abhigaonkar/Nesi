using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESI.BLL.Core.FileManager
{
    class CreditCardFile: NeFileBase
    {
        public override string BaseFolder => base.FileServer + $@"\TE\TE{CurrentUser.TaxEntityId}\credit_card_receipts";
        public override string BasePath => Path.Combine(BaseFolder, "");


        public CreditCardFile(Employee.Employee user) : base(user)
        {
            base.Validate_folder_contents();
        }

    }
}
