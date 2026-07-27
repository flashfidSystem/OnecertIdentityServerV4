using OnecertApiV1.Services.Interface;
using System.Net.Mail;
namespace OnecertApiV1.Services
{
    public class clsMail
    {
        private readonly clsParameters _pr; 
        public clsMail(clsParameters pr)
        { 
            _pr = pr;
        }
        private static string PrepareMessageBody(string MsgBody)
        {
            string PrepareMessageBodyRet = null;
            string MailMsg = "";

            MailMsg = "<table width=\"80%\" border=\"0\" cellspacing=\"2\" cellpadding=\"2\">" + "<tr>" + "<td style=\"font:Arial, Helvetica, sans-serif; font-size: 13px; color:#003300\">" + MsgBody + "</td>" + "</tr>" + "</table>";





            PrepareMessageBodyRet = MailMsg;
            return PrepareMessageBodyRet;
        }

        public  void MailSend(string ToEmail, string FromEmail, string Subject, string MsgBody, string ccEmail = "", string attachFile = "")
        {
            try
            {
                // Dim MM As New MailMessage(FromEmail, ToEmail, Subject, PrepareMessageBody(MsgBody))
                var MM = new MailMessage();
                object objState = "";

                MM.From = new MailAddress(FromEmail);
                MM.Subject = Subject;
                MM.Body = PrepareMessageBody(MsgBody);
                MM.IsBodyHtml = true;

                //var ToEmailArr = Strings.Split(ToEmail, ";");
                var ToEmailArr = ToEmail.Split(';');

                for (int I = ToEmailArr.GetLowerBound(0), loopTo = ToEmailArr.GetUpperBound(0); I <= loopTo; I++)
                    MM.To.Add(ToEmailArr[I]);

                if (!string.IsNullOrEmpty(ccEmail.Trim()))
                {
                    MM.CC.Add(ccEmail);
                }

                if (!string.IsNullOrEmpty(attachFile.Trim()))
                {
                    MM.Attachments.Add(new Attachment(attachFile));
                }

                // 'For Credentials
                var objParameters = _pr.GetParameters();

                string sMTPServerName = "";
                string sMTPServerPort = "";

                if (objParameters.parameterNames.Length > 0)
                {
                    sMTPServerName = _pr.getValue(objParameters, "SMTPServerName");
                    sMTPServerPort = _pr.getValue(objParameters, "SMTPServerPort");
                }

                var mySMTP = new SmtpClient(sMTPServerName, Convert.ToInt32(sMTPServerPort));

                // mySMTP.Credentials = New System.Net.NetworkCredential("", "yourpassword")
                // mySMTP.Credentials = New System.Net.NetworkCredential(objParameters.SMTPUserID, objParameters.SMTPUserPwd)
                mySMTP.UseDefaultCredentials = false;

                // Dim mySMTP As New SmtpClient  'No Credentials

                // If ToEmail <> "" Then mySMTP.SendAsync(MM, objState)
                if (!string.IsNullOrEmpty(ToEmail))
                    mySMTP.Send(MM);
                 
                MM.Dispose();
            }
            catch (Exception ex)
            {
               // _dataRepo.LogRequest(ToEmail, "Create Account",ex.ToString());
            }
        }

        public  void SendPasswordToEmail(string FromMail, string UserName, string Password, string Email)
        {
            string MsgBody = "";

            MsgBody = "Dear " + UserName + ",<br>" + "Your password has been reset. Your new password is given below:<br>" + "Password: " + Password + "<br><br>" + "Thank you.<br><br>" + "Yours sincerely,<br>" + "Administrator<br><br>" + "PS: Passwords are case sensitive.";





            MailSend(Email, "\"Administrator\"<" + FromMail + ">", "YOUR PASSWORD!", MsgBody);
        }

        public void SendAdminErrorNotificationAgencyCodes(string FromMail, string ToEmail, string State, string ErrorDesc)
        {
            string MsgBody = "";

            MsgBody = "Dear Admin,<br>" + "An error occured while running a scheduled Agency Codes download for <strong>" + State + "</strong>.<br><br>" + "Below is the error details:<br><br>" + "<span style='color: #FF0000'>" + ErrorDesc + "</span><br><br>" + "Thank you.<br><br>" + "Yours sincerely,<br>" + "Administrator";





            MailSend(ToEmail, "\"Administrator\"<" + FromMail + ">", "SCHEDULED AGENCY CODES DOWNLOAD FAILURE!", MsgBody);
        }

        public void SendAdminErrorNotificationBillTypes(string FromMail, string ToEmail, string State, string ErrorDesc)
        {
            string MsgBody = "";

            MsgBody = "Dear Admin,<br>" + "An error occured while running a scheduled Bill Types download for <strong>" + State + "</strong>.<br><br>" + "Below is the error details:<br><br>" + "<span style='color: #FF0000'>" + ErrorDesc + "</span><br><br>" + "Thank you.<br><br>" + "Yours sincerely,<br>" + "Administrator";





            MailSend(ToEmail, "\"Administrator\"<" + FromMail + ">", "SCHEDULED BILL TYPES DOWNLOAD FAILURE!", MsgBody);
        }
        public static string checkEmpt(string para)
        {
            string a = " (" + para + ")";
            if (para.Trim() == "")
            {
                return "";
            }
            else
            {
                return a;
            }
        }
        public void SendPostingtRequestToCheckers(string FromMail, string ToEmail, string ccEmailCopy, string CreatorUserID, string CreatorName, string PID, string PayerName, string AmountPaid, string State, string AgencyCode, string AgencyDesc, string RevenueCode, string RevenueDesc)
        {
            string MsgBody = "";

            MsgBody = $"Dear Checker,<br>" +
                  $"A SWEEP/Transfer posting request has been submitted for your immediate attention. Kindly find details below:<br><br>" +
                  $"Created By: {CreatorUserID} {checkEmpt(CreatorName)} <br>" +
                  $"PayerID: " + PID + "<br>" +
                  $"PayerName: " + PayerName + "<br>" +
                  $"Amount Paid: " + AmountPaid + "<br>" +
                  $"State: " + State.ToUpper() + "<br>" +
                  $"Agency Code: {AgencyCode} {checkEmpt(AgencyDesc)} <br>" +
                  $"Revenue Code: {RevenueCode} {checkEmpt(RevenueDesc)} <br><br>" +
                  $"Thank you.<br><br>" +
                  $"Yours sincerely,<br>" +
                  $"Administrator";

            MailSend(ToEmail, "\"Administrator\"<" + FromMail + ">", State.ToUpper() + " SWEEP/TRANSFER POSTING NOTIFICATION", MsgBody);
        }

        public void SendPaymentRequestToCheckers(string FromMail, string ToEmail, string ccEmailCopy, string CreatorUserID, string CreatorName, string PID, string PayerName, string AmountPaid, string State, string AgencyCode, string AgencyDesc, string RevenueCode, string RevenueDesc)
        {
            string MsgBody = "";

            MsgBody = $"Dear Checker,<br>" +
                  $"A revenue payment request has been submitted for your immediate attention. Kindly find details below:<br><br>" +
                  $"Created By: {CreatorUserID} {checkEmpt(CreatorName)} <br>" +
                  $"PayerID: " + PID + "<br>" +
                  $"PayerName: " + PayerName + "<br>" +
                  $"Amount Paid: " + AmountPaid + "<br>" +
                  $"State: " + State.ToUpper() + "<br>" +
                  $"Agency Code: {AgencyCode} {checkEmpt(AgencyDesc)} <br>" +
                  $"Revenue Code: {RevenueCode} {checkEmpt(RevenueDesc)} <br><br>" +
                  $"Thank you.<br><br>" +
                  $"Yours sincerely,<br>" +
                  $"Administrator";

            MailSend(ToEmail, "\"Administrator\"<" + FromMail + ">", State.ToUpper() + " REVENUE PAYMENT NOTIFICATION", MsgBody);
        }

        public void SendUpdatedPaymentRequestToMakers(string FromMail, string ToEmail, string ccEmailCopy, string CheckerUserID, string CheckerName, string PID, string PayerName, string AmountPaid, string State, string AgencyCode, string AgencyDesc, string RevenueCode, string RevenueDesc)
        {
            string MsgBody = "";

            MsgBody = "Dear Maker,<br>" +
                  $"A revenue payment request has just been updated with <strong>REJECT</strong> status by the checker and require your immediate attention. Kindly find details below:<br><br>" +
                  $"Checked By: CheckerUserID {checkEmpt(CheckerName)} <br>" +
                  $"PayerID: " + PID + "<br>" +
                  $"PayerName: " + PayerName + "<br>" +
                  $"Amount Paid: " + AmountPaid + "<br>" +
                  $"State: " + State.ToUpper() + "<br>" +
                  $"Agency Code: {AgencyCode} {checkEmpt(AgencyDesc)} <br>" +
                  $"Revenue Code: {RevenueCode} {checkEmpt(RevenueDesc)} <br><br>" +
                  $"Thank you.<br><br>" +
                  $"Yours sincerely,<br>" +
                  $"Administrator";

            MailSend(ToEmail, "\"Administrator\"<" + FromMail + ">", State.ToUpper() + " REVENUE PAYMENT NOTIFICATION - REJECTION", MsgBody);
        }

        public void SendUpdatedPostingRequestToMakers(string FromMail, string ToEmail, string ccEmailCopy, string CheckerUserID, string CheckerName, string PID, string PayerName, string AmountPaid, string State, string AgencyCode, string AgencyDesc, string RevenueCode, string RevenueDesc)
        {
            string MsgBody = "";

            MsgBody = $"Dear Maker,<br>" +
                  $"A SWEEP/Transfer request has just been updated with <strong>REJECT</strong> status by the checker and require your immediate attention. Kindly find details below:<br><br>" +
                  $"Checked By: {CheckerUserID}  {checkEmpt(CheckerName)} <br>" +
                  $"PayerID: " + PID + "<br>" +
                  $"PayerName: " + PayerName + "<br>" +
                  $"Amount Paid: " + AmountPaid + "<br>" +
                  $"State: " + State.ToUpper() + "<br>" +
                  $"Agency Code: {AgencyCode} {checkEmpt(AgencyDesc)} <br>" +
                  $"Revenue Code: {RevenueCode} {checkEmpt(RevenueDesc)} <br><br>" +
                  $"Thank you.<br><br>" +
                  $"Yours sincerely,<br>" +
                  $"Administrator";
            MailSend(ToEmail, "\"Administrator\"<" + FromMail + ">", State.ToUpper() + " SWEEP/TRANSFER POSTING NOTIFICATION - REJECTION", MsgBody);
        }

        public void SendSecurityCredentialsToAdmins(string FromMail, string ToEmail, string AttachFile, string UserID)
        {
            string MsgBody = "";

            MsgBody = "Dear CARP Admin,<br>" + "A service credential reset has just been done on the portal.<br><br>" + "Initiating Officer: " + UserID + "<br><br>" + "Kindly find new credential file attached. The password for the archive will be sent in a different mail." + "<br><br>" + "Thank you.<br><br>" + "Yours sincerely,<br>" + "Administrator";
            MailSend(ToEmail, "\"Administrator\"<" + FromMail + ">", "SERVICE CREDENTIAL RESET", MsgBody, attachFile: AttachFile);
        }

        public void SendSecurityCredentialsFilePassword(string FromMail, string ToEmail, string UserID, string Password)
        {
            string MsgBody = "";

            MsgBody = "Dear CARP Admin,<br>" + "A service credential reset has just been done on the portal.<br><br>" + "Initiating Officer: " + UserID + "<br>" + "File Password: " + Password + "<br><br>" + "Thank you.<br><br>" + "Yours sincerely,<br>" + "Administrator";
            MailSend(ToEmail, "\"Administrator\"<" + FromMail + ">", "SERVICE CREDENTIAL RESET: FILE PASSWORD", MsgBody);
        }

    }
}