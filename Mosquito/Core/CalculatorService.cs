using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Input.InputModels;
using Input.Interfaces;
using OutputWPF.OutputWPFModels;

namespace Core
{
    public class CalculatorService : ICalculatorService
    {
        private readonly IInputDataProvider inputDataProvider;

        public CalculatorService(IInputDataProvider inputDataProvider)
        {
            this.inputDataProvider = inputDataProvider;
        }

        public OutputWpfData GetDefault()
        {
            var inputData = inputDataProvider.GetInputData();

            var defaultData = new OutputWpfData
            {
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
                ExtraDetails = inputData.ExtraDetails,
                CurrentExtraDetails = new List<CurrentExtraDetail>(),
                WorkPrice = inputData.Settings.WorkPrice,
                OtherSpendingPrice = inputData.Settings.OtherSpendingPrice,
                Height = 600,
                Width = 1700,
                CrossProfileTolerance = inputData.Settings.CrossProfileTolerance,
                ProfileTolerance = inputData.Settings.ProfileTolerance,
                TrashPercent = inputData.Settings.TrashPercent
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

        private OutputWpfData Calculate(OutputWpfData notPricedOutputData)
        {
            var profileIm = notPricedOutputData.Profiles.FirstOrDefault(im => im.Name == notPricedOutputData.CurrentProfile.Name);
            notPricedOutputData.CurrentProfile.Count = Math.Round((notPricedOutputData.Width + notPricedOutputData.Height - (2 * notPricedOutputData.ProfileTolerance)) / 500, 2);
            notPricedOutputData.CurrentProfile.Price = Math.Round(notPricedOutputData.CurrentProfile.Count * profileIm.PricePerCount, 2);


            var crossProfileIm = notPricedOutputData.CrossProfiles.FirstOrDefault(im => im.Name == notPricedOutputData.CurrentCrossProfile.Name);
            notPricedOutputData.CurrentCrossProfile.Count = Math.Round((notPricedOutputData.Height - notPricedOutputData.CrossProfileTolerance) / 1000, 2);
            notPricedOutputData.CurrentCrossProfile.Price = Math.Round(notPricedOutputData.CurrentCrossProfile.Count * crossProfileIm.PricePerCount, 2);

            var netIm = notPricedOutputData.Nets.FirstOrDefault(im => im.Name == notPricedOutputData.CurrentNet.Name);
            notPricedOutputData.CurrentNet.Count = Math.Round(((notPricedOutputData.Width * notPricedOutputData.Height) / 1000000), 2);
            notPricedOutputData.CurrentNet.Price = Math.Round(notPricedOutputData.CurrentNet.Count * netIm.PricePerCount, 2);

            var cordIm = notPricedOutputData.Cords.FirstOrDefault(im => im.Name == notPricedOutputData.CurrentCord.Name);
            notPricedOutputData.CurrentCord.Count = Math.Round((notPricedOutputData.Width + notPricedOutputData.Height - (2 * notPricedOutputData.ProfileTolerance)) / 500, 2);
            notPricedOutputData.CurrentCord.Price = Math.Round(notPricedOutputData.CurrentCord.Count*cordIm.PricePerCount, 2);

            foreach (var currentExtraDetail in notPricedOutputData.CurrentExtraDetails)
            {
                var detailIm = notPricedOutputData.ExtraDetails.FirstOrDefault(im => im.Name == currentExtraDetail.Name);
                currentExtraDetail.Price = Math.Round(currentExtraDetail.Count * detailIm.PricePerCount, 2);
            }
           
            notPricedOutputData.TrashPrice = Math.Round(((notPricedOutputData.CurrentProfile.Price + notPricedOutputData.CurrentCrossProfile.Price + notPricedOutputData.CurrentNet.Price + notPricedOutputData.CurrentCord.Price) * notPricedOutputData.TrashPercent / 100), 2);

            var currentsSum = notPricedOutputData.CurrentProfile.Price +
                                             notPricedOutputData.CurrentCrossProfile.Price + 
                                             notPricedOutputData.CurrentNet.Price +
                                             notPricedOutputData.CurrentCord.Price +
                                             notPricedOutputData.CurrentExtraDetails.Select(detail => detail.Price).Sum() +
                                             notPricedOutputData.TrashPrice +
                                             notPricedOutputData.WorkPrice + 
                                             notPricedOutputData.OtherSpendingPrice;

            notPricedOutputData.TotalPrice = currentsSum;

            return notPricedOutputData;
        }
    }
}
