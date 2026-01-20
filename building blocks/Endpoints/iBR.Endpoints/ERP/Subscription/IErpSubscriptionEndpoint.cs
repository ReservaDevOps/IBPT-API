using Ibr.Core.Dtos.ERP.Subscription;

namespace iBR.Endpoints.ERP.Subscription;

public interface IErpSubscriptionEndpoint
{
    /// <summary>
    /// Busca um plano através do id do Asaas.
    /// </summary>
    /// <param name="asaasId"></param>
    /// <returns></returns>
    Task<CompanyPlanDto?> GetByAsaasId(string asaasId);

    /// <summary>
    /// Faz a criação de um companyPlan através de um dto.
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task<CompanyPlanDto?> CreateByDto(CompanyPlanDto dto);

    /// <summary>
    /// Atualiza o plano do cliente através de um dto.
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task<CompanyPlanDto> UpdateByDto(CompanyPlanDto dto);
}