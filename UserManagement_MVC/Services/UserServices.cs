using Microsoft.AspNetCore.Mvc;
using System.Collections;
using UserManagement_MVC.Models;
using UserManagement_MVC.Models.Responses;
using UserManagement_MVC.Repository;

namespace UserManagement_MVC.Services
{
    public class UserServices : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserServices(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<RegisterUserResponse> Register(RegisterUserModel registerUserModel)
        {
            return await _userRepository.Register(registerUserModel);
        }

        public async Task<LoginUserResponse> Login(LoginUserModel loginUserModel)
        {
            return await _userRepository.Login(loginUserModel);
        }

        public async Task<IEnumerable> Index()
        {
            return await  _userRepository.Index();
        }

        public async Task<UpdateUserModel> Update(Guid Id)
        {
            return await _userRepository.Update(Id);
        }

        public async Task<bool> Update(UpdateUserModel updateUserModel)
        {
            return await _userRepository.Update(updateUserModel);
        }

        public async Task<bool> Delete(Guid Id)
        {
            return  await _userRepository.Delete(Id);
        }



    }
}
