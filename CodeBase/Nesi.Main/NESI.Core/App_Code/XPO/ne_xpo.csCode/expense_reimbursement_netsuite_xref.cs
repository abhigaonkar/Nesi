using System;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
namespace ne_xpo.cs
{

    public partial class expense_reimbursement_netsuite_xref
    {
        public expense_reimbursement_netsuite_xref(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

}
