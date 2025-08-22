using Fretefy.Test.Domain.Entities;
using Fretefy.Test.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fretefy.Test.Infra.EntityFramework.Repositories
{
    public class RegiaoRepository : IRegiaoRepository
    {
        private readonly DbSet<Regiao> _dbSet;
        private readonly DbContext _dbContext;

        public RegiaoRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<Regiao>();
        }

        public async Task<IEnumerable<Regiao>> List()
        {
            return await _dbSet.AsNoTracking()
                .Include(r => r.RegiaoCidades)
                    .ThenInclude(rc => rc.Cidade)
                .OrderBy(r => r.Nome)
                .ToListAsync();
        }

        public async Task<Regiao> GetById(Guid id)
        {
            return await _dbSet
               .Include(r => r.RegiaoCidades)
                   .ThenInclude(rc => rc.Cidade)
               .FirstOrDefaultAsync(r => r.Id == id);
        }

        public Task<bool> IsSameName(string nome, Guid? id = null)
        {
            return _dbSet.AsNoTracking().AnyAsync(r => r.Nome.ToUpper() == nome.ToUpper() && (r.Id != id || id == null));
        }

        public async Task Add(Regiao regiao)
        {
            _dbSet.Add(regiao);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Update(Regiao regiao)
        {
            _dbSet.Update(regiao);
            await _dbContext.SaveChangesAsync();

        }

        public async Task Delete(Guid id)
        {
            var regiao = await _dbSet
               .Include(r => r.RegiaoCidades)
                   .ThenInclude(rc => rc.Cidade)
               .FirstOrDefaultAsync(r => r.Id == id);

            if (regiao != null)
            {
                _dbSet.Remove(regiao);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
