using System;
using System.Data;
using System.Web.Script.Serialization;
using nesi.core;

public partial class modules_hist_chart : System.Web.UI.UserControl
	{
	public NeMember myMember;
	Toolbox _tools;
	public DataTable _data;
	public string[] _columns;
	JavaScriptSerializer jSON				= new JavaScriptSerializer();
	
    protected void Page_Init(object sender, EventArgs e)
		{
		_tools								= new Toolbox();
		myMember							= Toolbox.do_handle_authentication(1);

		}
    protected void Page_Load(object sender, EventArgs e)
		{
		/*['Year', 'Sales', 'Expenses'],
          ['2004', 1000, 400],
          ['2005', 1170, 460],
          ['2006', 660, 1120],
          ['2007', 1030, 540]*/
	
		_columns = new string[] { "Month", "Cost", "Sell" };
		_data = _tools.getSQL_datatable(@"Call get_wage_margin_history(1,2)"  , null);
		 

	//	_data = 
				fill();
			
		}
	protected void fill()
	{

		var columns = "[[";
		object data = "[";

		foreach (var c in _columns)
		{
			columns += "'" + c + "',";
		}
		columns=columns.TrimEnd(',');
		columns += "],";

		foreach (DataRow dr in _data.Rows)
		{
			columns += "[";
			var x=0;
			foreach (var c in dr.ItemArray)
			{
				if (x == 0)
				{
					columns += "'" + c + "',";
				}
				else
				{
					columns += "" + c + ",";
					
				}
x++;
			}
			columns=columns.TrimEnd(',');
			columns += "],";
		}
		columns=columns.TrimEnd(',');
		columns += "]";
		div_graph.InnerHtml = @" <head>
    <script type='text/javascript' src='https://www.google.com/jsapi'></script>
    <script type='text/javascript'>

    	google.load('visualization', '1', { packages: ['corechart'] });
    	google.setOnLoadCallback(drawChart);

    	function drawChart() {
   		var data = google.visualization.arrayToDataTable(
		" + 
          columns
        + @");
   		var options = {
    			title: 'Company Performance'
    		};
    	var chart = new google.visualization.LineChart(document.getElementById('chart_div'));
		chart.draw(data, options);
	}

    </script>
 </head>";

	}
}
