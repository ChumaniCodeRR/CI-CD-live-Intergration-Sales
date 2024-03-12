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
    [Route("api/Shipment")]
    public class ShipmentController : ControllerBase
    {

        private readonly DataContext _context;
        public ShipmentController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("{ShipmentId:int}")]
        public async Task<IActionResult> GetShipmentById(int ShipmentId)
        {
            Shipment shipment = await _context.Shipment.Where(x => x.ShipmentId == ShipmentId)
                .FirstOrDefaultAsync();

            return Ok(shipment);
        }

        [HttpGet]
        public async Task<IActionResult> GetShipment()
        {
            List<Shipment> Items = await _context.Shipment.ToListAsync();
            int Count = Items.Count();
            return Ok(new { Items, Count });
        }

        [HttpPost("[action]")]
        public IActionResult InsertShipment([FromBody] CrudViewModel<Shipment> payload)
        {
            Shipment shipment = payload.value;
            _context.Shipment.Add(shipment);
            _context.SaveChanges();

            return Ok(new { message = "Shipment created successfully" });

        }

        [HttpPut("{ShipmentId:int}")]
        public IActionResult UpdateShipment([FromBody] CrudViewModel<Shipment> payload, int ShipmentId)
        {
            if (ShipmentId == payload.value.ShipmentId)
            {
                Shipment shipment = payload.value;
                _context.Shipment.Update(shipment);
                _context.SaveChanges();
            }
            return Ok(new { message = "Shipment modified successfully" });

        }

        [HttpDelete("{ShipmentId:int}")]
        public IActionResult RemoveShipment([FromBody] CrudViewModel<Shipment> payload, int ShipmentId)
        {
            var shipment = getShipment(ShipmentId);

            _context.Shipment.Remove(shipment);
            _context.SaveChanges();

            return Ok(new { message = "Shipment removed successfully" });

        }

        private Shipment getShipment(int id)
        {
            var shipment = _context.Shipment.Find(id);
            if (shipment == null) throw new KeyNotFoundException("Shipment not found");
            return shipment;
        }

    }
}
