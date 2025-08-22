using Fretefy.Test.Domain.Entities;
using Fretefy.Test.Domain.Interfaces;
using Fretefy.Test.WebApi.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fretefy.Test.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegiaoController : ControllerBase
    {
        private readonly IRegiaoService _regiaoService;

        public RegiaoController(IRegiaoService regiaoService)
        {
            _regiaoService = regiaoService;
        }

        //O ideal em projetos maiores é utilizar DTO para não expor a entidade de domínio a camada de web api como uma RegiaoDto por exemplo. 
        //Eu optei por não fazer aqui pois não havia necessidade e devido ao pouco tempo.

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Regiao>>> List()
        {
            try
            {
                var regioes = await _regiaoService.List();

                if (regioes == null || !regioes.Any())
                    return NotFound("Nenhuma região encontrada para exportação.");

                return Ok(regioes);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao exportar: {ex.Message}");
            }
        }

        [HttpGet("{id:guid}")] 
        public async Task<ActionResult<Regiao>> Get(Guid id)
        {
            try
            {
                var regiao = await _regiaoService.Get(id);

                if (regiao == null)
                    return NotFound("Região não encontrada.");

                return Ok(regiao);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao exportar: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Regiao regiao)
        {
            try
            {
                if (regiao == null)
                    return BadRequest("Dados inválidos.");

                await _regiaoService.Add(regiao);
                return CreatedAtAction(nameof(Get), new { id = regiao.Id }, regiao);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Regiao regiao)
        {
            try
            {
                if (regiao == null || regiao.Id != id)
                    return BadRequest("ID inconsistente ou dados inválidos.");

                await _regiaoService.Update(regiao);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{id:guid}/status")]
        public async Task<IActionResult> AlterarStatus(Guid id, [FromBody] AlterarStatusDto dto)
        {
            try
            {
                if (dto == null || dto.Id != id)
                    return BadRequest("ID inconsistente ou dados inválidos.");

                await _regiaoService.AlterarStatus(id, dto.Ativo);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _regiaoService.Delete(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("exportar")]
        public async Task<IActionResult> ExportToExcel()
        {
            try
            {
                var fileContent = await _regiaoService.GenerateExcelReport();

                return File(fileContent,
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            "regioes.xlsx");
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao exportar: {ex.Message}");
            }
        }
    }
}
