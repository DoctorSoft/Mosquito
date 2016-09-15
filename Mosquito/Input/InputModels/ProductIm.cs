using System.Collections.Generic;

namespace Input.InputModels
{
    public class ProductIm
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal PricePerCount { get; set; }

        public List<int> Systems { get; set; } 
    }
}
