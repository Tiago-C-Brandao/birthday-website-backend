using System.Net;

namespace BirthdayWebsiteAPI.Exceptions
{
    public class InvalidCredentialsException : BaseApiException
    {
        public InvalidCredentialsException()
            : base("INVALID_CREDENTIALS", "The provided credentials are invalid.", HttpStatusCode.Unauthorized)
        {
        }
    }

    public class UserNameAlreadyInUseException : BaseApiException
    {
        public UserNameAlreadyInUseException(string username)
            : base("USERNAME_ALREADY_IN_USE", $"A user with the given {username} already exists.", HttpStatusCode.Conflict)
        {
        }
    }

    public class WhatsappAlreadyInUseException : BaseApiException
    {
        public WhatsappAlreadyInUseException(string whatsapp)
            : base("WHATSAPP_ALREADY_IN_USE", $"A user with the given {whatsapp} number already exists.", HttpStatusCode.Conflict)
        {
        }
    }

    public class TokenExpiredExeption : BaseApiException
    {
        public TokenExpiredExeption() : base("TOKEN_EXPIRED", "Token has expired.", HttpStatusCode.Unauthorized)
        {
        }
    }

    public class UserNotFoundException : BaseApiException
    {
        public UserNotFoundException() : base("USER_NOT_FOUND", $"User not found", HttpStatusCode.NotFound) 
        {
        }
    }

    public class  WhatsappInvalidFormatException : BaseApiException
    {
        public WhatsappInvalidFormatException() : base("WHATSAPP_INVALID_FORMAT", "WhatsApp number must be in the format (XX) 9XXXX-XXXX or XX 9XXXX-XXXX.", HttpStatusCode.BadRequest)
        {
        }
    }
}
