using ClosedXML.Excel;
using Fretefy.Test.Domain.Entities;
using Fretefy.Test.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
            var regioes = await _regiaoService.List();
            return Ok(regioes);
        }

        [HttpGet("{id:guid}")] 
        public async Task<ActionResult<Regiao>> Get(Guid id)
        {
            var regiao = await _regiaoService.Get(id);
            if (regiao == null)
                return NotFound("Região não encontrada.");

            return Ok(regiao);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Regiao regiao)
        {
            if (regiao == null)
                return BadRequest("Dados inválidos.");

            await _regiaoService.Add(regiao);
            return CreatedAtAction(nameof(Get), new { id = regiao.Id }, regiao);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Regiao regiao)
        {
            if (regiao == null || regiao.Id != id)
                return BadRequest("ID inconsistente ou dados inválidos.");

            await _regiaoService.Update(regiao);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _regiaoService.Delete(id);
            return NoContent();
        }

        [HttpGet("export")]
        public async Task<IActionResult> ExportToExcel()
        {
            var fileContent = await _regiaoService.GenerateExcelReport();

            return File(fileContent,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "regioes.xlsx");
        }

    }
}
