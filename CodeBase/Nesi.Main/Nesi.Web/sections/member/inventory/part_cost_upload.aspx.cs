using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using System.IO;
using MySql.Data.MySqlClient;
using nesi.core;
using NESI.Common.Models;
using DevExpress.Web;
using CsvHelper;
using System.Globalization;

namespace Nesi.Web.sections.member.inventory
{
    public partial class part_cost_upload : System.Web.UI.Page
    {
        private NeMember CurrentUser;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                CurrentUser = Toolbox.do_handle_authentication(OpsPage.PartCostUpload);
                Session["CurrentUser"] = CurrentUser;
            }
        }

        protected void upControl_OnFileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        {
            if (e.IsValid && e.UploadedFile.FileName.EndsWith(".csv"))
            {
                try
                {
                    using (var stream = e.UploadedFile.FileContent)
                    using (var reader = new StreamReader(stream))
                    using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                    {
                        csv.Read();
                        csv.ReadHeader();

                        var expectedHeaders = new string[]
                        {
                            "internal_org_item_id", "avg_price", "recommended_price", "week_of", "Branch"
                        };

                        var actualHeaders = csv.Context.HeaderRecord;

                        if (actualHeaders == null || !expectedHeaders.SequenceEqual(actualHeaders))
                        {
                            throw new Exception("CSV file headers are incorrect or in the wrong order. Expected headers: internal_org_item_id, avg_price, recommended_price, week_of, Branch.");
                        }

                        var records = new List<PartCostUpload>();
                        var failedRecords = new List<FailedRecords>();
                        int rowIndex = 1; 

                        while (csv.Read())
                        {
                            rowIndex++;

                            string internalOrgItemId = csv.GetField("internal_org_item_id");
                            string avgPriceStr = csv.GetField("avg_price");
                            string recommendedPriceStr = csv.GetField("recommended_price");
                            string weekOfStr = csv.GetField("week_of");
                            string branch = csv.GetField("Branch");

                            if (string.IsNullOrWhiteSpace(internalOrgItemId) || internalOrgItemId.Length > 10)
                            {
                                failedRecords.Add(new FailedRecords
                                {
                                    InternalOrgItemId = string.IsNullOrWhiteSpace(internalOrgItemId)?null: internalOrgItemId,
                                    AvgPrice=avgPriceStr,
                                    RecommendedPrice=recommendedPriceStr,
                                    WeekOf=weekOfStr,
                                    Branch = branch,
                                    ErrorMessage = string.IsNullOrWhiteSpace(internalOrgItemId)
                                        ? "Missing Part Number"
                                        : "Part Number Not In Inventory Item Master"
                                });
                                continue;
                            }

                            decimal avgPrice = 0;
                            decimal recommendedPrice = 0;

                            if (!string.IsNullOrWhiteSpace(avgPriceStr) && !decimal.TryParse(avgPriceStr, out avgPrice))
                            {
                                failedRecords.Add(new FailedRecords
                                {
                                    InternalOrgItemId = internalOrgItemId,
                                    AvgPrice = avgPriceStr,
                                    RecommendedPrice = recommendedPriceStr,
                                    WeekOf = weekOfStr,
                                    Branch = branch,
                                    ErrorMessage = "Invalid value in 'avg_price'"
                                });
                                continue;
                            }
                            if (!string.IsNullOrWhiteSpace(recommendedPriceStr) && !decimal.TryParse(recommendedPriceStr, out recommendedPrice))
                            {
                                failedRecords.Add(new FailedRecords
                                {
                                    InternalOrgItemId = internalOrgItemId,
                                    AvgPrice = avgPriceStr,
                                    RecommendedPrice = recommendedPriceStr,
                                    WeekOf = weekOfStr,
                                    Branch = branch,
                                    ErrorMessage = "Invalid value in 'recommended_price'"
                                });
                                continue;
                            }

                            var dateFormats = new string[]
                            {
                                "yyyy-MM-dd", "MM/dd/yyyy", "dd-MM-yyyy", "M/d/yyyy", "yyyy/MM/dd", "dd/MM/yyyy", "dd/M/yyyy", "dd-M-yyyy", "M-dd-yyyy", "MM-dd-yyyy"
                            };

                            if (!DateTime.TryParseExact(weekOfStr, dateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime weekOf))
                            {
                                failedRecords.Add(new FailedRecords
                                {
                                    InternalOrgItemId = internalOrgItemId,
                                    AvgPrice = avgPriceStr,
                                    RecommendedPrice = recommendedPriceStr,
                                    WeekOf = weekOfStr,
                                    Branch = branch,
                                    ErrorMessage = "Invalid date format in 'week_of'"
                                });
                                continue;
                            }

                            records.Add(new PartCostUpload
                            {
                                InternalOrgItemId = internalOrgItemId,
                                AvgPrice = avgPrice,
                                RecommendedPrice = recommendedPrice,
                                WeekOf = weekOf,
                                Branch = branch,

                            });

                        }

                        SaveDataToDatabase(records);

                        CurrentUser = Session["CurrentUser"] as NeMember;

                        using (var conn = Toolbox.connect())
                        {
                            using (MySqlCommand cmd = new MySqlCommand("CALL Usp_Item_Cost_Upload(1, @userid)", conn))
                            {

                                cmd.Parameters.AddWithValue("@userid", CurrentUser.id);

                                using (var readerSP = cmd.ExecuteReader())
                                {
                                    while (readerSP.Read())
                                    {
                                        failedRecords.Add(new FailedRecords
                                        {
                                            InternalOrgItemId = readerSP.GetString("internal_org_item_id").ToString(),
                                            AvgPrice = readerSP.GetDecimal("avg_price").ToString(),
                                            RecommendedPrice = readerSP.GetDecimal("recommended_price").ToString(),
                                            WeekOf = readerSP.GetDateTime("week_of").ToString(),
                                            Branch = readerSP.GetString("Branch").ToString(),
                                            ErrorMessage = readerSP.GetString("error_message")
                                        });
                                    }

                                    if (failedRecords.Count > 0)
                                    {
                                        e.CallbackData = "These records were not uploaded. Please review.";
                                        Session["FailedUploads"] = failedRecords;
                                        lblUploadStatus.Text = "These records were not uploaded. Please review.";
                                        lblUploadStatus.Visible = true;
                                    }
                                    else
                                    {
                                        e.CallbackData = "CSV Data Uploaded Successfully!";
                                        lblUploadStatus.Text = "CSV Data Uploaded Successfully!";
                                        lblUploadStatus.Visible = true;
                                        gridFailedUploads.Visible = false;
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    lblUploadStatus.Visible = true;
                    e.CallbackData = "Error: " + ex.Message;
                }
            }
            else
            {
                lblUploadStatus.Visible = true;
                e.CallbackData = "Please upload a valid CSV file.";
            }
        }

        private void SaveDataToDatabase(List<PartCostUpload> records)
        {
            const int batchSize = 5000;

            using (var conn = Toolbox.connect())
            {
                using (MySqlCommand truncateCmd = new MySqlCommand("TRUNCATE TABLE temp_csv_item_cost_upload;", conn))
                {
                    truncateCmd.ExecuteNonQuery();
                }

                int totalRecords = records.Count;
                int totalBatches = (int)Math.Ceiling((double)totalRecords / batchSize);

                for (int i = 0; i < totalBatches; i++)
                {
                    var batch = records.Skip(i * batchSize).Take(batchSize).ToList();

                    using (var transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            foreach (var record in batch)
                            {
                                string query = @"INSERT INTO temp_csv_item_cost_upload (internal_org_item_id, avg_price, recommended_price, week_of, Branch) 
                                                 VALUES (@internal_org_item_id, @avg_price, @recommended_price, @week_of, @branch)";
                                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                                {
                                    cmd.Parameters.AddWithValue("@internal_org_item_id", record.InternalOrgItemId);
                                    cmd.Parameters.AddWithValue("@avg_price", record.AvgPrice);
                                    cmd.Parameters.AddWithValue("@recommended_price", record.RecommendedPrice);
                                    cmd.Parameters.AddWithValue("@week_of", record.WeekOf.ToString("yyyy-MM-dd"));
                                    cmd.Parameters.AddWithValue("@branch", record.Branch);

                                    cmd.ExecuteNonQuery();
                                }
                            }
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw new Exception($"Error in batch {i + 1}: {ex.Message}");
                        }
                    }
                }
            }
        }

        public class PartCostUpload
        {
            public string InternalOrgItemId { get; set; }
            public decimal AvgPrice { get; set; }
            public decimal RecommendedPrice { get; set; }
            public DateTime WeekOf { get; set; }
            public string Branch { get; set; }
            public int UploadedBy { get; set; }
            public string ErrorMessage { get; set; }
        }

        public class FailedRecords
        {
            public string InternalOrgItemId { get; set; }
            public string AvgPrice { get; set; }
            public string RecommendedPrice { get; set; }
            public string WeekOf { get; set; }
            public string Branch { get; set; }
            public int UploadedBy { get; set; }
            public string ErrorMessage { get; set; }
        }

        protected void callbackPanel_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            var failedUploads = Session["FailedUploads"] as List<FailedRecords>;

            if (failedUploads != null && failedUploads.Count > 0)
            {
                gridFailedUploads.DataSource = failedUploads;
                gridFailedUploads.DataBind();
                gridFailedUploads.Visible = true;
            }
            else
            {
                gridFailedUploads.Visible = false;
            }
        }

        protected void gridFailedUploads_PageIndexChanged(object sender, EventArgs e)
        {
            callbackPanel_Callback(callbackPanel, null);
        }

    }
}