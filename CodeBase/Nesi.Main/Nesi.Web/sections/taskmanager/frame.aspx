<%@ Page Language="C#" MasterPageFile="~/IntraDefault.master" AutoEventWireup="true" Inherits="taskmanager" Title="Task Manager" Codebehind="frame.aspx.cs" %>







<%@ Register src="../../modules/layout_control.ascx" tagname="LayoutControl" tagprefix="lc" %>


<asp:Content ID="Content1" ContentPlaceHolderID="cphMasterMenu" Runat="Server">
	<div id="divMenu" runat="server">
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMasterLeft" Runat="Server">
	<div id="divSide" runat="server">
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMasterSubMenu" Runat="Server">
	</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMasterBody" Runat="Server">
	<script type="text/javascript">

 function resizeIframe(obj)
 {

   obj.style.height = (obj.contentWindow.document.body.scrollHeight + 20) + 'px';
	
	if (obj.contentWindow.document.body.scrollHeight<800)
	{
	obj.style.height='800px';
	}
 }

 function Onddlstatus_Init(s, e) {
    ChangeComboColors(); 

}

function ChangePriorityComboColors(s)
{
if (s.GetValue()=="1")
	{
	s.GetMainElement().style.backgroundColor="white";
	s.GetInputElement().style.backgroundColor="white";
	}
	else if (s.GetValue()=="2")
	{
	s.GetMainElement().style.backgroundColor="red";
	s.GetInputElement().style.color="white";
	s.GetInputElement().style.backgroundColor="red";
	}
	else if (s.GetValue()=="3")
	{
	s.GetMainElement().style.backgroundColor="darkorange";
	s.GetInputElement().style.backgroundColor="darkorange";
	}
	else if (s.GetValue()=="4")
	{
	s.GetMainElement().style.backgroundColor="khaki";
	s.GetInputElement().style.backgroundColor="khaki";
	}
	else if (s.GetValue()=="6")
	{
	s.GetMainElement().style.backgroundColor="lightyellow";
	s.GetInputElement().style.backgroundColor="lightyellow";
	}
		else if (s.GetValue()=="5")
	{
	s.GetMainElement().style.backgroundColor="palegreen";
	s.GetInputElement().style.backgroundColor="palegreen";
	}

}
 function ChangeStatusComboColors(s) 
 {
	if (s.GetValue()=="1")
	{
	s.GetMainElement().style.backgroundColor="white";
	s.GetInputElement().style.backgroundColor="white";
	}
	else if (s.GetValue()=="2")
	{
	s.GetMainElement().style.backgroundColor="lightgreen";
	s.GetInputElement().style.backgroundColor="lightgreen";
	}
	else if (s.GetValue()=="3")
	{
	s.GetMainElement().style.backgroundColor="green";
	s.GetInputElement().style.backgroundColor="green";
	s.GetInputElement().style.color="white";
	}
	else if (s.GetValue()=="4")
	{
	s.GetMainElement().style.backgroundColor="lightyellow";
	s.GetInputElement().style.backgroundColor="lightyellow";
	}
	else if (s.GetValue()=="5")
	{
	s.GetMainElement().style.backgroundColor="blue";
	s.GetInputElement().style.backgroundColor="blue";
	s.GetInputElement().style.color="white";
	}
}


 </script>

<table width="100%">
<tr>
<td width="100%">
									<iframe runat="server" ID="I1" frameborder="0" name="I1" scrolling="yes" 
		style="border-top-style: none; border-right-style: none; border-left-style: none; border-bottom-style: none" 
		width="100%" height="1060" __designer:mapid="13a5" src="list.aspx"></iframe>


</td>
</tr>


</table>



</asp:Content>

