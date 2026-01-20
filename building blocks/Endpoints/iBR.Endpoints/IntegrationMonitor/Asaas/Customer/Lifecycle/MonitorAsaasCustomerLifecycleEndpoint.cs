using Ibr.Core.Helpers;
using Ibr.Core.Models.Asaas.Customer;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using System.Text.Json;

namespace iBR.Endpoints.IntegrationMonitor.Asaas.Customer.Lifecycle;

public class MonitorAsaasCustomerLifecycleEndpoint : IMonitorAsaasCustomerLifecycleEndpoint
{
    private readonly HttpClient _http;
    private const string BaseUrl = "api/v1/monitor";

    private readonly string _secretCode;

    public MonitorAsaasCustomerLifecycleEndpoint(HttpClient http, IConfiguration configuration)
    {
        _http = http ?? throw new ArgumentNullException(nameof(http));
        _secretCode = configuration["SecretCode:Monitor:Asaas"] 
            ?? throw new Exception("SecretCode para comunicação com o Integration Monitor (Asaas) não foi encontrado.");
    }

    public async Task<AsaasCustomerLifecycleModel?> Create(AsaasCustomerLifecycleModel model)
    {
        _http.DefaultRequestHeaders.Clear();
        _http.DefaultRequestHeaders.Add("secretCode", _secretCode);

        # region Realizando request

        HttpResponseMessage response =
            await _http.PostAsJsonAsync($"{BaseUrl}/integration/asaas/customer/lifecycle", model);
        Stream contentStream = await response.Content.ReadAsStreamAsync();
        JsonSerializerOptions options = new() { PropertyNameCaseInsensitive = true };

        await HttpHelper.HandleResponseStatus(contentStream, options, response);

        #endregion

        if (contentStream.Length == 0)
        {
            return null;
        }

        AsaasCustomerLifecycleModel? entities =
            await JsonSerializer.DeserializeAsync<AsaasCustomerLifecycleModel>(contentStream, options);

        return entities;
    }
}