using System;
using System.ComponentModel.DataAnnotations;

namespace BIAdvisor.Web.Models
{
    public class PayeeAddViewModel
    {
        public PayeeAddViewModel()
        {
            EffectiveDate = DateTime.Today;
            ExpirationDate = new DateTime(2099, 12, 31);
        }

        [Required(ErrorMessage = "*")]
        public string PayeeName { get; set; }

        [Required(ErrorMessage = "*")]
        public string PayeeType { get; set; }

        public string PayeeRole { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [Required(ErrorMessage = "*")]
        public DateTime EffectiveDate { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [Required(ErrorMessage ="*")]
        public DateTime ExpirationDate { get; set; }
    }
}
