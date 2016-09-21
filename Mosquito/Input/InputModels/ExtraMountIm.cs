using System.Collections.Generic;

namespace Input.InputModels
{
    public class ExtraMountIm: ProductIm
    {
        public int Count { get; set; }

        public List<int> AllowedMounts { get; set; }

        public int ClincherCount { get; set; }
    }
}
