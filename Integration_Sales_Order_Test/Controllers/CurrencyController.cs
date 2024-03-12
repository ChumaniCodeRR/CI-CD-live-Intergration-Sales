using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Integration_Sales_Order_Test.Model;
using Integration_Sales_Order_Test.Model.CRUDModels;
using Integration_Sales_Order_Test.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Integration_Sales_Order_Test.Controllers
{
    [Produces("application/json")]
    [Route("api/Currency")]
    public class CurrencyController : ControllerBase
    {
        private readonly DataContext _context;
        public CurrencyController(DataContext context)
        {
            _context = context;
        }


        [HttpGet("{CurrencyId:int}")]
        public async Task<IActionResult> ReturnCurrencyById(int CurrencyId)
        {
            Currency currency = await _context.Currency.Where(x => x.CurrencyId == CurrencyId)
                .FirstOrDefaultAsync();

            return Ok(currency);
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrency()
        {
            List<Currency> Items = await _context.Currency.ToListAsync();
            int Count = Items.Count();
            return Ok(new { Items, Count });
        }


        [HttpPost("[action]")]
        public IActionResult InsertCurrency([FromBody] CrudViewModel<Currency> payload)
        {
            Currency currency = payload.value;
            _context.Currency.Add(currency);
            _context.SaveChanges();

            return Ok(new { message = "Currency created successfully" });
           
        }

        [HttpPut("{CurrencyId:int}")]
        public IActionResult UpdateCurrency([FromBody] CrudViewModel<Currency> payload, int CurrencyId)
        {
            if (CurrencyId == payload.value.CurrencyId)
            {
                Currency currency = payload.value;
                _context.Currency.Update(currency);
                _context.SaveChanges();
            }

            return Ok(new { message = "Currency modified successfully" });

        }

        [HttpDelete("{CurrencyId:int}")]
        public IActionResult RemoveCurrency([FromBody] CrudViewModel<Currency> payload, int CurrencyId)
        {
            var currency = getCurrency(CurrencyId);

            _context.Currency.Remove(currency);
            _context.SaveChanges();
            return Ok(new { message = "Currency removed successfully" });

        }

        private Currency getCurrency(int id)
        {
            var account = _context.Currency.Find(id);
            if (account == null) throw new KeyNotFoundException("Currency not found");
            return account;
        }
    }
}
