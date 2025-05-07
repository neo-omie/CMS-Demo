using CMS.Application.Features.MasterCompanyCascade.Queries.GetCities;
using CMS.Application.Features.MasterCompanyCascade.Queries.GetCityById;
using CMS.Application.Features.MasterCompanyCascade.Queries.GetCountries;
using CMS.Application.Features.MasterCompanyCascade.Queries.GetCountryById;
using CMS.Application.Features.MasterCompanyCascade.Queries.GetStateById;
using CMS.Application.Features.MasterCompanyCascade.Queries.GetStates;
using CMS.Domain.Entities.CompanyMaster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MasterCompanyCascadeController : ControllerBase
    {
        readonly IMediator _mediator;

        public MasterCompanyCascadeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetCountries")]
        public async Task<IActionResult> GetCountries()
        {
            var countries = await _mediator.Send(new GetCountriesQuery());
            return Ok(countries);
        }

        [HttpGet("GetStates")]
        public async Task<IActionResult> GetStates(int countryId)
        {
            var states = await _mediator.Send(new GetStatesQuery(countryId));
            return Ok(states);
        }

        [HttpGet("GetCities")]
        public async Task<IActionResult> GetCities(int stateId)
        {
            var cities = await _mediator.Send(new GetCitiesQuery(stateId));
            return Ok(cities);
        }
        [HttpGet("GetCountryById")]
        public async Task<IActionResult> GetCountryById(int id)
        {
            var country = await _mediator.Send(new GetCountryByIdQuery(id));
            return Ok(country);
        }
        [HttpGet("GetStateById")]
        public async Task<IActionResult> GetStateById(int id)
        {
            var state = await _mediator.Send(new GetStateByIdQuery(id));
            return Ok(state);
        }
        [HttpGet("GetCityById")]
        public async Task<IActionResult> GetCityById(int id)
        {
            var city = await _mediator.Send(new GetCityByIdQuery(id));
            return Ok(city);
        }
    }
}
