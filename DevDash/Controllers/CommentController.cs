using AutoMapper;
using DevDash.DTO.Comment;
using DevDash.DTO.Sprint;
using DevDash.Migrations;
using DevDash.model;
using DevDash.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DevDash.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : Controller
    {
        private readonly ICommentRepository _dbComment;
        private readonly IMapper _mapper;
        private APIResponse _response;

        public CommentController(ICommentRepository commentRepo, IMapper mapper)
        {
            _dbComment = commentRepo;
            _mapper = mapper;
            this._response = new APIResponse();
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetComments()
        {
            try
            {
                IEnumerable<Comment> Comments = await _dbComment.GetAllAsync();
                _response.Result = _mapper.Map<List<CommentDTO>>(Comments);
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


        [HttpGet("{id:int}", Name = "GetComment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetComment(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var Comment = await _dbComment.GetAsync(u => u.Id == id);
                if (Comment == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<CommentDTO>(Comment);
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
        public async Task<ActionResult<APIResponse>> CreateComment([FromBody] CommentCreateDTO createDTO)
        {
            try
            {

                if (createDTO == null)
                {
                    return BadRequest(createDTO);
                }
                if (createDTO.CreationDate == default)
                {
                    createDTO.CreationDate = DateTime.UtcNow;
                }

                Comment comment = _mapper.Map<Comment>(createDTO);


                await _dbComment.CreateAsync(comment);
                _response.Result = _mapper.Map<CommentDTO>(comment);
                _response.StatusCode = HttpStatusCode.Created;
                if (comment.CreationDate == default)
                {
                    createDTO.CreationDate = DateTime.UtcNow;
                }

                return CreatedAtRoute("GetComment", new { id = comment.Id }, _response);
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
        [HttpDelete("{id:int}", Name = "DeleteComment")]
        public async Task<ActionResult<APIResponse>> DeleteComment(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var comment = await _dbComment.GetAsync(u => u.Id == id);
                if (comment == null)
                {
                    return NotFound();
                }

                await _dbComment.RemoveAsync(comment);
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

        [HttpPut("{id:int}", Name = "UpdateComment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateComment(int id, [FromBody] CommentUpdateDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.Id)
                {
                    return BadRequest();
                }



                var comment = await _dbComment.GetAsync(u => u.Id == updateDTO.Id);
                if (comment == null)
                {
                    ModelState.AddModelError("ErrorMessages", "Comment ID is Invalid!");
                    return BadRequest(ModelState);
                }

                _mapper.Map(updateDTO, comment);

                await _dbComment.UpdateAsync(comment);
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
