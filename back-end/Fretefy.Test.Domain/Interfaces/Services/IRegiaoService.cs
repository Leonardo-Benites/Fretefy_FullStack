using Fretefy.Test.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fretefy.Test.Domain.Interfaces
{
    public interface IRegiaoService
    {
        Task<Regiao> Get(Guid id);
        Task<IEnumerable<Regiao>> List();
        Task Add(Regiao regiao);
        Task Update(Regiao regiao);
        Task Delete(Guid id);
        Task<byte[]> GenerateExcelReport();
    }
}
