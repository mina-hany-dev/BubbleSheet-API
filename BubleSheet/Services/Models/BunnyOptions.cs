namespace BubleSheet.Services.Models
{
    public class BunnyOptions
    {
        public string ZoneName { get; set; } = null!;
        public string AccessKey { get; set; } = null!;
        public string Region { get; set; } = "de";
        public string BaseUrl { get; set; } = null!;
        public string UrlTokenAuthenticationKey { get; set; } = null!;

    }
}
