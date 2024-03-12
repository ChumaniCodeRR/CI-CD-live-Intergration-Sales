using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Integration_Sales_Order_Test.Model;
using Integration_Sales_Order_Test.Model.CRUDModels;
using Integration_Sales_Order_Test.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Integration_Sales_Order_Test.Controllers
{
    [Produces("application/json")]
    [Route("api/ShipmentType")]
    public class ShipmentTypeController : ControllerBase
    {
        private readonly DataContext _context;
        public ShipmentTypeController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("{ShipmentTypeId:int}")]
        public async Task<IActionResult> ReturnShipmentTypeById(int ShipmentTypeId)
        {
            ShipmentType ShipmentType = await _context.ShipmentType.Where(x => x.ShipmentTypeId == ShipmentTypeId)
                .FirstOrDefaultAsync();

            return Ok(ShipmentType);
        }

        [HttpGet]
        public async Task<IActionResult> ReturnShipmentType()
        {
            List<ShipmentType> Items = await _context.ShipmentType.ToListAsync();
            int Count = Items.Count();
            return Ok(new { Items, Count });
        }

        [HttpPost("[action]")]
        public IActionResult CreateShipmentType([FromBody] CrudViewModel<ShipmentType> payload)
        {
            ShipmentType ShipmentType = payload.value;
            _context.ShipmentType.Add(ShipmentType);
            _context.SaveChanges();

            return Ok(new { message = "ShipmentType created successfully" });
            //return Ok(branch);
        }

        [HttpPut("{shipmentTypeId:int}")]
        public IActionResult ModifyShipmentType([FromBody] CrudViewModel<ShipmentType> payload, int shipmentTypeId)
        {
            if (shipmentTypeId == payload.value.ShipmentTypeId)
            {
                ShipmentType ShipmentType = payload.value;
                _context.ShipmentType.Update(ShipmentType);
                _context.SaveChanges();
            }
            return Ok(new { message = "ShipmentType modified successfully" });
            
        }

        [HttpDelete("{shipmentTypeId:int}")]
        public IActionResult RemoveShipmentType([FromBody] CrudViewModel<ShipmentType> payload, int shipmentTypeId)
        {
            var shipmentType = getShipmentType(shipmentTypeId);

            _context.ShipmentType.Remove(shipmentType);
            _context.SaveChanges();
            return Ok(new { message = "ShipmentType removed successfully" });
           

        }

        private ShipmentType getShipmentType(int id)
        {
            var ShipmentType = _context.ShipmentType.Find(id);
            if (ShipmentType == null) throw new KeyNotFoundException("ShipmentType not found");
            return ShipmentType;
        }
    }
}
