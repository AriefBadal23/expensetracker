using System.Text.Json;

namespace HotelListing.Core.Models
{
    public class Error
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public override string ToString() => JsonSerializer.Serialize(this);
    }

}



