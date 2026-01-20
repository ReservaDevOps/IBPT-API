using Ibr.Core.Dtos.Integration.Asaas.Customer;

namespace iBR.Endpoints.Asaas.Customer;

public interface IAsaasCustomerEndpoint
{
    /// <summary>
    /// Faz a busca de um cliente no Asaas a partir do seu AsaasCustomerId.
    /// </summary>
    /// <param name="id">Id do cliente no Asaas.</param>
    /// <returns>Entidade do usuário.</returns>
    Task<AsaasCustomerDto> GetById(string id, string baseUrl, string accessToken);
}
