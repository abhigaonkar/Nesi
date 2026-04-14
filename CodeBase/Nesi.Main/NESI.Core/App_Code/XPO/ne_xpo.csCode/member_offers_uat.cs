using System;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
namespace ne_xpo.cs
{

    public partial class member_offers_uat
    {
        public member_offers_uat(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

}
