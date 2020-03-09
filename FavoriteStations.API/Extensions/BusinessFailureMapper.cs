using Microsoft.AspNetCore.Mvc;
using FavoriteStations.Models.Response;

namespace FavoriteStations.Extensions {
    public static class BusinessFailureMapper {
        public static IActionResult ToActionResult(this BusinessOperationFailureReason failureReason) {
            switch(failureReason) {
                case Models.Response.BusinessOperationFailureReason.ResourceDoesNotExist:
                    return new NotFoundObjectResult(new ProblemDetails() {
                        Status = 404,
                        Title = failureReason.ToString()
                    });
                case Models.Response.BusinessOperationFailureReason.ResourceDoesNotBelongToCurrentUser:
                    return new ObjectResult(new ProblemDetails() {
                        Status = 403,
                        Title = failureReason.ToString()
                    }) {
                        StatusCode = 403
                    };
                default:
                    return new ObjectResult(new ProblemDetails() {
                        Status = 500,
                        Title = "Unhandled Business Logic Failure"
                    }) {
                        StatusCode = 500
                    };
            }
        }
    }
}