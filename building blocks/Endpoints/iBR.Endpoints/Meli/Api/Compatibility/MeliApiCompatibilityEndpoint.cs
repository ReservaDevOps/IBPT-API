using System.Net.Http.Json;
using System.Text.Json;
using Ibr.Core.Dtos.Meli.Attribute;
using Ibr.Core.Helpers;

namespace iBR.Endpoints.Meli.Api.Compatibility;

public class MeliApiCompatibilityEndpoint : IMeliApiCompatibilityEndpoint
{
    private readonly HttpClient _http;
    private const string BaseUrl = "compatibilities";

    public MeliApiCompatibilityEndpoint(HttpClient httpClient)
    {
        _http = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public async Task<List<MeliApiTopValuesByAttributeDTO>> GetTopValuesByAttribute(
        string token, 
        string attribute, 
        MeliApiTopValuesKnownAttributesDTO knownAttributes)
    {
        if (_http.DefaultRequestHeaders.Contains("Authorization")) _http.DefaultRequestHeaders.Remove("Authorization");
        _http.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

        # region Realizando request

        HttpResponseMessage response = await _http.PostAsJsonAsync($"{BaseUrl}/TopValuesByAttribute?attribute={attribute}", knownAttributes);

        if (response == null || response.Content == null)
        {
            throw new Exception("Resposta da API foi nula.");
        }

        Stream contentStream = await response.Content.ReadAsStreamAsync();
        JsonSerializerOptions options = new() { PropertyNameCaseInsensitive = true };

        string content = await response.Content.ReadAsStringAsync();

        // Verificação explícita de corpo vazio
        if (string.IsNullOrWhiteSpace(content))
        {
            return null;
        }

        await HttpHelper.HandleResponseStatus(contentStream, options, response);

        #endregion

        if (contentStream.Length == 0)
        {
            throw new Exception("Ocorreu um erro na busca do seu token do mercado livre.");
        }

        try
        {
            List<MeliApiTopValuesByAttributeDTO>? result = await JsonSerializer.DeserializeAsync<List<MeliApiTopValuesByAttributeDTO>>(contentStream, options);
            
            return result ?? throw new Exception("Ocorreu um erro na reversão do retorno.");
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }


    }
}
