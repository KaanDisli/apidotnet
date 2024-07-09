using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using api.Models;
using api.Models.Repositories;
using api.Validations;
namespace api.Controllers
{
    [ApiController]
    [Route("/api")]
    public class UserController: ControllerBase
    {
        private UserRepository _userRepository;
        private readonly ILogger<UserController> _logger;

        public UserController(UserRepository userRepository, ILogger<UserController> logger){
            _userRepository = userRepository;
            _logger = logger;
        }
        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok("Test successful");
        }



        [HttpPost("user/register")]
        [ValidateUserAdd]
        public async Task<IActionResult> addUser([FromBody] User user){
            _logger.LogInformation($"Recieved request at user/register");
            await _userRepository.addUser(user);
            return Ok("User added");
        }
    }
}