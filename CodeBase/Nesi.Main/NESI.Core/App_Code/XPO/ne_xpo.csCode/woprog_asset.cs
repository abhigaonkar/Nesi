using System;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
namespace ne_xpo.cs
{

    public partial class woprog_asset
    {
        public woprog_asset(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

}
