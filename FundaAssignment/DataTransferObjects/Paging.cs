using System.Text.Json.Serialization;

namespace FundaAssignment.DataTransferObjects
{
    public class Paging
    {
        [JsonPropertyName("AantalPaginas")]
        public int TotalPages { get; set; }

        [JsonPropertyName("HuidigePagina")]
        public int CurrentPage { get; set; }
    }
}