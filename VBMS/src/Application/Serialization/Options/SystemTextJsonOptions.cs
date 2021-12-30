using System.Text.Json;
using VBMS.Application.Interfaces.Serialization.Options;

namespace VBMS.Application.Serialization.Options
{
    public class SystemTextJsonOptions : IJsonSerializerOptions
    {
        public JsonSerializerOptions JsonSerializerOptions { get; } = new();
    }
}