namespace Isitar.TimeTracking.Frontend.Common
{
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading.Tasks;
    using global::Common.Resources;

    public static class HttpResponseMessageExtensions
    {
        public static async Task<string[]> ErrorMessagesAsync(this HttpResponseMessage httpResponseMessage)
        {
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                return new string[0];
            }

            var responseString = await httpResponseMessage.Content.ReadAsStringAsync();
            try
            {
                return JsonSerializer.Deserialize<string[]>(responseString);
            }
            catch
            {
                return new[] {Translation.UnspecifiedError};
            }
        }
    }
}