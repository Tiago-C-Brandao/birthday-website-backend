using System.Net;

namespace BirthdayWebsiteAPI.Exceptions
{
    public class GiftNotFoundException : BaseApiException
    {
        public GiftNotFoundException()
            : base("GIFT_NOT_FOUND", "Gift not found.", HttpStatusCode.NotFound)
        {
        }
    }
}
