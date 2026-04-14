using System;
using nesi.core;
using System.Collections.Generic;

namespace NESI.BLL.Pages.Timesheet.BreakTime
{
    public class BreakTimeRecordChangeNotification
    {
        public BreakTimeRecord PrviousRecord { get; set; }
        public BreakTimeRecord PostRecord { get; set; }

        // Whose info is changed.
        public int memberIDInRRecord { get; set; }
        public string memberNameInRecord { get; set; }

        // Who made the changes.
        public int CurrentLoginUserId { get; set; }
        public string CurrentLoginUserName { get; set; }

        // BM that reports to when changed.
        public string BM { get; set; }
        public int BM_ID { get; set; }

        // Used to see who's record is updated. 
        public string Their { get; set; }

        // Email
        public string To { get; set; }
        public string Cc { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }
        public string From { get; set; }
        public bool IsHTML { get; set; }
    }

    public class BreakTimeEmailSender
    {
        public static bool Send(BreakTimeRecordChangeNotification notification)
        {
            bool sendOk = false;
            //
            // Load all required info.
            //
            var done = LoadInfo(notification);
            if (!done)
            {
                return sendOk;
            }

            //
            // If the BM changed the break time record, no email need to send out.
            //
            if (notification.BM_ID == notification.CurrentLoginUserId)
            {
                return sendOk;
            }

            //
            // Send Email
            //
            var em = new NeEMail
            {
                From = notification.From,
                To = notification.To,
                CC = notification.Cc,
                Subject = notification.Subject,
                Body = notification.Body,
                isHTML = notification.IsHTML
            };

            try
            {
                em.Send();
                sendOk = true;
            }
            catch (Exception ex)
            {
                sendOk = false;
            }

            return sendOk;
        }

        #region private code
        private static bool LoadInfo(BreakTimeRecordChangeNotification notification)
        {
            bool completed = false;

            //
            // Get user info that owns this break time record.
            //
            var whoOwnRecord = new NeMember(notification.memberIDInRRecord);
            if (whoOwnRecord == null || whoOwnRecord.id <=0 )
            {
                return completed;
            }

            notification.memberNameInRecord = whoOwnRecord.FullName;

            //
            // Get the BM and his/her email about above user
            //
            var branch = new NeBusinessUnit(notification.PrviousRecord.business_unit_id);
            if (branch == null || branch.branch_manager == null ||
                string.IsNullOrEmpty(branch.branch_manager.FullName) ||
                string.IsNullOrEmpty(branch.branch_manager.NEEmail))
            {
                return completed;
            }

            notification.BM = branch.branch_manager.Nickname;
            notification.To = branch.branch_manager.NEEmail;
            notification.BM_ID = branch.branch_manager.id;

            //
            // Get the subject for email.
            //
            notification.Subject = $"Alert: Break Time Record Updated - {notification.memberNameInRecord}";

            //
            // Get "their"
            //
            if (notification.memberIDInRRecord == notification.CurrentLoginUserId)
            {
                // The record' owner updated his/her own record.
                notification.Their = "their";
            }
            else
            {
                // Other pepole (the current signed in user) updated the record.
                notification.Their = notification.memberNameInRecord;
            }
            
            //
            // Get the email body.
            //
            notification.IsHTML = true;
            try
            {
                notification.Body = LoadEmailBodyTemplate(notification);
            }
            catch (Exception ex)
            {
                // Log.
                return completed;
            }

            if (string.IsNullOrEmpty(notification.Body))
            {
                return completed;
            }

            //
            // From
            //
            notification.From = "noreply@sparkpowercorp.com";

            // all set.
            completed = true;

            return completed;
        }

        private static string LoadEmailBodyTemplate(BreakTimeRecordChangeNotification notification)
        {
            string emailTemplate = @"
       <div>
        <div>{BMNickname},</div>
        <div>This is to inform you that {EmployeeNickname} has edited {their} break time request for {requestDate}.<br><br></div>
        <div>
          <h2><span>Old</span><b>:</b></h2>
          <b>Start Time: </b>{oldStartTime}<br>
          <b>Morning Break Start: </b>{oldMorningBreakstart}<br>
          <b>Morning Break Duration (minutes): </b>{oldMorningBreakduration}<br>
          <b>Lunch Break Start: </b>{oldLunchBreakStart}<br>
          <b>Lunch Break Duration (minutes): </b>{oldLunchBreakDuration}<br>
          <b>Afternoon Break Start: </b>{oldAfternoonBreakStart}<br>
          <b>Afternoon Break Duration (minutes): </b>{oldAfternoonBreakDuration}<br>

          <h2><span>New</span><b>:</b></h2>
          <b>Start Time: </b>{newStartTime}<br>
          <b>Morning Break Start: </b>{newMorningBreakStart}<br>
          <b>Morning Break Duration (minutes): </b>{newMorningBreakDuration}<br>
          <b>Lunch Break Start: </b>{newLunchBreakStart}<br>
          <b>Lunch Break Duration (minutes): </b>{newLunchBreakDuration}<br>
          <b>Afternoon Break Start: </b>{newAfternoonBreakStart}<br>
          <b>Afternoon Break Duration (minutes): </b>{newAfternoonBreakDuration}<br><br>
        </div>
      </div>";

            //
            // Part 1
            //

            // BM Nickname
            emailTemplate = emailTemplate.Replace("{BMNickname}", notification.BM);

            //Employee Nickname
            emailTemplate = emailTemplate.Replace("{EmployeeNickname}", notification.CurrentLoginUserName);

            //request Date
            emailTemplate = emailTemplate.Replace("{requestDate}", notification.PrviousRecord.date.ToString("yyyy-MM-dd"));

            // their
            emailTemplate = emailTemplate.Replace("{their}", notification.Their);

            //
            // Part 2
            //
            emailTemplate = emailTemplate.Replace("{oldStartTime}", notification.PrviousRecord.start_time.ToString().Substring(0, 5));
            emailTemplate = emailTemplate.Replace("{oldMorningBreakstart}", notification.PrviousRecord.morning_break_start.ToString().Substring(0, 5));
            emailTemplate = emailTemplate.Replace("{oldMorningBreakduration}", notification.PrviousRecord.morning_break_duration.ToString());
            emailTemplate = emailTemplate.Replace("{oldLunchBreakStart}", notification.PrviousRecord.lunch_break_start.ToString().Substring(0, 5));
            emailTemplate = emailTemplate.Replace("{oldLunchBreakDuration}", notification.PrviousRecord.lunch_break_duration.ToString());
            emailTemplate = emailTemplate.Replace("{oldAfternoonBreakStart}", notification.PrviousRecord.afternoon_break_start.ToString().Substring(0, 5));
            emailTemplate = emailTemplate.Replace("{oldAfternoonBreakDuration}", notification.PrviousRecord.afternoon_break_duration.ToString());

            //
            // Part 3
            //
            emailTemplate = emailTemplate.Replace("{newStartTime}", notification.PostRecord.start_time.ToString().Substring(0, 5));
            emailTemplate = emailTemplate.Replace("{newMorningBreakStart}", notification.PostRecord.morning_break_start.ToString().Substring(0, 5));
            emailTemplate = emailTemplate.Replace("{newMorningBreakDuration}", notification.PostRecord.morning_break_duration.ToString());
            emailTemplate = emailTemplate.Replace("{newLunchBreakStart}", notification.PostRecord.lunch_break_start.ToString().Substring(0, 5));
            emailTemplate = emailTemplate.Replace("{newLunchBreakDuration}", notification.PostRecord.lunch_break_duration.ToString());
            emailTemplate = emailTemplate.Replace("{newAfternoonBreakStart}", notification.PostRecord.afternoon_break_start.ToString().Substring(0, 5));
            emailTemplate = emailTemplate.Replace("{newAfternoonBreakDuration}", notification.PostRecord.afternoon_break_duration.ToString());

            return emailTemplate;
        }
        #endregion

        #region Email Template
        /*
             <div>
                <div>{BMNickname},</div>
                <div>This is to inform you that {EmployeeNickname} has edited their break time request for {requestDate}.<br><br></div>
                <div>
                  <h2><span>Old</span><b>:</b></h2>
                  <b>Start Time: </b>{oldStartTime}<br>
                  <b>Morning Break Start: </b>{oldMorningBreakstart}<br>
                  <b>Morning Break Duration (minutes): </b>{oldMorningBreakduration}<br>
                  <b>Lunch Break Start: </b>{oldLunchBreakStart}<br>
                  <b>Lunch Break Duration (minutes): </b>{oldLunchBreakDuration}<br>
                  <b>Afternoon Break Start: </b>{oldAfternoonBreakStart}<br>
                  <b>Afternoon Break Duration (minutes): </b>{oldAfternoonBreakDuration}<br>

                  <h2><span>New</span><b>:</b></h2>
                  <b>Start Time: </b>{newStartTime}<br>
                  <b>Morning Break Start: </b>{newMorningBreakStart}<br>
                  <b>Morning Break Duration (minutes): </b>{newMorningBreakDuration}<br>
                  <b>Lunch Break Start: </b>{newLunchBreakStart}<br>
                  <b>Lunch Break Duration (minutes): </b>{newLunchBreakDuration}<br>
                  <b>Afternoon Break Start: </b>{newAfternoonBreakStart}<br>
                  <b>Afternoon Break Duration (minutes): </b>{newAfternoonBreakDuration}<br><br>
                </div>
              </div>

         */
        #endregion
    }
}