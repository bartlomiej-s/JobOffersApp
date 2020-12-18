using System.Collections.Generic;

namespace WebApplication1.Models
{
    public class JobOfferCreateView : JobOffer
    {
        public IEnumerable<Company> Companies { get; set; }
        public bool wrongSalaries { get; set; } = false;
        public bool wrongSalaryFrom { get; set; } = false;
        public bool wrongSalaryTo { get; set; } = false;
        public bool wrongValidUntil { get; set; } = false;
    }
}