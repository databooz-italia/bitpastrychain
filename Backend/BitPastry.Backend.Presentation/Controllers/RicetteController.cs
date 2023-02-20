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
using BitPastry.Backend.DTO.HTTP_Messages.Response.Ricette;
using BitPastry.Backend.DTO.HTTP_Messages.Request;
using Microsoft.AspNetCore.Components;

namespace BitPastry.Backend.Presentation.Controllers {

    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]
    public class RicetteController : ControllerBase {
        private readonly ILogger<AuthController> _logger;
        private readonly BaseService _uow;
        private readonly TokensProvider _tokensProvider;

        public RicetteController(
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
        public IEnumerable<GetRicettaResponse> Get(
            [FromQuery(Name = "id")] int? id,
            [FromQuery(Name = "onlySemiLavorati")] bool isOnlySemiLavorati
        ) {
            _logger.LogInformation($"Requesting ricette: <ID := {(id == null ? "All" : id)} - Only Semi Lavorati := {isOnlySemiLavorati}>");
            IEnumerable<GetRicettaResponse> response = Enumerable.Empty<GetRicettaResponse>();

            using (_uow)
            {
                var service = new RicetteService(_uow);

                response = service
                    .Get(id, isOnlySemiLavorati)
                    .Select(RicettaMapper.Map)
                    .ToList();

                _uow.Complete();
            }

            return response;
        }

        /* ------------------------------------------------------------------------------------- */
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Create([FromBody] CreateRicettaRequest request) 
        {
            _logger.LogInformation($"Create ricetta");
            int id;

            using (_uow)
            {
                var service = new RicetteService(_uow);
                id = service.CreateOrUpdate(
                    request.ID,
                    request.Nome,
                    request.Autore,
                    request.Descrizione,
                    request.Semilavorato,
                    request.Unità,
                    request.Bops
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
            // METTERE TUTTO IN CASCATE IN AUTOMATICO SUL DB
            _logger.LogInformation($"Delete ricetta: ID {id}");

            using (_uow)
            {
                var service = new RicetteService(_uow);
                service.Delete(id);

                _uow.Complete();
            }

            return Ok();
        }
    }
}
