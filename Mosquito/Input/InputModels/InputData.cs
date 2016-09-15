using System.Collections.Generic;

namespace Input.InputModels
{
    public class InputData
    {
        public List<SystemIm> Systems { get; set; } 
        public List<ProfileIm> Profiles { get; set; }
        public List<CrossProfileIm> CrossProfiles { get; set; }
        public List<NetIm> Nets { get; set; }
        public List<CordIm> Cords { get; set; }
        public List<AngleIm> Angles { get; set; }
        public List<MountIm> Mounts { get; set; }
        public List<CrossMountIm> CrossMounts { get; set; }
        public List<KnobIm> Knobs { get; set; }
        public List<ExtraDetailIm> ExtraDetails { get; set; }
        public List<PackageDetailIm> PackageDetails { get; set; } 
        public SettingsIm Settings { get; set; } 
    }
}
