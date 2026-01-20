using Ibr.Core.Dtos.Compatibility.StandardPart;

namespace iBR.Endpoints.Compatibility.StandardPart;

public interface ICompatibilityStandardPartEndpoint
{
    /// <summary>
    /// Faz uma requisição para o endpoint da busca de uma peça padrão através do seu id.
    /// </summary>
    /// <param name="id">O id da peça padrão.</param>
    /// <returns></returns>
    Task<CompatibilityStandardPartDTO?> GetById(int id);
}
