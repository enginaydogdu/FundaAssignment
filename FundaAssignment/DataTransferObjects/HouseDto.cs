using System.Text.Json.Serialization;

namespace FundaAssignment.DataTransferObjects
{
    public class HouseDto
    {
        [JsonPropertyName("MakelaarId")]
        public int RealEstateId { get; set; }

        [JsonPropertyName("MakelaarNaam")]
        public string RealEstateName { get; set; }
    }
}