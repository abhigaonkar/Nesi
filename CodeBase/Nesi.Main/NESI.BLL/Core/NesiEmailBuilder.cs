using System;
using nesi.core;
using NESI.BLL.Common.Shared;

namespace NESI.BLL.Core
{
    /// <summary>
    /// Small utility class, centralize email construction
    /// makes target code more easily readable
    /// </summary>
    public static class NesiEmailBuilder
    {
        public static NeEMail BuildForgotPasswordEmail(string emailTo, string emailBody)
        {
            if (emailTo == null) throw new ArgumentNullException(nameof(emailTo));
            if (emailBody == null) throw new ArgumentNullException(nameof(emailBody));

            return new NeEMail
            {
                To = emailTo,
                isHTML = true,
                Subject = "Reset your password for SparkOps",
                Body = emailBody,
                From = Configuration.EmailFrom
            };
        }

        public static NeEMail CutTicketToIT(string emailFrom, string emailBody)
        {
            if (emailFrom == null) throw new ArgumentNullException(nameof(emailFrom));
            if (emailBody == null) throw new ArgumentNullException(nameof(emailBody));

            return new NeEMail
            {
                To = "help@" + Toolbox.app_setting("DomainForEmail"),
                isHTML = true,
                Subject = "Automatic Ticket Cut from " + Toolbox.app_setting("Domain"),
                Body = emailBody,
                CC = emailFrom,
                From = emailFrom
            };
        }

        public static NeEMail GetNewLoginRequestEmail(string emailAddress)
        {
            if (emailAddress == null) throw new ArgumentNullException(nameof(emailAddress));
            return new NeEMail
            {
                To = "help@sparkpower.ca",
                Subject = "New login request",
                Body = emailAddress + " has requested a login for " + Toolbox.app_setting("Domain")  + " Please process this request.",
                From = Configuration.EmailFrom
            };
        }
    }
}