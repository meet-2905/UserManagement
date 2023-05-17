
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Collections;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserManagement_MVC.Models;
using UserManagement_MVC.Models.Responses;

namespace UserManagement_MVC.Repository
{
    public class UserRepository : IUserRepository
    {     
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        
        public UserRepository(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
         
        }

        public async Task<RegisterUserResponse> Register(RegisterUserModel userModel)
        {
            var userFetch = await _userManager.FindByEmailAsync(userModel.Email);
            
            if (userFetch != null)
            {
                return new RegisterUserResponse
                {
                    IsSuccess = false,
                    Message = "User Already Exist"
                };
            }

            ApplicationUser newUser = new()
            {
                UserName = userModel.UserName,
                FirstName = userModel.FirstName,
                LastName = userModel.LastName,
                Email = userModel.Email,
            };
            if (await _roleManager.RoleExistsAsync(userModel.Role))
            {
                var result = await _userManager.CreateAsync(newUser, userModel.Password);


                if (!result.Succeeded)
                {
                    return new RegisterUserResponse
                    {
                        IsSuccess = false,
                        Message = "Registration Failed"
                    };
                }

                await _userManager.AddToRoleAsync(newUser, userModel.Role);

                return new RegisterUserResponse
                {
                    IsSuccess = true,
                    Message = "Registration Successful"
                };
            }

            return new RegisterUserResponse
            {
                IsSuccess = false,
                Message = "Unknown Error Occured"
            };
        }

        public async Task<LoginUserResponse> Login(LoginUserModel loginUserModel)
        {
            var userFetch = await _userManager.FindByNameAsync(loginUserModel.UserName);

            if (userFetch != null && await _userManager.CheckPasswordAsync(userFetch, loginUserModel.Password))
            {
                var userRole = await _userManager.GetRolesAsync(userFetch);
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userFetch.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    
                };

                foreach (var userrole in userRole)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userrole));
                }

                var jwtToken = GetToken(authClaims);

                string token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

                
                return new LoginUserResponse { IsSuccess = true, Message="Successful Login", jwtToken=token};
            }
            return new LoginUserResponse { IsSuccess = false, Message = "Username Not Found"}; 
        }

        public JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

        public async Task<IEnumerable> Index()
        {
            var userList = await _userManager.Users.ToListAsync();
            return userList;
        }

        public async Task<UpdateUserModel> Update(Guid Id)
        {
            //string sId = Id.ToString();
            var userFetch = await _userManager.Users.FirstOrDefaultAsync(e => e.Id == Id.ToString() );
            var roleFetch = await _userManager.GetRolesAsync(userFetch);
            var roleName = roleFetch.FirstOrDefault();
            
            if (userFetch != null)
            {
                var updateUserModel = new UpdateUserModel()
                {
                    Id = userFetch.Id,
                    UserName = userFetch.UserName,
                    FirstName = userFetch.FirstName,
                    LastName = userFetch.LastName,
                    Email = userFetch.Email,
                    Role = roleName
                };

                return updateUserModel;
            }
            return new UpdateUserModel();     
        }

        public async Task<bool> Update(UpdateUserModel updateUserModel)
        {
            var userFetch = await _userManager.FindByIdAsync(updateUserModel.Id);
            var roleFetch = await _userManager.GetRolesAsync(userFetch);
            var roleName = roleFetch.FirstOrDefault();


           
            if (userFetch != null)
            {
                userFetch.Id = updateUserModel.Id;
                userFetch.UserName = updateUserModel.UserName;
                userFetch.FirstName = updateUserModel.FirstName;
                userFetch.LastName = updateUserModel.LastName;
                userFetch.Email = updateUserModel.Email;



                if (roleName != updateUserModel.Role)
                {
                     await _userManager.RemoveFromRoleAsync(userFetch, roleName);
                     await _userManager.AddToRoleAsync(userFetch, updateUserModel.Role);
                    
                }
                await _userManager.UpdateAsync(userFetch);

                return true;
            }
            return false;    
        }

        public async Task<bool> Delete(Guid Id)
        {
            string sId = Id.ToString();

            var userFetch = await _userManager.FindByIdAsync(sId);

            if (userFetch != null)
            {
                await _userManager.DeleteAsync(userFetch);

                return true;
            }
            return false;
        }

    }
}
