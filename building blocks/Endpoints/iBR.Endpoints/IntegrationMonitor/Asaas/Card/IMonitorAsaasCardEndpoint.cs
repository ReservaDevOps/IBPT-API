using Ibr.Core.Dtos.Integration.Asaas.Card;
using Ibr.Core.Models.Asaas.Card;

namespace iBR.Endpoints.IntegrationMonitor.Asaas.Card;

public interface IMonitorAsaasCardEndpoint
{
    /// <summary>
    /// Busca um cartão pelo seu id.
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    Task<PaymentCardModel?> GetByNumber(string? number);

    /// <summary>
    /// Cria um novo cartão.
    /// </summary>
    /// <param name="card">Informações do cartão a serem registrados.</param>
    /// <returns></returns>
    Task<PaymentCardModel> Create(CreditCardDto card);
}