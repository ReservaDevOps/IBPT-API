using Ibr.Core.Dtos.Integration.Asaas.Customer;
using Ibr.Core.Helpers;
using Ibr.Core.Models.Asaas.Customer;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using System.Text.Json;

namespace iBR.Endpoints.IntegrationMonitor.Asaas.Customer;

public class MonitorCustomerEndpoint : IMonitorCustomerEndpoint
{
    private readonly HttpClient _http;
    private const string BaseUrl = "api/v1/monitor";

    private readonly string _secretCode;

    public MonitorCustomerEndpoint(HttpClient http, IConfiguration configuration)
    {
        _http = http;
        _secretCode = configuration["SecretCode:Monitor:Asaas"]
            ?? throw new Exception("SecretCode para comunicação com o Integration Monitor (Asaas) não foi encontrado.");
    }

    public async Task<AsaasCustomerDto?> GetByAsaasId(string asaasCustomerId, string asaasAccountKey)
    {
        _http.DefaultRequestHeaders.Clear();
        _http.DefaultRequestHeaders.Add("secretCode", _secretCode);
        _http.DefaultRequestHeaders.Add("asaasAccountKey", asaasAccountKey);

        #region Realizando request

        HttpResponseMessage response = await _http.GetAsync($"{BaseUrl}/integration/asaas/customer/asaas?asaasCustomerId={asaasCustomerId}");
        Stream contentStream = await response.Content.ReadAsStreamAsync();
        JsonSerializerOptions options = new() { PropertyNameCaseInsensitive = true };

        await HttpHelper.HandleResponseStatus(contentStream, options, response);

        #endregion

        if (contentStream.Length == 0)
        {
            return null;
        }

        AsaasCustomerDto? entities = await JsonSerializer.DeserializeAsync<AsaasCustomerDto>(contentStream, options);

        return entities;
    }

    public async Task<AsaasCustomerDto?> Create(AsaasCustomerModel? model)
    {
        _http.DefaultRequestHeaders.Clear();
        _http.DefaultRequestHeaders.Add("secretCode", _secretCode);

        # region Realizando request

        HttpResponseMessage response = await _http.PostAsJsonAsync($"{BaseUrl}/integration/asaas/customer", model);
        Stream contentStream = await response.Content.ReadAsStreamAsync();
        JsonSerializerOptions options = new() { PropertyNameCaseInsensitive = true };

        // Se ocorreu um erro ao consultar a API
        if (!response.IsSuccessStatusCode)
        {
            // Mapeia o erro
            string? errorMessage = await JsonSerializer.DeserializeAsync<string>(contentStream, options);

            if (errorMessage is not null)
            {
                throw new Exception($"Erro na conversão do retorno: {errorMessage}");
            }

            return null;
        }

        #endregion

        AsaasCustomerDto? entities = await JsonSerializer.DeserializeAsync<AsaasCustomerDto>(contentStream, options);

        return entities;
    }
}
