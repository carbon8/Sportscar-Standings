using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

using CMS.SettingsProvider;
using CMS.CMSHelper;
using CMS.GlobalHelper;
using CMS.UIControls;
using CMS.TranslationServices;
using CMS.ExtendedControls;

public partial class CMSModules_Translations_Controls_UI_TranslationSubmission_List : CMSAdminListControl
{
    #region "Variables"

    private bool modifyAllowed = false;

    #endregion

    #region "Properties"

    /// <summary>
    /// Inner grid.
    /// </summary>
    public UniGrid Grid
    {
        get
        {
            return this.gridElem;
        }
    }


    /// <summary>
    /// Indicates if the control should perform the operations.
    /// </summary>
    public override bool StopProcessing
    {
        get
        {
            return base.StopProcessing;
        }
        set
        {
            base.StopProcessing = value;
            this.gridElem.StopProcessing = value;
        }
    }


    /// <summary>
    /// Indicates if the control is used on the live site.
    /// </summary>
    public override bool IsLiveSite
    {
        get
        {
            return base.IsLiveSite;
        }
        set
        {
            base.IsLiveSite = value;
            gridElem.IsLiveSite = value;
        }
    }

    #endregion


    #region "Methods"

    protected void Page_Load(object sender, EventArgs e)
    {
        modifyAllowed = CMSContext.CurrentUser.IsAuthorizedPerResource("CMS.TranslationServices", "Modify");

        this.gridElem.WhereCondition = "SubmissionSiteID = " + CMSContext.CurrentSiteID;
        this.gridElem.OnExternalDataBound += new OnExternalDataBoundEventHandler(gridElem_OnExternalDataBound);
        this.gridElem.OnAction += new OnActionEventHandler(gridElem_OnAction);
    }


    protected void gridElem_OnAction(string actionName, object actionArgument)
    {
        string err = null;
        string info = null;

        // Check modify permission for all actions except for download ZIP
        if (!actionName.EqualsCSafe("downloadzip", true))
        {
            if (!modifyAllowed)
            {
                RedirectToAccessDenied("CMS.TranslationServices", "Modify");
            }
        }

        TranslationSubmissionInfo submissionInfo = TranslationSubmissionInfoProvider.GetTranslationSubmissionInfo(ValidationHelper.GetInteger(actionArgument, 0));
        if (submissionInfo != null)
        {
            switch (actionName.ToLowerCSafe())
            {
                case "downloadzip":
                    TranslationServiceHelper.DownloadXLIFFinZIP(submissionInfo, Page.Response);
                    break;

                case "resubmit":
                    err = TranslationServiceHelper.ResubmitSubmission(submissionInfo);
                    info = GetString("translationservice.translationresubmitted");
                    break;

                case "process":
                    err = TranslationServiceHelper.ProcessSubmission(submissionInfo);
                    info = GetString("translationservice.translationsimported");
                    break;

                case "cancel":
                    err = TranslationServiceHelper.CancelSubmission(submissionInfo);
                    info = GetString("translationservice.submissioncanceled");
                    break;

                case "delete":
                    TranslationServiceHelper.CancelSubmission(submissionInfo);
                    submissionInfo.Delete();
                    info = GetString("translationservice.submissioncanceledanddeleted");
                    break;
            }
            if (!string.IsNullOrEmpty(err))
            {
                ShowError(err);
            }
            else if (!string.IsNullOrEmpty(info))
            {
                ShowConfirmation(info);
            }
        }
    }


    protected object gridElem_OnExternalDataBound(object sender, string sourceName, object parameter)
    {
        switch (sourceName.ToLowerCSafe())
        {
            case "resubmitaction":
            case "processaction":
            case "cancelaction":
                ImageButton img = sender as CMSImageButton;
                if (img != null)
                {
                    img.Enabled = modifyAllowed;

                    GridViewRow gvr = parameter as GridViewRow;
                    if (gvr != null)
                    {
                        DataRowView drv = gvr.DataItem as DataRowView;
                        if (drv != null)
                        {
                            TranslationStatusEnum status = (TranslationStatusEnum)ValidationHelper.GetInteger(drv["SubmissionStatus"], 0);
                            switch (sourceName.ToLowerCSafe())
                            {
                                case "resubmitaction":
                                    img.Enabled = modifyAllowed && (status == TranslationStatusEnum.WaitingForTranslation) || (status == TranslationStatusEnum.SubmissionError);
                                    break;

                                case "processaction":
                                    img.Enabled = modifyAllowed && (status == TranslationStatusEnum.TranslationReady) || (status == TranslationStatusEnum.TranslationCompleted) || (status == TranslationStatusEnum.ProcessingError);
                                    break;

                                case "cancelaction":
                                    img.Enabled = modifyAllowed && (status == TranslationStatusEnum.WaitingForTranslation);
                                    break;
                            }
                        }
                    }
                }
                return img;

            case "submissionstatus":
                TranslationStatusEnum submissionstatus = (TranslationStatusEnum)ValidationHelper.GetInteger(parameter, 0);
                return TranslationServiceHelper.GetFormattedStatusString(submissionstatus);

            case "submissionprice":
                string price = GetString("general.notavailable");
                double priceVal = ValidationHelper.GetDouble(parameter, -1);
                if (priceVal >= 0)
                {
                    price = priceVal.ToString();
                }
                return price;
        }
        return parameter;
    }

    #endregion
}