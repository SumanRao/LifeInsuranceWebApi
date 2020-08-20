using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LifeInsurance
{
    [Table("RateChart")]
    public class RateChart
    {

        [Key]
        public long Id { get; set; }
        public string CoveragePlan { get; set; }
        public string CustomerGender { get; set; }
        public string CustomerAge { get; set; }
        public int NetPrice { get; set; }

    }
}