using System;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
namespace ne_xpo.cs
	{

	public partial class shopping_cart
		{
		public shopping_cart(Session session) : base(session) { }
		public override void AfterConstruction() { base.AfterConstruction(); }
		}

	}
