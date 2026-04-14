using DevExpress.Web;
using nesi.core;
using NESI.BLL.Pages.Reports;
using NESI.Common.Models;
using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Nesi.Web.sections.reports.customer_reports
{
    public partial class PartialBillingWithDetailCost : System.Web.UI.Page
    {
        private ASPxHiddenField _h;
        private SqlDataSource _ds_templates;
        private ASPxDropDownEdit _dde_filter;
        private Panel _panel_export;
        private NeMember current_user;
        readonly string _page_name = "Master WO Cost Report";
        readonly string _grid_name = "gvPartialBilling";

        protected void Page_Init(object sender, EventArgs e)
        {
            current_user = Toolbox.do_handle_authentication(OpsPage.PartialBillingWithCost);
            _h = (ASPxHiddenField)layout.FindControl("h");
            _ds_templates = (SqlDataSource)layout.FindControl("ds_templates");
            _dde_filter = (ASPxDropDownEdit)layout.FindControl("dde_filter");
            _panel_export = (Panel)layout.FindControl("panel_export");
            _panel_export.Visible = true;
            _ds_templates.SelectParameters["@page_name"].DefaultValue = _grid_name;
            _ds_templates.SelectParameters["@member_id"].DefaultValue = current_user.id.ToString();
            layout.__page_name = _grid_name;
            layout.used_gv = gvPartialBilling;

            if (IsPostBack || IsCallback) return;
            var gl = new NeGridLayouts(current_user.id, _grid_name);
            _h.Set("gridview_id", _grid_name);
            if (gl.GridLayoutID == 0)
            {
                gl.GridLayout_Layout = gvPartialBilling.SaveClientLayout();
                gl.member_id = current_user.id;
                gl.GridLayout_Name = "Default";
                gl.GridLayout_Gridid = _grid_name;
                gl.SaveGridLayout();

                _h.Set("ID", gl.GridLayoutID);
                _h.Set("NAME", gl.GridLayout_Name);
            }
            else
            {
                gvPartialBilling.LoadClientLayout(gl.GridLayout_Layout);
                _h.Set("ID", gl.GridLayoutID);
                _h.Set("NAME", gl.GridLayout_Name);
            }
            _dde_filter.Text = gl.GridLayout_Name;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            var lbltemp = (Label)Page.Master.FindControl("lblHeading");
            lbltemp.Text = _page_name;
        }
        protected void btSubmit_Click(object sender, EventArgs e)
        {

            string selectedWOStatuses = GetSelectedWOStatuses();
            for (int i = 0; i < sdsPartialBilling.SelectParameters.Count; i++)
            {
                if (sdsPartialBilling.SelectParameters[i].Name.Contains("wostatus"))
                {
                    sdsPartialBilling.SelectParameters.RemoveAt(i);
                    break;
                }
            }
            // Add new parameter with updated value
            sdsPartialBilling.SelectParameters.Add(new Parameter("@wostatus", DbType.String, selectedWOStatuses));
            gvPartialBilling.DataBind();

        }
        private string GetSelectedWOStatuses()
        {
            string woStatuses = "";

            foreach (string status in cl_wostatus.SelectedValues)
            {
                woStatuses += status + ",";
            }

            if (!string.IsNullOrEmpty(woStatuses))
            {
                // Remove trailing comma
                woStatuses = woStatuses.Remove(woStatuses.Length - 1);
            }

            return woStatuses;
        }
        protected void btExpand_Click(object sender, EventArgs e)
        {
            gvPartialBilling.ExpandAll();
        }

        protected void ddlBusinessUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlCustomer.Value = null;
            ddlCustomer.DataBind();
            ddlProjManager.Value = null;
            ddlProjManager.DataBind();

        }

        protected void gvPartialBilling_SummaryDisplayText(object sender, DevExpress.Web.ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            var total = Convert.ToDouble(e.Value);
            var field_name = e.Item.FieldName;
            if (field_name != "total")
            {
                if (field_name == "qty")
                {
                    e.Text = $"Qty: {total:N2}";
                }
            }
            else
            {
                e.Text = $"Total: {total:C2}";
            }
        }



        protected void gvPartialBilling_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.GroupFooter) return;
            var level = gvPartialBilling.GetRowLevel(e.VisibleIndex);
            if (level <= 0) return;
            var index = e.VisibleIndex - 1;

            while (!gvPartialBilling.IsGroupRow(index) || gvPartialBilling.GetRowLevel(index) >= level)
            {
                index--;
            }
            var count = 0;
            var rowcount = gvPartialBilling.GetChildRowCount(index) + index;
            for (var i = index + 1; i <= rowcount; i++)
            {
                if (gvPartialBilling.IsGroupRow(i))
                    count++;
            }
            if (count == 1 && level == 1)
                e.Row.Visible = false;
        }

        protected void gvPartialBilling_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
        {
            var gv = (ASPxGridView)sender;
            e.Properties["cpExp"] = gv.SaveClientLayout();
        }

        protected void gvPartialBilling_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            var gv = (ASPxGridView)sender;
            if (e.Parameters != "")
            {
                gv.LoadClientLayout(e.Parameters);
            }
            else
            {
                gv.FilterExpression = "";
                for (var i = 0; i < gv.Columns.Count; i++)
                {
                    if (!(gv.Columns[i] is GridViewDataColumn)) continue;
                    var col = (GridViewDataColumn)gv.Columns[i];
                    if (col.GroupIndex > -1)
                    {
                        gv.UnGroup(col);
                    }
                    col.Visible = true;
                }
            }
        }
    }
}