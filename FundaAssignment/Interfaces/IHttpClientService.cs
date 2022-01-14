using FundaAssignment.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FundaAssignment.Interfaces
{
    public interface IHttpClientService
    {
        Task<Dictionary<int, MakelaarNameHouseCount>> GetHouses(bool withTuin);
    }
}