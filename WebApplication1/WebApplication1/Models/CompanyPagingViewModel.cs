using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class CompanyPagingViewModel
    {
        public IEnumerable<Company> Companies { get; set; }
        public int TotalPage { get; set; }
    }
}
