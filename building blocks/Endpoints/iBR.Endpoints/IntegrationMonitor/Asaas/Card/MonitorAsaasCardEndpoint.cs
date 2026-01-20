using Ibr.Core.Dtos.Integration.Asaas.Card;
using Ibr.Core.Helpers;
using Ibr.Core.Models.Asaas.Card;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace iBR.Endpoints.IntegrationMonitor.Asaas.Card;

public class MonitorAsaasCardEndpoint : IMonitorAsaasCardEndpoint
{
    private readonly HttpClient _http;

    private readonly string _secretCode;

    public MonitorAsaasCardEndpoint(HttpClient http, IConfiguration configuration)
    {
        _http = http;
        _secretCode = configuration["SecretCode:Monitor:Asaas"] ?? throw new Exception("SecretCode para comunicação com o Integration Monitor (Asaas) não foi encontrado.");
    }

    public async Task<PaymentCardModel?> GetByNumber(string? number)
    {
        if (number is null) return null;

        _http.DefaultRequestHeaders.Clear();
        _http.DefaultRequestHeaders.Add("secretCode", _secretCode);
        _http.DefaultRequestHeaders.Add("number", number.Replace(".", "").Trim());

        # region Realizando request

        HttpResponseMessage response = await _http.GetAsync($"/integration/asaas/card/number");
        Stream contentStream = await response.Content.ReadAsStreamAsync();
        JsonSerializerOptions options = new() { PropertyNameCaseInsensitive = true };

        await HttpHelper.HandleResponseStatus(contentStream, options, response);

        #endregion

        PaymentCardModel? entities = await JsonSerializer.DeserializeAsync<PaymentCardModel>(contentStream, options);

        return entities;
    }

    public Task<PaymentCardModel> Create(CreditCardDto card)
    {
        throw new System.NotImplementedException();
    }
}