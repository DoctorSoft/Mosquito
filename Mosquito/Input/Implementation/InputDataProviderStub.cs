using System.Collections.Generic;
using Input.InputModels;
using Input.Interfaces;

namespace Input.Implementation
{
    public class InputDataProviderStub : IInputDataProvider
    {
        public InputData GetInputData()
        {
            return new InputData
            {
                Cords = new List<CordIm>
                {
                    new CordIm {Name = "Шнур Белый", PricePerCount = (decimal) 22.5},
                    new CordIm {Name = "Шнур Чёрный", PricePerCount = (decimal) 2.5},
                    new CordIm {Name = "Шнур Китайский", PricePerCount = (decimal) 0.5},
                },
                CrossProfiles = new List<CrossProfileIm>
                {
                    new CrossProfileIm {Name = "Профиль поперечены Белый", PricePerCount = (decimal) 33.5},
                    new CrossProfileIm {Name = "Профиль поперечены Афроамериканский", PricePerCount = (decimal) 3.5},
                    new CrossProfileIm {Name = "Профиль поперечены Японский", PricePerCount = (decimal) 133.5},
                },
                Profiles = new List<ProfileIm>
                {
                    new ProfileIm {Name = "Профиль Белый", PricePerCount = (decimal) 23.5},
                    new ProfileIm {Name = "Профиль Негритянский", PricePerCount = (decimal) 3.5},
                    new ProfileIm {Name = "Профиль Корейский", PricePerCount = (decimal) 13.5},
                },
                Nets = new List<NetIm>
                {
                    new NetIm {Name = "Сетка Белая", PricePerCount = (decimal) 23.5},
                    new NetIm {Name = "Сетка Обезьянья", PricePerCount = (decimal) 2.5},
                    new NetIm {Name = "Сетка Афганская", PricePerCount = (decimal) 0.5},
                },
                ExtraDetails = new List<ExtraDetailIm>
                {
                    new ExtraDetailIm {Name = "Крепление Белое", PricePerCount = (decimal) 3.5},
                    new ExtraDetailIm {Name = "Ручка Эфиопская", PricePerCount = (decimal) 0.1},
                    new ExtraDetailIm {Name = "Кронштейн Индусский", PricePerCount = (decimal) 5.5},
                    new ExtraDetailIm {Name = "Ручка Эфиопская", PricePerCount = (decimal) 0.1},
                    new ExtraDetailIm {Name = "Кронштейн Индусский", PricePerCount = (decimal) 5.5},
                },
                Settings = new SettingsIm
                {
                    CrossProfileTolerance = 48,
                    ProfileTolerance = 60,
                    OtherSpendingPrice = 20,
                    TrashPercent = 15,
                    WorkPrice = 50
                }
            };
        }
    }
}
