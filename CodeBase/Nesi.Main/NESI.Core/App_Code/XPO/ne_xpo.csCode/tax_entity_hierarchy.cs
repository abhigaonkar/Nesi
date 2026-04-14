using System;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
namespace ne_xpo.cs
{

    public partial class tax_entity_hierarchy
    {
        public tax_entity_hierarchy(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

}
