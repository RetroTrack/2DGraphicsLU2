using _2DGraphicsLU2.WebApi.Models;
using _2DGraphicsLU2.WebApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace _2DGraphicsLU2.WebApi.Controllers
{

    [ApiController]
    [Route("Objects2D")]
    public class Object2DController : ControllerBase
    {
        private readonly Object2DRepository _object2DRepository;
        private readonly ILogger<Object2DController> _logger;

        public Object2DController(Object2DRepository object2DRepository, ILogger<Object2DController> logger)
        {
            _object2DRepository = object2DRepository;
            _logger = logger;
        }

        [HttpGet(Name = "ReadObjects2D")]
        public async Task<ActionResult<IEnumerable<Object2D>>> Get()
        {
            var objects2D = await _object2DRepository.ReadAsync();
            return Ok(objects2D);
        }

        [HttpGet("{objectId}", Name = "ReadObject2D")]
        public async Task<ActionResult<Object2D>> Get(Guid objectId)
        {
            var object2D = await _object2DRepository.ReadAsync(objectId);
            if (object2D == null)
                return NotFound();

            return Ok(object2D);
        }

        [HttpPost(Name = "CreateObject2D")]
        public async Task<ActionResult> Add(Object2D object2D)
        {
            object2D.Id = Guid.NewGuid();

            var createdObject2D = await _object2DRepository.InsertAsync(object2D);
            return Created();
        }

        [HttpPut("{objectId}", Name = "UpdateObject2D")]
        public async Task<ActionResult> Update(Guid objectId, Object2D newObject2D)
        {
            var existingObject2D = await _object2DRepository.ReadAsync(objectId);

            if (existingObject2D == null)
                return NotFound();

            await _object2DRepository.UpdateAsync(newObject2D);

            return Ok(newObject2D);
        }

        [HttpDelete("{objectId}", Name = "DeleteObject2DById")]
        public async Task<IActionResult> Update(Guid objectId)
        {
            var existingObject2D = await _object2DRepository.ReadAsync(objectId);

            if (existingObject2D == null)
                return NotFound();

            await _object2DRepository.DeleteAsync(objectId);

            return Ok();
        }
    }
}
