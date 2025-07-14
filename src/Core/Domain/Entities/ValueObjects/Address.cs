using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.ValueObjects
{
    public class Address
    {
        public string? Street { get; set; }
        public int? HouseNumber { get; set; }
        public int? Floor { get; set; }
        public string? Apartment { get; set; }
        public string? AdditionalInfo { get; set; }
        public string? ZipCode { get; set; }
    }
}
