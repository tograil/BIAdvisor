using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace BIAdvisor.Web.Models.CaseMaster
{
    public class SearchCMAViewModel
    {
        public string CaseID { get; set; }
        public string PolicyNo { get; set; }
        public string Agent { get; set; }
        public string WholeSaler { get; set; }
        public string PayToWholeSaler { get; set; }
        public string SortOrder { get; set; }
        public string SortColumn { get; set; }
        public DateTime ArFromDate { get; set; }
        public DateTime ArToDate { get; set; }
        public DataTable Results { get; set; }
    }
}