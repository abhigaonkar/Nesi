using System;
using System.Data;
using System.Web.Script.Serialization;
using core;
using MySql.Data.MySqlClient;
using nesi.core;

public partial class modules_todolist : System.Web.UI.UserControl
{
    public NeMember myMember;
    JavaScriptSerializer jSON = new JavaScriptSerializer();

    protected void Page_Init(object sender, EventArgs e)
    {
        myMember = Toolbox.do_handle_authentication(1);

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!myMember.isContact && this.Visible)
        {
            fill_todo();
        }
    }

    protected void fill_todo()
    {
        
            var dt_final = new DataTable();
            dt_final.Columns.Add(new DataColumn("link"));
            dt_final.Columns.Add(new DataColumn("description"));
            dt_final.Columns.Add(new DataColumn("overdue"));
            dt_final.Columns.Add(new DataColumn("star"));
            dt_final.Columns.Add(new DataColumn("link2"));


            TodoList.getToDoList(dt_final, myMember);

            gv_todo.DataSource = dt_final;
            gv_todo.DataBind();
        

    }

  

}
