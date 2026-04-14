using MySql.Data.MySqlClient;
using System;
using System.Data;


namespace nesi.core
    {

    public class NSOpportunity
        {
        private const string ExistsStatement = @"SELECT COUNT(*) FROM ns_opportunity WHERE opportunity_internal_id = @v0 LIMIT 1";
        private const string UpdateStatement = @"
UPDATE 
    ns_opportunity
SET 
    subsidiary_internal_id = @v0,
    opportunity_number = @v1,
    customer_internal_id = @v2,
    projected_total = @v3,
    title = @v4
WHERE 
    opportunity_internal_id = @v5";

        private const string InsertStatement = @"
INSERT INTO ns_opportunity
    ( 
    opportunity_internal_id,
    opportunity_number,
    subsidiary_internal_id,
    customer_internal_id,
    projected_total,
    title
    ) 
VALUES 
    (
    @v0,
    @v1,
    @v2,
    @v3,
    @v4,
    @v5
    )";
        private const string LoadStatementByInternalId = @"SELECT * FROM ns_opportunity WHERE opportunity_internal_id = @v0";
        public int id { get; set; }
        public int OpportunityInternalId { get; set; }
        public int OpportunityNumber { get; set; }
        public int SubsidiaryInternalId { get; set; }
        public int CustomerInternalId { get; set; }
        public double ProjectedTotal { get; set; }
        public string Title { get; set; }

        /// <summary>
        /// Provides the Estimate based on the Opportunity internal id
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="opportunityInternalId"></param>
        public NSOpportunity(MySqlConnection conn, int opportunityInternalId)
            {
            OpportunityInternalId = opportunityInternalId;
            Load(conn, opportunityInternalId);
            }

        /// <summary>
        /// Provides the Estimate based on the Opportunity internal id
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="opportunityInternalId"></param>
        private void Load(MySqlConnection conn, int opportunityInternalId)
            {
            if(!Exists(conn, opportunityInternalId)) return;
            var dtOpportunity = Toolbox.doSQL_dt(LoadStatementByInternalId, new object[]{opportunityInternalId});
            if(dtOpportunity.Rows.Count != 1) return;
            var dr = dtOpportunity.Rows[0];
            id = Convert.ToInt32(dr["id"]);
            OpportunityNumber = Convert.ToInt32(dr["opportunity_number"]);
            SubsidiaryInternalId = Convert.ToInt32(dr["subsidiary_internal_id"]);
            CustomerInternalId = Convert.ToInt32(dr["customer_internal_id"]);
            ProjectedTotal = Convert.ToDouble(dr["projected_total"]);
            Title = Toolbox.ReturnBlankIfNull_string(dr["title"]);
            }
        /// <summary>
        /// Validates that the estimate exists based on the provided Opportunity internal ID
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="internalId"></param>
        /// <returns></returns>
        public static bool Exists(MySqlConnection conn, int internalId)
            {
            return Toolbox.doSQL_int(conn, ExistsStatement, new object[] { internalId }) > 0;
            }
        /// <summary>
        /// Saves the Estimate record to the database
        /// </summary>
        /// <param name="_conn"></param>
        public void Save(MySqlConnection _conn)
            {
            if(id > 0)
                { 
                Toolbox.doSQL_void(_conn, UpdateStatement, new object[] {   SubsidiaryInternalId,
                                                                            OpportunityNumber,
                                                                            CustomerInternalId,
                                                                            ProjectedTotal,
                                                                            Title,
                                                                            OpportunityInternalId
                                                                            });
                }
            else
                {
                Toolbox.doSQL_void(_conn, InsertStatement, new object[] {   OpportunityInternalId,
                                                                            OpportunityNumber,
                                                                            SubsidiaryInternalId,
                                                                            CustomerInternalId,
                                                                            ProjectedTotal,
                                                                            Title
                                                                            });
                }
            }
        }

    }