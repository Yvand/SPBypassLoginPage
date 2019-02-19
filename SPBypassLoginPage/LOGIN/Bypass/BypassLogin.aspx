<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BypassLogin.aspx.cs" Inherits="Yvand.SPBypassLoginPage.BypassLogin" MasterPageFile="~/_layouts/15/errorv15.master" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 

<asp:Content ContentPlaceHolderId="PlaceHolderPageTitle" runat="server">
	<SharePoint:EncodedLiteral runat="server"  EncodeMethod="HtmlEncode" Id="ClaimsLogonPageTitle" />
</asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderPageTitleInTitleArea" runat="server">
	<SharePoint:EncodedLiteral runat="server"  EncodeMethod="HtmlEncode" Id="ClaimsLogonPageTitleInTitleArea" />
</asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderMain" runat="server">
<SharePoint:EncodedLiteral runat="server" EncodeMethod="HtmlEncode" Id="ClaimsLogonPageMessage" />
<br />
<br />
<asp:DropDownList ID="ClaimsLogonSelector" runat="server" OnSelectedIndexChanged="ClaimsLogonSelector_SelectedIndexChanged"></asp:DropDownList>
</asp:Content>
