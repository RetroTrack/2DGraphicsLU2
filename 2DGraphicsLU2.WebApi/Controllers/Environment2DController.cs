using _2DGraphicsLU2.WebApi.Models;
using _2DGraphicsLU2.WebApi.Repositories;
using _2DGraphicsLU2.WebApi.Repositories.Interfaces;
using _2DGraphicsLU2.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace _2DGraphicsLU2.WebApi.Controllers
{
    [ApiController]
    [Route("environments")]
    public class Environment2DController : ControllerBase
    {
        private IAuthenticationService _authenticationService;
        private readonly IEnvironment2DRepository _environment2DRepository;

        public Environment2DController(IAuthenticationService authenticationService, IEnvironment2DRepository environment2DRepository)
        {
            _authenticationService = authenticationService;
            _environment2DRepository = environment2DRepository;
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

            IEnumerable<Environment2D> existingEnvironments = await _environment2DRepository.ReadAsync(userId);

            if (string.IsNullOrWhiteSpace(environment2D.Name))
                environment2D.Name = "New World";

            if (existingEnvironments.Count() >= 5 || 
                existingEnvironments.Any(environment => environment.Name.Equals(environment2D.Name)) || 
                environment2D.Name.Length > 25)
                return BadRequest();

            if (environment2D.MaxHeight < 10)
                environment2D.MaxHeight = 10;
            else if (environment2D.MaxHeight > 100)
                environment2D.MaxHeight = 100;

            if (environment2D.MaxLength < 20)
                environment2D.MaxLength = 20;
            else if (environment2D.MaxLength > 200)
                environment2D.MaxLength = 200;

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
