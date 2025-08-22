using ClosedXML.Excel;
using Fretefy.Test.Domain.Entities;
using Fretefy.Test.Domain.Interfaces;
using Fretefy.Test.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Fretefy.Test.Domain.Services
{
    public class RegiaoService : IRegiaoService
    {
        private readonly IRegiaoRepository _regiaoRepository;

        public RegiaoService(IRegiaoRepository cidadeRepository)
        {
            _regiaoRepository = cidadeRepository;
        }

        public async Task<Regiao> Get(Guid id)
        {
            return await _regiaoRepository.GetById(id);
        }

        public async Task<IEnumerable<Regiao>> List()
        {
            return await _regiaoRepository.List();

        }

        public async Task Add(Regiao regiao)
        {
            var nomeJaExiste = await _regiaoRepository.IsSameName(regiao.Nome);

            if (nomeJaExiste)
            { 
                return; //ideal adicionar uma mensagem de retorno "Uma região com o mesmo nome já existe, utilize outro nome"
            }

            if (!regiao.RegiaoCidades.Any())
            {
                return; // "Uma região não pode ser adicionada sem conter ao menos uma cidade"
            }

            if (regiao.RegiaoCidades.GroupBy(c => c.CidadeId).Any(g => g.Count() > 1))
            {
                return; //"Duas cidades iguais não podem ser adicionadas."
            }

            await _regiaoRepository.Add(regiao);
        }

        public async Task Update(Regiao regiao)
        {
            var nomeJaExiste = await _regiaoRepository.IsSameName(regiao.Nome, regiao.Id); //aqui valida o id do atual tbm

            if (nomeJaExiste)
            {
                throw new Exception("Uma região com o mesmo nome já existe, utilize outro nome"); 
            }

            if (!regiao.RegiaoCidades.Any())
            {
                throw new Exception("Uma região precisa conter ao menos uma cidade");
            }

            if (regiao.RegiaoCidades.GroupBy(c => c.CidadeId).Any(g => g.Count() > 1))
            {
                throw new Exception("Duas cidades iguais não podem ser adicionadas.");
            }

            await _regiaoRepository.Update(regiao);
        }

        public async Task AlterarStatus(Guid id, bool ativo)
        {
            var regiao = await _regiaoRepository.GetById(id);
            if (regiao == null)
                throw new ArgumentException("Região não encontrada.");

            regiao.Ativo = ativo;
            await _regiaoRepository.Update(regiao);
        }

        public async Task Delete(Guid id)
        {
            await _regiaoRepository.Delete(id);
        }

        public async Task<byte[]> GenerateExcelReport()
        {
            var regioes = await List();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Regiões");

                // Cabeçalho
                worksheet.Cell(1, 1).Value = "Id";
                worksheet.Cell(1, 2).Value = "Nome";
                worksheet.Cell(1, 3).Value = "Active";
                worksheet.Cell(1, 4).Value = "Quantidade de Cidades";
                worksheet.Cell(1, 5).Value = "Cidades";

                int currentRow = 2;
                foreach (var regiao in regioes)
                {
                    worksheet.Cell(currentRow, 1).Value = regiao.Id.ToString();
                    worksheet.Cell(currentRow, 2).Value = regiao.Nome;
                    worksheet.Cell(currentRow, 3).Value = regiao.Ativo ? "Sim" : "Não";

                    var qtdCidades = regiao.RegiaoCidades?.Count ?? 0;
                    worksheet.Cell(currentRow, 4).Value = qtdCidades;

                    var nomesCidades = qtdCidades > 0
                        ? string.Join(", ", regiao.RegiaoCidades.Select(c => c.Cidade.Nome))
                        : "";

                    worksheet.Cell(currentRow, 5).Value = nomesCidades;

                    currentRow++;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return stream.ToArray();
                }
            }
        }
    }
}
