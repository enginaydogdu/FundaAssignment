using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FundaAssignment.DataTransferObjects
{
    public class Response
    {
        [JsonPropertyName("Objects")]
        public IEnumerable<HouseDto> Houses { get; set; }

        public Paging Paging { get; set; }
    }
}