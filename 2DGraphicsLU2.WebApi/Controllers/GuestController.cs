using System;
using _2DGraphicsLU2.WebApi.Models;
using _2DGraphicsLU2.WebApi.Repositories;
using _2DGraphicsLU2.WebApi.Repositories.Interfaces;
using _2DGraphicsLU2.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace _2DGraphicsLU2.WebApi.Controllers
{

    [ApiController]
    [Route("guests")]
    public class GuestController : ControllerBase
    {
        private IAuthenticationService _authenticationService;
        private readonly IGuestRepository _guestRepository;

        public GuestController(IAuthenticationService authenticationService, IGuestRepository guestRepository)
        {
            _authenticationService = authenticationService;
            _guestRepository = guestRepository;
        }

        [HttpGet("{environmentId}", Name = "ReadEnvironmentFromGuest")]
        public async Task<ActionResult<Environment2D>> Get(Guid environmentId)
        {
            var userId = _authenticationService.GetCurrentAuthenticatedUserId();
            if (userId == null)
                return BadRequest();


            var environment = await _guestRepository.ReadAsync(environmentId, userId);
            if (environment == null)
                return NotFound();

            return Ok(environment);
        }

        [HttpGet(Name = "ReadEnvironmentsFromGuest")]
        public async Task<ActionResult<IEnumerable<Environment2D>>> Get()
        {
            var userId = _authenticationService.GetCurrentAuthenticatedUserId();
            if (userId == null)
                return BadRequest();

            var environments = await _guestRepository.ReadAsync(userId);
            return Ok(environments);
        }

        [HttpGet("{environmentId}/objects", Name = "ReadObjectsInEnvironmentFromGuest")]
        public async Task<ActionResult<IEnumerable<Object2D>>> GetObjects(Guid environmentId)
        {
            var userId = _authenticationService.GetCurrentAuthenticatedUserId();
            if (userId == null)
                return BadRequest();
            var objects2D = await _guestRepository.ReadObjectsAsync(environmentId, userId);
            return Ok(objects2D);
        }


        [HttpPost("{environmentId}/user/{guestUsername}", Name = "ShareEnvironmentWithGuest")]
        public async Task<ActionResult> Add(Guid environmentId, string guestUsername)
        {
            var userId = _authenticationService.GetCurrentAuthenticatedUserId();
            if (userId == null)
                return BadRequest();

            await _guestRepository.InsertAsync(environmentId, userId, guestUsername);

            return Ok();
        }

        [HttpDelete("{environmentId}", Name = "SelfRemoveGuestFromEnvironment")]
        public async Task<IActionResult> Delete(Guid environmentId)
        {
            var userId = _authenticationService.GetCurrentAuthenticatedUserId();
            if (userId == null)
                return BadRequest();
            var existingEnvironment = await _guestRepository.ReadAsync(environmentId, userId);

            if (existingEnvironment == null)
                return NotFound();

            await _guestRepository.DeleteAsync(environmentId, userId);

            return Ok();
        }

        [HttpDelete("{environmentId}/user/{guestUsername}", Name = "ForceRemoveGuestFromEnvironment")]
        public async Task<IActionResult> Delete(Guid environmentId, string guestUsername)
        {
            var userId = _authenticationService.GetCurrentAuthenticatedUserId();
            if (userId == null)
                return BadRequest();

            await _guestRepository.DeleteAsync(environmentId, userId, guestUsername);

            return Ok();
        }
    }
}
