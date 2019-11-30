using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FavoriteStations.Filters {
    public class ValidateModelAttribute : ActionFilterAttribute {
        public override void OnActionExecuting(ActionExecutingContext context) {
            if (!context.ModelState.IsValid) {
                context.Result = new BadRequestObjectResult(new ValidationProblemDetails(context.ModelState));
            }
        }
    }
}
