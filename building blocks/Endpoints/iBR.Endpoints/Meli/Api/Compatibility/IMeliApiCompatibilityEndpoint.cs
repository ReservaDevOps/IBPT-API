using Ibr.Core.Dtos.Meli.Attribute;

namespace iBR.Endpoints.Meli.Api.Compatibility;

public interface IMeliApiCompatibilityEndpoint
{
    Task<List<MeliApiTopValuesByAttributeDTO>> GetTopValuesByAttribute(string token, string attribute, MeliApiTopValuesKnownAttributesDTO knownAttributes);
}
