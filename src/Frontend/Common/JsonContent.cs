namespace Isitar.TimeTracking.Frontend.Common
{
    using System.Net.Http;
    using System.Text;
    using System.Text.Json;

    public class JsonContent : StringContent
    {
        private const string MediaType = "application/json";
        public JsonContent(string content) : base(content, Encoding.UTF8, MediaType) { }
        public JsonContent(string content, Encoding encoding) : base(content, encoding, MediaType) { }
        public JsonContent(string content, Encoding encoding, string mediaType) : base(content, encoding, mediaType) { }

        public JsonContent(object obj) : this(JsonSerializer.Serialize(obj)) { }
    }
}