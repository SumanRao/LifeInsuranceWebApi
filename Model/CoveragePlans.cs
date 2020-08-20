using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LifeInsurance
{
    [Table("CoveragePlan")]
    public class CoveragePlans
    {
        [Key]
        public long Id { get; set; }
        public string CoveragePlan { get; set; }
        public DateTime EligibilityDateFrom { get; set; }
        public DateTime EligibilityDateTo { get; set; }
        public string EligibilityCountry { get; set; }
    }
}