using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Fretefy.Test.Domain.Entities
{
    public class Regiao : IEntity
    {
        public Guid Id { get; set; }

        [Required]
        public string Nome { get; set; }

        public bool Active { get; set; }

        public virtual List<Cidade> Cidades { get; set; } = new List<Cidade>();
    }
}
