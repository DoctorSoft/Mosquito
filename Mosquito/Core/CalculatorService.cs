using System;
using System.Collections.Generic;
using System.Linq;
using Input.Constants;
using Input.InputModels;
using Input.Interfaces;
using OutputWPF.OutputWPFModels;

namespace Core
{
    public class CalculatorService : ICalculatorService
    {
        private readonly InputData inputData;

        public CalculatorService(IInputDataProvider inputDataProvider)
        {
            this.inputData = inputDataProvider.GetInputData(); 
        }

        public OutputWpfData GetDefault()
        { 
            var defaultData = new OutputWpfData
            {
                Height = 600,
                Width = 1700,
                Systems = inputData.Systems,
                CurrentSystem = new CurrentSystem
                {
                    Name = inputData.Systems.FirstOrDefault().Name
                },
                Profiles = inputData.Profiles,
                CurrentProfile = new CurrentProfile
                {
                    Name = inputData.Profiles.FirstOrDefault().Name,  
                },
                CrossProfiles = inputData.CrossProfiles,
                CurrentCrossProfile = new CurrentCrossProfile
                {
                    Name = inputData.CrossProfiles.FirstOrDefault().Name
                },
                Nets = inputData.Nets,
                CurrentNet = new CurrentNet
                {
                    Name = inputData.Nets.FirstOrDefault().Name
                },
                Cords = inputData.Cords,
                CurrentCord = new CurrentCord
                {
                    Name = inputData.Cords.FirstOrDefault().Name
                },
                Angles = inputData.Angles,
                CurrentAngle = new CurrentAngle
                {
                    Name = inputData.Angles.FirstOrDefault().Name
                },
                Mounts = inputData.Mounts,
                CurrentMount = new CurrentMount
                {
                    Name = inputData.Mounts.FirstOrDefault().Name
                },
                CrossMounts = inputData.CrossMounts,
                CurrentCrossMount = new CurrentCrossMount()
                {
                    Name = inputData.CrossMounts.FirstOrDefault().Name
                },
                Knobs = inputData.Knobs,
                CurrentKnob = new CurrentKnob()
                {
                    Name = inputData.Knobs.FirstOrDefault().Name
                },
                ExtraDetails = inputData.ExtraDetails,
                CurrentExtraDetails = new List<CurrentExtraDetail>(),
                RequiredExtraDetails = new List<CurrentExtraDetail>(),
                WorkPrice = inputData.Settings.WorkPrice,
                OtherSpendingPrice = inputData.Settings.OtherSpendingPrice,
                CrossProfileTolerance = inputData.Settings.CrossProfileTolerance,
                ProfileTolerance = inputData.Settings.ProfileTolerance,
                TrashPercent = inputData.Settings.TrashPercent,
                GluePrice = inputData.Settings.GluePrice,
                AmountNetsOnTheOneGlue = inputData.Settings.AmountNetsOnTheOneGlue
            };

            return Calculate(defaultData);
        }

        public OutputWpfData ChangeWidth(decimal width, OutputWpfData oldData)
        {
            oldData.Width = width;
            return Calculate(oldData);
        }

        public OutputWpfData ChangeHeight(decimal heigth, OutputWpfData oldData)
        {
            oldData.Height = heigth;
            return Calculate(oldData);
        }

        public OutputWpfData ChangeSystem(string systemName, OutputWpfData oldData)
        {
            oldData.CurrentSystem = new CurrentSystem
            {
                Name = systemName
            };
            return Calculate(oldData);
        }

        public OutputWpfData ChangeProfile(string profileName, OutputWpfData oldData)
        {
            oldData.CurrentProfile = new CurrentProfile
            {
                Name = profileName
            };
            return Calculate(oldData);
        }

        public OutputWpfData ChangeCrossProfile(string crossProfileName, OutputWpfData oldData)
        {
            oldData.CurrentCrossProfile = new CurrentCrossProfile
            {
                Name = crossProfileName
            };
            return Calculate(oldData);
        }

        public OutputWpfData ChangeNet(string netName, OutputWpfData oldData)
        {
            oldData.CurrentNet = new CurrentNet
            {
                Name = netName
            };
            return Calculate(oldData);
        }

        public OutputWpfData ChangeCord(string cordName, OutputWpfData oldData)
        {
            oldData.CurrentCord = new CurrentCord
            {
                Name = cordName
            };
            return Calculate(oldData);
        }

        public OutputWpfData ChangeAngle(string angleName, OutputWpfData oldData)
        {
            oldData.CurrentAngle.Name = angleName;
            return Calculate(oldData);
        }

        public OutputWpfData ChangeAngleCount(decimal angleCount, OutputWpfData oldData)
        {
            oldData.CurrentAngle.Count = angleCount;
            return Calculate(oldData);
        }

        public OutputWpfData ChangeMount(string mountName, OutputWpfData oldData)
        {
            oldData.CurrentMount.Name = mountName;
            return Calculate(oldData);
        }

        public OutputWpfData ChangeMountCount(decimal mountCount, OutputWpfData oldData)
        {
            oldData.CurrentMount.Count = mountCount;
            return Calculate(oldData);
        }

        public OutputWpfData ChangeCrossMount(string crossMountName, OutputWpfData oldData)
        {
            oldData.CurrentCrossMount.Name = crossMountName;
            return Calculate(oldData);
        }

        public OutputWpfData ChangeCrossMountCount(decimal crossMountCount, OutputWpfData oldData)
        {
            oldData.CurrentCrossMount.Count = crossMountCount;
            return Calculate(oldData);
        }

        public OutputWpfData ChangeKnob(string knobName, OutputWpfData oldData)
        {
            oldData.CurrentKnob.Name = knobName;
            return Calculate(oldData);
        }

        public OutputWpfData ChangeKnobCount(decimal knobCount, OutputWpfData oldData)
        {
            oldData.CurrentKnob.Count = knobCount;
            return Calculate(oldData);
        }

        public OutputWpfData AddExtraDetail(OutputWpfData oldData)
        {
            var newExtraDetail = new CurrentExtraDetail
            {
                Id = Guid.NewGuid(),
                Name = oldData.ExtraDetails.Select(im => im.Name).FirstOrDefault(),
                Count = 1
            };

            oldData.CurrentExtraDetails.Add(newExtraDetail);

            return Calculate(oldData);
        }

        public OutputWpfData RemoveExtraDetail(Guid id, OutputWpfData oldData)
        {
            oldData.CurrentExtraDetails.Remove(oldData.CurrentExtraDetails.FirstOrDefault(detail => detail.Id == id));

            return Calculate(oldData);
        }

        public OutputWpfData UpdateExtraDetailName(Guid id, string newName, OutputWpfData oldData)
        {
            foreach (var extraDetail in oldData.CurrentExtraDetails.Where(extraDetail => extraDetail.Id == id))
            {
                extraDetail.Name = newName;
            }

            return Calculate(oldData);
        }

        public OutputWpfData UpdateExtraDetailCount(Guid id, decimal newCount, OutputWpfData oldData)
        {
            foreach (var extraDetail in oldData.CurrentExtraDetails.Where(extraDetail => extraDetail.Id == id))
            {
                extraDetail.Count = newCount;
            }

            return Calculate(oldData);
        }

        private OutputWpfData SetAllowedImProductsBySystem(OutputWpfData notPricedOutputData)
        {
            var systemIm = inputData.Systems.FirstOrDefault(im => im.Name == notPricedOutputData.CurrentSystem.Name);

            notPricedOutputData.Angles = inputData.Angles.Where(im => im.Systems.Contains(systemIm.Id)).ToList();
            notPricedOutputData.Cords = inputData.Cords.Where(im => im.Systems.Contains(systemIm.Id)).ToList();
            notPricedOutputData.CrossProfiles = inputData.CrossProfiles.Where(im => im.Systems.Contains(systemIm.Id)).ToList();
            notPricedOutputData.ExtraDetails = inputData.ExtraDetails.Where(im => im.Systems.Contains(systemIm.Id)).ToList();
            notPricedOutputData.Mounts = inputData.Mounts.Where(im => im.Systems.Contains(systemIm.Id)).ToList();
            notPricedOutputData.CrossMounts = inputData.CrossMounts.Where(im => im.Systems.Contains(systemIm.Id)).ToList();
            notPricedOutputData.Knobs = inputData.Knobs.Where(im => im.Systems.Contains(systemIm.Id)).ToList();
            notPricedOutputData.Nets = inputData.Nets.Where(im => im.Systems.Contains(systemIm.Id)).ToList();
            notPricedOutputData.Profiles = inputData.Profiles.Where(im => im.Systems.Contains(systemIm.Id)).ToList();
            notPricedOutputData.RequiredExtraDetails = new List<CurrentExtraDetail>();

            return notPricedOutputData;
        }

        private OutputWpfData CalculateProfile(OutputWpfData notPricedOutputData)
        {
            var profileIm = notPricedOutputData.Profiles.FirstOrDefault(im => im.Name == notPricedOutputData.CurrentProfile.Name);
            profileIm = profileIm ?? notPricedOutputData.Profiles.FirstOrDefault();
            notPricedOutputData.CurrentProfile.Name = profileIm.Name;
            var profileTolerance = notPricedOutputData.CurrentAngle.Inner ? 0 : notPricedOutputData.ProfileTolerance;
            notPricedOutputData.CurrentProfile.Count = Math.Round((notPricedOutputData.Width + notPricedOutputData.Height - (2 * profileTolerance)) / 500, 2);
            notPricedOutputData.CurrentProfile.Price = Math.Round(notPricedOutputData.CurrentProfile.Count * profileIm.PricePerCount, 2);

            return notPricedOutputData;
        }

        private NetIm CalculateTheBestNetSize(List<NetIm> typedNets, decimal width, decimal height)
        {
            var fittedNet = typedNets.OrderBy(im => im.Size).FirstOrDefault(im => im.Size >= width);

            if (fittedNet != null)
            {
                return fittedNet;
            }

            var maxSize = typedNets.Max(im => im.Size);
            var minSize = typedNets.Min(im => im.Size);

            if (height * 2 <= minSize)
            {
                return typedNets.FirstOrDefault(im => im.Size == minSize);
            }

            return typedNets.FirstOrDefault(im => im.Size == maxSize);
        } 

        private OutputWpfData CalculateNet(OutputWpfData notPricedOutputData)
        {
            var netIm = notPricedOutputData.Nets.FirstOrDefault(im => im.Name == notPricedOutputData.CurrentNet.Name);
            var netId = netIm == null ? notPricedOutputData.Nets.FirstOrDefault().Id : netIm.Id;

            notPricedOutputData.Nets = notPricedOutputData.Nets
                .GroupBy(im => im.Id)
                .Select(ims => CalculateTheBestNetSize(ims.ToList(), notPricedOutputData.Width, notPricedOutputData.Height))
                .ToList();
            
            netIm = notPricedOutputData.Nets.FirstOrDefault(im => im.Id == netId);
            netIm = netIm ?? notPricedOutputData.Nets.FirstOrDefault();

            notPricedOutputData.CurrentNet.Name = netIm.Name;
            notPricedOutputData.CurrentNet.Count = Math.Round(((notPricedOutputData.Width * notPricedOutputData.Height) / 1000000), 2);
            notPricedOutputData.CurrentNet.Price = Math.Round(notPricedOutputData.CurrentNet.Count * netIm.PricePerCount, 2);

            return notPricedOutputData;
        }

        private OutputWpfData CalculateAngles(OutputWpfData notPricedOutputData)
        {
            var angleIm = notPricedOutputData.Angles.FirstOrDefault(im => im.Name == notPricedOutputData.CurrentAngle.Name);
            angleIm = angleIm ?? notPricedOutputData.Angles.FirstOrDefault();
            notPricedOutputData.CurrentAngle.Name = angleIm.Name;
            notPricedOutputData.CurrentAngle.Inner = angleIm.Inner;
            notPricedOutputData.CurrentAngle.ClincherCount = angleIm.ClincherCount;
            notPricedOutputData.CurrentAngle.Count = angleIm.Count;
            notPricedOutputData.CurrentAngle.Price = Math.Round(notPricedOutputData.CurrentAngle.Count * angleIm.PricePerCount, 2);

            if (angleIm.ClincherCount != 0)
            {
                var clincher = inputData.PackageDetails[(int) PackageDetail.Clincher];
                notPricedOutputData.RequiredExtraDetails.Add(new CurrentExtraDetail
                {
                    Name = clincher.Name + " (" + notPricedOutputData.CurrentAngle.Name + ")",
                    Count = angleIm.ClincherCount,
                    Price = clincher.PricePerCount * angleIm.ClincherCount
                });
            }

            return notPricedOutputData;
        }

        private OutputWpfData CalculateMounts(OutputWpfData notPricedOutputData)
        {
            var mountIm = notPricedOutputData.Mounts.FirstOrDefault(im => im.Name == notPricedOutputData.CurrentMount.Name);
            mountIm = mountIm ?? notPricedOutputData.Mounts.FirstOrDefault();
            notPricedOutputData.CurrentMount.Name = mountIm.Name;
            notPricedOutputData.CurrentMount.Count = mountIm.Count;
            notPricedOutputData.CurrentMount.Price = Math.Round(notPricedOutputData.CurrentMount.Count * mountIm.PricePerCount, 2);

            if (mountIm.ClincherCount != 0)
            {
                var clincher = inputData.PackageDetails[(int)PackageDetail.Clincher];
                notPricedOutputData.RequiredExtraDetails.Add(new CurrentExtraDetail
                {
                    Name = clincher.Name + " (" + notPricedOutputData.CurrentMount.Name + ")",
                    Count = mountIm.ClincherCount,
                    Price = clincher.PricePerCount * mountIm.ClincherCount
                });
            }

            return notPricedOutputData;
        }

        private OutputWpfData CalculateCrossMounts(OutputWpfData notPricedOutputData)
        {
            var crossMountIm = notPricedOutputData.CrossMounts.FirstOrDefault(im => im.Name == notPricedOutputData.CurrentCrossMount.Name);
            crossMountIm = crossMountIm ?? notPricedOutputData.CrossMounts.FirstOrDefault();
            notPricedOutputData.CurrentCrossMount.Name = crossMountIm.Name;
            notPricedOutputData.CurrentCrossMount.Count = crossMountIm.Count;
            notPricedOutputData.CurrentCrossMount.Price = Math.Round(notPricedOutputData.CurrentCrossMount.Count * crossMountIm.PricePerCount, 2);

            return notPricedOutputData;
        }

        private OutputWpfData CalculateKnobs(OutputWpfData notPricedOutputData)
        {
            var knobIm = notPricedOutputData.Knobs.FirstOrDefault(im => im.Name == notPricedOutputData.CurrentKnob.Name);
            knobIm = knobIm ?? notPricedOutputData.Knobs.FirstOrDefault();
            notPricedOutputData.CurrentKnob.Name = knobIm.Name;
            notPricedOutputData.CurrentKnob.Count = knobIm.Count;
            notPricedOutputData.CurrentKnob.Price = Math.Round(notPricedOutputData.CurrentKnob.Count * knobIm.PricePerCount, 2);

            return notPricedOutputData;
        }

        private OutputWpfData Calculate(OutputWpfData notPricedOutputData)
        {
            notPricedOutputData = SetAllowedImProductsBySystem(notPricedOutputData);

            notPricedOutputData = CalculateAngles(notPricedOutputData);
            notPricedOutputData = CalculateProfile(notPricedOutputData);
            
            var crossProfileIm = notPricedOutputData.CrossProfiles.FirstOrDefault(im => im.Name == notPricedOutputData.CurrentCrossProfile.Name);
            crossProfileIm = crossProfileIm ?? notPricedOutputData.CrossProfiles.FirstOrDefault();
            notPricedOutputData.CurrentCrossProfile.Name = crossProfileIm.Name;
            notPricedOutputData.CurrentCrossProfile.Count = Math.Round((notPricedOutputData.Height - notPricedOutputData.CrossProfileTolerance) / 1000, 2);
            notPricedOutputData.CurrentCrossProfile.Price = Math.Round(notPricedOutputData.CurrentCrossProfile.Count * crossProfileIm.PricePerCount, 2);

            notPricedOutputData = CalculateNet(notPricedOutputData);

            var cordIm = notPricedOutputData.Cords.FirstOrDefault(im => im.Name == notPricedOutputData.CurrentCord.Name);
            cordIm = cordIm ?? notPricedOutputData.Cords.FirstOrDefault();
            notPricedOutputData.CurrentCord.Name = cordIm.Name;
            notPricedOutputData.CurrentCord.Count = Math.Round((notPricedOutputData.Width + notPricedOutputData.Height - (2 * notPricedOutputData.ProfileTolerance)) / 500, 2);
            notPricedOutputData.CurrentCord.Price = Math.Round(notPricedOutputData.CurrentCord.Count*cordIm.PricePerCount, 2);

            notPricedOutputData = CalculateMounts(notPricedOutputData);
            notPricedOutputData = CalculateCrossMounts(notPricedOutputData);
            notPricedOutputData = CalculateKnobs(notPricedOutputData);

            foreach (var currentExtraDetail in notPricedOutputData.CurrentExtraDetails)
            {
                var detailIm = notPricedOutputData.ExtraDetails.FirstOrDefault(im => im.Name == currentExtraDetail.Name);
                if (detailIm == null)
                {
                    continue;
                }
                currentExtraDetail.Price = Math.Round(currentExtraDetail.Count * detailIm.PricePerCount, 2);
            }

            var gLuePrice = Math.Round(notPricedOutputData.GluePrice/notPricedOutputData.AmountNetsOnTheOneGlue, 2);

            notPricedOutputData.TrashPrice = Math.Round(((notPricedOutputData.CurrentProfile.Price + notPricedOutputData.CurrentCrossProfile.Price + notPricedOutputData.CurrentNet.Price + notPricedOutputData.CurrentCord.Price) * notPricedOutputData.TrashPercent / 100), 2);

            var currentsSum = notPricedOutputData.CurrentProfile.Price +
                                             notPricedOutputData.CurrentCrossProfile.Price + 
                                             notPricedOutputData.CurrentNet.Price +
                                             notPricedOutputData.CurrentCord.Price +
                                             notPricedOutputData.CurrentAngle.Price +
                                             notPricedOutputData.CurrentMount.Price +
                                             notPricedOutputData.CurrentCrossMount.Price +
                                             notPricedOutputData.CurrentKnob.Price +
                                             notPricedOutputData.CurrentExtraDetails.Select(detail => detail.Price).Sum() +
                                             notPricedOutputData.RequiredExtraDetails.Select(detail => detail.Price).Sum() +
                                             notPricedOutputData.TrashPrice +
                                             notPricedOutputData.WorkPrice + 
                                             notPricedOutputData.OtherSpendingPrice +
                                             gLuePrice;

            notPricedOutputData.TotalPrice = currentsSum;

            return notPricedOutputData;
        }
    }
}
