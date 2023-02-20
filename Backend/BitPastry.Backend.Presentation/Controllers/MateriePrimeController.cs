using BitPastry.Backend.Core.Services;
using BitPastry.Backend.Presentation.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BitPastry.Backend.DTO.HTTP_Messages.Response;
using BitPastry.Backend.Data;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using BitPastry.Backend.DTO.Models;
using BitPastry.Backend.DTO.Mappers;
using BitPastry.Backend.DTO.HTTP_Messages.Request.Operatore;
using BitPastry.Backend.DTO.HTTP_Messages.Request.Ordine;

using BitPastry.Backend.DTO.HTTP_Messages.Request;

namespace BitPastry.Backend.Presentation.Controllers {

    [Route("api/materie-prime")]
    [ApiController]
    public class MateriePrimeController : ControllerBase {
        private readonly ILogger<AuthController> _logger;
        private readonly BaseService _uow;
        private readonly TokensProvider _tokensProvider;

        public MateriePrimeController(
            ILogger<AuthController> logger,
            BaseService uow,
            TokensProvider tokenProvider
        ) {
            this._logger = logger;
            this._uow = uow;
            this._tokensProvider = tokenProvider;
        }

        /* ------------------------------------------------------------------------------------- */
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IEnumerable<GetMateriaPrimaResponse> Get() 
        {
            _logger.LogInformation("Requesting materie prime");
            IEnumerable<GetMateriaPrimaResponse> response = Enumerable.Empty<GetMateriaPrimaResponse>();

            using (_uow)
            {
                var service = new MateriePrimeService(_uow);

                response = service
                    .Get()
                    .Select(MateriaPrimaMapper.Map)
                    .ToList();

                _uow.Complete();
            }

            return response;
        }

        /* ------------------------------------------------------------------------------------- */
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Create([FromBody] CreateMateriaPrimaRequest request) 
        {
            _logger.LogInformation($"Create materia prima");
            int id;

            using (_uow)
            {
                var service = new MateriePrimeService(_uow);
                id = service.Create(
                    request.Nome,
                    request.UnitàMisura
                );

                _uow.Complete();
            }

            return Ok(id);
        }

        /* ------------------------------------------------------------------------------------- */
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Delete(int id) 
        {
            _logger.LogInformation($"Delete materia prima: ID {id}");

            using (_uow)
            {
                var service = new MateriePrimeService(_uow);
                service.Delete(id);

                _uow.Complete();
            }

            return Ok();
        }

        /* ------------------------------------------------------------------------------------- */
        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Update(int id, [FromBody] CreateMateriaPrimaRequest request) 
        {
            _logger.LogInformation($"Update materia prima: ID {id}");

            using (_uow)
            {
                var service = new MateriePrimeService(_uow);
                service.Update(
                    id,
                    request.Nome,
                    request.UnitàMisura
                );

                _uow.Complete();
            }

            return Ok();
        }
    }
}
