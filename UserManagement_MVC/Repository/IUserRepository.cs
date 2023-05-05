using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using UserManagement_MVC.Models;
using UserManagement_MVC.Models.Responses;

namespace UserManagement_MVC.Repository
{
    public interface IUserRepository
    {
        Task<RegisterUserResponse> Register(RegisterUserModel registerUserModel);
        Task<LoginUserResponse> Login(LoginUserModel loginUserModel);
        Task<IEnumerable> Index();
        Task<UpdateUserModel> Update(Guid Id);
        Task<bool> Update(UpdateUserModel updateUserModel);
        Task<bool> Delete(Guid Id);
      
        JwtSecurityToken GetToken(List<Claim> authClaims);
    }
}
