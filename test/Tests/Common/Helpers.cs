using System.Text.Json;
using System.Text.Json.Serialization;
using WebApi.Serialization;

namespace Tests.Common;

public class Helpers
{
    public static JsonSerializerOptions GetJsonOption()
    {
        var jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web)
        {
            Converters =
            {
                new JsonStringEnumConverter(new LowerCaseNamingPolicy())
            }
        };
        return jsonOptions;
    }
}