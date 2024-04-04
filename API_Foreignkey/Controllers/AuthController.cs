using API_Foreignkey.IReposiory;
using API_Foreignkey.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using Test2.Data;
using Test2.Models;
using Microsoft.Data.SqlClient;

namespace API_Foreignkey.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authrepository;
        private readonly IConfiguration _configuration;
        private object configuration;

        public AuthController(IAuthRepository authRepository, IConfiguration configuration)
        {
            _authrepository = authRepository;
            _configuration = configuration;
        }
        [HttpPost("login")]
        public ServiceResponse Login([FromBody]LoginRequest obj)
        {
            return _authrepository.Login(obj); 
        }  

        [HttpPost("assignRole")]
        public bool AssignRoleToUser([FromBody] AddUserAssignRole addUserRole)
        {
            var adduserrole=_authrepository.AssignRoleToUser(addUserRole);
            return adduserrole;
        }

        [HttpPost("addUser")]
        public IActionResult AddUser([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest("User object is null");
            }

            if (user.Password != user.ConfirmPassword)
            {
                return BadRequest("Password and ConfirmPassword do not match");
            }
            if(user.Email==""|| !Regex.IsMatch(user.Email, "^[a-zA-Z]+@gmail\\.com$") || user.Email == "string")
            {
                return BadRequest("Email Can'not Proper");
            }
            if(user.role =="" ||  user.role =="string")
            {
                return BadRequest("role is empty");
            }
            var addedUser = _authrepository.AddUser(user);
            return Ok(addedUser); // Or you can return CreatedAtAction to return 201 Created status
        }


        [HttpPost("addRole")]
        public Role AddRole([FromBody] Role role)
        {
            var addrole = _authrepository.AddRole(role);
            return addrole;
        }

            [HttpGet("updateActive")]
            public IActionResult UpdateActive(bool IsActive, int UserId)
            {
                string connectionString = _configuration.GetConnectionString("cn");

                string sqlQuery = "UPDATE [users] SET IsActive = @IsActive WHERE Id = @UserId";

                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@IsActive", IsActive);
                    command.Parameters.AddWithValue("@UserId", UserId);

                    try
                    {
                        connection.Open();

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            return Ok("User's active status updated successfully.");
                        }
                        else
                        {
                            return NotFound("User not found or no changes made.");
                        }
                    }
                    catch (Exception ex)
                    {
                        return StatusCode(500, $"Error updating user's active status: {ex.Message}");
                    }
                }
            }

        }
    }
