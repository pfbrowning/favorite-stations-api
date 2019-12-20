using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
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
            var problemDetails = new ProblemDetails() {
                Status = statusCode.Value,
                Title = statusCodeText
            };

            var ignoreNullValues = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            string serialized = JsonConvert.SerializeObject(problemDetails, Formatting.None, ignoreNullValues);

            return context.Response.WriteAsync(serialized);
        }

        public static void UseExceptionHandler(this IApplicationBuilder app, bool Development) {
            app.UseExceptionHandler(errorApp => {
                errorApp.Run(async context => {
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "application/json";

                    var error = context.Features.Get<IExceptionHandlerPathFeature>().Error;

                    var problemDetails = new ProblemDetails() {
                        Status = 500,
                        Title = Development ? error.GetType().ToString() : "Internal Server Error",
                        Detail = Development ? error.Message : null,
                    };
                    
                    var ignoreNullValues = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
                    string serialized = JsonConvert.SerializeObject(problemDetails, Formatting.None, ignoreNullValues);

                    await context.Response.WriteAsync(serialized);
                });
            });
        }
    }
}