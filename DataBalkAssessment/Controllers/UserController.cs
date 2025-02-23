using DataBalkAssessment.Data;
using DataBalkAssessment.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace DataBalkAssessment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _authContext;

        public UserController(ApplicationDbContext applicationDbContext)
        {
            _authContext = applicationDbContext;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] RegisterRequest userObj)
        {
            if (userObj == null || string.IsNullOrEmpty(userObj.Email) || string.IsNullOrEmpty(userObj.Password))
                return BadRequest();

            var user = await _authContext.users
                .FirstOrDefaultAsync(x => x.Email == userObj.Email && x.Password == userObj.Password);

            if (user == null)
                return NotFound(new { Message = "User Not Found!" });

            return Ok(new
            {
                Message = "Successful Login!"
            });
        }
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterRequest userObj)
        {
            if (userObj == null)
                return BadRequest();

            var newUser = new User
            {
                Id = userObj.Id,
                FirstName = userObj.FirstName,
                LastName = userObj.LastName,
                Email = userObj.Email,
                Password = userObj.Password, 
                ConfirmPassword = userObj.ConfirmPassword, 
                Role = userObj.Role,
                PhoneNumber = userObj.PhoneNumber,
                ApiKey = Guid.NewGuid().ToString()
            };

            
            await _authContext.users.AddAsync(newUser);
            await _authContext.SaveChangesAsync();

            return Ok(new
            {
                Message = "User Registered!",
                ApiKey = newUser.ApiKey
            });
        }




    }
}
