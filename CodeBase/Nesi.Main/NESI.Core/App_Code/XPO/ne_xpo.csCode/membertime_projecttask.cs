using System;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
namespace ne_xpo.cs
{

    public partial class membertime_projecttask
    {
        public membertime_projecttask(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

}
