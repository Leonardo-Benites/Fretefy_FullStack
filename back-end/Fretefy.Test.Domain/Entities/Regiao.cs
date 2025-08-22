using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Fretefy.Test.Domain.Entities
{
    public class Regiao : IEntity
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nome { get; set; }

        public bool Ativo { get; set; }

        public virtual ICollection<RegiaoCidade> RegiaoCidades { get; set; }

        public Regiao()
        {
            Id = Guid.NewGuid();
            RegiaoCidades = new HashSet<RegiaoCidade>();
        }
    }
}
