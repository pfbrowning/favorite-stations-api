using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FavoriteStations.Models.Dto;
using FavoriteStations.Services;

namespace FavoriteStations.Controllers {
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class StationsController : ControllerBase {
        private readonly IBusinessLayer businessLayer;
        private readonly IBusinessResponseMapper businessResponseMapper;

        public StationsController(IBusinessLayer businessLayer, IBusinessResponseMapper businessResponseMapper) {
            this.businessLayer = businessLayer;
            this.businessResponseMapper = businessResponseMapper;
        }

        [HttpGet]
        public async Task<IEnumerable<StationDto>> Get() {
            return await this.businessLayer.GetAllStations();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id) {
            var result = await this.businessLayer.GetStationAsync(id);
            return result.Match<IActionResult>(
                success => Ok(result.RightValue),
                failure => this.businessResponseMapper.ToActionResult(failure)
            );
        }

        [HttpPost]
        public async Task<StationDto> Post([FromBody] StationCreateUpdateDto create) {
            return await this.businessLayer.CreateStationAsync(create);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] StationCreateUpdateDto update) {
            var result = await this.businessLayer.UpdateStationAsync(update, id);
            return result.Match<IActionResult>(
                success => Ok(result.RightValue),
                failure => this.businessResponseMapper.ToActionResult(failure)
            );
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) {
            var result = await this.businessLayer.DeleteStationAsync(id);
            return result.Match<IActionResult>(
                success => NoContent(),
                failure => this.businessResponseMapper.ToActionResult(failure)
            );
        }
    }
}
