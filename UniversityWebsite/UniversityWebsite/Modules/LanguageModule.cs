using System;
using System.Threading;
using System.Web;

namespace UniversityWebsite.Modules
{
    /// <summary>
    /// Summary description for LanguageModule
    /// </summary>
    public class LanguageModule : IHttpModule
    {
        public static class Constants
        {
            public const string SessionLanguage = "language";
        }
        public void Init(HttpApplication context)
        {
            context.AcquireRequestState += new EventHandler(OnAcquireRequestState);
        }

        public void Dispose()
        {

        }

        public void OnAcquireRequestState(Object iObject, EventArgs iEventArgs)
        {
            HttpApplication lHttpApplication = iObject as HttpApplication;

            // check whether the language change parameter has been passed
            if (lHttpApplication == null) return;
            var lLanguage =
                lHttpApplication.Request.Params[Constants.SessionLanguage];
            var lBoolLanguageChanged = false;
            if (lLanguage == null)
            {
                // if language parameter is not sent, then take language from session
                lLanguage = (string)lHttpApplication.Session[Constants.SessionLanguage];
            }
            else
            {
                // If language parameter is indeed sent, then user wants to change language.
                // I will make sure I tag this in order to redirect to.
                lBoolLanguageChanged = true;
            }

            // having the language a thand, let us set it.
            var lCulture = new System.Globalization.CultureInfo(lLanguage);

            Thread.CurrentThread.CurrentCulture = lCulture;
            Thread.CurrentThread.CurrentUICulture = lCulture;

            // save language to session
            lHttpApplication.Session[Constants.SessionLanguage] = lLanguage;

            // check whether I have redirect
            if (lBoolLanguageChanged && lHttpApplication.Request.UrlReferrer != null)
            {
                lHttpApplication.Response.Redirect(
                    lHttpApplication.Request.UrlReferrer.AbsolutePath);
            }
        } // OnAcquireRequestState
        //-------------------------

    }
} // class LanguageModule
//------------------------