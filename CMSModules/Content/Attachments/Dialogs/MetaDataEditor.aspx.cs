﻿using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CMS.CMSHelper;
using CMS.GlobalHelper;
using CMS.SiteProvider;
using CMS.DocumentEngine;
using CMS.UIControls;

public partial class CMSModules_Content_Attachments_Dialogs_MetaDataEditor : CMSModalPage
{
    #region "Variables"

    private new string mCurrentSiteName = null;

    #endregion


    #region "Properties"

    /// <summary>
    /// Returns the site name from query string 'sitename' or 'siteid' if present, otherwise CMSContext.CurrentSiteName.
    /// </summary>
    protected new string CurrentSiteName
    {
        get
        {
            if (mCurrentSiteName == null)
            {
                mCurrentSiteName = QueryHelper.GetString("sitename", CMSContext.CurrentSiteName);

                int siteId = QueryHelper.GetInteger("siteid", 0);

                SiteInfo site = SiteInfoProvider.GetSiteInfo(siteId);
                if (site != null)
                {
                    mCurrentSiteName = site.SiteName;
                }
            }
            return mCurrentSiteName;
        }
    }

    #endregion


    #region "Methods"

    protected void Page_Load(object sender, EventArgs e)
    {
        // Initialize modal page
        RegisterEscScript();

        if (QueryHelper.ValidateHash("hash"))
        {
            string title = GetString("general.editmetadata");
            Page.Title = title;
            CurrentMaster.Title.TitleText = title;
            // Default image
            CurrentMaster.Title.TitleImage = GetImageUrl("Design/Controls/MetaDataEditor/Title.png");
            // Help
            mCurrentMaster.Title.HelpName = "helpTopic";
            CurrentMaster.Title.HelpTopicName = "metadata_editor";

            btnSave.Click += btnSave_Click;

            AddNoCacheTag();

            // Set metadata editor properties
            metaDataEditor.ObjectGuid = QueryHelper.GetGuid("attachmentguid", Guid.Empty);
            metaDataEditor.ObjectType = DocumentObjectType.ATTACHMENT;
            metaDataEditor.ExternalControlID = QueryHelper.GetText("clientid", null);
            metaDataEditor.VersionHistoryID = QueryHelper.GetInteger("versionhistoryid", 0);
            metaDataEditor.SiteName = CurrentSiteName;
            metaDataEditor.GetObjectExtension += metaDataEditor_GetObjectExtension;
        }
        else
        {
            // Hide all controls
            metaDataEditor.Visible = false;
            btnSave.Visible = false;

            string url = ResolveUrl("~/CMSMessages/Error.aspx?title=" + GetString("dialogs.badhashtitle") + "&text=" + GetString("dialogs.badhashtext") + "&cancel=1");
            ltlScript.Text = ScriptHelper.GetScript("window.location = '" + url + "';");
        }
    }


    /// <summary>
    /// Sets title of image according to file extension.
    /// </summary>
    /// <param name="extension">File extension</param>
    private void metaDataEditor_GetObjectExtension(string extension)
    {
        CurrentMaster.Title.TitleImage = GetFileIconUrl(extension, null);
    }


    /// <summary>
    /// Saves metadata of attachment.
    /// </summary>
    /// <param name="sender">Sender</param>
    /// <param name="e">Argument</param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (metaDataEditor.SaveMetadata())
        {
            ltlScript.Text = ScriptHelper.GetScript("CloseDialog();");
        }
    }

    #endregion
}