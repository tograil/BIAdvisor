using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIAdvisor.Web.Models
{
    public class SearchViewModel
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Role { get; set; }
        public string Timeframe { get; set; }
        public bool? boolModel { get; set; }

        public DataTable Payees { get; set; }
        public List<TypeRoleResult> TypesSelectListItems { get; set; }
    }
}
