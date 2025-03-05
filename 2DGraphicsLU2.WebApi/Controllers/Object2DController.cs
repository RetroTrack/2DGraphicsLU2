using _2DGraphicsLU2.WebApi.Models;
using _2DGraphicsLU2.WebApi.Repositories;
using _2DGraphicsLU2.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace _2DGraphicsLU2.WebApi.Controllers
{

    [ApiController]
    [Route("environments/{environmentId}/objects")]
    public class Object2DController : ControllerBase
    {
        private IAuthenticationService _authenticationService;
        private readonly Object2DRepository _object2DRepository;
        private readonly ILogger<Object2DController> _logger;

        public Object2DController(IAuthenticationService authenticationService, Object2DRepository object2DRepository, ILogger<Object2DController> logger)
        {
            _authenticationService = authenticationService;
            _object2DRepository = object2DRepository;
            _logger = logger;
        }

        [HttpGet(Name = "ReadObjectsInEnvironment")]
        public async Task<ActionResult<IEnumerable<Object2D>>> Get(Guid environmentId)
        {
            var userId = _authenticationService.GetCurrentAuthenticatedUserId();
            if (userId == null)
                return BadRequest();
            var objects2D = await _object2DRepository.ReadAsync(environmentId, userId);
            return Ok(objects2D);
        }

        [HttpGet("{objectId}", Name = "ReadObjectInEnvironment")]
        public async Task<ActionResult<Object2D>> Get(Guid environmentId, Guid objectId)
        {
            var userId = _authenticationService.GetCurrentAuthenticatedUserId();
            if (userId == null)
                return BadRequest();
            var object2D = await _object2DRepository.ReadAsync(environmentId, objectId, userId);
            if (object2D == null)
                return NotFound();

            return Ok(object2D);
        }

        [HttpPost(Name = "CreateObjectInEnvironment")]
        public async Task<ActionResult> Add(Guid environmentId, Object2D object2D)
        {
            var userId = _authenticationService.GetCurrentAuthenticatedUserId();
            if (userId == null)
                return BadRequest();
            object2D.Id = Guid.NewGuid();

            var createdObject2D = await _object2DRepository.InsertAsync(environmentId, object2D, userId);
            if (createdObject2D == null)
                return BadRequest();
            return Created("ReadObjectInEnvironment", object2D);
        }

        [HttpPut("{objectId}", Name = "UpdateObjectInEnvironment")]
        public async Task<ActionResult> Update(Guid environmentId, Guid objectId, Object2D newObject2D)
        {
            var userId = _authenticationService.GetCurrentAuthenticatedUserId();
            if (userId == null)
                return BadRequest();
            var existingObject2D = await _object2DRepository.ReadAsync(environmentId, objectId, userId);

            if (existingObject2D == null)
                return NotFound();

            await _object2DRepository.UpdateAsync(environmentId, newObject2D, userId);

            return Ok(newObject2D);
        }

        [HttpDelete("{objectId}", Name = "DeleteObjectInEnvironment")]
        public async Task<IActionResult> Delete(Guid environmentId, Guid objectId)
        {
            var userId = _authenticationService.GetCurrentAuthenticatedUserId();
            if (userId == null)
                return BadRequest();
            var existingObject2D = await _object2DRepository.ReadAsync(environmentId, objectId, userId);

            if (existingObject2D == null)
                return NotFound();

            await _object2DRepository.DeleteAsync(environmentId, objectId, userId);

            return Ok();
        }
    }
}
