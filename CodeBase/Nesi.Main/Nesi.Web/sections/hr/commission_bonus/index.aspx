<%@ Page Language="C#" MasterPageFile="../../../IntraDefault.master" AutoEventWireup="true" Inherits="backend" Title="NE:Bonuses & Commissions" Theme="NETheme01" EnableTheming="True" Codebehind="index.aspx.cs" %>

<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>





<asp:Content ID="Content1" ContentPlaceHolderID="cphMasterLeft" Runat="Server">
	<div id="divSide" runat="server">
		<asp:SqlDataSource ID="Users" runat="server"></asp:SqlDataSource>
	</div>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphMasterMenu" Runat="Server">
	<div id="divMenu" runat="server"></div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cphMasterBody" Runat="Server">
		<script>
			function chk_request()
				{
				var type			= $('#type_select').val();
				var member_id		= $('#member_select').val();
				var payperiod_id	= $('#payperiod_select').val();
				var amount			= $('.form input').val();
				var note			= $('.form textarea').val();
				var validate		= 0;
				if(type != 'B' && type != 'C' && type != 'M')
					{validate--;}
				if(member_id == '0')
					{validate--;}
				if(payperiod_id == '0')
					{validate--;}
				if(isNaN(amount) || amount <= 0 || amount.length == 0)
					{$('.form input').val('');validate--;}
				if(note.length == 0)
					{validate--;}
				if(validate < 0)
					{
					$('.s .save').attr('disabled', true);
					}
				else
					{
					$('.s .save').attr('disabled', false);
					}
				}
				
			
			$.fn.clearForm = function()
								{
								return this.each(function()
									{
									var type = this.type, tag = this.tagName.toLowerCase();
									if (type == 'text' || type == 'password' || tag == 'textarea')
										{
										this.value = '';
										}
									else if (type == 'checkbox' || type == 'radio')
										{
										this.checked = false;
										}
									else if (tag == 'select')
										{
										this.selectedIndex = 0;
										}
									else if (tag == 'button' && $(this).attr('class') == 'save')
										{
										$(this).attr('disabled', true);
										}
									if($(this).attr('id') == 'member_select')
										{
										$(this).attr('disabled', true);
										}
									switch($('#save_type').val())
										{
										case "new":		$('.form .save').text("SEND REQUEST");
														$('.form table th').html("NEW REQUEST");
										break;
										case "edit":	$('.form .save').text("EDIT REQUEST");
														$('.form table th').html("EDIT REQUEST");
										break;
										}
								});
							};
				
			function delete_request(request_id)
				{
				if(confirm("Are you sure you want to delete this request?"))
					{
					$.get(	"./index.aspx",
							{
							a:				"del",
							request_id:		request_id
							},
							function(data)
								{
								if(data != "SUCCESS")
									{
									alert('There was a problem sending your request.');
									}
								});
					}
				}
				
			function save_request()
				{
				var type			= $('#type_select').val();
				var member_id		= $('#member_select').val();
				var payperiod_id	= $('#payperiod_select').val();
				var amount			= $('.form input').val();
				var request_id		= $('#request_id').val();
				var note			= $('.form textarea').val();
				var save_type		= $('#save_type').val();
				
				$.post(	"./index.aspx",	
						{
						a:				"save",
						type:			type,
						member_id:		member_id,
						payperiod_id:	payperiod_id,
						amount:			amount,
						save_type:		save_type,
						note:			note,
						request_id:		request_id
						},
						function(data)
							{
							if(data == "SUCCESS")
								{
								$('#contents *').clearForm();
								$('#contents #type_select').focus();
								$('#contents').toggle('slide');
								}
							else
								{
								alert('There was a problem sending your request.');
								}
							});
				}
				
			function edit_request(request_id)
				{
				$.ajax(
					{
					type: "GET",
					url: './index.aspx?a=xml_request_detail&request_id='+request_id,
					dataType: "xml",
					beforeSend: function()
									{
									$('#save_type').val('edit');
									$('#request_id').val(request_id);
									},
					success:function(xml)
						{
						$(xml).find('request').each(
							function()
								{
								var type					= $(this).attr("type");
								var payperiod_id			= $(this).attr("payperiod_id");
								var member_id				= $(this).attr("member_id");
								var business_unit_id				= $(this).attr("business_unit_id");
								var amount					= $(this).attr("amount");
								var note					= $(this).attr("note");
								$('#contents #type_select').val(type);
								$('#contents #company_select').val(business_unit_id);
								get_memberlist(business_unit_id, member_id);
								$('#contents textarea').val(note);
								$('#contents #payperiod_select').val(payperiod_id);
								$('#contents input').val(amount);
								$('#contents').hide().toggle('slide');
								});
						},
					complete: function()
								{
								}			
					});
				}
			var row_count		= 0;
			function chk_requests()
				{
				$.ajax(
					{
					type: "GET",
					url: './index.aspx?a=xml_requests',
					dataType: "xml",
					success:function(xml)
						{
						if($(xml).find('request').length != row_count)
							{
							get_requests();
							}
						count		= $(xml).find('request').length;
						}		
					});
				}
			function get_requests()
				{
				var o					= "<div class='h'><b>PAST REQUESTS</b> <button type='button' onclick=\"$('#contents').toggle('slide');\"><img src='/images/icon/icon[add].gif' align='absmiddle' /> request window</button></div><table cellspacing='0' cellpadding='5' width='100%'><thead><th>TYPE</th><th>MEMBER</th><th>DATE REQ</th><th>PPID</th><th>PAY PERIOD</th><th>APPROVED?</th><th>AMOUNT</th><th>&nbsp;</th></thead><tbody>";
				var count				= 0;
				$.ajax(
					{
					type: "GET",
					url: './index.aspx?a=xml_requests',
					dataType: "xml",
					beforeSend: function()
									{
									$('.requests').html("");
									clearInterval();
									},
					success:function(xml)
						{
						$(xml).find('request').each(
							function()
								{
								var id							= $(this).find("id").text();
								var type						= $(this).find("type").text();
								switch(type)
									{
									case "B": type				= "Bonus";
									break;
									case "C": type				= "Commission";
									break;
									case "M": type				= "Misc Payment";
									break;
									}
								var payperiod_id				= $(this).find("payperiod_id").text();
								var payperiod_daterange			= $(this).find("payperiod_daterange").text();
								var member_name					= $(this).find("member_name").text();
								var requested_when				= $(this).find("requested_when").text();
								var approved					= $(this).find("approved").text();
								var approved_when				= $(this).find("approved_when").text();
								var disabled					= "";
								switch(approved)
									{
									case "-1":	approved					= "Denied";
												disabled					= " disabled";
									break;
									case "0":	approved					= "Waiting";
												disabled					= "";
									break;
									case "1":	approved					= "Approved <br> "+approved_when;
												disabled					= " disabled";
									break;
									}
								var amount						= $(this).find("amount").text();
								var note						= $(this).find("note").text();
								o += "<tr><td><b>"+type+"</b></td><td align='center'>"+member_name+"<div class='tip'>"+note+"</div></td><td align='center'>"+requested_when+"</td><td align='center'>"+payperiod_id+"</td><td align='center'>"+payperiod_daterange+"</td><td><center>"+approved+"</center></td><td align='center'>"+amount+"</td><td width='50'><button type='button' onclick='edit_request("+id+");' "+disabled+"><img src='/images/icon/icon[edit].gif' /></button><button type='button' onclick='delete_request("+id+");' "+disabled+"><img src='/images/icon/icon[delete].gif' /></button></td></tr>";
								});
						if($(xml).find('request').length == 0)
							{
							o			+= "<tr><td colspan='8' style='padding:50px;background-color:#fff;' align='center'>You haven't made a request yet.<br><button type='button' onclick=\"$('#contents').toggle('slide');\" style='color:#090;font-size:11px;margin:5px;'><b>start one</b></button></td></tr>";
							}
						count		= $(xml).find('request').length;
						row_count	= count;
						},
					complete: function()
								{
								o						+= "</tbody></table>";
								$(o).appendTo('.requests').hide().fadeIn();
								if(count > 0)
									{
									$('.requests table').tablesorter();
									$('.requests table tbody > tr').each(function(){$(this).tip()});
									}
								setInterval('chk_requests()', 5000);
								}			
					});
				}
				
			function get_memberlist(business_unit_id, member_id)
				{
				var url			= "./index.aspx?a=xml_member&business_unit_id="+business_unit_id;
				$.ajax(
					{
					type: "GET",
					url: url,
					dataType: "xml",
					success:function(xml)
						{
						var detail								= $("#detail");
						detail.html("");
						var member								= $("#member_select");
						member.attr("disabled", false);
						member[0].options.length			= 0;
						member.append("<option value='0'>Employee</option");
						$(xml).find('member').each(
							function()
								{
								var id							= $(this).attr("id");
								var name						= $(this).attr("name");
								var selected					= "";
								if(id == member_id)
									{
									selected					= "selected";
									}
								else
									{
									selected					= "";
									}
								member.append("<option value='"+id+"'"+selected+">"+name+"</option>");
								});
						}
					});
				}
		</script>

			<dx:ASPxGridView ID="gv_requests" ClientInstanceName="gv_requests" 
			runat="server" AutoGenerateColumns="False" DataSourceID="sds_requests" 
			KeyFieldName="id" Width="1280px" 
			oncommandbuttoninitialize="gv_requests_CommandButtonInitialize" 
			onhtmleditformcreated="gv_requests_HtmlEditFormCreated" 
			onhtmldatacellprepared="gv_requests_HtmlDataCellPrepared" 
			onhtmlrowcreated="gv_requests_HtmlRowCreated" 
			onrowdeleting="gv_requests_RowDeleting" Theme="NETheme01">
				<ClientSideEvents EndCallback="function(s, e) {
	$(&quot;body&quot;).find(&quot;.ttip&quot;).each(function()
		{
		$(this).tip();
		});
}" Init="function(s, e) {
	$(&quot;body&quot;).find(&quot;.ttip&quot;).each(function()
		{
		$(this).tip();
		});
}" />
				<Columns>
					<dx:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="50px">
						
						
						
					</dx:GridViewCommandColumn>
					<dx:GridViewDataTextColumn Caption="ID" FieldName="id" ReadOnly="True" VisibleIndex="1" Width="50px">
						<EditFormSettings Visible="False" />
						<CellStyle HorizontalAlign="Center">
						</CellStyle>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Type" FieldName="type" VisibleIndex="2" Width="100px">
						<CellStyle HorizontalAlign="Center">
						</CellStyle>
					</dx:GridViewDataTextColumn>
				    <dx:GridViewDataTextColumn Caption="Business Unit" FieldName="ddl_name" VisibleIndex="3" Width="100px" >
				        
				    </dx:GridViewDataTextColumn>
					
					
					<dx:GridViewDataTextColumn Caption="Pay Period" FieldName="payperiod_daterange" VisibleIndex="4" Width="150px">
						<CellStyle HorizontalAlign="Center">
						</CellStyle>
					</dx:GridViewDataTextColumn>
				    <dx:GridViewDataTextColumn Caption="Employee" FieldName="member_name" VisibleIndex="5" Width="80px">
				        <CellStyle HorizontalAlign="Left">
				        </CellStyle>
				    </dx:GridViewDataTextColumn>
				    <dx:GridViewDataTextColumn Caption="Date Req." FieldName="requested_when" VisibleIndex="6" Width="100px">
				        <CellStyle HorizontalAlign="Center">
				        </CellStyle>
				    </dx:GridViewDataTextColumn>
				    <dx:GridViewDataTextColumn Caption="Amount" FieldName="amount" VisibleIndex="7" Width="100px">
				        <PropertiesTextEdit DisplayFormatString="C2">
				        </PropertiesTextEdit>
				        <CellStyle HorizontalAlign="Center">
				        </CellStyle>
				    </dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Approved?" FieldName="approved" VisibleIndex="8" Width="50px">
						<CellStyle HorizontalAlign="Center">
						</CellStyle>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Approved Date" FieldName="approved_when" VisibleIndex="9" Width="150px">
						<CellStyle HorizontalAlign="Center">
						</CellStyle>
					</dx:GridViewDataTextColumn>
				
				    <dx:GridViewDataTextColumn Caption="PPID" FieldName="payperiod_id" VisibleIndex="11" Width="50px" Visible="False">
				        <CellStyle HorizontalAlign="Center">
				        </CellStyle>
				    </dx:GridViewDataTextColumn>
					
				</Columns>
				<SettingsBehavior EnableRowHotTrack="True" ConfirmDelete="True" />
				<SettingsPager PageSize="50">
				</SettingsPager>
				<Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowFilterRowMenuLikeItem="True" ShowGroupPanel="True" ShowHeaderFilterButton="True" ShowTitlePanel="True" />
				<Styles>
					<Header HorizontalAlign="Center">
					</Header>
					<TitlePanel BackColor="White">
					</TitlePanel>
				</Styles>
				<Templates>
					<TitlePanel>
						<dx:ASPxButton ID="bt_new" runat="server" AutoPostBack="false" Text="New">
							<ClientSideEvents Click="function(s,e){gv_requests.AddNewRow();}" />
						</dx:ASPxButton>
					</TitlePanel>
					<EditForm>
						<dx:ASPxCallbackPanel ID="cbp_edit" runat="server" ClientInstanceName="cbp_edit" oncallback="cbp_edit_Callback">
							<ClientSideEvents EndCallback="function(s,e){if(notify.GetText() == ''){gv_requests.CancelEdit();}}" />
							<PanelCollection>
								<dx:PanelContent>
						<table cellpadding="3" cellspacing="0">
							<tr>
								<td width="150"><b>Type:</b></td>
								<td width="150"><dx:ASPxComboBox ID="edit_type" runat="server">
									<Items>
										<dx:ListEditItem Value="B" Text="Bonus" />
										<dx:ListEditItem Value="C" Text="Commission" />
										<dx:ListEditItem Value="M" Text="Miscellaneous" />
									</Items>
								</dx:ASPxComboBox></td>
								<td rowspan="4" valign="top" align="left"><br />
									<dx:ASPxLabel ID="notify" runat="server" ClientInstanceName="notify" EncodeHtml="false"></dx:ASPxLabel></td>
							</tr>
							<tr>
								<td><b>Business Unit:</b></td>
								<td><dx:ASPxComboBox ID="edit_branch" runat="server" DataSourceID="sds_branches" TextField="name" ValueField="id" ValueType="System.String">
								<ClientSideEvents SelectedIndexChanged="function(s,e){edit_employee.PerformCallback(s.GetValue());}" />
								</dx:ASPxComboBox>
									<asp:SqlDataSource ID="sds_branches" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT CAST(id AS CHAR(4)) id, ddl_name name from business_unit  WHERE enable_timesheet = 1 and FIND_IN_SET(id,@visibleBU)">
									    <SelectParameters>
									        <asp:SessionParameter Name="@visibleBU" SessionField="visibleBU" Type="String" />
									    </SelectParameters>
									</asp:SqlDataSource>
								</td>
							</tr>
							<tr>
								<td><b>Employee:</b></td>
								<td><dx:ASPxComboBox ID="edit_employee" runat="server" ClientInstanceName="edit_employee" DataSourceID="sds_users" TextField="name" ValueField="id" ValueType="System.String" oncallback="edit_employee_Callback"></dx:ASPxComboBox>
									<asp:SqlDataSource ID="sds_users" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT CAST(a.member_id AS CHAR(5)) id, a.member_fullname name FROM member a WHERE a.member_status='Active' and a.business_unit_id = @business_unit_id ORDER BY member_lastname, member_fullname">
										<SelectParameters>
											<asp:ControlParameter ControlID="edit_branch" Name="@business_unit_id" PropertyName="Value" />
										</SelectParameters>
									</asp:SqlDataSource>
								</td>
							</tr>
							<tr>
								<td><b>Pay Period:</b></td>
								<td><dx:ASPxComboBox ID="edit_payperiod" runat="server" DataSourceID="sds_payperiods" TextField="daterange" ValueField="id" ValueType="System.String"></dx:ASPxComboBox>
									<asp:SqlDataSource ID="sds_payperiods" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT CAST(payperiodid AS CHAR(5)) id, CONCAT('(', payperiodid,') ', DATE_FORMAT(startdate, '%Y-%m-%d'), ' - ', DATE_FORMAT(enddate, '%Y-%m-%d')) daterange FROM payperiods WHERE completed = 0 ORDER BY payperiodid"></asp:SqlDataSource>
								</td>
							</tr>
							<tr>
								<td><b>Amount:</b></td>
								<td><dx:ASPxTextBox ID="edit_amount" runat="server"></dx:ASPxTextBox></td>
							</tr>
							<tr>
								<td valign="top"><b>Note/Reason:</b></td>
								<td colspan="2"><dx:ASPxMemo ID="edit_memo" runat="server" Width="450px" Height="250px"></dx:ASPxMemo></td>
							</tr>
							<tr>
								<td>&nbsp;<asp:HiddenField runat="server" id="hid_id" /></td>
								<td colspan="2">
									<table cellpadding="3" cellspacing="0">
										<tr>
											<td><dx:ASPxButton ID="edit_btcancel" AutoPostBack="false" runat="server" Text='Cancel'>
												<ClientSideEvents Click="function(s,e){gv_requests.CancelEdit();}" />
											</dx:ASPxButton></td>
											<td><dx:ASPxButton ID="edit_btsave" AutoPostBack="false" runat="server" Text='Save'>
												<ClientSideEvents Click="function(s, e) {
	cbp_edit.PerformCallback();
}" />
												</dx:ASPxButton></td>
										</tr>
									</table></td>
							</tr>
						</table></dx:PanelContent>
							</PanelCollection>
						</dx:ASPxCallbackPanel>
					</EditForm>
				</Templates>
			</dx:ASPxGridView>
<asp:SqlDataSource ID="sds_requests" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="
SELECT 
	a.id,
	a.type _type,
	CASE a.type 
		WHEN 'B' THEN 'Bonus' 
		WHEN 'M' THEN 'Manual' 
		WHEN 'C' THEN 'Commission' 
		ELSE NULL 
	END AS 'type',
	CAST(b.member_id AS CHAR(5)) member_id,
	CAST(b.business_unit_id AS CHAR(4)) business_unit_id,
	b.member_fullname member_name,
	DATE_FORMAT(a.requested_when, '%Y-%m-%d') requested_when,
	CAST(a.payperiod_id AS CHAR(5)) payperiod_id,
	CAST(CONCAT(DATE_FORMAT(d.startdate, '%Y-%m-%d'), ' - ', DATE_FORMAT(d.enddate, '%Y-%m-%d')) AS CHAR(50)) payperiod_daterange,
a.approved approved_n,
CASE a.approved WHEN  0 THEN 'Denied' WHEN 1 THEN 'Approved' WHEN -1 THEN 'Requested' END AS approved,
	DATE_FORMAT(a.approved_when, '%Y-%m-%d %h:%i%p') approved_when,
	a.amount,
	a.note,
	b.member_status,
                bb.ddl_name
FROM 
	payroll_extra_payments a
LEFT JOIN
	member b
		ON a.member_id = b.member_id
LEFT JOIN
	payperiods d
		ON a.payperiod_id = d.payperiodid
                left join business_unit bb on bb.id = b.business_unit_id
WHERE 
	((a.requested_by = @member_id) or (is_supervisor(a.member_id,@member_id))) AND
	a.approved &gt;= -1
ORDER BY
	a.requested_when DESC">
				<SelectParameters>
					<asp:SessionParameter Name="@member_id" SessionField="member_id" Type="Int32" />
				</SelectParameters>
			</asp:SqlDataSource>
		<div id="outer" class="payroll_bonuses" Runat="Server">
			<br />

		</div>
</asp:Content>
<asp:Content ID="Content4" runat="server" contentplaceholderid="header_placeholder">
</asp:Content>

