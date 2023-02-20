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

namespace BitPastry.Backend.Presentation.Controllers {

    [Route("api/[controller]")]
    [ApiController]
    public class OperatoriController : ControllerBase {
        private readonly ILogger<AuthController> _logger;
        private readonly BaseService _uow;
        private readonly TokensProvider _tokensProvider;

        public OperatoriController(
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
        public IEnumerable<GetOperatoreResponse> Get() 
        {
            _logger.LogInformation("Requesting operatori");
            IEnumerable<GetOperatoreResponse> response = Enumerable.Empty<GetOperatoreResponse>();

            using (_uow)
            {
                var service = new OperatoriService(_uow);

                response = service
                    .Get()
                    .Select(OperatoreMapper.Map)
                    .ToList();

                _uow.Complete();
            }

            return response;
        }

        /* ------------------------------------------------------------------------------------- */
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Create([FromBody] CreateOperatoreRequest request) 
        {
            _logger.LogInformation($"Create operatore: Matricola {request.Matricola}");
            int id;

            using (_uow)
            {
                var service = new OperatoriService(_uow);
                id = service.Create(
                    request.Matricola,
                    request.Nome,
                    request.Cognome,
                    request.Contatto,
                    request.Livello
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
            _logger.LogInformation($"Delete operatore: ID {id}");



            using (_uow)
            {
                var service = new OperatoriService(_uow);
                service.Delete(id);

                _uow.Complete();
            }

            return Ok();
        }

        /* ------------------------------------------------------------------------------------- */
        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Update(int id, [FromBody] CreateOperatoreRequest request) 
        {
            _logger.LogInformation($"Update ricetta: ID {id}");

            using (_uow)
            {
                var service = new OperatoriService(_uow);
                service.Update(
                    id,
                    request.Matricola,
                    request.Nome,
                    request.Cognome,
                    request.Contatto,
                    request.Livello
                );

                _uow.Complete();
            }

            return Ok();
        }
    }
}
