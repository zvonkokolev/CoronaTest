using CoronaTest.Api.Dtos;
using CoronaTest.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using static CoronaTest.Core.Enums.Enums;

namespace CoronaTest.Api.Controllers
{
    /// <summary>
    /// API-Controller für die Abfrage von Statistiken
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize()]
    public class StatisticsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Constructor mit DI
        /// </summary>
        /// <param name="unitOfWork"></param>
        public StatisticsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Liefert die Teststatistik im Zeitraum
        /// </summary>
        /// <response code="200">Die Abfrage war erfolgreich.</response>
        [HttpGet]
        [Authorize(Roles = "Admin,User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<StatisticsDto>> GetStatisticsInPeriode(DateTime from, DateTime to)
        {
            var examinations = await _unitOfWork.Examinations.GetFilteredTests(from, to);

            StatisticsDto statistic = new StatisticsDto();

            if (examinations != null)
            {
                statistic.CountOfExaminations = examinations.Count();
                statistic.CountOfUnknownResults = examinations.Count(t => t.TestResult == TestResult.Unknown);
                statistic.CountOfPositiveResults = examinations.Count(t => t.TestResult == TestResult.Positive);
                statistic.CountOfNegativeResults = examinations.Count(t => t.TestResult == TestResult.Negative);
            }

            return Ok(statistic);
        }

        /// <summary>
        /// Liefert die Teststatistik in der Gemeinde und im Zeitraum
        /// </summary>
        /// <response code="200">Die Abfrage war erfolgreich.</response>
        [HttpGet]
        [Route("byPostalCode")]
        [Authorize(Roles = "Admin,User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<StatisticsDto>> GetStatisticsInAreaAndPeriode(string postalCode, DateTime from, DateTime to)
        {
            var examinations = await _unitOfWork.Examinations.GetExaminationsWithFilterAsync(postalCode, from, to);

            StatisticsDto statistic = new StatisticsDto();

            if (examinations != null)
            {
                statistic.CountOfExaminations = examinations.Count();
                statistic.CountOfUnknownResults = examinations.Count(_ => _.Result == TestResult.Unknown);
                statistic.CountOfPositiveResults = examinations.Count(_ => _.Result == TestResult.Positive);
                statistic.CountOfNegativeResults = examinations.Count(_ => _.Result == TestResult.Negative);
            }

            return Ok(statistic);
        }

        /// <summary>
        /// Liefert die Teststatistik je Kalenderwochen im Zeitraum
        /// </summary>
        /// <response code="200">Die Abfrage war erfolgreich.</response>
        [HttpGet]
        [Route("perCalendarWeek")]
        [Authorize(Roles = "Admin,User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<StatisticsDto[]>> GetStatisticsWithWeekNumberInPeriode(DateTime from, DateTime to)
        {
            var examinations = await _unitOfWork.Examinations.GetFilteredTests(from, to);

            StatisticsDto[] statistic = new StatisticsDto[0];
            Calendar calendar = CultureInfo.InvariantCulture.Calendar;

            statistic = examinations.GroupBy(_ => $"{_.ExaminationAt.Year}_{calendar.GetWeekOfYear(_.ExaminationAt, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday):d2}")
                .Select(_ => new StatisticsDto
                {
                    Year = int.Parse(_.Key.Split('_')[0]),
                    WeekNumber = int.Parse(_.Key.Split('_')[1]),
                    CountOfExaminations = _.Count(),
                    CountOfUnknownResults = _.Count(t => t.TestResult == TestResult.Unknown),
                    CountOfPositiveResults = _.Count(t => t.TestResult == TestResult.Positive),
                    CountOfNegativeResults = _.Count(t => t.TestResult == TestResult.Negative)
                })
                .OrderBy(_ => _.Year)
                .ThenBy(_ => _.WeekNumber)
                .ToArray();

            return Ok(statistic);
        }
    }
}
