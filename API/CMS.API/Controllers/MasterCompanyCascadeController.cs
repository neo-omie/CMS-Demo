using CMS.Application.Features.MasterCompanyCascade.Queries.GetCities;
using CMS.Application.Features.MasterCompanyCascade.Queries.GetCountries;
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

        public JsonResult GetCountries()
        {
            var countries = _mediator.Send(new GetCountriesQuery());
            return new JsonResult(countries);
        }
        public JsonResult GetStates(int countryId)
        {
            var states = _mediator.Send(new GetStatesQuery(countryId));
            return new JsonResult(states);
        }
        public JsonResult GetCities(int stateId)
        {
            var cities = _mediator.Send(new GetCitiesQuery(stateId));
            return new JsonResult(cities);
        }
    }
}
