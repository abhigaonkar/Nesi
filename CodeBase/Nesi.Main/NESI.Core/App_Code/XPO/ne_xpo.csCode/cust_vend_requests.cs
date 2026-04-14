using System;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
namespace ne_xpo.cs
{

    public partial class cust_vend_requests
    {
        public cust_vend_requests(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

}
