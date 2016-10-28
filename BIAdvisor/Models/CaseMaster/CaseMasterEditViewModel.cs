using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BIAdvisor.Web.Models.CaseMaster
{
    public class CaseMasterEditViewModel
    {
        public string AGPCOMID { get; set; }
        public long CaseID { get; set; }
        public string PolNo { get; set; }
        public DateTime CrtDate { get; set; }
        public long AgtNo { get; set; }
        public string AgtName { get; set; }
        public decimal AgtPct { get; set; }
        public long ComLvl { get; set; }
        public string Wholesaler { get; set; }
        public string PayToWholesaler { get; set; }
        public string Internal { get; set; }
        public string  Recruiter { get; set; }
        public string Channel { get; set; }
        public string PlChannel { get; set; }
        public string NSM { get; set; }
        public string BrokeDealer { get; set; }
        public string ThruBD { get; set; }
        public string Agency { get; set; }
        public string AgencyMP { get; set; }
        public DateTime ArchiveDateTime { get; set; }
        public string ArchiveReason { get; set; }
    }
}