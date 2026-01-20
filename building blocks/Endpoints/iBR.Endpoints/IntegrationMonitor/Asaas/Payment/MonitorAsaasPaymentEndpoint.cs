using Ibr.Core.Dtos.Integration.Asaas.Payment;
using Ibr.Core.Dtos.Utils;
using Ibr.Core.Helpers;
using Ibr.Core.Models.Asaas.Payment;
using Ibr.Core.ValueObjects.Asaas.Payment;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using System.Text.Json;

namespace iBR.Endpoints.IntegrationMonitor.Asaas.Payment;

public class MonitorAsaasPaymentEndpoint : IMonitorAsaasPaymentEndpoint
{
    private readonly HttpClient _http;
    private const string BaseUrl = "api/v1/monitor/integration/asaas/payment";

    private readonly string _secretCode;

    public MonitorAsaasPaymentEndpoint(HttpClient http, IConfiguration configuration)
    {
        _http = http;
        _secretCode = configuration["SecretCode:Monitor:Asaas"]
            ?? throw new Exception("SecretCode para comunicação com o Integration Monitor (Asaas) não foi encontrado.");
    }

    public async Task<AsaasPaymentModel?> GetByAsaasId(string asaasPaymentId, string asaasAccountKey)
    {
        _http.DefaultRequestHeaders.Clear();
        _http.DefaultRequestHeaders.Add("secretCode", _secretCode);
        _http.DefaultRequestHeaders.Add("asaasPaymentId", asaasPaymentId);
        _http.DefaultRequestHeaders.Add("asaasAccountKey", asaasAccountKey);

        # region Realizando request

        HttpResponseMessage response = await _http.GetAsync($"{BaseUrl}/asaas");
        Stream contentStream = await response.Content.ReadAsStreamAsync();
        JsonSerializerOptions options = new() { PropertyNameCaseInsensitive = true };

        await HttpHelper.HandleResponseStatus(contentStream, options, response);

        #endregion

        if (contentStream.Length == 0)
        {
            return null;
        }

        AsaasPaymentModel? entity = await JsonSerializer.DeserializeAsync<AsaasPaymentModel>(contentStream, options);

        return entity;
    }

    public async Task<List<AsaasPaymentDto>> GetPendingBySubscription(RequestParamsDTO rp, string? asaasSubscriptionId, Guid? subscriptionId, string asaasAccountKey)
    {
        _http.DefaultRequestHeaders.Clear();
        _http.DefaultRequestHeaders.Add("asaasSubscriptionId", asaasSubscriptionId);
        _http.DefaultRequestHeaders.Add("subscriptionId", subscriptionId?.ToString());
        _http.DefaultRequestHeaders.Add("pageNumber", rp.Pagination.PageNumber.ToString());
        _http.DefaultRequestHeaders.Add("pageSize", rp.Pagination.PageSize.ToString());
        _http.DefaultRequestHeaders.Add("secretCode", _secretCode);
        _http.DefaultRequestHeaders.Add("asaasAccountKey", asaasAccountKey);

        # region Realizando request

        HttpResponseMessage response = await _http.GetAsync($"{BaseUrl}/status/pending");
        Stream contentStream = await response.Content.ReadAsStreamAsync();
        JsonSerializerOptions options = new() { PropertyNameCaseInsensitive = true };

        await HttpHelper.HandleResponseStatus(contentStream, options, response);

        #endregion

        if (contentStream.Length == 0)
        {
            return new List<AsaasPaymentDto>();
        }

        List<AsaasPaymentDto> entity = await JsonSerializer.DeserializeAsync<List<AsaasPaymentDto>>(contentStream, options)
            ?? throw new Exception("Ocorreu um erro ao tentar mapear o retorno da requisição.");

        return entity;
    }

    public async Task<AsaasPaymentModel?> Create(CreateAsaasPaymentVO vo, string customerEmail)
    {
        _http.DefaultRequestHeaders.Clear();
        _http.DefaultRequestHeaders.Add("customerEmail", customerEmail);
        _http.DefaultRequestHeaders.Add("secretCode", _secretCode);

        # region Realizando request

        HttpResponseMessage response = await _http.PostAsJsonAsync($"{BaseUrl}", vo);
        Stream contentStream = await response.Content.ReadAsStreamAsync();
        JsonSerializerOptions options = new() { PropertyNameCaseInsensitive = true };

        await HttpHelper.HandleResponseStatus(contentStream, options, response);

        #endregion

        if (contentStream.Length == 0)
        {
            return null;
        }

        AsaasPaymentModel? entity = await JsonSerializer.DeserializeAsync<AsaasPaymentModel>(contentStream, options);

        return entity;
    }
}
