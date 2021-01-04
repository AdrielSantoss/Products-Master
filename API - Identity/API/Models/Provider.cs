using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class Provider
    {
        public Provider(int Id, string Name, string Cnpj)
        {
            this.Id = Id;
            this.Name = Name;
            this.Cnpj = Cnpj;
        }

        public int Id{ get; set; }
        public string Name { get; set; }
        public string Cnpj { get; set; }
    }
}
