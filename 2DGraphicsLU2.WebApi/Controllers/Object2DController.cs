using _2DGraphicsLU2.WebApi.Models;
using _2DGraphicsLU2.WebApi.Repositories;
using _2DGraphicsLU2.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace _2DGraphicsLU2.WebApi.Controllers
{

    [ApiController]
    [Route("Objects2D")]
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

        [HttpGet(Name = "ReadObjects2D")]
        public async Task<ActionResult<IEnumerable<Object2D>>> Get()
        {
            var userId = _authenticationService.GetCurrentAuthenticatedUserId();
            var objects2D = await _object2DRepository.ReadAsync(userId);
            return Ok(objects2D);
        }

        [HttpGet("{objectId}", Name = "ReadObject2D")]
        public async Task<ActionResult<Object2D>> Get(Guid objectId, string userId)
        {
            var object2D = await _object2DRepository.ReadAsync(objectId, userId);
            if (object2D == null)
                return NotFound();

            return Ok(object2D);
        }

        [HttpPost(Name = "CreateObject2D")]
        public async Task<ActionResult> Add(Object2D object2D, string userId)
        {
            object2D.Id = Guid.NewGuid();

            var createdObject2D = await _object2DRepository.InsertAsync(object2D, userId);
            return Created();
        }

        [HttpPut("{objectId}", Name = "UpdateObject2D")]
        public async Task<ActionResult> Update(Guid objectId, Object2D newObject2D, string userId)
        {
            var existingObject2D = await _object2DRepository.ReadAsync(objectId, userId);

            if (existingObject2D == null)
                return NotFound();

            await _object2DRepository.UpdateAsync(newObject2D, userId);

            return Ok(newObject2D);
        }

        [HttpDelete("{objectId}", Name = "DeleteObject2DById")]
        public async Task<IActionResult> Update(Guid objectId, string userId)
        {
            var existingObject2D = await _object2DRepository.ReadAsync(objectId, userId);

            if (existingObject2D == null)
                return NotFound();

            await _object2DRepository.DeleteAsync(objectId, userId);

            return Ok();
        }
    }
}
