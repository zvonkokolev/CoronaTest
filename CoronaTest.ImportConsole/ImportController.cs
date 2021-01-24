using CoronaTest.Core.Entities;
using CoronaTest.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoronaTest.ImportConsole
{
    public class ImportController
    {
        public static IList<Campaign> ReadFromCsv()
        {
            string csvCampaign = "Campaigns.csv";
            string csvTestCenters = "TestCenters.csv";

            string[][] matrixTestCenters = CsvReader.ReadStringMatrixFromCsv(csvTestCenters, true);
            string[][] matrixCampaigns = CsvReader.ReadStringMatrixFromCsv(csvCampaign, true);

            var testCenters = matrixTestCenters
                .Select(line => new TestCenter()
                {
                    Name = line[0],
                    City = line[1],
                    Postalcode = line[2],
                    Street = line[3],
                    SlotCapacity = int.Parse(line[4])
                }).ToDictionary(key => key.Name);

            var campaigns = matrixCampaigns
                .Select(c => new Campaign
                {
                    Name = c[0],
                    AvailableTestCenters = c[1].Split(',').Select(tc => testCenters[tc]).ToList(),
                        From = Convert.ToDateTime(c[2]),
                        To = Convert.ToDateTime(c[3])
                    })
                .ToList();

            return campaigns;
        }
    }
}

