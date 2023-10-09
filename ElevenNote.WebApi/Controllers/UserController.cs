using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElevenNote.Models.User;
using ElevenNote.Services.User;
using ElevenNote.Models.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ElevenNote.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegister model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var registerResult = await _userService.RegisterUserAsync(model);
            if(registerResult)
            {
                TextResponse response = new("User was regitered.");
                return Ok(response);
            }

            return BadRequest(new TextResponse("User could not be registered."));
        }

        [HttpGet("{userId:int}")]
        public async Task<IActionResult> GetById([FromRoute] int userId)
        {
            UserDetail? detail = await _userService.GetUserByIdAsync(userId);

            if (detail is null)
            {
                return NotFound();
            }

            return Ok(detail);
        }
    }
}
