using System;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Xml;

public partial class sections_workorder_modules_project_schedule : System.Web.UI.UserControl
	{
	public int woprog_id { get; set; }
	protected void Page_Load(object sender, EventArgs e)
	{
	if(!Visible) return;
		if (!IsPostBack)
		{
			//Fill dropdownlist with all projects names
			var mcon = new SqlConnection("Data Source=CYBERMIND\\SQLEXPRESS;Database=Projects;Trusted_Connection=True"); //Cahnge Connection string of your SQL Server Here
			mcon.Open();
			cmbproject.Items.Clear();
			var GetProjectName = new SqlCommand("select projectid,name from project", mcon);
			SqlDataReader ReadPrjName;
			ReadPrjName = GetProjectName.ExecuteReader();
			cmbproject.Items.Add("--------Select-----");
			while (ReadPrjName.Read())
			{
				var LIT = new ListItem();
				LIT.Text = ReadPrjName.GetValue(1).ToString();//Add Project name as dropdownlists Text
				LIT.Value = ReadPrjName.GetValue(0).ToString();//Add Projectid as dropdownlists Value
				cmbproject.Items.Add(LIT);
			}
			ReadPrjName.Close();
			mcon.Close();
		}
	}
	protected void cmbproject_SelectedIndexChanged(object sender, EventArgs e)
	{
		var projectid = "";
		var mcon = new SqlConnection("Data Source=CYBERMIND\\SQLEXPRESS;Database=Projects;Trusted_Connection=True");
		mcon.Open();
		//Creating data of ou Task.xml programatically
		var xwriter = new XmlTextWriter(Server.MapPath("Tasks.xml"), null); //Creating a Xml Text writer
		xwriter.Formatting = Formatting.Indented;// Setting indendation in xml file
		xwriter.Indentation = 4;// Indentation space is 4 here
		var mmcmd = new SqlCommand("select projectid,name,convert(varchar(10),startdate,101),convert(varchar(10),enddate,101) from project where projectid =" + cmbproject.SelectedItem.Value + "", mcon);
		var projectReader = mmcmd.ExecuteReader();
		xwriter.WriteStartElement("project");
		while (projectReader.Read())
		{
			projectid = projectReader.GetValue(0).ToString();
			xwriter.WriteStartElement("task");
			xwriter.WriteStartElement("pID"); // WriteStartElement writes a tag
			xwriter.WriteString(projectReader.GetValue(0).ToString()); // WriteString writes strin to xml file
			xwriter.WriteEndElement();// WriteStartElement writes a end tag
			xwriter.WriteStartElement("pName");
			xwriter.WriteString(projectReader.GetValue(1).ToString());
			xwriter.WriteEndElement();
			xwriter.WriteStartElement("pStart");
			xwriter.WriteString(projectReader.GetValue(2).ToString());
			xwriter.WriteEndElement();
			xwriter.WriteStartElement("pEnd");
			xwriter.WriteString(projectReader.GetValue(3).ToString());
			xwriter.WriteEndElement();
			xwriter.WriteStartElement("pParent");
			xwriter.WriteString("0");
			xwriter.WriteEndElement();
			xwriter.WriteEndElement();
		}
		//End of Project Details Read and Write to XML
		mcon.Close();
		var mcon2 = new SqlConnection("Data Source=CYBERMIND\\SQLEXPRESS;Database=Projects;Trusted_Connection=True");
		mcon2.Open();
		var str = "select taskid,taskname,convert(varchar(10),startdate,101),convert(varchar(10),enddate,101), percentagecompleted from Task where projectid =" + cmbproject.SelectedItem.Value + " ";
		var mcmd = new SqlCommand(str, mcon2);
		var reader = mcmd.ExecuteReader();
		//Start to read and write Tasks to Xml file
		while (reader.Read())
		{
			xwriter.WriteStartElement("task");
			xwriter.WriteStartElement("pID");
			xwriter.WriteString(reader.GetValue(0).ToString());
			xwriter.WriteEndElement();
			xwriter.WriteStartElement("pName");
			xwriter.WriteString(reader.GetValue(1).ToString());
			xwriter.WriteEndElement();
			xwriter.WriteStartElement("pStart");
			xwriter.WriteString(reader.GetValue(2).ToString());
			xwriter.WriteEndElement();
			xwriter.WriteStartElement("pEnd");
			xwriter.WriteString(reader.GetValue(3).ToString());
			xwriter.WriteEndElement();
			xwriter.WriteStartElement("pColor");// Sets chart task bar color to blue
			xwriter.WriteString("ff00ff");
			xwriter.WriteEndElement();
			xwriter.WriteStartElement("pComp");
			xwriter.WriteString(reader.GetValue(4).ToString());
			xwriter.WriteEndElement();
			xwriter.WriteStartElement("pParent");
			xwriter.WriteString(projectid);
			xwriter.WriteEndElement();
			xwriter.WriteEndElement();
		}
		xwriter.WriteEndElement();
		//Start to read and write Tasks to Xml file
		xwriter.Flush();
		xwriter.Close(); //Close XML File
		mcon2.Close();
	}
	}