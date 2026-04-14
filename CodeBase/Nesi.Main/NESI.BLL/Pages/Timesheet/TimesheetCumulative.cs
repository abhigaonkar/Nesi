using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web.Http.ModelBinding;
using nesi.core;
using NESI.BLL.Base;
using NESI.BLL.Common.Cache;
using NESI.BLL.Common.Shared;
using NESI.BLL.Core;
using NESI.BLL.Core.Employee;
using NESI.Data.Entities;
using NESI.DTO.ViewModels.Core;
using NESI.DTO.ViewModels.Page.TimeSheet;
using huddle = nesi.core.huddle;
using Ne2WOProg = NESI.BLL.Core.Ne2WOProg;
using User = NESI.DTO.ViewModels.Page.TimeSheet.User;


// ReSharper disable CompareOfFloatsByEqualityOperator
#pragma warning disable 168

namespace NESI.BLL.Pages.Timesheet
{
    public partial class Timesheet
    {
        #region des
        //
        // Use the class to implement the task https://spcnesidev.visualstudio.com/Nesi/_workitems/edit/1801/
        //

        //
        // Recap, when creating a timesheet record, the calling stack looks like below:
        //
        //     .... public static void time_entry_create(ref time_entry_vars tev)
        //     ........... public bool AddMemberTime(int mem_id)
        //
        // Based on above, we will make a checking point before doing any timesheet updates (add/edit/delete).
        //
        #endregion

        public CumulativeInformation GetCumulativeInformation(CumulativeParameter parameter)
        {
            CumulativeInformation result = new CumulativeInformation();
            result.DoesItExceedMaxValue = true;

            if (parameter == null || parameter.action == TimesheetAction.Default)
            {
                // Input is not valid, return true to block any creation due to can't go through this check.
                result.DoesItExceedMaxValue = true;
                result.Errors = new List<CheckErrorRecord>
                {
                    new CheckErrorRecord{ error = "Input not valid." }
                };
                return result;
            }

            //
            // Validaton checks.
            //
            var errors = GetCumulativeInfor_Validation(parameter);
            if (errors.Count > 0)
            {
                // Input is not valid, return true to block any creation due to can't go through this check.
                result.DoesItExceedMaxValue = true;
                result.Errors = errors;
                return result;
            }

            //
            // Get existing records.
            //
            var alreadyCommitedRecords = GetCommittedTimesheetRecords(parameter.ForWho, parameter.OnWhichDate);

            //
            // Checking now.
            //

            if (parameter.action == TimesheetAction.Add)
            {
                return GetCumulativeInfor_WhenAdding(parameter, alreadyCommitedRecords);
            }

            if (parameter.action == TimesheetAction.Edit)
            {
                return GetCumulativeInfor_WhenEditing(parameter, alreadyCommitedRecords);
            }

            if (parameter.action == TimesheetAction.Delete)
            {
                return GetCumulativeInfor_WhenDeleting(parameter, alreadyCommitedRecords);
            }

            return result;
        }

        private double GetCumulativeInfor_Total(List<membertime> list)
        {
            if (list.Count == 0)
            {
                return 0;
            }

            double total = list.Sum(record => record.NumberOfHours.Value); // better ro set a breakpoint...
            return total;
        }

        private CumulativeInformation GetCumulativeInfor_WhenAdding(CumulativeParameter parameter, List<membertime> list)
        {
            CumulativeInformation result = new CumulativeInformation();
            result.DoesItExceedMaxValue = false;

            //
            // Get total commmitted hours.
            //
            double previous = GetCumulativeInfor_Total(list);

            //
            // Get the post value after adding: // For Add/Delete, save it in newValue, leave oldValue to 0
            //
            double post = previous + parameter.newValue; // note the ++++++++++

            //
            // Compare with the bar value
            //
            var max = parameter.GetMax();
            if (post > max)
            {
                // Exceeds
                result.DoesItExceedMaxValue = true;
                result.AlreadyCommitedHours = previous;

                result.Errors = new List<CheckErrorRecord>
                {
                    new CheckErrorRecord{ error = $"You have exceeded the maximum hours allowed per day. (24 hours per day)" }
                };
            }

            return result;
        }

        private CumulativeInformation GetCumulativeInfor_WhenEditing(CumulativeParameter parameter, List<membertime> list)
        {
            CumulativeInformation result = new CumulativeInformation();
            result.DoesItExceedMaxValue = false;

            if (parameter.oldValue == parameter.newValue)
            {
                // For editing...., we will let it go.
                return result;
            }

            //
            // Get total commmitted hours.
            //
            double previous = GetCumulativeInfor_Total(list);

            //
            // Get the post value after adding: // For Edit, newValue will be expected hours, and oldValue will be the previous one.
            //
            double post = previous - parameter.oldValue + parameter.newValue; 

            //
            // Compare with the bar value
            //
            var max = parameter.GetMax();
            if (post > max)
            {
                // Exceeds
                result.DoesItExceedMaxValue = true;
                result.AlreadyCommitedHours = previous;

                result.Errors = new List<CheckErrorRecord>
                {
                    new CheckErrorRecord{ error = $"You have exceeded the maximum hours allowed per day. (24 hours per day)" }
                };
            }

            if (result.DoesItExceedMaxValue)
            {
                //
                // Corner case:
                // Supposed for Adam on a past day that exceeds 24 hours:
                // and the manager wants to edit hours from 12 to 6 hours, so his cumulative hours will be descrease from 26 to 20 hours.
                //

                if (parameter.oldValue > 0 && parameter.newValue > 0 && 
                    parameter.oldValue > parameter.newValue)
                {
                    result.DoesItExceedMaxValue = false;
                    result.Errors = new List<CheckErrorRecord> { };
                }
            }

            return result;
        }

        private CumulativeInformation GetCumulativeInfor_WhenDeleting(CumulativeParameter parameter, List<membertime> list)
        {
            CumulativeInformation result = new CumulativeInformation();
            result.DoesItExceedMaxValue = false;

            //
            // Get total commmitted hours.
            //
            double previous = GetCumulativeInfor_Total(list);

            //
            // Get the post value after deleting: // For Add/Delete, save it in newValue, leave oldValue to 0
            //
            double post = previous - parameter.newValue; // note the ---------

            //
            // Compare with the bar value
            //
            var max = parameter.GetMax();
            if (post > max)
            {
                // Exceeds
                result.DoesItExceedMaxValue = true;
                result.AlreadyCommitedHours = previous;

                result.Errors = new List<CheckErrorRecord>
                {
                    new CheckErrorRecord{ error = $"You have exceeded the maximum hours allowed per day. (24 hours per day)" }
                };
            }

            if (result.DoesItExceedMaxValue)
            {
                //
                // Corner case:
                // Supposed for Adam on a past day that exceeds 24 hours:
                // and the manager wants to delete a positive one, 6 hours, so his cumulative hours will be descrease from 26 to 20 hours.
                //

                if (parameter.newValue > 0)
                {
                    result.DoesItExceedMaxValue = false;
                    result.Errors = new List<CheckErrorRecord> { };
                }
            }

            return result;
        }

        private List<CheckErrorRecord> GetCumulativeInfor_Validation(CumulativeParameter parameter)
        {
            var errors = new List <CheckErrorRecord>{};

            if (parameter.GetMax() <= 0)
            {
                var e = new CheckErrorRecord {  error = "Max hours not set."};
                errors.Add(e);
                return errors;
            }

            if (parameter.ForWho <= 0)
            {
                var e = new CheckErrorRecord { error = "User not set." };
                errors.Add(e);
                return errors;
            }

            if (parameter.OnWhichDate == null)
            {
                var e = new CheckErrorRecord { error = "Date not set." };
                errors.Add(e);
                return errors;
            }

            //
            // Server side validation for https://spcnesidev.visualstudio.com/Nesi/_workitems/edit/1846/
            //
            if (parameter.action == TimesheetAction.Add)
            {
                if (parameter.newValue <= 0)
                {
                    var e = new CheckErrorRecord { error = "You may only enter positive time." };
                    errors.Add(e);
                    return errors;
                }
            }

            if (parameter.action == TimesheetAction.Edit)
            {
                if (parameter.newValue == parameter.oldValue)
                {
                    // Corner case: if it is a negative value input in past day and the user just make a change for others like comments.
                }
                else
                {
                    if (parameter.newValue <= 0)
                    {
                        var e = new CheckErrorRecord { error = "You may only enter positive time." };
                        errors.Add(e);
                        return errors;
                    }
                }
            }

            return errors;
        }

        private List<membertime> GetCommittedTimesheetRecords(int userId, DateTime date)
        {
            // This is to call the existing function, why not working
            // var list = this.GetList(userId, date);

            var list = _db.membertime.Where(
                r => r.membertime_memberid == userId &&
                r.Date.Year == date.Year && r.Date.Month == date.Month && r.Date.Day == date.Day).ToList();

            if (list == null)
            {
                return new List<membertime> { };
            }

            return list;
        }
    }

    #region classes
    public class CumulativeInformation : CumulativeParameter
    {
        public double AlreadyCommitedHours { get; set; }
        public bool DoesItExceedMaxValue { get; set; }

        public List<CheckErrorRecord> Errors { get; set; }
    }

    public class CumulativeParameter
    {
        private static double MaxHour = 24;

        // The Max hours, don't need to set it.
        protected double ExceedHours { get; set; }

        //
        // For who on which day.
        //
        public int ForWho { get; set; }
        public DateTime OnWhichDate { get; set; }

        //
        // The hours to add/delete/changed to
        //
        public TimesheetAction action { get; set; }

        //
        // For Add/Delete, save it in newValue, leave oldValue to 0
        // For Edit, newValue will be expected hours, and oldValue will be the previous one.
        // 
        public double newValue { get; set; }
        public double oldValue;

        public CumulativeParameter()
        {
            // By default hours.
            this.ExceedHours = MaxHour;
        }

        public double GetMax()
        {
            return this.ExceedHours;
        }
    }

    public enum TimesheetAction
    {
        Default = 0,
        Add,
        Edit,
        Delete
    }

    public class CheckErrorRecord
    {
         public string error { get; set; }
    }
    #endregion
}
