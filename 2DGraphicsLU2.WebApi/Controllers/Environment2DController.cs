using _2DGraphicsLU2.WebApi.Models;
using _2DGraphicsLU2.WebApi.Repositories;
using _2DGraphicsLU2.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace _2DGraphicsLU2.WebApi.Controllers
{
    [ApiController]
    [Route("environments")]
    public class Environment2DController : ControllerBase
    {
        private IAuthenticationService _authenticationService;
        private readonly Environment2DRepository _environment2DRepository;
        private readonly ILogger<Environment2DController> _logger;

        public Environment2DController(IAuthenticationService authenticationService, Environment2DRepository environment2DRepository, ILogger<Environment2DController> logger)
        {
            _authenticationService = authenticationService;
            _environment2DRepository = environment2DRepository;
            _logger = logger;
        }

        [HttpGet(Name = "ReadEnvironments")]
        public async Task<ActionResult<IEnumerable<Environment2D>>> Get()
        {
            var userId = _authenticationService.GetCurrentAuthenticatedUserId();
            if (userId == null)
                return BadRequest();
            var environments2D = await _environment2DRepository.ReadAsync(userId);
            return Ok(environments2D);
        }

        [HttpGet("{environmentId}", Name = "ReadEnvironment")]
        public async Task<ActionResult<Environment2D>> Get(Guid environmentId)
        {
            var userId = _authenticationService.GetCurrentAuthenticatedUserId();
            if (userId == null)
                return BadRequest();
            var environment2D = await _environment2DRepository.ReadAsync(environmentId, userId);
            if (environment2D == null)
                return NotFound();

            return Ok(environment2D);
        }

        [HttpPost(Name = "CreateEnvironment")]
        public async Task<ActionResult> Add(Environment2D environment2D)
        {
            var userId = _authenticationService.GetCurrentAuthenticatedUserId();
            if (userId == null)
                return BadRequest();
            environment2D.Id = Guid.NewGuid();

            var createdEnvironment2D = await _environment2DRepository.InsertAsync(environment2D, userId);

            if (createdEnvironment2D == null)
                return BadRequest();
            return Created("ReadEnvironment", environment2D);
        }

        [HttpPut("{environmentId}", Name = "UpdateEnvironment")]
        public async Task<ActionResult> Update(Guid environmentId, Environment2D newEnvironment2D)
        {
            var userId = _authenticationService.GetCurrentAuthenticatedUserId();
            if (userId == null)
                return BadRequest();
            var existingEnvironment2D = await _environment2DRepository.ReadAsync(environmentId, userId);

            if (existingEnvironment2D == null)
                return NotFound();
            newEnvironment2D.Id = environmentId;
            await _environment2DRepository.UpdateAsync(newEnvironment2D, userId);

            return Ok(newEnvironment2D);
        }

        [HttpDelete("{environmentId}", Name = "DeleteEnvironmentById")]
        public async Task<IActionResult> Delete(Guid environmentId)
        {
            var userId = _authenticationService.GetCurrentAuthenticatedUserId();
            if (userId == null)
                return BadRequest();
            var existingEnvironment2D = await _environment2DRepository.ReadAsync(environmentId, userId);

            if (existingEnvironment2D == null)
                return NotFound();

            await _environment2DRepository.DeleteAsync(environmentId, userId);

            return Ok();
        }
    }
}
