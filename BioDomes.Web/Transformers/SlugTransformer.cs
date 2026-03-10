using BioDomes.Domains.Extensions;

namespace BioDomes.Web.Transformers;

public class SlugTransformer : IOutboundParameterTransformer
{
    public string? TransformOutbound(object? value)
    {
        if (value == null) return null;

        return value.ToString().ToKebabCase();
    }
}