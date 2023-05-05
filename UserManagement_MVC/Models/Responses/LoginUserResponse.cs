namespace UserManagement_MVC.Models.Responses
{
    public class LoginUserResponse
    {
        

        public bool IsSuccess { get; set; }

        public string? Message { get; set; }

        public string? jwtToken { get; set; }

      


    }
}
