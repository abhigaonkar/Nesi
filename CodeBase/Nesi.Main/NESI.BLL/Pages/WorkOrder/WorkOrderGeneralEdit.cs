using System;
using System.IO;
using nesi.core;
using NESI.BLL.Core.Employee;
using NESI.DTO.ViewModels.Core;
using NESI.DTO.ViewModels.Page.WorkOrder;

namespace NESI.BLL.Pages.WorkOrder
{
    public class WorkOrderGeneralEdit : WorkOrderEdit
    {

        //protected int woprog_id { get; set; }

        //protected NeWOProg wo { get; set; }

        //public bool can_access { get; set; }

        //protected NeBusinessUnit wo_bu { get; set; }

        //public LabelValueInt[] ActingRAMList { get; set; }

        //public LabelValueInt[] AddressList { get; set; }

        //public LabelValueInt[] ContactList { get; set; }

        //public LabelValueInt[] ProjectManagerList { get; set; }

        public WorkOrderGeneralEdit(Employee user, int buid, string woid) : base(user, buid, woid)
        {

        }



        public object Profile()
        {
            var workordergeneral_entity = new DTO.ViewModels.Page.WorkOrder.WorkOrderGeneral();
            workordergeneral_entity.Business_unit_id = wo.business_unit_id;
            workordergeneral_entity.Business_unit= BLL.Common.Cache.Global.BusinessUnit.GetValue(business_unit_id).Name;
            workordergeneral_entity.Is_ServiceCall = wo.chkServiceCall;
            workordergeneral_entity.Is_RDFlag = wo.chkRD;
            workordergeneral_entity.Is_DiscountApplied = wo.woprog_apply_discount == 0 ? false : true;
            workordergeneral_entity.Is_VisibletoCustomer = wo.woprog_vis_to_cust == 0 ? false : true;
            workordergeneral_entity.Is_LaborOnly = wo.labor_only;
            workordergeneral_entity.Is_MaterialOnly = wo.mat_only;
            workordergeneral_entity.Is_Warranty = wo.woprog_warranty == 0 ? false : true;


            workordergeneral_entity.Customer_id = wo.WOProg_Customer_ID;


            workordergeneral_entity.DaysCredit = wo.woprog_dayscredit;
            workordergeneral_entity.DefaultInvoiceType_id = wo.default_invoicetype;
            workordergeneral_entity.Is_AutoInvoice = wo.woprog_auto_invoice == 0 ? false : true;
            workordergeneral_entity.Currency_Id = wo.currency_id;
            workordergeneral_entity.Is_RequiresInspection = wo.inspection_required;
            workordergeneral_entity.InspectionLink = wo.inspection_link;
            workordergeneral_entity.Is_SustainabilityProject = wo.sustainability_project;
            workordergeneral_entity.Jobtag_Id = wo.woprog_jobtag_id;
            workordergeneral_entity.Description = wo.Description;
            workordergeneral_entity.SpecialInstructions = wo.special_instructions;

            workordergeneral_entity.Is_OnHold = wo.woprog_hold == 0 ? false : true;
            workordergeneral_entity.whyhold = wo.woprog_whyhold;
            workordergeneral_entity.CustomerPO = wo.PONumber;
            workordergeneral_entity.AreaInPlant = wo.woprog_Location_in_plant;
            workordergeneral_entity.StartDate = wo.woprog_Expected_StartDate;
            workordergeneral_entity.ExpEndDate = wo.woprog_Expected_EndDate;
            workordergeneral_entity.ExpSalesValue = wo.woprog_expected_sales_value;
            workordergeneral_entity.ExpHoursNeeded = wo.woprog_exp_labor;

            workordergeneral_entity.Is_ProcessBill = wo.woprog_ProgressBilled == 0.0 ? false : true;



            return new
            {
                workordergeneral_entity,
                ActingRAMList,
                Workordercustomers,
                AddressList,
                ContactList,
                ProjectManagerList,
                QuoteList,
                DefaultInvoiceTypeList,
                WorkOrderTagList,
                CurrencyList

            };
        }


        public string SaveGeneral(WorkOrderGeneral model)
        {
            //var a = model.address_id <= 0 ? new NEAddress() : new NEAddress(model.address_id);
            //a.Tax1 = model.tax_1;
            //a.Tax2 = model.tax_2;
            //a.Tax3 = model.tax_3;
            //a.Tax4 = model.tax_4;
            //a.Tax1Exempt = string.IsNullOrEmpty(model.taxex_1) ? "" : model.taxex_1;
            //a.Tax2Exempt = string.IsNullOrEmpty(model.taxex_2) ? "" : model.taxex_2;
            //a.Tax3Exempt = string.IsNullOrEmpty(model.taxex_3) ? "" : model.taxex_3;
            //a.Tax4Exempt = string.IsNullOrEmpty(model.taxex_4) ? "" : model.taxex_4;

            var a=model.Address_id<=0? new NEAddress() : new NEAddress(model.Address_id);


            a.Save();
            try
            {
                NEAddress.sync_bvs(a, new NeMember(UserId));
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }
            return "Address has been saved Successfully.";
        }

    }
}
