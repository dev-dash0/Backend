using AutoMapper;
using DevDash.DTO.Tenant;
using DevDash.model;
using DevDash.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace DevDash.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TenantController : ControllerBase
    {
        private readonly ITenantRepository _dbTenant;
        private readonly IMapper _mapper;
        private APIResponse _response;

        public TenantController(ITenantRepository tenantRepo,IMapper mapper)
        {
            _dbTenant = tenantRepo;
            _mapper = mapper;   
            this._response = new APIResponse();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetTenants()
        {
            try
            {
                IEnumerable<Tenant> tenants = await _dbTenant.GetAllAsync(includeProperties: "Owner");
                _response.Result = _mapper.Map<List<TenantDTO>>(tenants);
                _response.StatusCode = HttpStatusCode.OK;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;

        }

        [HttpGet("{id:int}", Name = "GetTenant")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetTenant(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var tenant = await _dbTenant.GetAsync(u => u.Id == id);
                if (tenant == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<TenantDTO>(tenant);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateTenant([FromBody] TenantCreateDTO createDTO)
        {
            try
            {

                //if (await _dbVilla.GetAsync(u => u.Id == createDTO.VillaID) == null)
                //{
                //    ModelState.AddModelError("ErrorMessages", "Villa ID is Invalid!");
                //    return BadRequest(ModelState);
                //}
                

                if (createDTO == null)
                {
                    return BadRequest(createDTO);
                }

                Tenant tenant = _mapper.Map<Tenant>(createDTO);


                await _dbTenant.CreateAsync(tenant);
                _response.Result = _mapper.Map<TenantDTO>(tenant);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetTenant", new { id = tenant.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("{id:int}", Name = "DeleteTenant")]
        public async Task<ActionResult<APIResponse>> DeleteTenant(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var tenant = await _dbTenant.GetAsync(u => u.Id == id);
                if (tenant == null)
                {
                    return NotFound();
                }
                await _dbTenant.RemoveAsync(tenant);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }



        [HttpPut("{id:int}", Name = "UpdateTenant")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateTenant(int id, [FromBody] TenantUpdateDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id!=updateDTO.Id )
                {
                    return BadRequest();
                }
                var tenant = await _dbTenant.GetAsync(u => u.Id == updateDTO.Id);
                if (tenant == null)
                {
                    ModelState.AddModelError("ErrorMessages", "Tenant ID is Invalid!");
                    return BadRequest(ModelState);
                }

                _mapper.Map(updateDTO, tenant);

                await _dbTenant.UpdateAsync(tenant);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }


    }
}

