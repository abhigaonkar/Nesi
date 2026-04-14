using System;
using nesi.core;

public partial class this_picklist_header_totals_index : System.Web.UI.Page
{
    NeMember _member;
    NeBusinessUnit _company;
    private const int _page_id = 1; // from Page table in DB

    protected void Page_Load(object sender, EventArgs e)
    {
        var _tools = new Toolbox();
        _member = Toolbox.do_handle_authentication(_page_id);
        _tools.dont_cache_page();
        _company = new NeBusinessUnit(_member.business_unit_id);
        var _q = Request.QueryString;
        var OUTPUT = "";
        _tools.set_XML_header();

        var RegLab2 = "0";
        var RegMat2 = "0";
        var RegTot2 = "0";

        var VNCLab2 = "0";
        var VNCMat2 = "0";
        var VNCTot2 = "0";

        var BlnLab2 = "0";
        var BlnMat2 = "0";
        var BlnTot2 = "0";

        var ICRLab2 = "0";
        var ICRMat2 = "0";
        var ICRTot2 = "0";

        var DNILab2 = "0";
        var DNIMat2 = "0";
        var DNITot2 = "0";

        var VCLab2 = "0";
        var VCMat2 = "0";
        var VCTot2 = "0";

        var AllLab = "0";
        var AllMat = "0";
        var AllTot = "0";

		

        var origin = _q["origin"];
        var id = _q["id"];
        var company = _q["c"];
        var quoteid = "";
        var erid = "";
        if (origin == "workorder")
        {
            
            var linewoid = id;
           

            double AllItemTotals = 0;
            double AllLabItemTotals = 0;
            double AllMatItemsTotals = 0;
            double itemtotals = 0;
            double itemlabtotals = 0;
            double itemmattotals = 0;
            var tabletype = "current";
            quoteid = _tools.getSQL_string(@"SELECT WOProg_QuoteID FROM woprog WHERE WOProg_ID = @v0 ", new object[] {  linewoid } );
            erid = _tools.getSQL_string(@"SELECT WOProg_ERID FROM woprog WHERE WOProg_ID = @v0 ", new object[] {  linewoid } );
            var SQLBase = @"
SELECT 
	IFNULL(SUM(wo_detail_{0}_qty_committed * wo_detail_{0}_price_sell * (1 - (wo_detail_{0}_discount/100))), 0) AS Linetotal 
FROM 
	wo_detail_{0} 
WHERE 
	wo_detail_{0}_woprog_id = {1} AND 
	(wo_detail_{0}_type {2}= 'L' {5} wo_detail_{0}_code {3} LIKE 'LB%' {5} wo_detail_{0}_master_id {4} 990000) AND 
	wo_detail_{0}_billtypeid ={6}";
            if (quoteid == "0" && erid == "0")
            {
                try { itemlabtotals = _tools.getSQL_double(string.Format(SQLBase, tabletype, linewoid, "", "", ">=", "OR", "0"), null); }
                catch { itemlabtotals = 0; }
                try { itemmattotals = _tools.getSQL_double(string.Format(SQLBase, tabletype, linewoid, "!", "NOT", "<", "AND", "0"), null); }
                catch { itemmattotals = 0; }
                RegLab2 = itemlabtotals.ToString("C2");
                RegMat2 = itemmattotals.ToString("C2");
                itemtotals = itemlabtotals + itemmattotals;
                AllItemTotals += itemtotals;
                AllLabItemTotals += itemlabtotals;
                AllMatItemsTotals += itemmattotals;
                RegTot2 = itemtotals.ToString("C2");

                try { itemlabtotals = _tools.getSQL_double(string.Format(SQLBase, tabletype, linewoid, "", "", ">=", "OR", "7"), null); }
                catch { itemlabtotals = 0; }
                try { itemmattotals = _tools.getSQL_double(string.Format(SQLBase, tabletype, linewoid, "!", "NOT", "<", "AND", "7"), null); }
                catch { itemmattotals = 0; }
                VNCLab2 = itemlabtotals.ToString("C2");
                VNCMat2 = itemmattotals.ToString("C2");
                itemtotals = itemlabtotals + itemmattotals;
                AllItemTotals += itemtotals;
                AllLabItemTotals += itemlabtotals;
                AllMatItemsTotals += itemmattotals;
                VNCTot2 = itemtotals.ToString("C2");


                try { itemlabtotals = _tools.getSQL_double(string.Format(SQLBase, tabletype, linewoid, "", "", ">=", "OR", "4"), null); }
                catch { itemlabtotals = 0; }
                try { itemmattotals = _tools.getSQL_double(string.Format(SQLBase, tabletype, linewoid, "!", "NOT", "<", "AND", "4"), null); }
                catch { itemmattotals = 0; }
                BlnLab2 = itemlabtotals.ToString("C2");
                BlnMat2 = itemmattotals.ToString("C2");
                itemtotals = itemlabtotals + itemmattotals;
                AllItemTotals += itemtotals;
                AllLabItemTotals += itemlabtotals;
                AllMatItemsTotals += itemmattotals;
                BlnTot2 = itemtotals.ToString("C2");


                try { itemlabtotals = _tools.getSQL_double(string.Format(SQLBase, tabletype, linewoid, "", "", ">=", "OR", "2"), null); }
                catch { itemlabtotals = 0; }
                try { itemmattotals = _tools.getSQL_double(string.Format(SQLBase, tabletype, linewoid, "!", "NOT", "<", "AND", "2"), null); }
                catch { itemmattotals = 0; }
                ICRLab2 = itemlabtotals.ToString("C2");
                ICRMat2 = itemmattotals.ToString("C2");
                itemtotals = itemlabtotals + itemmattotals;
                AllItemTotals += itemtotals;
                AllLabItemTotals += itemlabtotals;
                AllMatItemsTotals += itemmattotals;
                ICRTot2 = itemtotals.ToString("C2");

                try { itemlabtotals = _tools.getSQL_double(string.Format(SQLBase, tabletype, linewoid, "", "", ">=", "OR", "5"), null); }
                catch { itemlabtotals = 0; }
                try { itemmattotals = _tools.getSQL_double(string.Format(SQLBase, tabletype, linewoid, "!", "NOT", "<", "AND", "5"), null); }
                catch { itemmattotals = 0; }
                DNILab2 = itemlabtotals.ToString("C2");
                DNIMat2 = itemmattotals.ToString("C2");
                itemtotals = itemlabtotals + itemmattotals;
                AllItemTotals += itemtotals;
                AllLabItemTotals += itemlabtotals;
                AllMatItemsTotals += itemmattotals;
                DNITot2 = itemtotals.ToString("C2");

                try { itemlabtotals = _tools.getSQL_double(string.Format(SQLBase, tabletype, linewoid, "", "", ">=", "OR", "10"), null); }
                catch { itemlabtotals = 0; }
                try { itemmattotals = _tools.getSQL_double(string.Format(SQLBase, tabletype, linewoid, "!", "NOT", "<", "AND", "10"), null); }
                catch { itemmattotals = 0; }
                VCLab2 = itemlabtotals.ToString("C2");
                VCMat2 = itemmattotals.ToString("C2");
                itemtotals = itemlabtotals + itemmattotals;
                AllItemTotals += itemtotals;
                AllLabItemTotals += itemlabtotals;
                AllMatItemsTotals += itemmattotals;
                VCTot2 = itemtotals.ToString("C2");

            }
            else
            {
                try { itemlabtotals = _tools.getSQL_double(string.Format(SQLBase, tabletype, linewoid, "", "", ">=", "OR", "1"), null); }
                catch { itemlabtotals = 0; }
                try { itemmattotals = _tools.getSQL_double(string.Format(SQLBase, tabletype, linewoid, "!", "NOT", "<", "AND", "1"), null); }
                catch { itemmattotals = 0; }
                RegLab2 = itemlabtotals.ToString("C2");
                RegMat2 = itemmattotals.ToString("C2");
                itemtotals = itemlabtotals + itemmattotals;
                AllItemTotals += itemtotals;
                AllLabItemTotals += itemlabtotals;
                AllMatItemsTotals += itemmattotals;
                RegTot2 = itemtotals.ToString("C2");

                try { itemlabtotals = _tools.getSQL_double(string.Format(SQLBase, tabletype, linewoid, "", "", ">=", "OR", "0"), null); }
                catch { itemlabtotals = 0; }
                try { itemmattotals = _tools.getSQL_double(string.Format(SQLBase, tabletype, linewoid, "!", "NOT", "<", "AND", "0"), null); }
                catch { itemmattotals = 0; }
                VNCLab2 = itemlabtotals.ToString("C2");
                VNCMat2 = itemmattotals.ToString("C2");
                itemtotals = itemlabtotals + itemmattotals;
                AllItemTotals += itemtotals;
                AllLabItemTotals += itemlabtotals;
                AllMatItemsTotals += itemmattotals;
                VNCTot2 = itemtotals.ToString("C2");

            }
            AllLab = AllLabItemTotals.ToString("C2");
            AllMat = AllMatItemsTotals.ToString("C2");
            AllTot = AllItemTotals.ToString("C2");


        }

        OUTPUT = string.Format
            (@"<TotalsReturn>
             <RegLab2>{0}</RegLab2>
             <RegMat2>{1}</RegMat2> 
             <RegTot2>{2}</RegTot2>
             <VNCLab2>{3}</VNCLab2>
             <VNCMat2>{4}</VNCMat2>
             <VNCTot2>{5}</VNCTot2>
             <BlnLab2>{6}</BlnLab2> 
             <BlnMat2>{7}</BlnMat2>
             <BlnTot2>{8}</BlnTot2>
             <ICRLab2>{9}</ICRLab2>
             <ICRMat2>{10}</ICRMat2>
             <ICRTot2>{11}</ICRTot2>
             <DNILab2>{12}</DNILab2>
             <DNIMat2>{13}</DNIMat2>
             <DNITot2>{14}</DNITot2>
             <VCLab2>{15}</VCLab2>
             <VCMat2>{16}</VCMat2>
             <VCTot2>{17}</VCTot2>
             <AllLab>{18}</AllLab>
             <AllMat>{19}</AllMat>
             <AllTot>{20}</AllTot>
             <quoteid>{21}</quoteid>
             <erid>{22}</erid>			 
            </TotalsReturn>",
                            RegLab2,        //0
                            RegMat2,        //1
                            RegTot2,        //2
                            VNCLab2,        //3
                            VNCMat2,        //4
                            VNCTot2,        //5
                            BlnLab2,        //6
                            BlnMat2,        //7
                            BlnTot2,        //8
                            ICRLab2,        //9
                            ICRMat2,        //10
                            ICRTot2,        //11
                            DNILab2,        //12
                            DNIMat2,        //13
                            DNITot2,        //14
                            VCLab2,         //15
                            VCMat2,         //16
                            VCTot2,         //17
                            AllLab,         //18
                            AllMat,         //19
                            AllTot,         //20
                            quoteid,        //21
                            erid            //22
							);
		Toolbox.QuickReponse(Response, OUTPUT);

    }    
}
