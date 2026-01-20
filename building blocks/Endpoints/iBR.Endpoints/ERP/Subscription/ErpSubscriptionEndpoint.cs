using Ibr.Core.Dtos.ERP.Subscription;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using System.Text.Json;

namespace iBR.Endpoints.ERP.Subscription;

public class ErpSubscriptionEndpoint : IErpSubscriptionEndpoint
{
    private readonly HttpClient _client;
    private const string _baseUrl = "subscriptions";
    private readonly string _secretKey;

    public ErpSubscriptionEndpoint(HttpClient client, IConfiguration configuration)
    {
        _client = client;
        _secretKey = configuration["SecretCode:Endpoint"];
    }

    public async Task<CompanyPlanDto?> GetByAsaasId(string asaasId)
    {
        AddSecretKey();

        HttpResponseMessage response = await _client.GetAsync($"{_baseUrl}/asaas/id/{asaasId}");

        Stream contentStream = await response.Content.ReadAsStreamAsync();
        JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        if (!response.IsSuccessStatusCode)
        {
            string? responseModel = await JsonSerializer.DeserializeAsync<string>(contentStream, options);

            throw new Exception(responseModel);
        }

        if (contentStream == null || contentStream.Length == 0)
        {
            return null;
        }

        return await JsonSerializer.DeserializeAsync<CompanyPlanDto?>(contentStream, options);
    }

    public async Task<CompanyPlanDto?> CreateByDto(CompanyPlanDto dto)
    {
        AddSecretKey();

        HttpResponseMessage response = await _client.PostAsJsonAsync($"{_baseUrl}", dto);

        Stream contentStream = await response.Content.ReadAsStreamAsync();
        JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        if (!response.IsSuccessStatusCode)
        {
            string? responseModel = await JsonSerializer.DeserializeAsync<string>(contentStream, options);

            throw new Exception(responseModel);
        }

        if (contentStream == null || contentStream.Length == 0)
        {
            return null;
        }

        return await JsonSerializer.DeserializeAsync<CompanyPlanDto?>(contentStream, options);
    }

    public async Task<CompanyPlanDto?> UpdateByDto(CompanyPlanDto dto)
    {
        AddSecretKey();

        HttpResponseMessage response = await _client.PutAsJsonAsync($"{_baseUrl}", dto);

        Stream contentStream = await response.Content.ReadAsStreamAsync();
        JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        if (!response.IsSuccessStatusCode)
        {
            string? responseModel = await JsonSerializer.DeserializeAsync<string>(contentStream, options);

            throw new Exception(responseModel);
        }

        if (contentStream == null || contentStream.Length == 0)
        {
            return null;
        }

        return await JsonSerializer.DeserializeAsync<CompanyPlanDto?>(contentStream, options);
    }


    private void AddSecretKey()
    {
        if (_client.DefaultRequestHeaders.Contains("SecretKey"))
        {
            _client.DefaultRequestHeaders.Remove("SecretKey");
        }

        _client.DefaultRequestHeaders.Add("SecretKey", _secretKey);
    }
}
