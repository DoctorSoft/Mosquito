using System.Collections.Generic;

namespace Input.InputModels
{
    public class InputData
    {
        public List<ProfileIm> Profiles { get; set; }
        public List<CrossProfileIm> CrossProfiles { get; set; }
        public List<NetIm> Nets { get; set; }
        public List<CordIm> Cords { get; set; }
        public List<ExtraDetailIm> ExtraDetails { get; set; }
        public SettingsIm Settings { get; set; } 
    }
}
