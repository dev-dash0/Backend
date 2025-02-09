using AutoMapper;
using DevDash.DTO.Sprint;
using DevDash.DTO.Tenant;
using DevDash.Migrations;
using DevDash.model;
using DevDash.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DevDash.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SprintController : Controller
    {
        private readonly ISprintRepository _dbSprint;
        private readonly IMapper _mapper;
        private APIResponse _response;

        public SprintController(ISprintRepository tenantRepo, IMapper mapper)
        {
            _dbSprint = tenantRepo;
            _mapper = mapper;
            this._response = new APIResponse();
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetSprints()
        {
            try
            {
                IEnumerable<Sprint> Sprints = await _dbSprint.GetAllAsync();
                _response.Result = _mapper.Map<List<SprintDTO>>(Sprints);
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

        [HttpGet("{id:int}", Name = "GetSprint")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetSprint(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var Sprint = await _dbSprint.GetAsync(u => u.Id == id);
                if (Sprint == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<SprintDTO>(Sprint);
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
        public async Task<ActionResult<APIResponse>> CreateSprint([FromBody] SprintCreateDTO createDTO)
        {
            try
            {

                if (createDTO == null)
                {
                    return BadRequest(createDTO);
                }
                if (createDTO.CreatedAt==default)
                {
                    createDTO.CreatedAt = DateTime.UtcNow;
                }

                Sprint Sprint = _mapper.Map<Sprint>(createDTO);


                await _dbSprint.CreateAsync(Sprint);
                _response.Result = _mapper.Map<SprintDTO>(Sprint);
                _response.StatusCode = HttpStatusCode.Created;
                if (Sprint.CreatedAt == default)
                {
                    createDTO.CreatedAt = DateTime.UtcNow;
                }
               
                return CreatedAtRoute("GetSprint", new { id = Sprint.Id }, _response);
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
        [HttpDelete("{id:int}", Name = "DeleteSprint")]
        public async Task<ActionResult<APIResponse>> DeleteSprint(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var tenant = await _dbSprint.GetAsync(u => u.Id == id);
                if (tenant == null)
                {
                    return NotFound();
                }
                await _dbSprint.RemoveAsync(tenant);
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



        [HttpPut("{id:int}", Name = "UpdateSprint")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateSprint(int id, [FromBody] SprintUpdateDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.Id)
                {
                    return BadRequest();
                }

               
               
                var tenant = await _dbSprint.GetAsync(u => u.Id == updateDTO.Id);
                if (tenant == null)
                {
                    ModelState.AddModelError("ErrorMessages", "Sprint ID is Invalid!");
                    return BadRequest(ModelState);
                }

                _mapper.Map(updateDTO, tenant);

                await _dbSprint.UpdateAsync(tenant);
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
