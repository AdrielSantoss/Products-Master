using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class Product
    {
        public Product(int Id, string Name, double Price, int ProviderId, int BrandId )
        {
            this.Id = Id;
            this.Name = Name;
            this.Price = Price;
            this.ProviderId = ProviderId;
            this.BrandId = BrandId;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int ProviderId { get; set; }
        public  Provider Provider { get; set; }
        public int BrandId { get; set; }
        public Brand Brand { get; set; }
    }
}
