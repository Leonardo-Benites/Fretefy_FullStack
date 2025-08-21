using Fretefy.Test.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fretefy.Test.Domain.Interfaces.Repositories
{
    public interface IRegiaoRepository
    {
        Task<IEnumerable<Regiao>> List();
        Task<Regiao> GetById(Guid id);
        Task Add(Regiao regiao);
        Task Update(Regiao regiao);
        Task Delete(Guid id);
        Task<bool> IsSameName(string nome, Guid? id = null);
    }
}
