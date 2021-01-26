using CoronaTest.Core.Entities;
using CoronaTest.Persistence;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CoronaTest.ImportConsole
{
    class Program
    {
        static async Task Main()
        {
            await InitDataAsync();

            Console.WriteLine();
            Console.Write("Beenden mit Eingabetaste ...");
            Console.ReadLine();
        }

        private static async Task InitDataAsync()
        {
            Console.WriteLine("***************************");
            Console.WriteLine("          Import");
            Console.WriteLine("***************************");
            Console.WriteLine("Import der Daten in die Datenbank");
            await using var unitOfWork = new UnitOfWork();
            Console.WriteLine("Datenbank löschen");
            await unitOfWork.DeleteDatabaseAsync();
            Console.WriteLine("Datenbank migrieren");
            await unitOfWork.MigrateDatabaseAsync();

            Console.WriteLine("Daten werden von csv-Dateien eingelesen");
            var campaigns = ImportController.ReadFromCsv();

            var testCenters = campaigns.SelectMany(tc => tc.AvailableTestCenters).Distinct().ToList();
            await unitOfWork.TestCenters.AddRangeAsync(testCenters);
            Console.WriteLine("Testzentren werden in Datenbank gespeichert");
            await unitOfWork.SaveChangesAsync();

            //campaigns = campaigns
            //    .Select(c => new Campaign
            //    {
            //        Name = c.Name,
            //        AvailableTestCenters = c.AvailableTestCenters,
            //        From = c.From,
            //        To = c.To
            //    })
            //    .ToList();

            await unitOfWork.Campaigns.AddRangeAsync(campaigns);
            Console.WriteLine("Kampagnen werden in Datenbank gespeichert");
            await unitOfWork.SaveChangesAsync();

            var cntCampaigns = await unitOfWork.Campaigns.GetCountAsync();
            var cntTestCenters = await unitOfWork.TestCenters.GetCountAsync();
            var cntParticipants = await unitOfWork.Participants.GetCountAsync();
            var cntExaminations = await unitOfWork.Examinations.GetCountAsync();
            Console.WriteLine($"  Es wurden {cntCampaigns} Kampagnen gespeichert!");
            Console.WriteLine($"  Es wurden {cntTestCenters} Testzentren gespeichert!");
            Console.WriteLine($"  Es wurden {cntParticipants} Teilnehmer gespeichert!");
            Console.WriteLine($"  Es wurden {cntExaminations} Prüfungen gespeichert!");

        }
    }
}
