using System.Net;

namespace BirthdayWebsiteAPI.Exceptions
{
    public class GuestNotFoundException : BaseApiException
    {
        public GuestNotFoundException()
            : base("GUEST_NOT_FOUND", "Guest not found.", HttpStatusCode.NotFound)
        {
        }
    }
}
