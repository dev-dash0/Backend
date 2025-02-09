using AutoMapper;
using DevDash.DTO.Project;
using DevDash.DTO.Tenant;
using DevDash.model;
using DevDash.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DevDash.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectRepository _dbProject;
        private readonly ITenantRepository _dbTenant;
        private readonly IMapper _mapper;
        private APIResponse _response;

        public ProjectController(IProjectRepository dbProject,ITenantRepository dbTenant ,IMapper mapper)
        {
            _dbTenant = dbTenant;
            _dbProject = dbProject;
            _mapper = mapper;
            this._response = new APIResponse();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetProjects()
        {
            try
            {
                IEnumerable<Project> projects = await _dbProject.GetAllAsync(includeProperties: "Tenant");
                _response.Result = _mapper.Map<List<ProjectDTO>>(projects);
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

        [HttpGet("{id:int}", Name = "GetProject")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetProject(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var model = await _dbProject.GetAsync(u => u.Id == id);
                if (model == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<ProjectDTO>(model);
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
        public async Task<ActionResult<APIResponse>> CreateProject([FromBody] ProjectCreateDTO createDTO)
        {
            try
            {

                if (await _dbTenant.GetAsync(u => u.Id == createDTO.TenantId) == null )
                {
                    ModelState.AddModelError("ErrorMessages", "Tenant ID is Invalid!");
                    return BadRequest(ModelState);
                }

                if (createDTO.CreationDate == default)
                {
                    createDTO.CreationDate = DateTime.UtcNow;
                }

                if (createDTO == null)
                {
                    return BadRequest(createDTO);
                }

                Project project = _mapper.Map<Project>(createDTO);

                if (project.CreationDate == default)
                {
                    project.CreationDate = DateTime.UtcNow;
                }

                await _dbProject.CreateAsync(project);
                _response.Result = _mapper.Map<ProjectDTO>(project);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetProject", new { id = project.Id }, _response);
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
        [HttpDelete("{id:int}", Name = "DeleteProject")]
        public async Task<ActionResult<APIResponse>> DeleteProject(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var project = await _dbProject.GetAsync(u => u.Id == id);
                if (project == null)
                {
                    return NotFound();
                }
                await _dbProject.RemoveAsync(project);
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



        [HttpPut("{id:int}", Name = "UpdateProject")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateProject(int id, [FromBody] ProjectUpdateDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.Id)
                {
                    return BadRequest();
                }
                var project = await _dbProject.GetAsync(u => u.Id == updateDTO.Id);
                if (project == null)
                {
                    ModelState.AddModelError("ErrorMessages", "Tenant ID is Invalid!");
                    return BadRequest(ModelState);
                }

                _mapper.Map(updateDTO, project);

                await _dbProject.UpdateAsync(project);
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
