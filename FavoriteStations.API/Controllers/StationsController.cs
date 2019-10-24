using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FavoriteStations.Models;
using FavoriteStations.Models.Dto;
using FavoriteStations.Services;

namespace FavoriteStations.Controllers {
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class StationsController : ControllerBase {
        private readonly IBusinessLayer businessLayer;
        private readonly User user;
        public StationsController(IBusinessLayer businessLayer, User user) {
            this.businessLayer = businessLayer;
            this.user = user;
        }

        [HttpGet]
        public async Task<IEnumerable<StationDto>> Get() {
            return await this.businessLayer.GetAllStations();
        }
    }
}
