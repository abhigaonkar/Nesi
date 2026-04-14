using System;
using System.Data;
using System.Web.Script.Serialization;
using nesi.core;

public partial class modules_timeline : System.Web.UI.UserControl
	{
	public NeMember myMember;
	public string type = "";
	public string id = "";
	
	Toolbox _tools;
	JavaScriptSerializer jSON				= new JavaScriptSerializer();
	
    protected void Page_Init(object sender, EventArgs e)
		{
		_tools								= new Toolbox();
		myMember							= Toolbox.do_handle_authentication(1);

		}
    protected void Page_Load(object sender, EventArgs e)
		{
			load_timeline();
		}
	protected void load_timeline()
	{
		var newrow = "";
		if (type == "Big Work Orders")
		{
			var sql_query = "Select * from woprog,company where business_unit_id = business_unit_id and woprog_expected_sales_value >100000 and WOProg_Associate_WOProg_ID=0 and woprog_iscredit=0 and woprog_isrebill = 0 order by woprog_id desc limit 50";
			var dt = _tools.getSQL_datatable(sql_query,null);
			foreach (DataRow dr in dt.Rows)
			{
				newrow += "[new " + _date(Convert.ToDateTime(dr["woprog_cutdatetime"])) + ", ,'" + @"<div style=""color: " + dr["company_rgb"] + @"; border:0px solid;""" + @">" + dr["woprog_customername"] + "</div>'],";
			}
			newrow = newrow.TrimEnd(',');
		}
		if (type == "Employees")
		{
			var sql_query = "Select * from member,company where business_unit_id = business_unit_id and member_status ='Active' and business_unit_id !=8 order by member_id desc";
			var dt = _tools.getSQL_datatable(sql_query, null);
			foreach (DataRow dr in dt.Rows)
			{
				newrow += "[new " + _date(Convert.ToDateTime(dr["member_startdate"])) + ", ,'" + @"<div style=""color: " + dr["company_rgb"] + @"; border:0px solid;""" + @">" + dr["member_fullname"] + "</div>'],";
			}
			newrow = newrow.TrimEnd(',');
		}
		if (type == "Terminations")
		{
			var sql_query = "Select * from member,company where business_unit_id = business_unit_id and member_status ='Not Active' and business_unit_id !=8 order by member_id desc";
			var dt = _tools.getSQL_datatable(sql_query, null);
			foreach (DataRow dr in dt.Rows)
			{
				newrow += "[new " + _date(Convert.ToDateTime(dr["member_termdate"])) + ", ,'" + @"<div style=""color: " + dr["company_rgb"] + @"; border:0px solid;""" + @">" + dr["member_fullname"] + "</div>'],";
			}
			newrow = newrow.TrimEnd(',');
		}
		if (type == "wo")
		{
			var sql_query = "Select * from woprogstatus where woprogstatus_woprog_id = @v0";
			var dt = _tools.getSQL_datatable(sql_query, new object[] { id});
			foreach (DataRow dr in dt.Rows)
			{
				newrow += "[new " + _date(Convert.ToDateTime(dr["woprogstatus_datetime"])) + ", ,'" + @"<div style=""color: Black; border:0px solid;""" + @">" + dr["woprogstatus_status"] + "</div>'],";
			}
			newrow = newrow.TrimEnd(',');
		}
		
		
		
		mytimeline.InnerHtml = @"<html>
  <head>
    <title>Timeline demo</title>

    <style>
      body {font: 8pt arial;}
    </style>
    <script type='text/javascript' src='//www.google.com/jsapi'></script>
    <script type='text/javascript' src='../../../js/timeline.js'></script>
    <link rel='stylesheet' type='text/css' href='../../../css/timeline.css'>

    <script type='text/javascript'>
    	google.load('visualization', '1');

    	// Set callback to run when API is loaded
    	google.setOnLoadCallback(drawVisualization);
    	
    	// Called when the Visualization API is loaded.
    	function drawVisualization() {
    		// Create and populate a data table.
    				var data = new google.visualization.DataTable();
     				data.addColumn('datetime', 'start');
    				data.addColumn('datetime', 'end');
    				data.addColumn('string', 'content');
   					data.addRows([
								" + newrow + @"
								]);
    		
    				var options =
					{
 axisOnTop: true,
 

						eventMargin: 5,  // minimal margin between events
						eventMarginAxis: 0, // minimal margin beteen events and the axis
						'width': '100%',
						'height': '98%',
						'style': 'box' // optional
					};

    			var timeline = new links.Timeline(document.getElementById('mytimeline1'));
    				timeline.draw(data, options);
    		
    	}
   </script>
  </head>

  <body>
      <form id='form1' runat='server'>
   <div id='mytimeline1'></div>
  	
	  </form>
  </body>
</html>";
		

	}

	protected string _date(DateTime d)
	{

		return ("Date(" + d.Year + "," + d.Month + "," + d.Day + "," + d.Hour + "," + d.Minute + "," + d.Second + ")");


	}
}
