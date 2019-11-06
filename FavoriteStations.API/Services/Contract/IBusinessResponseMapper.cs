using Microsoft.AspNetCore.Mvc;
using FavoriteStations.Models.Response;

namespace FavoriteStations.Services {
    public interface IBusinessResponseMapper {
        IActionResult ToActionResult(BusinessOperationFailureReason failureReason);   
    }
}