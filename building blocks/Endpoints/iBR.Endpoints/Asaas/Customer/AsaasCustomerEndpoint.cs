using Ibr.Core.Dtos.Integration.Asaas.Customer;
using Ibr.Core.Helpers;
using System;
using System.Text.Json;

namespace iBR.Endpoints.Asaas.Customer;

public class AsaasAsaasCustomerEndpoint : IAsaasCustomerEndpoint
{
    private readonly HttpClient _http;

    public AsaasAsaasCustomerEndpoint(HttpClient client)
    {
        _http = client;
    }

    public async Task<AsaasCustomerDto> GetById(string id, string baseUrl, string accessToken)
    {
        _http.DefaultRequestHeaders.Clear();
        _http.BaseAddress = new Uri(baseUrl);
        _http.DefaultRequestHeaders.Add("access_token", accessToken);
        _http.DefaultRequestHeaders.UserAgent.ParseAdd("Desmonte/1.0");

        # region Realizando request 

        HttpResponseMessage response = await _http.GetAsync($"customers/{id}");

        Stream contentStream = await response.Content.ReadAsStreamAsync();
        JsonSerializerOptions options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        await HttpHelper.HandleResponseStatus(contentStream, options, response);

        #endregion

        AsaasCustomerDto? entities =
            await JsonSerializer.DeserializeAsync<AsaasCustomerDto>(contentStream, options);

        if (entities is null)
        {
            throw new Exception("A conversão da entidade resultou null.");
        }

        return entities;
    }
}
