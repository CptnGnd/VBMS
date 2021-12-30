
using VBMS.Application.Interfaces.Serialization.Settings;
using Newtonsoft.Json;

namespace VBMS.Application.Serialization.Settings
{
    public class NewtonsoftJsonSettings : IJsonSerializerSettings
    {
        public JsonSerializerSettings JsonSerializerSettings { get; } = new();
    }
}