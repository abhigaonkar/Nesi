using System;
using System.Collections;
using System.Collections.Generic;
using DevExpress.Web;
using DevExpress.Web.Data;
using nesi.core;

public partial class sections_workorder_modules_wo_tasklist : System.Web.UI.UserControl
{
    public int woprog_id { get; set; }

	public void LoadPage()
	{
		enableTimesheetEditing.Checked = Toolbox.doSQL_bool("SELECT enableEditingScopeOnTimesheet FROM woprog WHERE woprog_id = @v0", new object[] { woprog_id });
		hdn_tasks_woid.Value = woprog_id.ToString();
	}

    protected void enableTimesheetEditing_OnCheckedChanged(object sender, EventArgs e)
    {
        var isEnabled = enableTimesheetEditing.Checked;
        Toolbox.doSQL_void("UPDATE woprog SET enableEditingScopeOnTimesheet = @v0 WHERE woprog_id = @v1", new object[] { isEnabled, woprog_id });
    }

protected void gv_wotasks_OnRowDeleting(object sender, ASPxDataDeletingEventArgs e)
	{
	var scopeList = new List<int>();
	var issues = 0;
	// This existence check is needed if the data from the grid is stale upon pushing the delete button.
	foreach(DictionaryEntry item in e.Keys)
		{
		var scopeId = (int) item.Value;
		var isUsed = Toolbox.doSQL_int(@"SELECT COUNT(*) FROM membertime WHERE scope_id =@v0", new object[] { scopeId }) > 0;
		if(isUsed)
			{
			issues++;
			}
		if(!isUsed)
			{
			scopeList.Add(scopeId);
			}
		}
	if(scopeList.Count > 0)
		{
		Toolbox.doSQL_void(@"DELETE FROM woprog_tasks WHERE FIND_IN_SET(id, @v0)", new object[] { string.Join(",", scopeList) });
		gv_wotasks.DataBind();
		}
	}

protected void gv_wotasks_OnCommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
	{
	if(e.VisibleIndex < 0) return;
	if(e.ButtonType == ColumnCommandButtonType.Delete)
		{
		var sliceCount = Toolbox.ReturnZeroIfNull_int(gv_wotasks.GetRowValues(e.VisibleIndex, "n_timeslices"));
		e.Visible = sliceCount == 0;
		}
	}
}