
using System;

namespace LifeInsurance
{
    public class ContractDTO
    {

        public long Id { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerGender { get; set; }
        public string CustomerCountry { get; set; }
        public DateTime CustomerDateofBirth { get; set; }
        public DateTime SaleDate { get; set; }
        public string CoveragePlan { get; set; }
        public int? NetPrice { get; set; }

    }
}