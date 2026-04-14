<%@ Control Language="C#" AutoEventWireup="true" Inherits="sections_hr_i_modules_employee_information" Codebehind="employee_information.ascx.cs" %>
<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<asp:HiddenField ID="dtl_id" runat="server" />
<asp:HiddenField ID="is_loaded" runat="server" Value="0" />
<script type="text/javascript">
	$(document).ready(function()
	{
	fvr.client.employee_information.initial_snapshot();
	});
</script>
<table style="width: 100%;" class="emp_info">
    <tr>
        <td class="h" colspan="4">Please verify the below information and correct where needed.<br />
            (*) Denotes a required field</td>
    </tr>
    <tr>
        <td colspan="4">
            <div id="error_report" style="color: #f00;" runat="server"></div>
        </td>
    </tr>
    <tr>
        <td class="c">*First Name:</td>
        <td class="v" colspan="3">
            <asp:TextBox ID="first_name" runat="server" CssClass="i" autocomplete="off" TabIndex="1" required></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="c">*Middle Initial:</td>
        <td class="v" colspan="3">
            <asp:TextBox ID="middle_initial" runat="server" CssClass="i" autocomplete="off" MaxLength="1" TabIndex="2" required></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="c">*Last Name:</td>
        <td class="v" colspan="3">
            <asp:TextBox ID="last_name" runat="server" CssClass="i" autocomplete="off" TabIndex="3" required></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="c">Preferred First Name:
			<div style="font-size: 10px;">(If different from your first name)</div>
        </td>
        <td class="v" colspan="3">
            <asp:TextBox ID="preferred_name" runat="server" CssClass="i" TabIndex="4" autocomplete="off"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="c">*Home Address:</td>
        <td class="v" colspan="3">
            <asp:TextBox ID="address" runat="server" CssClass="i" Width="250px" TabIndex="5" autocomplete="off" required></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="c">*City:</td>
        <td class="v" colspan="3">
            <asp:TextBox ID="city" runat="server" CssClass="i" autocomplete="off" TabIndex="6" required></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="c">*Postal (Zip) Code:</td>
        <td class="v" colspan="3">
            <asp:TextBox ID="zip_code" runat="server" CssClass="i" Width="50px" autocomplete="off" TabIndex="7" required></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="c">*Province / State:</td>
        <td class="v" colspan="3">
            <asp:DropDownList ID="prov_state" runat="server" DataTextField="Prov_Desc" DataValueField="Prov_Abbv" TabIndex="8" Width="150px" CssClass="i" required>
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td class="c">*Country: </td>
        <td class="v" colspan="3">
            <asp:DropDownList ID="country" runat="server" DataTextField="Prov_Desc" DataValueField="Prov_Abbv" TabIndex="9" Width="150px" CssClass="i" required>
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td class="c">*Personal Phone #:</td>
        <td class="v" colspan="3">
            <table>
                <tr>
                    <td>
                        <asp:TextBox ID="phone_area" runat="server" Width="35px" CssClass="i" autocomplete="off" type="number" TabIndex="10" MaxLength="3" required></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="phone_pre" runat="server" Width="35px" CssClass="i" autocomplete="off" type="number" TabIndex="11" MaxLength="3" required></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="phone_suffix" runat="server" Width="45px" CssClass="i" autocomplete="off" type="number" TabIndex="12" MaxLength="4" required></asp:TextBox>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td class="c">Work Cell #:</td>
        <td class="v" colspan="3">
            <table>
                <tr>
                    <td>
                        <asp:TextBox ID="work_area" runat="server" Width="35px" CssClass="i" autocomplete="off" type="number" TabIndex="13" MaxLength="3"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="work_pre" runat="server" Width="35px" CssClass="i" autocomplete="off" type="number" TabIndex="14" MaxLength="3"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="work_suffix" runat="server" Width="45px" CssClass="i" autocomplete="off" type="number" TabIndex="15" MaxLength="4"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td class="c">Work Phone Ext.:</td>
        <td class="v" colspan="3">
            <asp:TextBox ID="extension" runat="server" CssClass="i" Width="35px" autocomplete="off" TabIndex="16" type="number"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="c">*Birthdate:</td>
        <td class="v" colspan="3">
            <asp:TextBox ID="birthdate" runat="server" CssClass="i" Width="150px" autocomplete="off" TabIndex="17" type="date" required></asp:TextBox> <div style="font-size: 10px;">Format: MM/DD/YYYY</div>
        </td>
    </tr>

    <tr>
        <td class="c">*Personal Email Address:</td>
        <td class="v" colspan="3">
            <asp:TextBox ID="email_address" runat="server" CssClass="i" autocomplete="off" type="email" TabIndex="19" required></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="c">Work Email Address:</td>
        <td class="v" colspan="3">
            <asp:TextBox ID="work_email" runat="server" CssClass="i" autocomplete="off" type="email" TabIndex="20"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="c">Driver&#39;s License:</td>
        <td class="v" colspan="3">
            <asp:TextBox ID="drivers_license" runat="server" CssClass="i" autocomplete="off" TabIndex="21"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="c">Electrical License:</td>
        <td class="v" colspan="3">
            <asp:TextBox ID="electric_license" runat="server" CssClass="i" autocomplete="off" TabIndex="22"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="c">Apprentice Contract:</td>
        <td class="v" colspan="3">
            <asp:TextBox ID="apprentice_contract" runat="server" CssClass="i" autocomplete="off" TabIndex="23"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td colspan="4">
            <hr />
        </td>
    </tr>
    <tr>
        <td class="c">*Emergency Contact</td>
        <td class="v">&nbsp;</td>

    </tr>
    <tr>
        <td class="c">*First Name:</td>
        <td class="v">
            <asp:TextBox ID="emergency_first_name" runat="server" CssClass="i" autocomplete="off" TabIndex="24" required></asp:TextBox>
        </td>

    </tr>
    <tr>
        <td class="c">*Last Name:</td>
        <td class="v">
            <asp:TextBox ID="emergency_last_name" runat="server" CssClass="i" autocomplete="off" TabIndex="25" required></asp:TextBox>
        </td>

    </tr>
    <tr>
        <td class="c">*Phone Number:</td>
        <td class="v">
            <table>
                <tr>
                    <td>
                        <asp:TextBox ID="emergency_phone_area" runat="server" Width="35px" CssClass="i" autocomplete="off" TabIndex="26" type="number" MaxLength="3" required></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="emergency_phone_pre" runat="server" Width="35px" CssClass="i" autocomplete="off" TabIndex="27" type="number" MaxLength="3" required></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="emergency_phone_suffix" runat="server" Width="45px" CssClass="i" autocomplete="off"  TabIndex="28" type="number" MaxLength="4" required></asp:TextBox>
                    </td>
                </tr>
            </table>
        </td>

    </tr>
    <tr>
        <td colspan="4">
            <hr />
        </td>
    </tr>
    <tr>
        <td class="v" colspan="3">
            <dx:ASPxButton ID="bt_save" runat="server" Native="True" CssClass="whitetext aligncenter" OnClick="bt_save_Click"  TabIndex="29" Text="Save">
                <Image Url="~/images/icon/icon[save].gif">
                </Image>
            </dx:ASPxButton>
            <br />
            <dx:ASPxLabel ID="lb_saved" runat="server" Font-Bold="True" ForeColor="#009933">
            </dx:ASPxLabel>
        </td>
    </tr>
    <tr>
        <td class="v" colspan="3" align="center">
            <input type="checkbox" id="cb" runat="server"  TabIndex="30"
                onchange="fvr.client.check_for_acknowledge(this)" class="cb"
                disabled="disabled" /><label for="<%= cb.ClientID %>"> By checking this, you confirm that this information is correct & complete</label></p>		</td>
    </tr>
</table>

<p>
    &nbsp;
</p>


