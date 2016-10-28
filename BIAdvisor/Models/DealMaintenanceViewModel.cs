using System;
using System.ComponentModel.DataAnnotations;

namespace BIAdvisor.Web.Models
{
    public class DealMaintenanceViewModel
    {
        public int PayeeDealKey { get; set; }
        public int PayeeKey { get; set; }
        public string PayeeType { get; set; }


        //Criteria Section Properties

        [Required, Display(Name = "Criteria:")]
        public string DealCriteria { get; set; }

        [Display(Name = "Carrier:")]
        public int? CarrierID { get; set; }

        [Display(Name = "Carrier:")]
        public string Carrier { get; set; }

        [Display(Name = "Carrier Attribute:")]
        public int? CarrierAttribute { get; set; }

        public string CarrierAttributeDescription { get; set; }

        public short? ProductLine { get; set; }

        [Display(Name = "Product:")]
        public int? ProductID { get; set; }

        [Display(Name = "Product:")]
        public string Product { get; set; }

        [Display(Name = "Product Attribute:")]
        public int? ProductAttribute { get; set; }

        public string ProductAttributeDescription { get; set; }

        [Display(Name = "Wholesaler:")]
        public string Wholesaler { get; set; }

        [Display(Name = "Role:")]
        public string WholesalerRole { get; set; }

        [Display(Name = "BD:")]
        public string BrokerDealer { get; set; }

        [Display(Name = "BD Attribute:")]
        public int? BrokerDealerAttribute { get; set; }

        public string BDAttributeDescription { get; set; }

        [Display(Name = "BD In Hierarchy:")]
        public string BrokerDealerInHierarchy { get; set; }

        [Display(Name = "Channel:")]
        public string Channel { get; set; }

        [Display(Name = "Internal:")]
        public string Internal { get; set; }

        [Display(Name = "Producer:")]
        public int? ProducerID { get; set; }

        public string Producer { get; set; }

        public string MSA { get; set; }

        [Display(Name = "Recruiter:")]
        public string Recruiter { get; set; }

        [Display(Name = "Recruiter Attribute:")]
        public int? RecruiterAttribute { get; set; }

        public string RecruiterAttributeDescription { get; set; }

        [Display(Name = "MP Partner:")]
        public string MPPartner { get; set; }

        [Display(Name = "Payee Group:")]
        public string PayeeGroup { get; set; }

        [Display(Name = "Statement Date: "), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? StatementDate { get; set; }

        [Display(Name = "Submit Date: "), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? SubmitDate { get; set; }

        [Display(Name = "Notes: ")]
        public string Notes { get; set; }


        //Calculation Code Section Properties

        [Display(Name = "Calculation Code:")]
        public string CalculationID { get; set; }

        [Display(Name = "Calculation Code:")]
        public string CalculationCode { get; set; }

        [Display(Name = "Carrier Level:")]
        public string CarrierLevel { get; set; }

        [Display(Name = "Virtual Carrier Level:")]
        public string VirtualCarrierLevel { get; set; }

        [Display(Name = "Pay Index:")]
        public string PayIndexKey { get; set; }

        [Display(Name = "Deal Cap:")]
        public decimal? DealCap { get; set; }

        [Display(Name = "LOA:")]
        public bool LOA { get; set; }

        [Display(Name = "Street:")]
        public bool Street { get; set; }

        [Display(Name = "Deduction:")]
        public string DeductionModel { get; set; }

        [Display(Name = "Directive:")]
        public string Directive { get; set; }

        [Display(Name = "Exp Delta:")]
        public decimal? ExpDelta { get; set; }

        [Display(Name = "Pct Gross:")]
        public decimal? PctGross { get; set; }

        [Display(Name = "Pct Premium:")]
        public decimal? PctPremium { get; set; }

        [Display(Name = "Pct Override:")]
        public decimal? PctOverride { get; set; }

        [Display(Name = "Pct Carrier Level:")]
        public decimal? PctCarrierLevel { get; set; }

        [Display(Name = "Pct Wholesaler Net:")]
        public decimal? PctWholesalerNet { get; set; }

        public decimal? RecruiterDebit { get; set; }

        public decimal? RecruiterCredit { get; set; }

        public decimal? ContingentBonus { get; set; }

        public decimal? YearEndBonus { get; set; }

        public decimal? Amount1 { get; set; }

        public decimal? Amount2 { get; set; }

        public decimal? Amount3 { get; set; }

        public decimal? Rate1 { get; set; }

        public decimal? Rate2 { get; set; }

        public decimal? Rate3 { get; set; }
    }
}
