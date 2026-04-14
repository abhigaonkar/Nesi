using System;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
namespace ne_xpo.cs
{

    public partial class po_import_stage
    {
        public po_import_stage(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

}
