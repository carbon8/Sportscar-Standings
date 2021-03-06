<%@ Page Language="C#" AutoEventWireup="true" Inherits="CMSModules_Content_CMSDesk_Properties_Wireframe"
    Theme="Default" CodeFile="Wireframe.aspx.cs" MaintainScrollPositionOnPostback="true"
    MasterPageFile="~/CMSMasterPages/UI/SimplePage.master" %>

<%@ Register Src="~/CMSAdminControls/UI/PageElements/Help.ascx" TagName="Help" TagPrefix="cms" %>
<%@ Register Src="~/CMSModules/Content/Controls/editmenu.ascx" TagName="editmenu"
    TagPrefix="cms" %>
<%@ Register Src="~/CMSModules/Content/Controls/Wireframe.ascx" TagName="Wireframe"
    TagPrefix="cms" %>
<%@ Register TagPrefix="cms" Namespace="CMS.UIControls" Assembly="CMS.UIControls" %>
<asp:Content ContentPlaceHolderID="plcBeforeContent" runat="server">
    <cms:editmenu ID="menuElem" runat="server" ShowApprove="true" ShowReject="true" ShowSubmitToApproval="true"
        ShowProperties="false" HelpTopicName="wireframe" IsLiveSite="false" />
</asp:Content>
<asp:Content ID="cntBody" runat="server" ContentPlaceHolderID="plcContent">
    <asp:Panel ID="pnlContent" runat="server" CssClass="PropertiesPanel">
        <cms:Wireframe ID="wfElem" runat="server" />
    </asp:Panel>
    <div class="Clear">
    </div>
</asp:Content>
