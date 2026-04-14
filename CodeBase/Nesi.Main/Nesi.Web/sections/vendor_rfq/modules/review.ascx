<%@ Control Language="C#" AutoEventWireup="true" Inherits="sections_vendor_rfq_modules_review"  EnableTheming="True" Codebehind="review.ascx.cs" %>
<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web" TagPrefix="dx" %>


	

	<script type="text/javascript" src="/js/jquery-1.3.2.min.js"></script>
		<script type="text/javascript" src="/js/jquery-ui-1.7.1.custom.min.js"></script>

	<script type="text/javascript" src="/js/jquery.tip.js"></script>
	<script type="text/javascript" src="/js/functions.js"></script>
    <script type="text/javascript">
		$(document).ready(function()
							{
							bind_tooltips();
							bind_tooltips();
							});
		function bind_tooltips()
			{
			$(".review").find(".note").each(function(){$(this).tip()});
			}
		function note_show(obj)
			{
			var exists			= $("#note_window").size() > 0;
			var id				= $(obj).attr("data-id");
			var note			= $(obj).attr("data-note");
			if(exists && $("#note_window"))
				{  
				$("#note_window").remove();
				}
			var html			= "<div id='note_window' data-id='"+id+"'>";
			html				+= "<b>Note:</b>\
									<br/><textarea style='width:99%;height:100px;' maxlength='512'></textarea>\
									<br><button style='font-size:11px;font-weight:bold;' onclick='handle_note(this)' data-id='"+id+"' type='button'><img align='absmiddle' src='/images/icon/icon[save].gif'/> Save</button>\
									<button style='font-size:11px;font-weight:bold;' onclick=\"$('#note_window').remove();\"><img align='absmiddle' src='/images/icon/icon[delete].gif'/> Close</button>";
			html				+= "</div>";
			$("body").append(html);
			var off			= $(obj).offset();
			$("#note_window").css({	"position"			: "absolute",
									"top"				: off.top+"px", 
									"left"				: off.left+25+"px",
									"width"				: "300px",
									"padding"			: "5px",
									"border"			: "solid 1px #cc7",
									"background-color"	: "#ff7"
									}).find("textarea").focus().val(note);
			}
		function handle_note(obj)
			{
			$(obj).attr("disabled", true);
			please_wait("start");
			var _defs				=   {
										a:			"handle_note",
										id:			$(obj).attr('data-id'),
										note:		$("#note_window").find("textarea").val()
										};

			$.get("./index.aspx", _defs, function(_returned)
								{
								if(_returned == "SUCCESS")
									{
									$('#note_window').remove();
									please_wait("stop");
									$('#status_message').hide().html("<b style='display:block;padding:5px;color:#090;'>Note Saved</b>").fadeIn(1000);
									setTimeout("$('#status_message').slideUp().html('')", 2500);
									gv_review.Refresh();
									}
								else	
									{
									please_wait("stop");
									$(obj).removeAttr('disabled');
									alert(_returned);
									}
								});
			}
		function handle_rowsave(obj, is_save_all, is_final_save)
				{
				obj.SetEnabled(false);
				//please_wait("start", "Saving...");
				var b			= obj.mainElement;
				if(is_final_save == undefined)
					{
					is_final_save		= false;
					}
				if(is_save_all)
					{
					// iterate through each line
					var li				= [];
					var c				= gv_review.GetVisibleRowsOnPage();
					var _defs			=	{
											a:			"handle_mass_save"
											}
					var data			= new Object();
					for(i = 0; i < c; i++)
						{
						var p			= gv_review.GetRow(i);
						var o			=	{
											id:		eval("mod_review_cbp_review_gv_review_cell"+i+"_7_b_save").cpId,
											code:	$(p).find('.t_vendor_code').find('input').val(),
											qty:	$(p).find('.t_qty').find('input').val(),
											cost:	$(p).find('.t_cost').find('input').val(),
											lead:	$(p).find('.t_lead').find('input').val()
											};
						if(!isNaN(o.cost) && !isNaN(o.qty))
							{
							li.push(o);
							}
						}
					_defs.li		= JSON.stringify(li);
					$.post("./index.aspx", _defs, function(_returned)
										{
										if(_returned == "SUCCESS")
											{
											if(!is_final_save)
												{
												please_wait("stop");
												$('#status_message').hide().html("<b style='display:block;padding:5px;color:#090;'>All Rows Saved Successfully.</b>").fadeIn(1000);
												setTimeout("$('#status_message').slideUp().html('')", 2500);
												gv_review.Refresh();
												}
											}
										else	
											{
											please_wait("stop");
											alert(_returned);
											}
										obj.SetEnabled(true);
										});
					}
				else
					{
					//only save one line
					var p			= $(b).parents('tr:first');
					var code		= $(p).find('.t_vendor_code').find('input').val();
					var qty			= $(p).find('.t_qty').find('input').val();
					var cost		= $(p).find('.t_cost').find('input').val();
					var lead		= $(p).find('.t_lead').find('input').val();
					var _defs		=	{
										a:			"handle_rowsave",
										cost:		cost,
										code:		code,
										qty:		qty,
										lead:		lead,
										id:			obj.cpId
										};
					if(isNaN(cost))
						{
						alert("The cost supplied is not in a valid format");
						please_wait("stop");
						return false;
						}
					if(isNaN(qty))
						{
						alert("The quantity supplied is not in a valid format");
						please_wait("stop");
						return false;
						}
					$.get("./index.aspx", _defs, function(_returned)
										{
										if(_returned == "SUCCESS")
											{
											please_wait("stop");
											$('#status_message').hide().html("<b style='display:block;padding:5px;color:#090;'>Row Saved</b>").fadeIn(1000);
											setTimeout("$('#status_message').slideUp().html('')", 2500);
											gv_review.Refresh();
											}
										else	
											{
											please_wait("stop");
											alert(_returned);
											}
										});
					}
				}
		function handle_verify(obj)
				{
				if(confirm("Please confirm that all information displayed is correct"))
					{
					obj.SetEnabled(false);
					please_wait("start", "Sending...");
					var _defs		=   {
										a:			"handle_verify"
										};
					$.ajax( {
							type:		"GET",
							cached:		false,
							url:		"./index.aspx",
							data:		_defs,
							dataType:	"text",
							beforeSend:	function()
											{
											handle_rowsave(obj, true, true);
											},
							success:	function(_returned)
											{
											if(_returned == "SUCCESS")
												{
												please_wait("stop");
												$('#status_message').hide().html("<b style='display:block;padding:5px;color:#090;'>Reply Sent. Thank you.<br/>You will be logged out in a moment.</b>").fadeIn(1000);
												setTimeout("location.href='./index.aspx'", 3000);
												}
											else	
												{
												please_wait("stop");
												alert(_returned);
												}
											},
							error:		function()
											{
											please_wait("stop");
											alert("There was an error sending quote, please before you try again, call the person that sent you the RFQ.");
											}
							});
					}
				}
				
	</script>
	<div id="status_message"></div>
<dx:aspxcallbackpanel id="cbp_review" runat="server" clientinstancename="cbp_review"
	width="100%">
	<PanelCollection>
<dx:PanelContent runat="server"><DIV class="review"><dx:ASPxGridView runat="server" 
		AutoGenerateColumns="False" ClientInstanceName="gv_review" 
		DataSourceID="ds_review" Width="100%"  ID="gv_review" 
		KeyFieldName="id" Theme="NETheme01">

	<ClientSideEvents EndCallback="function(s, e) {
	bind_tooltips();
}" />
<ClientSideEvents EndCallback="function(s, e) {
	bind_tooltips();
}"></ClientSideEvents>
	<Columns>
	<dx:GridViewDataTextColumn Caption="RFQ Qty" FieldName="rfq_qty" VisibleIndex="4" 
			Width="35px">
		<CellStyle HorizontalAlign="Left">
		</CellStyle>
	</dx:GridViewDataTextColumn>
	<dx:GridViewDataTextColumn Caption="Your Part #" FieldName="vendor_code" VisibleIndex="0"
		Width="125px">
		<DataItemTemplate>
			<dx:ASPxTextBox ID="t_vendor_code" runat="server" AutoResizeWithContainer="True"
				CssClass="t_vendor_code" Text='<%# Eval("vendor_code") %>' Width="70px">
			</dx:ASPxTextBox>
		</DataItemTemplate>
	</dx:GridViewDataTextColumn>
	<dx:GridViewDataTextColumn Caption="Our Part #" FieldName="master_id" VisibleIndex="1"
		Width="75px">
		<CellStyle HorizontalAlign="Center">
		</CellStyle>
	</dx:GridViewDataTextColumn>
	<dx:GridViewDataTextColumn Caption="Description" FieldName="description" VisibleIndex="2" width="100%">
		<CellStyle HorizontalAlign="Left">
		</CellStyle>
	</dx:GridViewDataTextColumn>
	<dx:GridViewDataTextColumn Caption="Qty" FieldName="qty" VisibleIndex="5"
		Width="60px" HeaderStyle-HorizontalAlign="Center">
		<CellStyle HorizontalAlign="Center">
		</CellStyle>
		<DataItemTemplate>
			<dx:ASPxTextBox ID="t_qty" runat="server" CssClass="t_qty" Text='<%# Eval("qty") %>' Width="60px" HorizontalAlign="Center">
				<ClientSideEvents KeyDown="function(s, e) {
	only_int(e.htmlEvent);
}" />
			</dx:ASPxTextBox>
		</DataItemTemplate>
		
		<HeaderTemplate>
			Package QTY<br /><b class="note" data-title="Packaged QTY (Qty of our SKUs in your SKU)" style="text-align: center">[?]</b>
		</HeaderTemplate>
	</dx:GridViewDataTextColumn>
	<dx:GridViewDataTextColumn Caption="Note" VisibleIndex="3" Width="20px">
		<DataItemTemplate>
			<%# note_handler(Container)%>
		</DataItemTemplate>
		<CellStyle HorizontalAlign="Center">
		</CellStyle>
	</dx:GridViewDataTextColumn>
	<dx:GridViewDataTextColumn Caption="Cost" FieldName="vendor_price" VisibleIndex="6"
		Width="60px">
		<DataItemTemplate>
			<dx:ASPxTextBox ID="t_cost" runat="server" CssClass="t_cost" Text='<%# Eval("vendor_price") %>' Width="60px">
			</dx:ASPxTextBox>
		</DataItemTemplate>
		<CellStyle horizontalalign="Center">
		</CellStyle>
	</dx:GridViewDataTextColumn>
	<dx:GridViewDataButtonEditColumn VisibleIndex="9" Width="25px" Caption=" ">
		
		<DataItemTemplate>
<dx:ASPxButton id="b_save"  runat="server" AutoPostBack="False" UseSubmitBehavior="False" OnCustomJSProperties="b_save_CustomJSProperties" ToolTip="Save this line information for later">
<Image Url="~/images/icon/icon[save].gif"></Image>
	<ClientSideEvents Click="function(s, e) {
	handle_rowsave(s, false);
}" />
</dx:ASPxButton>
		</DataItemTemplate>
	</dx:GridViewDataButtonEditColumn>
		<dx:GridViewDataTextColumn Caption="Lead Time" FieldName="lead_time" ShowInCustomizationForm="True" VisibleIndex="7" Width="50px"  HeaderStyle-HorizontalAlign="Center">
			<dataitemtemplate>
				<dx:ASPxSpinEdit ID="ASPxSpinEdit1" runat="server" CssClass="t_lead" Height="21px" NumberType="Integer" Value='<%# Eval("lead_time") %>' Width="70px" />
			</dataitemtemplate>
			<HeaderStyle Wrap="True" />
			<cellstyle wrap="True">
			</cellstyle>
			<HeaderTemplate>
				Lead Time<br /><b class="note" data-title="The number of days between placing an order&lt;br/&gt; and receiving all the ordered item(s)." style="text-align: center">[?]</b>
			</HeaderTemplate>
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn Caption="Required Date" FieldName="required_date" ShowInCustomizationForm="True" VisibleIndex="8" Width="100px">
		</dx:GridViewDataTextColumn>
</Columns>

<SettingsBehavior EnableRowHotTrack="True"></SettingsBehavior>

	<settingspager mode="ShowAllRecords" pagesize="75">
	</settingspager>

</dx:ASPxGridView>
 <asp:SqlDataSource runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT
a.id,
UPPER(URLDECODE(a.vendor_code)) vendor_code,
a.master_id,
full_part_description(a.master_id, true, c.country) description,
IF(IFNULL(a.qty, 0) = 0, 1, a.qty) qty,
rp.qty rfq_qty,
a.lead_time lead_time,
DATE_FORMAT(a.required_date, &quot;%m/%d/%Y&quot;) required_date,
vendor_price 
FROM 
rfq_lineitem a 
LEFT JOIN 
rfq_header b ON a.rfq_header_id = b.id 
LEFT join 
business_unit c 
ON b.business_unit_id = c.id 
LEFT JOIN
rfq_part_list rp
ON a.master_id = rp.master_id AND a.rfq_header_id = rp.rfq_header_id
WHERE 
a.rfq_header_id = @rfq_header_id AND 
a.vendor_id = @vendor_id" ID="ds_review">
	 <SelectParameters>
		 <asp:SessionParameter Name="@rfq_header_id" SessionField="rfq_header_id" />
		 <asp:SessionParameter Name="@vendor_id" SessionField="rfq_vendor_id" />
	 </SelectParameters>
 </asp:SqlDataSource>
</DIV></dx:PanelContent>
</PanelCollection>
</dx:aspxcallbackpanel>
