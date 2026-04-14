using System;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
namespace ne_xpo.cs
{

	public partial class tax_entity_group_link
	{
		public tax_entity_group_link(Session session) : base(session) { }
		public override void AfterConstruction() { base.AfterConstruction(); }
	}

}
