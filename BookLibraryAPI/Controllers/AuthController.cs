using Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookLibraryAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        #region Private Variables

        private readonly TokenHelper _tokenHelper;

        #endregion

        #region Constructor

        public AuthController(TokenHelper tokenHelper)
        {
            _tokenHelper = tokenHelper;
        }

        #endregion

        #region Public Methods

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLogin user)
        {
            if (user.Username == "admin" && user.Password == "password")
            {
                var token = _tokenHelper.GenerateToken(user.Username);
                return Ok(new { token });
            }

            return Unauthorized("Invalid credentials");
        }

        public class UserLogin
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        #endregion
    }
}
