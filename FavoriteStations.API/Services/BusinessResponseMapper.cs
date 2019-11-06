using Microsoft.AspNetCore.Mvc;
using FavoriteStations.Models.Response;

namespace FavoriteStations.Services {
    public class BusinessResponseMapper : IBusinessResponseMapper {
        public IActionResult ToActionResult(BusinessOperationFailureReason failureReason) {
            switch(failureReason) {
                case Models.Response.BusinessOperationFailureReason.ResourceDoesNotExist:
                    return new NotFoundResult();
                case Models.Response.BusinessOperationFailureReason.ResourceDoesNotBelongToCurrentUser:
                    return new ForbidResult();
                default:
                    return new StatusCodeResult(500);
            }
        }
    }
}