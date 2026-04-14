<%@ Control Language="C#" AutoEventWireup="true" Inherits="sections_workorder_modules_accounting_notes" Codebehind="accounting_notes.ascx.cs" %>
<style type="text/css">
	.accounting_notes_frame
		{
		border:					solid 1px #999;
		margin:					0px;
		height:					150px;
		padding:				0px;
		overflow-y:				scroll;
		background-color:		#eee;
		}
	.accounting_notes_frame .heading
		{
		
		border-bottom:			solid 1px #ccc;
		}
</style>
<div id="notes" class="accounting_notes_frame">
	<div class="heading">Job Cost - Accounting Notes</div>
	<div id="accounting_notes" runat="server"></div>
</div>