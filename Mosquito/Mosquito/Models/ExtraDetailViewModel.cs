using System;
using System.Collections.Generic;

namespace Mosquito.Models
{
    public class ExtraDetailViewModel
    {
        public string Name { get; set; }

        public decimal Price { get; set; }

        public decimal Count { get; set; }

        public List<string> NameList { get; set; }

        public Guid ItemId { get; set; }
    }
}
