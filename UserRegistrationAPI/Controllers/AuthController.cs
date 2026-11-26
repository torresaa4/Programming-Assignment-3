using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using UserRegistrationAPI.Data;
using UserRegistrationAPI.Models;

namespace UserRegistrationAPI.Controllers
{


    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserDatabaseAccess connection;

        public AuthController(UserDatabaseAccess database_connection)
        {
            connection = database_connection;
        }
        
        // get frontend data in form of RegisterModel object user
        [HttpPost("register")]
        public IActionResult Register(RegisterModel user)
        {

            if (connection.EmailExists(user.Email))
            {
                return BadRequest("Email already exist");
            }

            string passwordHashed = HashPassword(user.Password);
            bool isRegistered = connection.RegisterUser(user, passwordHashed);

            if (isRegistered)
            {
                return Ok("User registered");
            }

            return Ok("Registration failed");
        }

        [HttpPost("login")]
        public IActionResult Login(LoginModel login)
        {
            User? exist = connection.FindUser(login.Email);


            if (exist == null)
            {
                return Unauthorized("Invalid email or password");
            }

            //  check password
            var hash = new PasswordHasher<string>();

            var PasswordCheck = hash.VerifyHashedPassword(
                null!,
                exist.PasswordHash,
                login.Password
            );

            if (PasswordCheck == PasswordVerificationResult.Failed)
            {
                return Unauthorized("Invalid email or password");
            }
            return Ok("Login Successful");
        }



        private string HashPassword(string password)
        {
            var hasher = new PasswordHasher<string>();

            var hash = hasher.HashPassword(null!, password);

            return hash;
        }
    }
}







