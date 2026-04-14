using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using DevExpress.Web;
using Microsoft.VisualBasic.FileIO;
using NESI.Common.Models;
using nesi.core;

namespace Nesi.Web.sections.customer
    {
    public partial class opportunity_import : Page
        {
        private const string TempFolder = @"~\App_Data\OpportunityUploadTemp\";
        private const string SuccessLine = "0|Success|0\n";
        private const string FieldInternalId = "internalId";
        private const string HardErrorsLine = "0|HardErrors|0\n";
        private const string SoftErrorsLine = "0|SoftErrors|0\n";
        private const string SubsidiaryTrue = "T";
        private const string Pipe = "|";
        private const string Comma = ",";
        private const int NumberOfColumns = 6;
        private readonly Dictionary<int, List<Tuple<string,string>>> HardErrors = new Dictionary<int, List<Tuple<string,string>>>();
        private readonly Dictionary<int, List<Tuple<string,string>>> SoftErrors = new Dictionary<int, List<Tuple<string,string>>>();

        private NeMember CurrentUser;

        protected void Page_Init(object sender, EventArgs e)
            {
            // Ensure that the temp folder exists in AppData for this
            if (!Directory.Exists(MapPath(TempFolder)))
                {
                Directory.CreateDirectory(MapPath(TempFolder));
                }
            upControl.FileSystemSettings.UploadFolder = TempFolder;
            upControl.AdvancedModeSettings.TemporaryFolder = TempFolder;
            CurrentUser = Toolbox.do_handle_authentication(OpsPage.OpportunityImport);
            }
        protected void Page_Load(object sender, EventArgs e)
            {
            }

        protected void btProcess_Click(object sender, EventArgs e)
            {
            }
        private struct FieldHeaders
            {
            public const int OpportunityId = 0;
            public const int OpportunityNumber = 1;
            public const int SubsidiaryId = 2;
            public const int CustomerInternalId = 3;
            public const int ProjectedTotal = 4;
            public const int Title = 5;
            public struct Names
                {
                public const string ColumnOpportunityId     = "INTERNALID";
                public const string ColumnOpportunityNumber = "NSNUMBER";
                public const string ColumnSubsidiaryId      = "SUBSIDIARYID";
                public const string ColumnCustomerId        = "CUSTOMERID";
                public const string ColumnTotal             = "TOTAL";
                public const string ColumnTitle             = "TITLE";
                public struct ByOrder
                    {
                    public const string FirstHeader      = "First Header should be internalId";
                    public const string SecondHeader     = "Second Header should be nsNumber";
                    public const string ThirdHeader      = "Third Header should be subsidiaryId";
                    public const string FourthHeader     = "Fourth Header should be customerId";
                    public const string FifthHeader      = "Fifth Header should be total";
                    public const string SixthHeader      = "Sixth Header should be title";
                    }
                }
            }
        private struct ErrorType
            {
            public const bool HardError = true;
            public const bool SoftError = false;
            }
        private struct ErrorMessages
            {
            public struct Invalid
                {
                public const string OpportunityId = "Invalid Opportunity Internal Id";
                public const string OpportunityNumber = "Invalid Opportunity Number";
                public const string SubsidiaryId  = "Invalid Subsidiary Internal Id";
                public const string CustomerId = "Invalid Customer Internal Id";
                public const string Total = "Invalid Total";
                public const string EntireFile = "File has an invalid number of columns - Expecting 6";
                public const string ColumnArrangement = "File has an invalid arrangement of columns - Please review the order on the left-hand side.";
                }
            public struct Exists
                {
                public const string SubsidiaryId = "Referenced Subsidiary Id does not exist - Skipping";
                public const string CustomerId = "Referenced Customer Id does not exist - Skipping";

                }
            public struct Active
                { 
                public const string SubsidiaryId = "Referenced Subsidiary Id is not active - Skipping";
                public const string CustomerId = "Referenced Customer Id is not active - Skipping";
                }
            public struct NotSupplied
                {
                public const string Text = "Not Supplied";
                public const string Title = "Note: Opportunity Title not supplied - using 'Not Supplied' as Title";
                }
            public struct Duplicate
                {
                public const string OpportunityId = "Opportunity Internal Id referenced twice in CSV file - Not processing";
                }
            }
        protected void upControl_OnFileUploadComplete(object _sender, FileUploadCompleteEventArgs _e)
            {
            if (!upControl.UploadedFiles.Any()) return;
            var toProcess = new List<NSOpportunity>();
            var updateList = new List<double>();
            var passedHeader = false;
            using (var conn = Toolbox.connect())
                {
                var subsidiaryList = NeBusinessUnit.SubsidiaryList(conn);
                var customerList = NECustomer.NSCustomerList(conn);
                using (var parser = new TextFieldParser(upControl.UploadedFiles[0].FileContent))
                    {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(Comma);
                    var row = 0;
                    while (!parser.EndOfData)
                        {
                        var hasHardError = false;
                        var fields = parser.ReadFields();
                        if (fields == null) break;
                        // Possible Hard Stop: Number of columns don't match
                        if(fields.Length != NumberOfColumns)
                            {
                            AddError(ErrorType.HardError, row, ErrorMessages.Invalid.EntireFile,fields.Length+" columns");
                            break;
                            }
                        // Possible Hard Stop: Columns aren't arranged as expected
                        // Validate the first line to ensure that the 
                        if(row == 0)
                            {
                            var headerOne = fields[FieldHeaders.OpportunityId].Replace(Pipe, string.Empty).ToUpper();
                            if(headerOne != FieldHeaders.Names.ColumnOpportunityId)
                                {
                                AddError(ErrorType.HardError, row, ErrorMessages.Invalid.ColumnArrangement, FieldHeaders.Names.ByOrder.FirstHeader);
                                hasHardError = true;
                                }
                            var headerTwo = fields[FieldHeaders.OpportunityNumber].Replace(Pipe, string.Empty).ToUpper();
                            if(headerTwo != FieldHeaders.Names.ColumnOpportunityNumber)
                                {
                                AddError(ErrorType.HardError, row, ErrorMessages.Invalid.ColumnArrangement, FieldHeaders.Names.ByOrder.SecondHeader);
                                hasHardError = true;
                                }
                            var headerThree = fields[FieldHeaders.SubsidiaryId].Replace(Pipe, string.Empty).ToUpper();
                            if(headerThree != FieldHeaders.Names.ColumnSubsidiaryId)
                                {
                                AddError(ErrorType.HardError, row, ErrorMessages.Invalid.ColumnArrangement, FieldHeaders.Names.ByOrder.ThirdHeader);
                                hasHardError = true;
                                }
                            var headerFour = fields[FieldHeaders.CustomerInternalId].Replace(Pipe, string.Empty).ToUpper();
                            if(headerFour != FieldHeaders.Names.ColumnCustomerId)
                                {
                                AddError(ErrorType.HardError, row, ErrorMessages.Invalid.ColumnArrangement, FieldHeaders.Names.ByOrder.FourthHeader);
                                hasHardError = true;
                                }
                            var headerFive = fields[FieldHeaders.ProjectedTotal].Replace(Pipe, string.Empty).ToUpper();
                            if(headerFive != FieldHeaders.Names.ColumnTotal)
                                {
                                AddError(ErrorType.HardError, row, ErrorMessages.Invalid.ColumnArrangement, FieldHeaders.Names.ByOrder.FifthHeader);
                                hasHardError = true;
                                }
                            var headerSix = fields[FieldHeaders.Title].Replace(Pipe, string.Empty).ToUpper();
                            if(headerSix != FieldHeaders.Names.ColumnTitle)
                                {
                                AddError(ErrorType.HardError, row, ErrorMessages.Invalid.ColumnArrangement, FieldHeaders.Names.ByOrder.SixthHeader);
                                hasHardError = true;
                                }
                            }
                        if(hasHardError) break; // We don't want to proceed, the order is off.
                        row++;
                        var importedOpportunityId = fields[FieldHeaders.OpportunityId].Replace(Pipe, string.Empty);
                        var importedOpportunityNumber = fields[FieldHeaders.OpportunityNumber].Replace(Pipe, string.Empty);
                        var importedSubsidiaryId = fields[FieldHeaders.SubsidiaryId].Replace(Pipe, string.Empty);
                        var importedCustomerId = fields[FieldHeaders.CustomerInternalId].Replace(Pipe, string.Empty);
                        var importedTotal = fields[FieldHeaders.ProjectedTotal].Replace(Pipe, string.Empty);
                        var importedTitle = fields[FieldHeaders.Title].Trim().Replace(Pipe, string.Empty);

                        passedHeader = !passedHeader && string.Equals(importedOpportunityId.ToUpper(), FieldInternalId.ToUpper());
                        if(passedHeader) continue;
                        //validation

                        if (!int.TryParse(importedOpportunityId, out var opportunityInternalId))
                            {
                            AddError(ErrorType.HardError, row, ErrorMessages.Invalid.OpportunityId,importedOpportunityId);
                            hasHardError = true;
                            }

                        if (!int.TryParse(importedSubsidiaryId, out var subsidiaryInternalId))
                            {
                            AddError(ErrorType.HardError, row, ErrorMessages.Invalid.SubsidiaryId,importedSubsidiaryId);
                            hasHardError = true;
                            }
                        else
                            {
                            if(!subsidiaryList.ContainsKey(subsidiaryInternalId))
                                {
                                AddError(ErrorType.SoftError, row, ErrorMessages.Exists.SubsidiaryId,importedSubsidiaryId);
                                }
                            else if(!subsidiaryList[subsidiaryInternalId])
                                {
                                AddError(ErrorType.SoftError, row, ErrorMessages.Active.SubsidiaryId,importedSubsidiaryId);
                                }
                            }
                        if(!int.TryParse(importedOpportunityNumber, out var opportunityNumber))
                            {
                            AddError(ErrorType.HardError, row, ErrorMessages.Invalid.OpportunityNumber, importedOpportunityId);
                            hasHardError = true;
                            }

                        if (!int.TryParse(importedCustomerId, out var customerInternalId))
                            {
                            AddError(ErrorType.HardError, row, ErrorMessages.Invalid.CustomerId,importedCustomerId);
                            hasHardError = true;
                            }
                        else
                            {
                            if(!customerList.ContainsKey(customerInternalId))
                                {
                                AddError(ErrorType.SoftError, row, ErrorMessages.Exists.CustomerId,importedCustomerId);
                                }
                            else if(!customerList[customerInternalId])
                                {
                                AddError(ErrorType.SoftError, row, ErrorMessages.Active.CustomerId,importedCustomerId);
                                }
                            }

                        if (!double.TryParse(importedTotal, out var total))
                            {
                            AddError(ErrorType.HardError, row, ErrorMessages.Invalid.Total,importedTotal);
                            hasHardError = true;
                            }
                        if(string.IsNullOrEmpty(importedTitle))
                            {
                            importedTitle = ErrorMessages.NotSupplied.Text;
                            AddError(ErrorType.SoftError, row, ErrorMessages.NotSupplied.Title,importedOpportunityId);
                            }
                        if(hasHardError) continue; // We don't want to proceed until everything is solid with this record.
                        // opportunity_internal_id is unique
                        var duplicateExists = updateList.Contains(opportunityInternalId);
                        if(duplicateExists)
                            {
                            AddError(ErrorType.SoftError, row, ErrorMessages.Duplicate.OpportunityId,importedOpportunityId);
                            continue; // We don't want to reprocess an update if it already exists in the upload file.
                            }

                        var opportunityObject = new NSOpportunity(conn, opportunityInternalId)
                                                    {
                                                    SubsidiaryInternalId = subsidiaryInternalId,
                                                    OpportunityNumber = opportunityNumber,
                                                    CustomerInternalId   = customerInternalId,
                                                    ProjectedTotal       = total,
                                                    Title                = importedTitle
                                                    };
                        updateList.Add(opportunityInternalId);
                        toProcess.Add(opportunityObject);
                        }
                    }
                if (HardErrors.Any())
                    {
                    _e.CallbackData = JoinErrors(ErrorType.HardError);
                    return;
                    }
                if (toProcess.Any())
                    {
                    foreach (var lineToUpdate in toProcess)
                        {
                        if(SoftErrors.ContainsKey(lineToUpdate.OpportunityInternalId))
                            {
                            continue;
                            }
                        lineToUpdate.Save(conn);
                        }
                    }
                if(SoftErrors.Any())
                    {
                    _e.CallbackData = JoinErrors(ErrorType.SoftError);
                    return;
                    }
                _e.CallbackData = SuccessLine;
                }
            }
        private void AddError(bool isHardError, int rowId, string errorMessage, string value)
            {
            var error = new Tuple<string, string>(errorMessage, value);
            if(isHardError)
                {
                if(HardErrors.ContainsKey(rowId))
                    {
                    HardErrors[rowId].Add(error);
                    }
                else
                    {
                    HardErrors.Add(rowId, new List<Tuple<string, string>>{error});
                    }
                }
            else
                {
                if(SoftErrors.ContainsKey(rowId))
                    {
                    SoftErrors[rowId].Add(error);
                    }
                else
                    {
                    SoftErrors.Add(rowId, new List<Tuple<string, string>>{error});
                    }
                }
            }
        private string JoinErrors(bool isHardError)
            {
            var pipeList = new StringBuilder();
            if(isHardError)
                {
                pipeList.Append(HardErrorsLine);
                foreach(var hardError in HardErrors)
                    {
                    var rowNumber  = hardError.Key;
                    foreach(var error in hardError.Value)
                        {
                        var errorText  = error.Item1;
                        var errorValue = error.Item2;
                        pipeList.AppendFormat("{0}|{1}|{2}\n", rowNumber, errorText, errorValue);
                        }
                    }
                }
            else
                {
                pipeList.Append(SoftErrorsLine);
                foreach(var softError in SoftErrors)
                    {
                    var rowNumber   = softError.Key;
                    foreach(var error in softError.Value)
                        {
                        var errorText  = error.Item1;
                        var errorValue = error.Item2;
                        pipeList.AppendFormat("{0}|{1}|{2}\n", rowNumber, errorText, errorValue);
                        }
                    }
                }
            return pipeList.ToString();
            }
        }
    }