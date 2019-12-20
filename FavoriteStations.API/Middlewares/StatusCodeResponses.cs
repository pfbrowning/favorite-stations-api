using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;

namespace FavoriteStations.Middlewares {
    public static class StatusCodeResponses {
        public static Task WriteStatusCodeResponse(this HttpContext context, int? statusCode = null) {
            context.Response.ContentType = "application/json";

            if(!statusCode.HasValue) {
                statusCode = context.Response.StatusCode;
            }
            else {
                context.Response.StatusCode = statusCode.Value;
            }

            string statusCodeText = ReasonPhrases.GetReasonPhrase(statusCode.Value);
            var problemDetails = new ProblemDetails() {
                Status = statusCode.Value,
                Title = statusCodeText
            };

            var ignoreNullValues = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            string serialized = JsonConvert.SerializeObject(problemDetails, Formatting.None, ignoreNullValues);

            return context.Response.WriteAsync(serialized);
        }

        public static void UseStatusCodeResponses(this IApplicationBuilder app) {
            app.UseStatusCodePages(context => context.HttpContext.WriteStatusCodeResponse());
        }
    }
}