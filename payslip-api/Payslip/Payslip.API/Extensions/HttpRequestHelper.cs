namespace Payslip.API.Extensions
{
    public static class HttpRequestHelper
    {
        public static string GetAccessToken(this HttpRequest httpRequest) => httpRequest.Headers.ContainsKey("Authorization")
                                                                        ? httpRequest.Headers["Authorization"].ToString().Split(" ")[1]
                                                                        : string.Empty;
        public static string ReadBodyAsString(this HttpRequest httpRequest)
        {
            if (httpRequest.Body is null)
                return null;

            httpRequest.Body.Position = 0;
            var streamReader = new StreamReader(httpRequest.Body);
            var body = streamReader.ReadToEndAsync().Result;
            return body;
        }

        public static string GetFullUrl(this HttpRequest httpRequest)
        {
            var url = $"{httpRequest.Scheme}://{httpRequest.Host}{httpRequest.Path}";
            return url;
        }
    }
}
