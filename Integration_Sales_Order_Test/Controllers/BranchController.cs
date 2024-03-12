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
    [Route("api/Branch")]
    public class BranchController : ControllerBase
    {
        private readonly DataContext _context;
        public BranchController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("{branchId:int}")]
        public async Task<IActionResult> GetBranchById(int branchId)
        {
            Branch branch = await _context.Branch.Where(x => x.BranchId == branchId)
                .FirstOrDefaultAsync();

            return Ok(branch);
        }

        [HttpGet]
        public async Task<IActionResult> GetBranch()
        {
            List<Branch> Items = await _context.Branch.ToListAsync();
            int Count = Items.Count();
            return Ok(new { Items, Count });
        }

        [HttpPost("[action]")]
        public IActionResult InsertBranch([FromBody] CrudViewModel<Branch> payload)
        {
            Branch branch = payload.value;
            _context.Branch.Add(branch);
            _context.SaveChanges();

            return Ok(new { message = "Branch created successfully" });
          
        }

        [HttpPut("{branchId:int}")]
        public IActionResult UpdateBranch([FromBody] CrudViewModel<Branch> payload)
        {
            Branch branch = payload.value;
            _context.Branch.Update(branch);
            _context.SaveChanges();

            return Ok(new { message = "Branch modified successfully" });
            
        }

        [HttpDelete("{branchId:int}")]
        public IActionResult RemoveBranch([FromBody] CrudViewModel<Branch> payload, int branchId)
        {
            var branch = getBranch(branchId);

            _context.Branch.Remove(branch);
            _context.SaveChanges();
            return Ok(new { message = "Branch removed successfully" });
        }

        private Branch getBranch(int id)
        {
            var Branch = _context.Branch.Find(id);
            if (Branch == null) throw new KeyNotFoundException("Branch not found");
            return Branch;
        }
    }
}
