using Newtonsoft.Json;

namespace MvcApplication7.Models
{
    public class FacebookPhoto
    {
        [JsonProperty("picture")] // Cette option renomme la propriété en image.
        public string ThumbnailUrl { get; set; }

        public string Link { get; set; }
    }
}
