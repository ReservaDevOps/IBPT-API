using Ibr.Core.Dtos.Integration.Asaas.Customer;
using Ibr.Core.Models.Asaas.Customer;

namespace iBR.Endpoints.IntegrationMonitor.Asaas.Customer;

/// <summary>
/// Centraliza as requests para a API de monitoração Asaas para os clientes cadastrados.
/// </summary>
public interface IMonitorCustomerEndpoint
{
    /// <summary>
    /// Busca um cliente através do seu AsaasCustomerId.
    /// </summary>
    /// <param name="asaasCustomerId"></param>
    /// <returns></returns>
    Task<AsaasCustomerDto?> GetByAsaasId(string asaasCustomerId, string asaasAccountKey);

    /// <summary>
    /// Faz a criação de um cliente Asaas.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<AsaasCustomerDto?> Create(AsaasCustomerModel model);
}
