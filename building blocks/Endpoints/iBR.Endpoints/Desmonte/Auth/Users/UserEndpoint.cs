using Ibr.Core.Dtos.User;
using Ibr.Core.Helpers;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace iBR.Endpoints.Desmonte.Auth.Users;

public class UserEndpoint : IUserEndpoint
{
    private readonly HttpClient _http;
    private readonly string _baseUrl;

    public UserEndpoint(HttpClient httpClient, IConfiguration configuration)
    {
        _http = httpClient;
        _baseUrl = configuration["Applications:Desmonte"] + "/api/v1/erpusers/";
    }

    public async Task<UserPublicInfoDto?> GetPublicByEmail(string email)
    {
        _http.DefaultRequestHeaders.Add("email", email);
        
        string fullUrl = _baseUrl + "email";

        #region Realizando request

        HttpResponseMessage response = await _http.GetAsync(fullUrl);
        Stream contentStream = await response.Content.ReadAsStreamAsync();
        JsonSerializerOptions options = new() { PropertyNameCaseInsensitive = true };

        await HttpHelper.HandleResponseStatus(contentStream, options, response);

        #endregion

        if (contentStream.Length == 0) return null;

        UserPublicInfoDto? entity = await JsonSerializer.DeserializeAsync<UserPublicInfoDto>(contentStream, options);

        return entity;
    }
}