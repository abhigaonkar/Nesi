using System;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
namespace ne_xpo.cs
{

    public partial class n2_user_profile
    {
        public n2_user_profile(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

}
