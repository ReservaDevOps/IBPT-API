using Ibr.Core.Enums.Asaas;
using Ibr.Core.Helpers;
using Ibr.Core.Models.Asaas.Payment;
using Ibr.Core.ValueObjects.Asaas.Lifecycle;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using System.Text.Json;

namespace iBR.Endpoints.IntegrationMonitor.Asaas.Payment.Lifecycle;

public class MonitorAsaasPaymentLifecycleEndpoint : IMonitorAsaasPaymentLifecycleEndpoint
{
    private readonly HttpClient _http;
    private const string BaseUrl = "api/v1/monitor/integration/asaas/payment/lifecycle";

    private readonly string _secretCode;

    public MonitorAsaasPaymentLifecycleEndpoint(HttpClient http, IConfiguration configuration)
    {
        _http = http;
        _secretCode = configuration["SecretCode:Monitor:Asaas"]
            ?? throw new Exception("SecretCode para comunicação com o Integration Monitor (Asaas) não foi encontrado.");
    }

    #region Get

    public async Task<List<AsaasPaymentLifecycleModel>?> GetByAsaas(string asaasPaymentId)
    {
        _http.DefaultRequestHeaders.Clear();
        _http.DefaultRequestHeaders.Add("secretCode", _secretCode);
        _http.DefaultRequestHeaders.Add("asaasPaymentId", asaasPaymentId);

        # region Realizando request

        HttpResponseMessage response = await _http.GetAsync($"{BaseUrl}/asaas");
        Stream contentStream = await response.Content.ReadAsStreamAsync();
        JsonSerializerOptions options = new() { PropertyNameCaseInsensitive = true };

        await HttpHelper.HandleResponseStatus(contentStream, options, response);

        #endregion

        if (contentStream.Length == 0) return null;

        List<AsaasPaymentLifecycleModel>? entities =
            await JsonSerializer.DeserializeAsync<List<AsaasPaymentLifecycleModel>>(contentStream, options);

        return entities;
    }

    public async Task<List<AsaasPaymentLifecycleModel>?> GetByStatus(EAsaasEventWebhook status)
    {
        _http.DefaultRequestHeaders.Clear();
        _http.DefaultRequestHeaders.Add("secretCode", _secretCode);
        _http.DefaultRequestHeaders.Add("status", status.ToString());

        # region Realizando request

        HttpResponseMessage response = await _http.GetAsync($"{BaseUrl}/status");
        Stream contentStream = await response.Content.ReadAsStreamAsync();
        JsonSerializerOptions options = new() { PropertyNameCaseInsensitive = true };

        await HttpHelper.HandleResponseStatus(contentStream, options, response);

        #endregion

        if (contentStream.Length == 0) return null;

        List<AsaasPaymentLifecycleModel>? entities =
            await JsonSerializer.DeserializeAsync<List<AsaasPaymentLifecycleModel>>(contentStream, options);

        return entities;
    }

    public async Task<List<AsaasPaymentLifecycleModel>?> GetByLastStatus(EAsaasEventWebhook status)
    {
        _http.DefaultRequestHeaders.Clear();
        _http.DefaultRequestHeaders.Add("secretCode", _secretCode);
        _http.DefaultRequestHeaders.Add("status", status.ToString());

        # region Realizando request

        HttpResponseMessage response = await _http.GetAsync($"{BaseUrl}/status/last");
        Stream contentStream = await response.Content.ReadAsStreamAsync();
        JsonSerializerOptions options = new() { PropertyNameCaseInsensitive = true };

        await HttpHelper.HandleResponseStatus(contentStream, options, response);

        #endregion

        if (contentStream.Length == 0) return null;

        List<AsaasPaymentLifecycleModel>? entities =
            await JsonSerializer.DeserializeAsync<List<AsaasPaymentLifecycleModel>>(contentStream, options);

        return entities;
    }

    #endregion

    #region Create

    public async Task<AsaasPaymentLifecycleModel?> Create(CreateAsaasPaymentLifecycleVO vo)
    {
        _http.DefaultRequestHeaders.Clear();
        _http.DefaultRequestHeaders.Add("secretCode", _secretCode);

        # region Realizando request

        HttpResponseMessage response = await _http.PostAsJsonAsync($"{BaseUrl}", vo);
        Stream contentStream = await response.Content.ReadAsStreamAsync();
        JsonSerializerOptions options = new() { PropertyNameCaseInsensitive = true };

        await HttpHelper.HandleResponseStatus(contentStream, options, response);

        #endregion

        if (contentStream.Length == 0) return null;

        AsaasPaymentLifecycleModel? entity =
            await JsonSerializer.DeserializeAsync<AsaasPaymentLifecycleModel>(contentStream, options);

        return entity;
    }

    #endregion
}