using API_Foreignkey.Migrations;
using API_Foreignkey.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Test2.Data;
using Test2.Models;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace API_Foreignkey.IReposiory.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        public AuthRepository(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;

        }
        public Role AddRole(Role role)
        {
            var addrole = _context.roles.Add(role);
            _context.SaveChanges();
            return addrole.Entity;
        }

        public User AddUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "Invalid User!!");
            }
            if (_context.users.Any(u => u.Email == user.Email))
            {
                throw new ArgumentException("Email already exists in the system.", nameof(user.Email));
            }

            //else if(user.role.ToLower() != "user")
            //  {
            //      throw new ArgumentNullException(nameof(user), "role User is not valid!!");
            //  }
            else if (string.IsNullOrWhiteSpace(user.FirstName) || user.FirstName == "string")
            {
                throw new ArgumentException("Username cannot be empty or whitespace.", nameof(user.Username));
            }
            else if (string.IsNullOrWhiteSpace(user.Username) || user.Username == "string")
            {
                throw new ArgumentException("Username cannot be empty or whitespace.", nameof(user.Username));
            }

            else if (string.IsNullOrWhiteSpace(user.Password) || user.Password == "string")
            {
                throw new ArgumentException("Password cannot be empty, whitespace, or the string 'string'.", nameof(user.Password));
            }
            //if (user.Password != user.ConfirmPassword)
            //{
            //    return BadRequest("Password and ConfirmPassword do not match");
            //}
            if (user.Password != user.ConfirmPassword)
            {
                throw new ArgumentException("Password and ConfirmPassword do not match.", nameof(user.ConfirmPassword));
            }
            //if(user.role.ToLower()== "user")
            //{
            //    user.role = "User";
            //}
            //else if(user.role.ToLower() == "admin")
            //{
            //    user.role="Admin";
            //}
            user.Password = HashPassword(user.Password);
            user.ConfirmPassword = HashPassword(user.ConfirmPassword);
            var added = _context.users.Add(user);
            _context.SaveChanges();
            var user1 = _context.users.Where(x => x.Email == user.Email).FirstOrDefault();
            var user2 = _context.roles.Where(x => x.Name == user.role).FirstOrDefault();
            if (user2 == null)
            {
                throw new ArgumentNullException(nameof(user2), "Invalid User!!");
            }
            UserRole userRole = new UserRole()
            {
                UserId = user1.Id,
                RoleId = user2.Id
            };
            var added1 = _context.userRoles.Add(userRole);

            _context.SaveChanges();

            return added.Entity;
        }
        private string HashPassword(string password)
        {
            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 32));

            string combinedHash = $"{Convert.ToBase64String(salt)}:{hashedPassword}";

            return combinedHash;
        }
        public bool AssignRoleToUser(AddUserAssignRole obj)
        {
            try
            {
                var addroles = new List<UserRole>();
                var user = _context.users.SingleOrDefault(x => x.Id == obj.UserId);
                var role = _context.roles.SingleOrDefault(x => x.Id == obj.RoleIds.SingleOrDefault());
                if (user == null)
                    throw new Exception("user is not valid");
                if (role == null)
                    throw new Exception("role is not valid");
                foreach (int roles in obj.RoleIds)
                {
                    var userRole = new UserRole();
                    userRole.RoleId = role.Id;
                    userRole.UserId = user.Id;
                    addroles.Add(userRole);
                }
                _context.userRoles.AddRange(addroles);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public ServiceResponse Login(LoginRequest loginRequest)
        {
            ServiceResponse response = new ServiceResponse();
            string UserType = "";
            if (loginRequest != null && loginRequest.Password != null)
            {
                var user = _context.users.SingleOrDefault(x => x.Email == loginRequest.UserName && x.Password == loginRequest.Password);
                if (user != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["jwt:Subject"]),
                        //new Claim("Id",user.Id.ToString()),
                        new Claim("UserName",user.FirstName),
                    };
                    var userrole = _context.userRoles.Where(u => u.UserId == user.Id).ToList();
                    var rolid = userrole.Select(s => s.RoleId).ToList();
                    var roles = _context.roles.Where(r => rolid.Contains(r.Id)).ToList();
                    foreach (var role in roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role.Name));
                    }
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var tocken = new JwtSecurityToken(
                        _configuration["jwt:Issuer"],
                        _configuration["jwt:Audience"],
                        claims,
                        expires: DateTime.UtcNow.AddMinutes(10),
                        signingCredentials: signIn
                        );
                    var jwttocken = new JwtSecurityTokenHandler().WriteToken(tocken);
                    var allusers = new List<UserVM>();
                    //foreach (var role in roles)
                    //{
                    //}
                    //if (roles.Count > 0)
                    //{

                    //}
                    if (jwttocken != "" && user.role.ToLower() == "admin")
                    {
                        var allactiveuser = _context.users.Where(x => /*x.IsActive == true &&*/ x.role.ToLower() == "user").ToList();
                        foreach (var item in allactiveuser)
                        {
                            UserVM userVM = new UserVM();
                            userVM.Id = item.Id;
                            userVM.PhoneNumber = item.PhoneNumber;
                            userVM.Username = item.Username;
                            userVM.Password = item.Password;
                            userVM.Email = item.Email;
                            userVM.ConfirmPassword = item.ConfirmPassword;
                            userVM.FirstName = item.FirstName;
                            userVM.LastName = item.LastName;
                            userVM.role = item.role;
                            userVM.IsActive = item.IsActive;
                            userVM.Token = jwttocken;
                            UserType = "admin";
                            allusers.Add(userVM);
                        }
                    }
                    else if (jwttocken != "" && user.role.ToLower() == "user")
                    {
                        var allactiveuser = _context.users.Where(x => x.IsActive == true && x.Email == loginRequest.UserName && x.Password == loginRequest.Password).ToList();
                        if (allactiveuser != null)
                        {
                            foreach (var item in allactiveuser)
                            {
                                UserVM userVM = new UserVM();
                                userVM.Id = item.Id;
                                userVM.PhoneNumber = item.PhoneNumber;
                                userVM.Username = item.Username;
                                userVM.Password = item.Password;
                                userVM.Email = item.Email;
                                userVM.ConfirmPassword = item.ConfirmPassword;
                                userVM.FirstName = item.FirstName;
                                userVM.LastName = item.LastName;
                                userVM.role = item.role;
                                userVM.IsActive = item.IsActive;
                                userVM.Token = jwttocken;
                                UserType = "user";
                                allusers.Add(userVM);
                            }
                        }

                    }
                    response.issuccess = true;
                    response.message = "Token Generated";
                    response.data = allusers;
                    response.logintype = UserType;
                    return response;
                }
                else
                {
                    response.issuccess = false;
                    response.message = "Token not Generated";
                    response.data = "";
                    response.logintype = UserType;
                    return response;
                }
            }
            else
            {
                response.issuccess = false;
                response.message = "Token not Generated due to invalid credential";
                response.data = "";
                response.logintype = UserType;
                return response;
            }
        }
    }
}
