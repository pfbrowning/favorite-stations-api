using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;

namespace FavoriteStations.Middlewares {
    public static class Middlewares {
        public static Task WriteStatusCodePage(this HttpContext context, int? statusCode = null) {
            context.Response.ContentType = "application/json";

            if(!statusCode.HasValue) {
                statusCode = context.Response.StatusCode;
            }
            else {
                context.Response.StatusCode = statusCode.Value;
            }

            string statusCodeText = ReasonPhrases.GetReasonPhrase(statusCode.Value);
            var pd = new ProblemDetails() {
                Status = statusCode.Value,
                Title = statusCodeText
            };

            var ignoreNullValues = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            string serialized = JsonConvert.SerializeObject(pd, Formatting.None, ignoreNullValues);
            
            return context.Response.WriteAsync(serialized);
        }
    }
}