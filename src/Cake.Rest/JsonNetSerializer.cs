using Newtonsoft.Json;
using RestSharp;
using RestSharp.Serialization;


namespace Cake.Rest
{
    /// <summary>
    /// Newtonsoft.Json wrapper for RestSharp
    /// </summary>
    public class JsonNetSerializer : IRestSerializer
    {
        /// <inheritdoc />
        public string Serialize(object obj) => 
            JsonConvert.SerializeObject(obj);

        /// <inheritdoc />
        public T Deserialize<T>(IRestResponse response) => 
            JsonConvert.DeserializeObject<T>(response.Content);

        /// <inheritdoc />
        public string Serialize(Parameter parameter) =>
            JsonConvert.SerializeObject(parameter.Value);

        /// <inheritdoc />
        public string[] SupportedContentTypes { get; } =
        {
            "application/json", "text/json", "text/x-json", "text/javascript", "*+json"
        };

        /// <inheritdoc />
        public string ContentType { get; set; } = "application/json";

        /// <inheritdoc />
        public DataFormat DataFormat { get; } = DataFormat.Json;
    }

}