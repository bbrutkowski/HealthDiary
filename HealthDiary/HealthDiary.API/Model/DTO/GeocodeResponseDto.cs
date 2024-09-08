using Newtonsoft.Json;

namespace HealthDiary.API.Model.Main
{
    public record GeocodeResponseDto
    {
        [JsonProperty("results")]
        public GeocodeResultDto[] Results { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }

    public record GeocodeResultDto
    {
        [JsonProperty("address_components")]
        public AddressComponentDto[] AddressComponents { get; set; }

        [JsonProperty("formatted_address")]
        public string FormattedAddress { get; set; }
    }

    public record AddressComponentDto
    {
        [JsonProperty("long_name")]
        public string LongName { get; set; }

        [JsonProperty("short_name")]
        public string ShortName { get; set; }

        [JsonProperty("types")]
        public string[] Types { get; set; }
    }
}
