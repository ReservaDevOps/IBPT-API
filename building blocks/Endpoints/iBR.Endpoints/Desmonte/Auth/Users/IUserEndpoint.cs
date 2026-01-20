using Ibr.Core.Dtos.User;

namespace iBR.Endpoints.Desmonte.Auth.Users;

public interface IUserEndpoint
{
    /// <summary>
    /// Busca informações públicas de um usuário do sistema através do seu email.
    /// </summary>
    /// <param name="email">Email do usuário a ser encontrado.</param>
    /// <returns>A entidade pública do usuário informado ou null caso não encontrado.</returns>
    Task<UserPublicInfoDto?> GetPublicByEmail(string email);
}