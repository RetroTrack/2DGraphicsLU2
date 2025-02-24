using _2DGraphicsLU2.WebApi.Models;
using _2DGraphicsLU2.WebApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace _2DGraphicsLU2.WebApi.Controllers
{
    [ApiController]
    [Route("Environment2D")]
    public class Environment2DController : ControllerBase
    {
        private readonly Environment2DRepository _environment2DRepository;
        private readonly ILogger<Environment2DController> _logger;

        public Environment2DController(Environment2DRepository environment2DRepository, ILogger<Environment2DController> logger)
        {
            _environment2DRepository = environment2DRepository;
            _logger = logger;
        }

        [HttpGet(Name = "ReadEnvironments2D")]
        public async Task<ActionResult<IEnumerable<Environment2D>>> Get()
        {
            var environments2D = await _environment2DRepository.ReadAsync();
            return Ok(environments2D);
        }

        [HttpGet("{environmentId}", Name = "ReadEnvironment2D")]
        public async Task<ActionResult<Environment2D>> Get(Guid environmentId)
        {
            var environment2D = await _environment2DRepository.ReadAsync(environmentId);
            if (environment2D == null)
                return NotFound();

            return Ok(environment2D);
        }

        [HttpPost(Name = "CreateEnvironment2D")]
        public async Task<ActionResult> Add(Environment2D environment2D)
        {
            environment2D.Id = Guid.NewGuid();

            var createdEnvironment2D = await _environment2DRepository.InsertAsync(environment2D);
            return Created();
        }

        [HttpPut("{environmentId}", Name = "UpdateEnvironment2D")]
        public async Task<ActionResult> Update(Guid environmentId, Environment2D newEnvironment2D)
        {
            var existingEnvironment2D = await _environment2DRepository.ReadAsync(environmentId);

            if (existingEnvironment2D == null)
                return NotFound();

            await _environment2DRepository.UpdateAsync(newEnvironment2D);

            return Ok(newEnvironment2D);
        }

        [HttpDelete("{environmentId}", Name = "DeleteEnvironment2DById")]
        public async Task<IActionResult> Update(Guid environmentId)
        {
            var existingEnvironment2D = await _environment2DRepository.ReadAsync(environmentId);

            if (existingEnvironment2D == null)
                return NotFound();

            await _environment2DRepository.DeleteAsync(environmentId);

            return Ok();
        }
    }
}
