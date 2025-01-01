using DevDash.model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DevDash.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TenantController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TenantController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Tenant
        [HttpGet("GetTenants")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public IActionResult GetTenants()
        {
            List<Tenant> tenants = _context.Tenants.ToList();
            return Ok(tenants);
        }

        // GET: api/Tenant/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Tenant>> GetTenant(int id)
        //{
        //    var tenant = await _context.Tenants.FindAsync(id);

        //    if (tenant == null)
        //    {
        //        return NotFound();
        //    }

        //    return tenant;
        //}

        // POST: api/Tenant
        //[HttpPost]
        //public async Task<ActionResult<Tenant>> PostTenant(Tenant tenant)
        //{
        //    _context.Tenants.Add(tenant);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction(nameof(GetTenant), new { id = tenant.Id }, tenant);
        //}

        //// PUT: api/Tenant/5
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutTenant(int id, Tenant tenant)
        //{
        //    if (id != tenant.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(tenant).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!TenantExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// DELETE: api/Tenant/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteTenant(int id)
        //{
        //    var tenant = await _context.Tenants.FindAsync(id);
        //    if (tenant == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Tenants.Remove(tenant);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool TenantExists(int id)
        //{
        //    return _context.Tenants.Any(e => e.Id == id);
        //}
    }
}

