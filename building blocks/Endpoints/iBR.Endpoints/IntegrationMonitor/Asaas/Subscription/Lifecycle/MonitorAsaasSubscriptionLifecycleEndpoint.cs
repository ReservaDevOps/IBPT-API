using Ibr.Core.Helpers;
using Ibr.Core.Models.Asaas.Subscription.Lifecycle;
using Ibr.Core.ValueObjects.Asaas.Subscription;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using System.Text.Json;

namespace iBR.Endpoints.IntegrationMonitor.Asaas.Subscription.Lifecycle;

public class MonitorAsaasSubscriptionLifecycleEndpoint : IMonitorAsaasSubscriptionLifecycleEndpoint
{
    private readonly HttpClient _http;
    private const string BaseUrl = "api/v1/monitor/integration/asaas/subscription/lifecycle";

    private readonly string _secretCode;

    public MonitorAsaasSubscriptionLifecycleEndpoint(HttpClient http, IConfiguration configuration)
    {
        _http = http ?? throw new ArgumentNullException(nameof(http));
        _secretCode = configuration["SecretCode:Monitor:Asaas"]
            ?? throw new Exception("SecretCode para comunicação com o Integration Monitor (Asaas) não foi encontrado.");
    }

    #region Get

    public async Task<AsaasSubscriptionLifecycleModel?> GetByAsaasId(string asaasId)
    {
        _http.DefaultRequestHeaders.Clear();
        _http.DefaultRequestHeaders.Add("secretCode", _secretCode);

        # region Realizando request

        HttpResponseMessage response = await _http.GetAsync($"{BaseUrl}/asaas/{asaasId}");
        Stream contentStream = await response.Content.ReadAsStreamAsync();
        JsonSerializerOptions options = new() { PropertyNameCaseInsensitive = true };

        await HttpHelper.HandleResponseStatus(contentStream, options, response);

        #endregion

        if (contentStream.Length == 0)
        {
            return null;
        }

        AsaasSubscriptionLifecycleModel? entity = await JsonSerializer.DeserializeAsync<AsaasSubscriptionLifecycleModel>(contentStream, options);

        return entity;
    }

    public async Task<List<AsaasSubscriptionLifecycleModel>> GetByAsaasSubscriptionId(string subscriptionId)
    {
        _http.DefaultRequestHeaders.Clear();
        _http.DefaultRequestHeaders.Add("secretCode", _secretCode);

        # region Realizando request

        HttpResponseMessage response = await _http.GetAsync($"{BaseUrl}/asaas/subscription/{subscriptionId}");
        Stream contentStream = await response.Content.ReadAsStreamAsync();
        JsonSerializerOptions options = new() { PropertyNameCaseInsensitive = true };

        await HttpHelper.HandleResponseStatus(contentStream, options, response);

        #endregion

        if (contentStream.Length == 0)
        {
            return null;
        }

        List<AsaasSubscriptionLifecycleModel>? entity = await JsonSerializer.DeserializeAsync<List<AsaasSubscriptionLifecycleModel>>(contentStream, options);

        if(entity is null)
        {
            throw new Exception("Ocorreu um erro na tentativa da conversão do retorno.");
        }

        return entity;
    }

    public async Task<List<AsaasSubscriptionLifecycleModel>> GetByCompanyId(int companyId)
    {
        _http.DefaultRequestHeaders.Clear();
        _http.DefaultRequestHeaders.Add("secretCode", _secretCode);

        # region Realizando request

        HttpResponseMessage response = await _http.GetAsync($"{BaseUrl}/asaas/company/{companyId}");
        Stream contentStream = await response.Content.ReadAsStreamAsync();
        JsonSerializerOptions options = new() { PropertyNameCaseInsensitive = true };

        await HttpHelper.HandleResponseStatus(contentStream, options, response);

        #endregion

        if (contentStream.Length == 0)
        {
            return null;
        }

        List<AsaasSubscriptionLifecycleModel>? entity = await JsonSerializer.DeserializeAsync<List<AsaasSubscriptionLifecycleModel>>(contentStream, options);

        if (entity is null)
        {
            throw new Exception("Ocorreu um erro na tentativa da conversão do retorno.");
        }

        return entity;
    }

    #endregion

    #region Create

    public async Task<AsaasSubscriptionLifecycleModel> Create(CreateAsaasSubscriptionLifecycleVo valueObject)
    {
        _http.DefaultRequestHeaders.Clear();
        _http.DefaultRequestHeaders.Add("secretCode", _secretCode);

        # region Realizando request

        HttpResponseMessage response = await _http.PostAsJsonAsync($"{BaseUrl}", valueObject);
        Stream contentStream = await response.Content.ReadAsStreamAsync();
        JsonSerializerOptions options = new() { PropertyNameCaseInsensitive = true };

        await HttpHelper.HandleResponseStatus(contentStream, options, response);

        #endregion

        if (contentStream.Length == 0)
        {
            return null;
        }

        AsaasSubscriptionLifecycleModel? entity = await JsonSerializer.DeserializeAsync<AsaasSubscriptionLifecycleModel>(contentStream, options);

        if (entity is null)
        {
            throw new Exception("Ocorreu um erro na tentativa da conversão do retorno.");
        }

        return entity;
    }

    #endregion
}