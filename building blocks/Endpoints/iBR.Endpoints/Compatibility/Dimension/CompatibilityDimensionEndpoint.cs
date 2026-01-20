using Ibr.Core.Dtos.Compatibility.Dimension;
using Ibr.Core.ValueObjects.Compatibility.Dimension;
using System.Net.Http.Json;
using System.Text.Json;

namespace iBR.Endpoints.Compatibility.Dimension;

public class CompatibilityDimensionEndpoint : ICompatibilityDimensionEndpoint
{
    private const string _url = "dimension";
    private readonly HttpClient _client;

    public CompatibilityDimensionEndpoint(HttpClient client)
    {
        _client = client;
    }

    public async Task<CompatibilityDimensionDto> Create(CreateDimensionVO valueObject)
    {
        HttpResponseMessage response = await _client.PostAsJsonAsync("", valueObject);

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

        CompatibilityDimensionDto? entity = await JsonSerializer.DeserializeAsync<CompatibilityDimensionDto>(contentStream, options);

        if(entity is null)
        {
            throw new Exception("Ocorreu um erro na conversão do retorno.");
        }

        return entity;
    }

    public async Task<CompatibilityDimensionDto> CreateIfDoesntExist(CreateDimensionVO valueObject)
    {
        HttpResponseMessage response = await _client.PostAsJsonAsync($"{_url}/exist", valueObject);

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

        CompatibilityDimensionDto? entity = await JsonSerializer.DeserializeAsync<CompatibilityDimensionDto>(contentStream, options);

        if(entity is null)
        {
            throw new Exception("Ocorreu um erro na conversão do retorno.");
        }

        return entity;
    }
}
