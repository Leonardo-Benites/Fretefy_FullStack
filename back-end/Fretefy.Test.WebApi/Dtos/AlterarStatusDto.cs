using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fretefy.Test.WebApi.Dtos
{
    public class AlterarStatusDto
    {
        public Guid Id { get; set; }

        public bool Ativo  { get; set; }
    }
}
