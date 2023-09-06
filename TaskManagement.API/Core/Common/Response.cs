namespace TaskManagement.API.Core.Common
{
    public class Response
    {
        public bool IsSucceed { get; set; }
        public string? Message { get; set; }
    }
    public class AuthServiceResponse : Response
    {
        public string? RefreshToken { get; set; }
    }

    public class TokenServiceResponse : Response
    {
    }

    public class RegisterServiceResponse : Response
    {
    }

    public class UserServiceResponse : Response
    {
    }

    public class RolesServiceResponse : Response
    {
    }

    public class CommentServiceResponse : Response
    {
    }
}
