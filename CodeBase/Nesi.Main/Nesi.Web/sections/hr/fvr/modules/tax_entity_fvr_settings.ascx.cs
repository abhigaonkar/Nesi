using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using DevExpress.Xpo;
using nesi.core;
using ne_xpo.cs;

public partial class sections_hr_fvr_modules_tax_entity_fvr_settings : System.Web.UI.UserControl
	{
	NeMember current_user;
	protected void Page_Init(object sender, EventArgs e)
		{
		current_user = Toolbox.do_handle_authentication(47);
		if(!IsPostBack)
			{
			gv_taxentity.FocusedRowIndex = -1;
			}
		gv_taxentity.DataSource			= Toolbox.doSQL_dt(@"SELECT id, ddl_name FROM tax_entity WHERE is_active = 1 AND is_test = 0 AND is_holdco = 0 AND FIND_IN_SET(id, @v0)", new object[] { new Current_User().visible_tax_entities });
		gv_taxentity.DataBind();
		}
	private void FillLayouts()
		{
		lb_layouts.DataSource = Toolbox.doSQL_dt(@"SELECT 0 id, 'Please Select' name UNION ALL SELECT id,name FROM member_fvr_template_header WHERE type_id = 1", null );
		lb_layouts.DataBind();
		}
	private void PopulateForm(ne_xpo.cs.business_unit _bu)
		{
		FillLayouts();
		if(_bu != null &&_bu.default_fvr_template_ids != "")
			{
			if(_bu.default_fvr_template_ids.Contains(","))
				{
				foreach(var _temp in _bu.default_fvr_template_ids.Split(','))
					{
					lb_layouts.Items.FindByValue(Convert.ToInt32(_temp)).Selected = true;
					}
				}
			else
				{
				lb_layouts.Value = Convert.ToInt32(_bu.default_fvr_template_ids);
				}
			}
		else
			{
			lb_layouts.SelectedIndex = -1;
			}
		if(_bu == null) return;
		html_emaillayout.Html = _bu.default_onboarding_email == "" ? new NeTaxEntity(_bu.tax_entity_id).default_onboarding_email : _bu.default_onboarding_email ;
		}
	private void PopulateForm(ne_xpo.cs.tax_entity _te)
		{
		FillLayouts();
		if(_te != null &&_te.default_fvr_template_ids != "")
			{
			if(_te.default_fvr_template_ids.Contains(","))
				{
				foreach(var _temp in _te.default_fvr_template_ids.Split(','))
					{
					lb_layouts.Items.FindByValue(Convert.ToInt32(_temp)).Selected = true;
					}
				}
			else
				{
				lb_layouts.Value = Convert.ToInt32(_te.default_fvr_template_ids);
				}
			}
		else
			{
			lb_layouts.SelectedIndex = -1;
			}
		if(_te == null) return;
		html_emaillayout.Html = _te.default_onboarding_email;
		}
	protected void cbp_taxentity_OnCallback(object _sender, CallbackEventArgsBase _e)
		{
		using (var uow = new UnitOfWork())
			{
			if (_e.Parameter.Contains("|"))
				{
				var paras			= _e.Parameter.Split('|');
				var action			= paras[0];
				var type			= paras[1];
				tax_entity objTaxEntity = null;
				business_unit objBusinessUnit = null;
				if(type == "TE")
					{
					objTaxEntity = uow.GetObjectByKey<tax_entity>(Convert.ToInt32(paras[2]));
					}
				else
					{
					objBusinessUnit = uow.GetObjectByKey<business_unit>(Convert.ToInt32(paras[2]));
					}
				
				switch (action)
					{
					case "load":
						if(type == "TE")
							{
							PopulateForm(objTaxEntity);
							}
						else
							{
							PopulateForm(objBusinessUnit);
							}
					break;
					case "save":
						if(type == "TE")
							{
							if(objTaxEntity != null)
								{
								objTaxEntity.default_fvr_template_ids = paras[3] == "0" ? "" : paras[3];
								objTaxEntity.default_onboarding_email = paras[4];
								objTaxEntity.Save();
								uow.CommitChanges();
								}
							}
						else
							{
							if(objBusinessUnit != null)
								{
								objBusinessUnit.default_fvr_template_ids = paras[3] == "0" ? "" : paras[3];
								objBusinessUnit.default_onboarding_email = paras[4];
								objBusinessUnit.Save();
								uow.CommitChanges();
								}
							}
						if(type == "TE")
							{
							PopulateForm(objTaxEntity);
							}
						else
							{
							PopulateForm(objBusinessUnit);
							}
					break;
					}
				}
			}
		}

	protected void gv_taxentity_DetailRowExpandedChanged(object sender, ASPxGridViewDetailRowEventArgs e)
		{
		if (!e.Expanded) return;
		var index = e.VisibleIndex;
		var teId = Convert.ToInt32(gv_taxentity.GetRowValues(index, "id"));
		var gv = (ASPxGridView) gv_taxentity.FindDetailRowTemplateControl(index, "gv_bus");
		gv.DataSource = Toolbox.doSQL_dt(@"SELECT id, ddl_name FROM business_unit WHERE tax_entity_id = @v0 AND istest = 'F' AND active = 'T' AND FIND_IN_SET(id, @v1)", new object[] {teId, new Current_User().visible_business_units, });
		gv.DataBind();
		}
	}