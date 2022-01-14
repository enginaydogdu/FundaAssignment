using FundaAssignment.DataTransferObjects;
using FundaAssignment.Interfaces;
using FundaAssignment.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace FundaAssignment.Services
{
    public class HttpClientService : IHttpClientService
    {
        private readonly JsonSerializerOptions _options;
        private readonly Configuration _configuration;
        private readonly ILogger<HttpClientService> _logger;
        private Dictionary<int, MakelaarNameHouseCount> _makelaarHouseCount;
        private HttpClient _httpClient;

        public HttpClientService(IOptions<Configuration> configuration, ILogger<HttpClientService> logger)
        {
            _configuration = configuration.Value;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            _logger = logger;
        }

        public async Task<Dictionary<int, MakelaarNameHouseCount>> GetHouses(bool withTuin)
        {
            int page = 1;
            int retryCount = 1;
            Response response = null;
            HttpResponseMessage httpResponse;
            int maxRetryCount = _configuration.MaxRetryCount;
            int retryDuration = _configuration.RetryDuration;
            bool retryInProgress;
            _makelaarHouseCount = new Dictionary<int, MakelaarNameHouseCount>();
            do
            {
                retryInProgress = false;
                try
                {
                    using (_httpClient = new HttpClient())
                    {
                        _httpClient.BaseAddress = new Uri(_configuration.BaseAddress);
                        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                        string uri = $"feeds/Aanbod.svc/json/{_configuration.Key}/?type=koop&zo=/amsterdam/{(withTuin ? "tuin/" : null)}&page={page}&pagesize={_configuration.PageSize}";
                        httpResponse = await _httpClient.GetAsync(uri);
                        httpResponse.EnsureSuccessStatusCode();

                        if (retryCount != 1)
                        {
                            _logger.LogInformation($"The Funda API responded successfully again after {retryCount} attempt.");
                            retryCount = 1;
                        }
                    }
                }
                catch (HttpRequestException ex)
                {
                    if (retryCount <= maxRetryCount)
                    {
                        _logger.LogWarning($"retry in progress retryCount: {retryCount}");
                        retryInProgress = true;
                        retryCount++;
                        await Task.Delay(retryDuration);
                        continue;
                    }
                    throw;
                }
                catch (Exception ex)
                {
                    _logger.LogError("Exception occured during api call!", ex);
                    throw;
                }

                var content = await httpResponse.Content.ReadAsStringAsync();
                response = JsonSerializer.Deserialize<Response>(content, _options);
                AddToDictionary(response.Houses);
                page++;
            } while (retryInProgress || response.Paging.CurrentPage <= response.Paging.TotalPages);

            return _makelaarHouseCount;
        }

        private void AddToDictionary(IEnumerable<HouseDto> houses)
        {
            foreach (var item in houses)
            {
                if (!_makelaarHouseCount.ContainsKey(item.RealEstateId))
                {
                    _makelaarHouseCount.Add(item.RealEstateId, new MakelaarNameHouseCount() { MakelaarName = item.RealEstateName, TotalHouse = 1 });
                }
                else
                {
                    _makelaarHouseCount[item.RealEstateId].TotalHouse++;
                }
            }
        }
    }
}