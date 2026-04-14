<%@ Page Title="Huddle" Language="C#" MasterPageFile="~/IntraDefault.master" AutoEventWireup="true"  Inherits="sections_member_huddle_index" EnableTheming="True" Codebehind="index.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="header_placeholder" runat="Server">
	
	<script type="text/javascript">
		var menuVisible = false;
	</script>
    <style type="text/css">

       mg .ar {
           background-color: #CCFFFF;
       }
       mg .tr.hover {
           background-color: #F0F0F0;
       }

		.highlight
			{
			border:solid 1px #ccc;
			min-height:72px;
			margin:2px;
			border-radius:5px;
			height:72px;
			width:100%;
			max-width:880px;
			background-color:#fff;
			}
		.task_li
			{
			list-style: none;
			padding: 0;
			}
			      td
        {
            vertical-align: top;
			}

        .draggingStyle
        {
            background-color: lightblue;
        }

        .targetGrid
        {
            background-color: lightcoral;
        }
        
#siteMenuBar
{
position: fixed;
top: 0px;
margin-left: -150px;
width: 200px;
background-color: #ccc;
}
 
#siteMenuBar > #menuContainer
{
margin-left: auto;
margin-right: auto;
width: 150px;
padding-top: 5px;
}
 
 
 
        
        
	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMasterMenu" runat="Server">
                <div id='divMenu' runat='server'></div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cphMasterBody" runat="Server">
    <div style="position:relative;">
    <div id="toggle_huddle_menu_button" style="height:25px;width:25px;top:0px;left:0px;position: absolute;  background-color: #E6E6E6;">
        <img alt="" src="../../../mobile/img/menu_button.png" imageurl="../../../mobile/img/menu_button.png" height="25" width="25" />
                </div>
      
    <script type="text/javascript">
    //    $("#huddle_menu").hide("slide", { direction: "left" }, 10);
	
        $(document).ready(function () {
            getGridDeleteButtons().on("click", function () {
                alert("delete?");
            });

            
          

            $("#toggle_huddle_menu_button").click(function () {
              
                $("#huddle_menu").toggle("slide");
                cp_left_panel.PerformCallback();
               menuVisible = !menuVisible;
            });
            

        })
        function getGridDeleteButtons() {
            var regexp = new RegExp("aspxGVScheduleCommand\\('gv',\\['delete',-?\\d+(,|\\])+");
            var buttons = $(gv.GetMainElement()).find("a, img").filter(function () {
                return this.onclick && regexp.test(this.onclick.toString());
            });
            return buttons;
        }


        function InitalizejQuery() {
            
    		$('.draggable').draggable({
    			helper: 'clone',
    			start: function (ev, ui) {
    				var $draggingElement = $(ui.helper);
    				$draggingElement.width(gv.GetWidth());
    			}
    		});
    		$('.draggable').droppable({
    			activeClass: "hover",
    			tolerance: "intersect",
    			hoverClass: "activeHover",
    			drop: function (event, ui) {
    				var draggingSortIndex = ui.draggable.attr("sortOrder");
    				var targetSortIndex = $(this).attr("sortOrder");
    				if (draggingSortIndex != null) {
    					gv.PerformCallback("DRAGROW|" + draggingSortIndex + '|' + targetSortIndex);
    				}
    			}
    		});

    		$('.draggableRow').draggable({ //http://api.jqueryui.com/draggable/
    			helper: 'clone',
    			start: function (ev, ui) {
    				var $sourceElement = $(ui.helper.context);
    				var $draggingElement = $(ui.helper);
    				var sourceGrid = ASPxClientGridView.Cast($draggingElement.hasClass("right") ? "gv_source" : "");

    				//style elements
    				$sourceElement.addClass("draggingStyle");
    				$draggingElement.addClass("draggingStyle");
    				$draggingElement.width(sourceGrid.GetWidth());

    				//find key
    				rowKey = sourceGrid.GetRowKey(sourceGrid.GetTopVisibleIndex() + $sourceElement.index() - 1);
    			},
    			stop: function (e, ui) {
    				$(".draggingStyle").removeClass("draggingStyle");
    			}
    		});

    		var settings = function (className) {
    			return {
    				tolerance: "intersect",
    				accept: className,
    				drop: function (ev, ui) {
    					$(".targetGrid").removeClass("targetGrid");
    					var leftToRight = ui.helper.hasClass("left");
    					gv.PerformCallback("ADD_SOURCE|" + rowKey);
    				},
    				over: function (ev, ui) {
    					$(this).addClass("targetGrid");
    				},
    				out: function (ev, ui) {
    					$(".targetGrid").removeClass("targetGrid");
    				}
    			};
    		};

    		$(".droppableRight").droppable(settings(".left"));
    		$(".droppableLeft").droppable(settings(".right"));

    	}


    	function OnControlsInitialized(s, e) {
    	   

    		$('.draggableRow').draggable({ //http://api.jqueryui.com/draggable/
    			helper: 'clone',
    			start: function (ev, ui) {
    				var $sourceElement = $(ui.helper.context);
    				var $draggingElement = $(ui.helper);
    				var sourceGrid = ASPxClientGridView.Cast($draggingElement.hasClass("right") ? "gv_source" : "");

    				//style elements
    				$sourceElement.addClass("draggingStyle");
    				$draggingElement.addClass("draggingStyle");
    				$draggingElement.width(sourceGrid.GetWidth());

    				//find key
    				rowKey = sourceGrid.GetRowKey(sourceGrid.GetTopVisibleIndex() + $sourceElement.index() - 1);
    			},
    			stop: function (e, ui) {
    				$(".draggingStyle").removeClass("draggingStyle");
    			}
    		});

    		var settings = function (className) {
    			return {
    				tolerance: "intersect",
    				accept: className,
    				drop: function (ev, ui) {
    					$(".targetGrid").removeClass("targetGrid");
    					var leftToRight = ui.helper.hasClass("right");
    					gv.PerformCallback("ADD_SOURCE|" + rowKey);
    				},
    				over: function (ev, ui) {
    					$(this).addClass("targetGrid");
    				},
    				out: function (ev, ui) {
    					$(".targetGrid").removeClass("targetGrid");
    				}
    			};
    		};

    		$(".droppableRight").droppable(settings(".left"));
    		$(".droppableLeft").droppable(settings(".right"));

    	}

    	

    	function MyFunction() {
    	    InitalizejQuery();
    	    
    	}

    	

    	var rowKey = -1;

  //  	$(document).ready(function () {
   // 		$(".context").animate({
   // 			backgroundColor: "#aa0000",
   // 			color: "#fff",
   // 			width: 30
    		// 		}, 1000);
//			var options = {};
//			$(".context").hide("slide", options, 10, callback);
			
//$(".mmenu").hover(function(){
	//$(".context").removeAttr("style").hide().fadeIn();
//}, function () {
//	var options = {};
//	$(".context").hide("slide", options, 10, callback);
//	});

  //  });

    //	$("#lbltitle").click(function () {
    //	    $("#toggle_menu").toggle("slide");
    //	});


    	function callback() {

    	  //  setTimeout(function () { $("#huddle_menu").hide("slide"); }, 1000);

  //    setTimeout(function() {
  //      $(".context" ).removeAttr( "style" ).hide().fadeIn();
   //   }, 5000 );
};

	</script>
    <style>
 
  </style>
	<asp:UpdatePanel ID="UpdatePanel1" runat="server">
		<ContentTemplate>
            
            <div id="huddle_menu"  style="position:absolute; float:left; top:25px; left:0px; width: 200px; display: none;  background-color:#EFEFEF; white-space: nowrap; ">
            <div id="huddle_menu_top"  style="position:relative; top:0px; left:0px; width: 200px;  background-color:#EFEFEF; padding-left:3px; ">
						<dx:ASPxComboBox ID="view_huddles" runat="server" AutoPostBack="True" 
							Caption="Viewing Huddle" ClientInstanceName="view_huddles" DropDownRows="100" 
							NullText="Select Huddle" OnCallback="view_huddles_Callback" 
							OnSelectedIndexChanged="view_huddles_SelectedIndexChanged" Theme="NETheme01" 
							ValueType="System.Int32" Width="180px">
							
							<CaptionSettings Position="Top" />
							<CaptionCellStyle Width="100px">
							</CaptionCellStyle>
						</dx:ASPxComboBox>
						<br />
<dx:ASPxButton ID="new_huddle" runat="server" AutoPostBack="False" 
										Font-Bold="False" Text="New Huddle" Theme="NETheme01">
										<ClientSideEvents Click="function(s, e) {
cp_left_panel.SetVisible(true);
lbltitle.SetText('');
save_huddle.SetText('Add');
view_huddles.SetText('');
view_huddles.SetSelectedIndex(-1);
	cp_left_panel.PerformCallback('new');
	gv.SetVisible(false);
	if (pc!=null && pc!='undefined')
	{
pc.SetVisible(false);

}



}" />
									</dx:ASPxButton>
									<br />
						<dx:ASPxCallbackPanel ID="cp_left_panel" runat="server" Width="200px" 
							ClientInstanceName="cp_left_panel" oncallback="cp_left_panel_Callback" ClientVisible="False" BackColor="#EFEFEF">
							
							
						<PanelCollection>
<dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
						<table align="right" style="width:100%;" bgcolor="#EFEFEF">
							<tr>
								<td align="left" colspan="2">
									
								</td>
							</tr>
							<tr>
								<td align="left" colspan="2">
									&nbsp;</td>
							</tr>
							<tr>
								<td align="left" colspan="2">
									&nbsp;</td>
							</tr>
							<tr>
								<td align="left" colspan="2">
									<dx:ASPxTextBox ID="name" runat="server" Caption="Name:" 
										ClientInstanceName="ex_name" Theme="NETheme01" Width="180px">
										<CaptionSettings Position="Top" />
										<CaptionCellStyle Width="100px">
										</CaptionCellStyle>
									</dx:ASPxTextBox>
								</td>
							</tr>
							<tr>
								<td align="left" nowrap="nowrap" colspan="2">
									<dx:ASPxMemo ID="description" runat="server" Caption="Description" 
										ClientInstanceName="ex_description" Height="100px" Theme="NETheme01" 
										Width="180px">
										<CaptionSettings Position="Top" />
										<CaptionCellStyle Width="100px">
										</CaptionCellStyle>
									</dx:ASPxMemo>
								</td>
							</tr>
							<tr>
								<td align="left" colspan="2">
									<dx:ASPxComboBox ID="captain" runat="server" Caption="Captain:" 
										ClientInstanceName="ex_captain" DropDownWidth="150px" TextField="member_fullname" 
										Theme="NETheme01" ValueField="member_id" ValueType="System.Int32" Width="180px" 
										DataSourceID="sqlmembers">
										<CaptionSettings Position="Top" />
										<CaptionCellStyle Width="100px">
										</CaptionCellStyle>
									</dx:ASPxComboBox>
								</td>
							</tr>
							<tr>
								<td align="left" colspan="2">
									&nbsp;</td>
							</tr>
							<tr>
								<td align="left" colspan="2">
									<dx:ASPxTokenBox ID="tb_users" runat="server" AllowCustomTokens="False" 
										AllowMouseWheel="True" Caption="Teammates" DataSourceID="sqlmembers" 
										ItemValueType="System.Int32"  TextField="member_fullname" 
										Theme="NETheme01" Tokens="" ValueField="member_id" CallbackPageSize="10" EnableCallbackMode="True" 
										Width="180px">
										<CaptionSettings Position="Top" />
									</dx:ASPxTokenBox>
								</td>
							</tr>
							<tr>
								<td align="left" colspan="2">
									<dx:ASPxDateEdit ID="dte" runat="server" DisplayFormatString="yyyy-MM-dd HH:mm" 
										EditFormat="Custom" EditFormatString="yyyy-MM-dd HH:mm" Theme="NETheme01" Caption="Date of Huddle" 
										Width="180px">
										<TimeSectionProperties Visible="True">
										</TimeSectionProperties>
										<CaptionSettings Position="Top" />
									</dx:ASPxDateEdit>
								</td>
							</tr>
							<tr>
								<td align="left" colspan="2">
									&nbsp;</td>
							</tr>
							<tr>
								<td align="left" colspan="2">
									&nbsp;</td>
							</tr>
							<tr>
								<td align="left" colspan="2">
									<dx:ASPxCheckBox ID="active" runat="server" CheckState="Unchecked" 
										ClientInstanceName="ex_active" Text="Active:" TextAlign="Left" 
										Theme="NETheme01" Width="130px">
									</dx:ASPxCheckBox>
								</td>
							</tr>
							<tr>
								<td align="left" colspan="2">
									<dx:ASPxLabel ID="createddate" runat="server" Text="Created Date" 
										Theme="NETheme01">
									</dx:ASPxLabel>
								</td>
							</tr>
							<tr>
								<td align="left" colspan="2">
									&nbsp;</td>
							</tr>
							<tr>
								<td align="left">
									<dx:ASPxButton ID="save_huddle" runat="server" Font-Bold="False" Height="25px" 
										onclick="save_huddle_Click" Text="Save" Theme="NETheme01" ClientInstanceName="save_huddle" Width="90px" UseSubmitBehavior="False">
										<Image Height="16px" Url="/images/icon/icon[save].gif" Width="16px">
										</Image>
									</dx:ASPxButton>
								</td>
								<td align="left" style="text-align: right">
									<dx:ASPxButton ID="delete_huddle" runat="server" AutoPostBack="False" 
										ClientInstanceName="delete_huddle" Font-Bold="False" Height="25px" 
										OnClick="Delete_huddle_Click" Text="Delete" Theme="NETheme01" ClientVisible="False" Width="90px" UseSubmitBehavior="False">
										<ClientSideEvents Click="function(s, e) {
	
e.processOnServer = confirm('Are you sure you want to delete the huddle?');

}" />
										<Image Height="16px" Url="~/images/icon/icon[delete].gif" Width="16px">
										</Image>
									</dx:ASPxButton>
								</td>
							</tr>
							<tr>
								<td align="left" colspan="2">
									&nbsp;</td>
							</tr>
							<tr>
								<td align="left" colspan="2"> 
									&nbsp;</td>
							</tr>
							<tr>
								<td align="left" colspan="2">
									&nbsp;</td>
							</tr>
							<tr>
								<td align="left" colspan="2">
									&nbsp;</td>
							</tr>
							<tr>
								<td align="left" colspan="2">
									&nbsp;</td>
							</tr>
						</table>
						</dx:PanelContent>
</PanelCollection>
						</dx:ASPxCallbackPanel>
					</div>
           
            </div>
            
			<table cellpadding="10" style="width: 100%;">
				<tr>
					<td align="left" valign="top" width="100%">
						<table width="100%">
							<tr>
								<td width="100%">
									<dx:ASPxLabel ID="lbltitle" runat="server" Font-Size="18pt" 
										Text="No Huddle Selected" Theme="NETheme01" ClientInstanceName="lbltitle">
									</dx:ASPxLabel>
									
									<br />
								</td>
								<td width="10%" >
									&nbsp;</td>
							</tr>
							<tr>
								<td colspan="2" width="100%" >
									<dx:ASPxTabControl ID="pc" runat="server" AutoPostBack="True" 
										ClientInstanceName="pc" onactivetabchanged="pc_ActiveTabChanged" 
										Theme="NETheme01" Width="100%" onprerender="pc_PreRender">
									</dx:ASPxTabControl>
									<dx:ASPxGridView ID="gv" runat="server" AutoGenerateColumns="False" 
										ClientInstanceName="gv" ClientVisible="False" DataSourceID="SqlDataSource1" 
										KeyFieldName="id" oncustomcallback="gv_CustomCallback" 
										oncustomjsproperties="gv_CustomJSProperties" 
										onhtmlrowprepared="gv_HtmlRowPrepared" Width="100%" 
										oncustombuttoncallback="gv_CustomButtonCallback" onbatchupdate="gv_BatchUpdate" 
										oncustombuttoninitialize="gv_CustomButtonInitialize" OnHtmlDataCellPrepared="gv_HtmlDataCellPrepared"  CssClass="mg" EnableTheming="False" >
										<ClientSideEvents Init="function(s,e)
											{
											if(menuVisible)
												{
												$('#huddle_menu').show(0);
												}
											}" CustomButtonClick="function(s, e) {

if ((e.buttonID != 'delete')&&(e.buttonID!='push')) return;

if (e.buttonID=='delete')
{
if (confirm('Are you sure you want to delete this task?'))
{
gv.PerformCallback('DELETE|'+e.visibleIndex);
}
}
else
{

hdn_pop_task['key'] = e.visibleIndex;

pop_push.PerformCallback(hdn_pop_task['key']);
pop_push.Show();

}	

}"  BatchEditStartEditing="function(s, e) {
edit_mode.SetVisible(true);
hl_save.SetVisible(true);
hl_cancel.SetVisible(true);
	$( &quot;.draggable&quot; ).draggable( &quot;destroy&quot; ); 

}" 


/>
										<Columns>
											<dx:GridViewCommandColumn Caption=" " VisibleIndex="0" ButtonType="Image" 
												 Width="65px" SelectAllCheckboxMode="Page" ShowSelectCheckbox="True">
												<CustomButtons>
													<dx:GridViewCommandColumnCustomButton ID="delete">
														<Image Url="~/images/icon/icon[delete].gif">
														</Image>
													</dx:GridViewCommandColumnCustomButton>
													<dx:GridViewCommandColumnCustomButton ID="push">
														<Image Url="~/images/icon/icon[right].gif">
														</Image>
													</dx:GridViewCommandColumnCustomButton>
												</CustomButtons>
												<CellStyle Wrap="False">
													<Paddings PaddingBottom="15px" PaddingTop="15px" />
												</CellStyle>
											</dx:GridViewCommandColumn>
											<dx:GridViewDataTextColumn FieldName="id" Visible="False" VisibleIndex="1">
												<EditFormSettings Visible="False" />
											</dx:GridViewDataTextColumn>
											<dx:GridViewDataTextColumn FieldName="source_id" Visible="False" 
												VisibleIndex="4">
											</dx:GridViewDataTextColumn>
											<dx:GridViewDataTextColumn FieldName="type" Visible="False" VisibleIndex="5">
											</dx:GridViewDataTextColumn>
											<dx:GridViewDataTextColumn Caption="Source" FieldName="huddle_source" 
												Visible="False" VisibleIndex="6">
												<EditFormSettings Visible="False" />
											</dx:GridViewDataTextColumn>
											<dx:GridViewDataDateColumn Caption="Due Date" FieldName="date_due" 
												VisibleIndex="11" Width="80px">
												<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd">
													
												</PropertiesDateEdit>
											</dx:GridViewDataDateColumn>
											<dx:GridViewDataTextColumn FieldName="link" Visible="False" VisibleIndex="12">
											</dx:GridViewDataTextColumn>
											<dx:GridViewDataTextColumn FieldName="ticket_status" Visible="False" 
												VisibleIndex="13">
											</dx:GridViewDataTextColumn>
											<dx:GridViewDataTextColumn FieldName="assigned_to" Visible="False" 
												VisibleIndex="14">
											</dx:GridViewDataTextColumn>
											<dx:GridViewDataTextColumn FieldName="order_n" Visible="False" 
												VisibleIndex="15">
											</dx:GridViewDataTextColumn>
											<dx:GridViewDataTextColumn FieldName="color" Visible="False" VisibleIndex="16">
											</dx:GridViewDataTextColumn>
											<dx:GridViewDataComboBoxColumn Caption="Owner" FieldName="assigned_to_id" 
												Visible="False" VisibleIndex="3">
												<PropertiesComboBox DataSourceID="sqlmembers" TextField="member_fullname" 
													ValueField="member_id">
													
												</PropertiesComboBox>
											</dx:GridViewDataComboBoxColumn>
											<dx:GridViewDataHyperLinkColumn Caption="Item" FieldName="task" 
												VisibleIndex="2" Width="150px" EditFormSettings-Visible="True">
												<EditFormSettings Visible="False" />
												<DataItemTemplate>
													<dx:ASPxHyperLink ID="ASPxHyperLink1" runat="server" Cursor="pointer" 
														NavigateUrl="<%# (Eval(&quot;link&quot;)!=&quot;&quot;)?string.Format(&quot;javascript:boing('{0}', 'stuff{1}', 1100,750)&quot;, Eval(&quot;link&quot;), Eval(&quot;id&quot;)) : &quot;&quot; %>" 
														Text='<%# Eval("task") %>' Theme="NETheme01">
													</dx:ASPxHyperLink>
												</DataItemTemplate>
												<CellStyle Wrap="True">
												</CellStyle>
											</dx:GridViewDataHyperLinkColumn>
											<dx:GridViewDataComboBoxColumn Caption="Status" FieldName="status" 
												VisibleIndex="8" Width="80px" PropertiesComboBox-ValueType="System.Int32">
												<PropertiesComboBox>
													
													<Items>
														
														<dx:ListEditItem Text="Not Started" Value="1" />
														
														<dx:ListEditItem Text="Started" Value="2" />
														
														<dx:ListEditItem Text="Completed" Value="3" />
														<dx:ListEditItem Text="Pushed" Value="4" />
														
													</Items>
													
												</PropertiesComboBox>
											</dx:GridViewDataComboBoxColumn>
											<dx:GridViewDataMemoColumn Caption="Notes" FieldName="task_description" 
												VisibleIndex="9" Width="50%" PropertiesMemoEdit-Rows="10">
												
												<PropertiesMemoEdit Rows="10">
												</PropertiesMemoEdit>
												
												<CellStyle Wrap="True">
												</CellStyle>
											</dx:GridViewDataMemoColumn>
											<dx:GridViewDataMemoColumn Caption="To Be Done" FieldName="to_be_done" 
												VisibleIndex="10" Width="50%">
												<PropertiesMemoEdit Rows="10">
												</PropertiesMemoEdit>
												<CellStyle Wrap="True">
												</CellStyle>
											</dx:GridViewDataMemoColumn>
											<dx:GridViewDataComboBoxColumn Caption="Tabled By" FieldName="created_by" 
												ReadOnly="True" VisibleIndex="7" Width="93px">
												<PropertiesComboBox DataSourceID="sqlmembers" TextField="member_fullname" 
													ValueField="member_id" ValueType="System.Int32">
												</PropertiesComboBox>
											</dx:GridViewDataComboBoxColumn>
										</Columns>
										<SettingsBehavior ColumnResizeMode="Control" />
										<SettingsPager Mode="ShowAllRecords" Visible="False">
										</SettingsPager>
										<SettingsEditing Mode="Batch">
										</SettingsEditing>
										<Settings ShowFooter="True" />
										<SettingsText EmptyDataRow=" " 
											Title="Agenda Items" />
								
										<SettingsCommandButton>
											<DeleteButton Image-Url="~/images/icon/icon[delete].gif" Image-Width="16px" Text="Delete" />
											<EditButton Image-Url="~/images/icon/icon[edit].gif" Image-Width="16px" Text="Edit" />
											<NewButton Image-Url="~/images/icon/icon[add].gif" Image-Width="16px" Text="New" />
										</SettingsCommandButton>										
										
										<Styles>
											<RowHotTrack CssClass="hover">
                                            </RowHotTrack>
											<AlternatingRow CssClass="ar" BackColor="Transparent" Enabled="False">
                                            </AlternatingRow>
											<SelectedRow BackColor="#FF9900">
											</SelectedRow>
											<Row CssClass="draggable">
											</Row>
											<Table CssClass="droppableLeft">
											    <Border BorderStyle="None" />
											</Table>
											<CommandColumn HorizontalAlign="Left">
											</CommandColumn>
											<StatusBar HorizontalAlign="Right" Wrap="False">
											</StatusBar>
											<BatchEditModifiedCell BackColor="#FFFFCC">
											</BatchEditModifiedCell>
										</Styles>
										<Templates>
											<FooterRow>
												<dx:ASPxLabel ID="ASPxLabel2" runat="server" 
													Text="Drag an Item from the right source lists -&gt;" Width="100%">
												</dx:ASPxLabel>
											</FooterRow>
											<StatusBar>
												<table style="width:100%;">
													<tr>
														<td style="text-decoration: blink; font-family: Arial, Helvetica, sans-serif; font-size: 14px" 
															width="100%" align="left">
															<dx:ASPxLabel ID="edit_mode" runat="server" ClientInstanceName="edit_mode" 
																ClientVisible="False" Font-Bold="True" Font-Names="Arial" ForeColor="Red" 
																Text="Edit Mode">
															</dx:ASPxLabel>
														</td>
														<td>
															<dx:ASPxHyperLink ID="hl_save" runat="server" Cursor="pointer" Text="Save" 
																Width="50px" ClientVisible="False" ClientInstanceName="hl_save">
																<ClientSideEvents Click="function(s, e){edit_mode.SetVisible(false);hl_save.SetVisible(false);hl_cancel.SetVisible(false); gv.UpdateEdit(); }" />
															</dx:ASPxHyperLink>
														</td>
														<td>
															<dx:ASPxHyperLink ID="hl_cancel" runat="server" Cursor="pointer" Text="Cancel" ClientVisible="False" ClientInstanceName="hl_cancel">
																<ClientSideEvents Click="function(s, e){  edit_mode.SetVisible(false);hl_save.SetVisible(false);hl_cancel.SetVisible(false); gv.CancelEdit(); InitalizejQuery(); }" />
															</dx:ASPxHyperLink>
														</td>
													</tr>
												</table>
											</StatusBar>
										</Templates>
									</dx:ASPxGridView>
								</td>
							</tr>
							<tr>
								<td colspan="2">
									<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
										ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
										ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT 
	a.id,
	a.source_id,
	IF(a.huddle_source = 1, CONCAT(d.ticketheader_id,'-',d.ticketheader_issue, ' - (', f.ticketstatus_status, ')'), a.linetext) task, 
	e.name type, 
	e.id huddle_source,
	a.date_due,
	if(a.status=1,'Not Started',if(a.status=2,'Started',if (a.status=3,'Completed','Pushed'))) status,
	a.link link,
	c.member_id assigned_to_id,
	IFNULL(d.ticketheader_status_id, 0) ticket_status,
	c.member_fullname assigned_to,
	a.description task_description,
	b.color,
	a.order_n,
	a.created_by,
	a.to_be_done
FROM 
	huddle_task a 
LEFT JOIN 
	huddle_user b on a.assigned_to = b.id 
LEFT JOIN 
	member c on b.member = c.member_id 
LEFT JOIN 
	ticketheader d ON a.source_id = d.ticketheader_id
LEFT JOIN
	huddle_source e ON a.huddle_source = e.id
LEFT JOIN
	ticketstatus f ON d.ticketheader_status_id = f.ticketstatus_id
WHERE 
	a.huddle = ?id and c.member_id = @mid
ORDER BY 
	a.order_n" UpdateCommand="update huddle_task set [order_n] = ?order_n where [id] = ?id">
										<SelectParameters>
											<asp:ControlParameter ControlID="view_huddles" Name="id" PropertyName="Value" />
											<asp:Parameter Name="@mid" />
										</SelectParameters>
										<UpdateParameters>
											<asp:Parameter Name="order_n" />
											<asp:Parameter Name="id" />
										</UpdateParameters>
									</asp:SqlDataSource>
									<asp:SqlDataSource ID="sqlmembers" runat="server" 
										ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
										ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
										SelectCommand="Select member_id,member_fullname from member where member_status = 'Active'">
									</asp:SqlDataSource>
									<asp:SqlDataSource ID="sql_helper" runat="server" 
								ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
								ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
								
										SelectCommand="select id from huddle_task where order_n = ?DisplayOrder and huddle=@id and assigned_to=@mid">
										<SelectParameters>
											<asp:Parameter Name="DisplayOrder" />
											<asp:Parameter Name="mid" />
											<asp:ControlParameter ControlID="view_huddles" Name="id" PropertyName="Value" />
										</SelectParameters>
									</asp:SqlDataSource>
							<br />
								</td>
							</tr>
							<asp:HiddenField ID="hdnid" runat="server" />
							
							
							
							
						</table>
					</td>
					<td bgcolor="#EFEFEF" valign="top" width="400px">
						<table style="width:100%;">
							<tr>
								<td>
									<dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Custom Agenda Item" 
										Theme="NETheme01">
									</dx:ASPxLabel>
								</td>
								<td>
									&nbsp;</td>
							</tr>
							<tr>
								<td width="100%">
									<dx:ASPxMemo ID="txt_custom_item" runat="server" Caption="Item" 
										Theme="NETheme01" Width="100%" Rows="4">
										<CaptionCellStyle Width="70px">
										</CaptionCellStyle>
									</dx:ASPxMemo>
								</td>
								<td rowspan="3">
									<dx:ASPxButton ID="btn_add_custom" runat="server" Text="Add" Theme="NETheme01" 
										AutoPostBack="False" onclick="btn_add_custom_Click">
									</dx:ASPxButton>
								</td>
							</tr>
							<tr>
								<td>
									<dx:ASPxDateEdit ID="dte_custom_date" runat="server" Caption="Due Date" 
										Theme="NETheme01" Width="100%">
										<CaptionCellStyle Width="70px">
										</CaptionCellStyle>
									</dx:ASPxDateEdit>
								</td>
							</tr>
							<tr>
								<td>
									<dx:ASPxComboBox ID="ddl_custom_member" runat="server" Caption="Assigned To" 
										TextField="member_fullname" Theme="NETheme01" ValueField="member_id" 
										ValueType="System.String" Width="100%">
										<CaptionCellStyle Width="70px">
										</CaptionCellStyle>
									</dx:ASPxComboBox>
								</td>
							</tr>
						</table>
						<br />
						<dx:ASPxTabControl ID="ASPxTabControl1" runat="server" 
							EnableTabScrolling="True" Theme="NETheme01" Width="400px">
							<ClientSideEvents ActiveTabChanged="function(s, e) {
	gv_source.PerformCallback();
}" />
						</dx:ASPxTabControl>
						<dx:ASPxGridView ID="gv_source" runat="server" AutoGenerateColumns="False" 
							Theme="NETheme01" Width="400px" ClientInstanceName="gv_source" 
							oncustomcallback="gv_source_CustomCallback" KeyFieldName="id">
							<Columns>
								<dx:GridViewCommandColumn ShowClearFilterButton="True" VisibleIndex="0" 
									Caption=" " Visible="False">
								</dx:GridViewCommandColumn>
								<dx:GridViewDataTextColumn FieldName="id" VisibleIndex="1" Width="35px" 
									Visible="False">
									<CellStyle>
										<Paddings PaddingBottom="15px" PaddingTop="15px" />
									</CellStyle>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn FieldName="visible_id" VisibleIndex="2" Width="35px" 
									Caption=" " Visible="False">
									<Settings AutoFilterCondition="Contains" />
									<CellStyle>
										<Paddings PaddingBottom="15px" PaddingTop="15px" />
									</CellStyle>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn FieldName="name" VisibleIndex="4" Width="100%" Caption="Link">
									<Settings AutoFilterCondition="Contains" />
									<DataItemTemplate>
											<dx:ASPxHyperLink ID="ASPxHyperLink2" runat="server" Cursor="pointer" Text='<%# Eval("name") %>' 
														Theme="NETheme01"  NavigateUrl="<%# (Eval(&quot;link&quot;)!=&quot;&quot;)?string.Format(&quot;javascript:boing('{0}', 'stuff', 1100,750)&quot;, Eval(&quot;link&quot;)) : &quot;&quot; %>"  Wrap="True" Width="100%">
														
													</dx:ASPxHyperLink>
									</DataItemTemplate>
									<CellStyle Wrap="True">
									</CellStyle>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn FieldName="link" VisibleIndex="3" Width="35px" 
									Visible="False">
									<CellStyle>
										<Paddings PaddingBottom="15px" PaddingTop="15px" />
									</CellStyle>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="Status" FieldName="status" VisibleIndex="5"
									Width="100px">
									<Settings HeaderFilterMode="CheckedList" />
									<CellStyle Wrap="True">
									</CellStyle>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataDateColumn Caption="Date" Name="Date" FieldName="date" Visible="False" 
									VisibleIndex="6" Width="100px">
									<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" 
										EditFormatString="yyyy-MM-dd">
										
									</PropertiesDateEdit>
									<Settings AutoFilterCondition="Equals" />
								</dx:GridViewDataDateColumn>
								<dx:GridViewDataTextColumn Caption="Group" Name="Group" FieldName="group" Visible="False" 
									VisibleIndex="7" Width="100px">
									<Settings AutoFilterCondition="Contains" />
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn FieldName="source" Visible="False" VisibleIndex="8">
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="name2" FieldName="name2" Visible="False" 
									VisibleIndex="9">
								</dx:GridViewDataTextColumn>
							</Columns>
							
							 <Styles>
                                            <Row CssClass="draggableRow right"></Row>
                                        </Styles>
							<SettingsBehavior ColumnResizeMode="Control" />
							
							<SettingsPager PageSize="50" NumericButtonCount="3">
							</SettingsPager>
							
							<Settings  ShowFilterRow="True" 
								ShowFilterRowMenu="True" ShowHeaderFilterButton="True" VerticalScrollableHeight="600" VerticalScrollBarMode="Auto" />
							<SettingsSearchPanel Visible="True" />
						</dx:ASPxGridView>
						<br />
					</td>
				</tr>
			</table>
			<dx:ASPxPopupControl ID="pop_push" runat="server" AutoUpdatePosition="True" 
								ClientInstanceName="pop_push" CloseAction="CloseButton" HeaderText="Push Task to Future Huddle" 
								Height="200px" Modal="True" ModalBackgroundStyle-BackColor="Transparent" 
								PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" 
								Width="350px" Theme="NETheme01" AppearAfter="0"  
				onwindowcallback="pop_push_WindowCallback" PopupAnimationType="None" 
				>
								<ClientSideEvents EndCallback="function(s, e) {
	if (s.cp_alert!=null &amp;&amp; s.cp_alert!='')
{


alert(s.cp_alert);
s.cp_alert = '';
pop_push.Hide();
gv.refresh();
}
}" />
								<ModalBackgroundStyle BackColor="Transparent">
								</ModalBackgroundStyle>
								<SettingsLoadingPanel Text="" />
								<ContentCollection>
									<dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
										<table style="width:100%;">
											<tr>
												<td>
													<dx:ASPxLabel ID="lbl_pop_task" runat="server" Text="Unknown" Theme="NETheme01">
													</dx:ASPxLabel>
												</td>
												<td>
													&nbsp;</td>
											</tr>
											<tr>
												<td>
													<dx:ASPxComboBox ID="ddl_pop_huddle" runat="server" Theme="NETheme01" 
														Width="100%">
													</dx:ASPxComboBox>
												</td>
												<td>
													&nbsp;</td>
											</tr>
											<tr>
												<td>
													To Be Done:</td>
												<td>
													&nbsp;</td>
											</tr>
											<tr>
												<td>
													<dx:ASPxMemo ID="ASPxMemo1" runat="server" Height="71px" Theme="NETheme01" 
														Width="100%">
													</dx:ASPxMemo>
												</td>
												<td>
													&nbsp;</td>
											</tr>
											<tr>
												<td>
													<dx:ASPxHiddenField ID="hdn_pop_task" runat="server" 
														ClientInstanceName="hdn_pop_task">
													</dx:ASPxHiddenField>
												</td>
												<td>
													&nbsp;</td>
											</tr>
											<tr>
												<td>
													<dx:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="False" 
														Text="Push -&gt;" Theme="NETheme01">
														<ClientSideEvents Click="function(s, e) {
	pop_push.PerformCallback('push');
}" />
													</dx:ASPxButton>
												</td>
												<td>
													&nbsp;</td>
											</tr>
										</table>
									</dx:PopupControlContentControl>
								</ContentCollection>
							</dx:ASPxPopupControl>
							<dx:ASPxGlobalEvents ID="ge" runat="server">
                <ClientSideEvents ControlsInitialized="InitalizejQuery" EndCallback="InitalizejQuery" CallbackError="InitalizejQuery" />
				
            </dx:ASPxGlobalEvents>
			<asp:ScriptManager ID="ScriptManager1" runat="server">
			</asp:ScriptManager>
			<asp:UpdateProgress ID="UpdateProgress1" runat="server" 
				AssociatedUpdatePanelID="UpdatePanel1" DisplayAfter="500">
				<ProgressTemplate>
					<img alt="" src="../../../images/loading_panel.gif" style="position: absolute; top: 50%; right: 50%; z-index: 999" />
				</ProgressTemplate>
			</asp:UpdateProgress>
		</ContentTemplate>
	</asp:UpdatePanel>

    
	
	 <style>
        .hover {
            background-color: white;
            
        }

        .activeHover {
            background-color: red;

        }

        .ui-draggable-dragging {
            background-color: lightgreen;
            color: White;
            
        }
    </style>
	
		</div>
	</asp:Content>
 
<asp:Content ID="Content4" ContentPlaceHolderID="cphMasterSubMenu" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphMasterLeft" runat="Server">
</asp:Content>
