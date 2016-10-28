using Foolproof;
using System;
using System.ComponentModel.DataAnnotations;

namespace BIAdvisor.Web.Models
{
    public class PayeeDetailsViewModel
    {
		public int PayeeKey { get; set; }
		public string Payee { get; set; }
        public string PayeeType { get; set; }
        public string PayeeRole { get; set; }
        public char Published { get; set; }
		public bool boolPublished { get; set; }

        [LessThanOrEqualTo("ExpirationDate")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
		public DateTime EffectiveDate { get; set; }

        [GreaterThanOrEqualTo("EffectiveDate")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
		public DateTime ExpirationDate { get; set; }
        public string PayeeModel { get; set; }
        public string PayIndexKey { get; set; }
        public string BlacksmithCode { get; set; }

        [Required]
        public decimal COCPCT { get; set; }
        public string DeductionModel { get; set; }
        public char MethodBCheck { get; set; }
		public bool boolMethodBCheck { get; set; }
		public string PayeeGroup { get; set; }
        public char Model { get; set; }
		public bool boolModel { get; set; }
		public string PayeeNotes { get; set; }
        public char DoNotPay { get; set; }
		public bool boolDoNotPay { get; set; }

	}
}