using System;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
namespace ne_xpo.cs
	{

	public partial class quote_kill_reason_map
		{
		public quote_kill_reason_map(Session session) : base(session) { }
		public override void AfterConstruction() { base.AfterConstruction(); }
		}

	}
