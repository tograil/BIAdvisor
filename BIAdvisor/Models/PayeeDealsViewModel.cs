using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIAdvisor.Web.Models
{
    public class PayeeDealsViewModel
    {
        public UserRole userRole { get; set; }
        public DataTable dtDeals { get; set; }
        public bool boolPublished { get; set; }
    }
}
