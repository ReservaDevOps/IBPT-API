namespace iBR.Endpoints.Meli.Api.User;

public interface IMeliApiUserEndpoint
{
    /// <summary>
    /// Busca o AccessToken do mercado livre a partir de um companyId.
    /// </summary>
    /// <param name="userToken">Token de autenticação do usuário no sistema.</param>
    /// <param name="companyId">Id da empresa no sistema.</param>
    /// <returns></returns>
    Task<string?> GetAccessTokenByCompany(string userToken, int companyId);
}
