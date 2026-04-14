<%@ Page Language="C#" AutoEventWireup="true" Inherits="sections_admin_errorMessageBoard_it_dashboard" 
   EnableTheming="true" Theme ="DevDashboard01"  Codebehind="it_dashboard.aspx.cs" %>
<%@ Register Src="~/modules/invoiceService_runTimes.ascx" TagPrefix="uc1" TagName="invoiceService_runTimes" %>
<%@ Register Src="~/modules/event_viewer.ascx" TagPrefix="uc1" TagName="event_viewer" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>MessageBoard</title>
      <script src="//ajax.googleapis.com/ajax/libs/jquery/2.1.3/jquery.min.js"></script>
    <link rel="stylesheet" href="//code.jquery.com/ui/1.11.4/themes/smoothness/jquery-ui.css">
  <script src="//code.jquery.com/jquery-1.10.2.js"></script>
  <script src="//code.jquery.com/ui/1.11.4/jquery-ui.js"></script>
</head>
<body>
    <form id="form1" runat="server" style="border: 3px solid black;font-family:'Arial';">

        

        <div style="padding: 7px;" >
            <div id="box1">
                <uc1:event_viewer runat="server" ID="event_viewer" />
            </div>
            <div id="box2">
                <uc1:invoiceService_runTimes runat="server" ID="invoiceService_runTimes1" />
            </div>
        </div>

    </form>
     
    	<script>
    	    $(document).ready(function () {

    	        $("body").css("overflow", "hidden");

    	        $("#box2").hide();
                
    	        boxsMaster = [35,10];
    	        box = boxsMaster.slice(0);
    	        toshow = 1;

    	        window.setInterval(function () {

    	            $("#box1").hide();
    	            $("#box2").hide();

    	            $("#box" + toshow).show();
    	            console.log(boxsMaster);

    	           

    	            if (toshow >= 2 && box[toshow - 1] == 1) {
    	                toshow = 1;
    	                box = boxsMaster.slice(0);
    	            }else{
    	                if (box[toshow-1] == 1) {
    	                    toshow++;
    	                    console.log("++");
    	                }
    	                else
    	                    box[toshow-1]--;
                    }
    	        }, 1000);


    	    });

		</script>
</body>
</html>
