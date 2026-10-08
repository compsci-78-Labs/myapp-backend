using System.Text.Json;

namespace WebApi.Serialization;

public class LowerCaseNamingPolicy:JsonNamingPolicy
{
    public override string ConvertName(string name)
        => name.ToLowerInvariant();
}