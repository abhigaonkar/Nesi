using System;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
namespace ne_xpo.cs
{

    public partial class inventory_dollar_balance_daily
    {
        public inventory_dollar_balance_daily(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

}
