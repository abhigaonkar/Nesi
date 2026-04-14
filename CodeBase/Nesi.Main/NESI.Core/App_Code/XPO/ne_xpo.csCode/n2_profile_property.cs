using System;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
namespace ne_xpo.cs
{

    public partial class n2_profile_property
    {
        public n2_profile_property(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

}
