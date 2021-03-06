<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSInstall_Controls_StepNavigation"
    CodeFile="StepNavigation.ascx.cs" %>
<%@ Register TagPrefix="cms" TagName="Help" Src="~/CMSAdminControls/UI/PageElements/Help.ascx" %>
<%@ Register TagPrefix="cms" TagName="ActivityBar" Src="~/CMSAdminControls/UI/System/ActivityBar.ascx" %>
<asp:Literal ID="ltlTableBefore" runat="server" Visible="False" />
<div class="InstallFooter FloatRight">
    <div id="buttonsDiv" class="ButtonsDiv">
        <cms:LocalizedButton ID="StepPrevButton" Source="file" CssClass="StepButton" runat="server"
            CommandName="MovePrevious" Text="{$Install.BackStep$}" Width="100" RenderScript="true"
            UseSubmitBehavior="False" />
        <cms:LocalizedButton UseSubmitBehavior="True" Source="file" CssClass="StepButton"
            ID="StepNextButton" runat="server" CommandName="MoveNext" Width="100" Text="{$Install.NextStep$}"
            RenderScript="true" />
    </div>
    <span class="FloatLeft SeparationWizardHelp">
        <cms:Help ID="hlpContext" runat="server" Visible="True" />
    </span><span id="activity" class="FloatLeft SeparationWizardActivity" style="display: none;">
        <cms:ActivityBar runat="server" ID="barActivity" Visible="true" />
    </span>
</div>
<asp:Literal ID="ltlTableAfter" runat="server" Visible="False" />