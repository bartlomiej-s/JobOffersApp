using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class JobOfferPagingViewModel
    {
        public IEnumerable<JobOffer> JobOffers { get; set; }
        public int TotalPage { get; set; }
    }
}
