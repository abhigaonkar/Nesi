using System;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
namespace ne_xpo.cs
{

	public partial class gl_control_accounts
	{
		public gl_control_accounts(Session session) : base(session) { }
		public override void AfterConstruction() { base.AfterConstruction(); }
	}

}
