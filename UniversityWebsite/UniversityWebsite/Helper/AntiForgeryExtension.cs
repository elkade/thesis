﻿using System;
using System.Web.Mvc;

namespace UniversityWebsite.Helper
{
    public static class AntiForgeryExtension
    {
        public static string RequestVerificationToken(this HtmlHelper helper)
        {
            return String.Format("ncg-request-verification-token={0}", GetTokenHeaderValue());
        }

        private static string GetTokenHeaderValue()
        {
            string cookieToken, formToken;
            System.Web.Helpers.AntiForgery.GetTokens(null, out cookieToken, out formToken);
            return cookieToken + ":" + formToken;
        }
    }
}