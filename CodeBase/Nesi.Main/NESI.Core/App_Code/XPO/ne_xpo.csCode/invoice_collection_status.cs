using System;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
namespace ne_xpo.cs
	{

	public partial class invoice_collection_status
		{
		public invoice_collection_status(Session session) : base(session) { }
		public override void AfterConstruction() { base.AfterConstruction(); }
		}

	}
