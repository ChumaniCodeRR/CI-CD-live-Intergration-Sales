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
    [Route("api/Warehouse")]
    public class WarehouseController : ControllerBase
    {
        private readonly DataContext _context;
        public WarehouseController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("{warehouseid:int}")]
        public async Task<IActionResult> ReturnWarehouseById(int warehouseid)
        {
            Warehouse warehouse = await _context.Warehouse.Where(x => x.WarehouseId == warehouseid)
                .FirstOrDefaultAsync();

            return Ok(warehouse);
        }

        [HttpGet]
        public async Task<IActionResult> ReturnWareHouse()
        {
            List<Warehouse> Items = await _context.Warehouse.ToListAsync();
            int Count = Items.Count();
            return Ok(new { Items, Count });
        }

        [HttpPost("[action]")]
        public IActionResult CreateWarehouse([FromBody] CrudViewModel<Warehouse> payload)
        {
            Warehouse warehouse = payload.value;
            _context.Warehouse.Add(warehouse);
            _context.SaveChanges();

            return Ok(new { message = "Warehouse created successfully" });
        }
        [HttpPut("{warehouseid:int}")]
        public IActionResult ModifyWareHouse([FromBody] CrudViewModel<Warehouse> payload, int warehouseid)
        {
            if (warehouseid == payload.value.WarehouseId)
            {
                Warehouse warehouse = payload.value;
                _context.Warehouse.Update(warehouse);
                _context.SaveChanges();
            }
            return Ok(new { message = "Warehouse modified successfully" });

        }

        [HttpDelete("{warehouseid:int}")]
        public IActionResult RemoveShipmentType([FromBody] CrudViewModel<Warehouse> payload, int warehouseid)
        {
            var warehouse = getWarehouse(warehouseid);

            _context.Warehouse.Remove(warehouse);
            _context.SaveChanges();
            return Ok(new { message = "Warehouse removed successfully" });

        }

        private Warehouse getWarehouse(int id)
        {
            var Warehouse = _context.Warehouse.Find(id);
            if (Warehouse == null) throw new KeyNotFoundException("Warehouse not found");
            return Warehouse;
        }
    }
}
