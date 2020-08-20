using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace LifeInsurance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LIController : ControllerBase
    {
        private readonly ContractContext _context;

        public LIController(ContractContext context)
        {
            _context = context;
        }

        #region Helpers
        private bool ContractExists(long id) => _context.Contracts.Any(e => e.Id == id);

        private static ContractDTO ItemToDTO(Contract contract) =>
                                    new ContractDTO
                                    {
                                        Id = contract.Id,
                                        CustomerName = contract.CustomerName.Trim(),
                                        CustomerAddress = contract.CustomerAddress.Trim(),
                                        CustomerGender = contract.CustomerGender.Trim(),
                                        CustomerCountry = contract.CustomerCountry.Trim(),
                                        CustomerDateofBirth = contract.CustomerDateofBirth,
                                        SaleDate = contract.SaleDate,
                                        CoveragePlan = contract.CoveragePlan.Trim(),
                                        NetPrice = contract.NetPrice
                                    };


        internal bool IsContractValid(ContractDTO contractDTO)
        {
            var retVal = false;

            if (contractDTO == null) return retVal;

            if (string.IsNullOrEmpty(contractDTO.CustomerName)
            || string.IsNullOrEmpty(contractDTO.CustomerGender)
            || string.IsNullOrEmpty(contractDTO.CustomerCountry)) return retVal;

            if ((contractDTO.CustomerDateofBirth == DateTime.MinValue)
            || (contractDTO.SaleDate == DateTime.MinValue)) return retVal;

            return true;
        }
        internal async Task<ActionResult<IEnumerable<CoveragePlans>>> GetCoveragePlans()
        {
            return await _context.CoveragePlans.ToListAsync();
        }

        internal async Task<ActionResult<IEnumerable<RateChart>>> GetRateCharts()
        {
            return await _context.RateCharts.ToListAsync();
        }


        internal async Task CalculateCoverageForContract(Contract contract)
        {
            var covPlan = string.Empty;
            var coveragePlanTask = await GetCoveragePlans();

            if (coveragePlanTask.Value != null)
            {
                //Filter coverage by country 
                var coveragePlan = coveragePlanTask.Value.Where(x => x.EligibilityCountry.ToLower().Trim() == contract.CustomerCountry.ToLower().Trim()).FirstOrDefault();

                if (coveragePlan == null)
                {
                    coveragePlan = coveragePlanTask.Value.Where(x => x.EligibilityCountry.ToLower().Trim().Contains("any")).FirstOrDefault();
                }

                //Filter and date later
                if (coveragePlan != null)
                {

                    if (contract.SaleDate >= coveragePlan.EligibilityDateFrom && contract.SaleDate <= coveragePlan.EligibilityDateTo)
                    {
                        covPlan = string.IsNullOrEmpty(coveragePlan.CoveragePlan) ? string.Empty : coveragePlan.CoveragePlan.Trim();
                    }
                }
            }

            int? netPrice = null;
            if (!string.IsNullOrEmpty(covPlan))
            {

                var ratePlanTask = await GetRateCharts();
                if (ratePlanTask.Value != null)
                {
                    //Filter of the Coverage Plan
                    var ratePlans = ratePlanTask.Value.Where(x => x.CoveragePlan.Trim() == covPlan.Trim()).ToList();

                    if (ratePlans.Any())
                    {
                        var customerAge = GetCustomerAge(contract.CustomerDateofBirth);
                        var gender = contract.CustomerGender.Trim();

                        if (!string.IsNullOrEmpty(customerAge) && !string.IsNullOrEmpty(gender))
                        {
                            var ratePlan = ratePlans.Find(x => x.CustomerGender.Trim() == gender && x.CustomerAge.Trim() == customerAge);
                            if (ratePlan != null)
                            {
                                netPrice = ratePlan.NetPrice;
                            }
                        }

                    }
                }
            }

            contract.CoveragePlan = covPlan;
            contract.NetPrice = netPrice;

        }

        internal string GetCustomerAge(DateTime cDoB)
        {

            var retVal = string.Empty;

            int age = 0;
            age = DateTime.Now.Year - cDoB.Year;
            if (DateTime.Now.DayOfYear < cDoB.DayOfYear)
                age = age - 1;


            retVal = (age <= 40) ? "<=40" : ">40";

            return retVal;
        }
        #endregion

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContractDTO>>> GetContracts()
        {

            return await _context.Contracts
                .Select(x => ItemToDTO(x))
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ContractDTO>> GetContract(long id)
        {
            var contractItem = await _context.Contracts.FindAsync(id);

            if (contractItem == null)
            {
                return NotFound();
            }

            return ItemToDTO(contractItem);
        }

        [HttpPost]
        public async Task<ActionResult<ContractDTO>> PostContract([FromBody] ContractDTO contractDTO)
        {
            try
            {

                //Validation
                if (!IsContractValid(contractDTO))
                {
                    return BadRequest("Contract is missing mandatory values");
                }

                var contract = new Contract
                {
                    CustomerName = contractDTO.CustomerName,
                    CustomerAddress = contractDTO.CustomerAddress,
                    CustomerGender = contractDTO.CustomerGender,
                    CustomerCountry = contractDTO.CustomerCountry,
                    CustomerDateofBirth = contractDTO.CustomerDateofBirth,
                    SaleDate = contractDTO.SaleDate,
                    CoveragePlan = null,
                    NetPrice = null
                };

                await CalculateCoverageForContract(contract);


                _context.Contracts.Add(contract);
                await _context.SaveChangesAsync();

                return CreatedAtAction(
                       nameof(GetContract),
                       new { id = contract.Id },
                       ItemToDTO(contract));
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutContract(long id, [FromBody] ContractDTO contractDTO)
        {
            if (id != contractDTO.Id)
            {
                return BadRequest();
            }

            //Validation
            if (!IsContractValid(contractDTO))
            {
                return BadRequest("Contract is missing mandatory values");
            }

            var contract = await _context.Contracts.FindAsync(id);
            if (contract == null)
            {
                return NotFound();
            }


            contract.CustomerName = contractDTO.CustomerName;
            contract.CustomerAddress = contractDTO.CustomerAddress;
            contract.CustomerGender = contractDTO.CustomerGender;
            contract.CustomerCountry = contractDTO.CustomerCountry;
            contract.CustomerDateofBirth = contractDTO.CustomerDateofBirth;
            contract.SaleDate = contractDTO.SaleDate;

            await CalculateCoverageForContract(contract);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!ContractExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContract(long id)
        {
            var contract = await _context.Contracts.FindAsync(id);

            if (contract == null)
            {
                return NotFound();
            }

            _context.Contracts.Remove(contract);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
