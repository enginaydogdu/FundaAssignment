using FundaAssignment.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace FundaAssignment.Services
{
    public class OrchestrationService : IOrchestrationService
    {
        private readonly IConsoleService _consoleService;
        private readonly IHttpClientService _httpClientService;

        public OrchestrationService(IConsoleService consoleService, IHttpClientService httpClientService)
        {
            _consoleService = consoleService;
            _httpClientService = httpClientService;
        }

        public async Task Execute(bool withTuin)
        {
            var houses = await _httpClientService.GetHouses(withTuin);

            var top10makelaar = houses.Values.OrderByDescending(x => x.TotalHouse).Take(10);
            _consoleService.WriteToConsole(top10makelaar, withTuin);
        }
    }
}