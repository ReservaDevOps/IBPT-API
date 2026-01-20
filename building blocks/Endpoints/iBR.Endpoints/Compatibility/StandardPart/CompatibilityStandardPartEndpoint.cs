using System.Text.Json;
using Ibr.Core.Dtos.Compatibility.StandardPart;

namespace iBR.Endpoints.Compatibility.StandardPart;

public class CompatibilityStandardPartEndpoint : ICompatibilityStandardPartEndpoint
{
    private readonly HttpClient _client;
    private const string _baseUrl = "api/v1/StandardPart";

    public CompatibilityStandardPartEndpoint(HttpClient httpClient)
    {
        _client = httpClient;
    }

    public async Task<CompatibilityStandardPartDTO?> GetById(int id)
    {
        HttpResponseMessage response = await _client.GetAsync($"{_baseUrl}/{id}");

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

        return await JsonSerializer.DeserializeAsync<CompatibilityStandardPartDTO?>(contentStream, options);
    }
}
