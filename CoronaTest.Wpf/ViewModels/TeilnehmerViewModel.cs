using CoronaTest.Core.DTOs;
using CoronaTest.Core.Entities;
using CoronaTest.Core.Interfaces;
using CoronaTest.Core.Services;
using CoronaTest.Persistence;
using CoronaTest.Wpf.Common;
using Microsoft.Extensions.Configuration;
using StringRandomizer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Windows.Input;
using static CoronaTest.Core.Enums.Enums;

namespace CoronaTest.Wpf.ViewModels
{
    public class TeilnehmerViewModel : BaseViewModel
    {
        #region fields
        private Examination _examination;
        private ISmsService _smsService;
        private Randomizer _stringRandomizer;
        private string _participantIdentifier;
        private string _examinationIdentifier;
        private string _participantSmsIdentifier;
        private List<TestResult> _testResults;
        private TestResult _selectedTestResult;
        private string _message;
        #endregion

        #region properies
        public Examination Examination
        {
            get { return _examination; }
            set
            {
                _examination = value;
                OnPropertyChanged();
            }
        }

        public List<TestResult> TestResults
        {
            get { return _testResults; }
            set
            {
                _testResults = value;
                OnPropertyChanged();
            }
        }

        public TestResult SelectedTestResult
        {
            get { return _selectedTestResult; }
            set
            {
                _selectedTestResult = value;
                OnPropertyChanged();
            }
        }

        public string ExaminationIdentifier
        {
            get { return _examinationIdentifier; }
            set
            {
                _examinationIdentifier = value;
                OnPropertyChanged();
            }
        }

        public string ParticipantIdentifier
        {
            get { return _participantIdentifier; }
            set
            {
                _participantIdentifier = value;
                OnPropertyChanged();
            }
        }

        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region constructor
        public TeilnehmerViewModel(IWindowController controller) : base(controller)
        {
            TestResults = new List<TestResult>
            {
                TestResult.Unknown,
                TestResult.Negative,
                TestResult.Positive
            };

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("appsettings.json")
                .AddUserSecrets<TwilioSmsService>()
                .AddEnvironmentVariables()
                .Build();

            _smsService = new TwilioSmsService(
                configuration["Twilio:AccountSid"], configuration["Twilio:AuthToken"]);

            _stringRandomizer = new Randomizer();
            Message = string.Empty;
            Examination = null;
            _participantSmsIdentifier = string.Empty;
            ExaminationIdentifier = null;
            ParticipantIdentifier = null;
            SelectedTestResult = TestResult.Unknown;
            LoadCommandsAsync();
        }

        public TeilnehmerViewModel(IWindowController controller, TestsDto test) : base(controller)
        {
            LoadCommandsAsync();
        }
        #endregion

        #region commands
        public ICommand CmdExaminationIdentifier { get; set; }
        public ICommand CmdParticipantIdentifier { get; set; }
        public ICommand CmdTestResult { get; set; }
        public ICommand CmdStartNewExamination { get; set; }
        public ICommand CmdQuitExamination { get; set; }

        private void LoadCommandsAsync()
        {
            CmdExaminationIdentifier = new RelayCommand(
                execute: async _ =>
                {
                    await using IUnitOfWork unitOfWork = new UnitOfWork();
                    Examination = await unitOfWork.Examinations
                        .GetExaminationByIdentifierAsync(ExaminationIdentifier);

                    if (Examination != null)
                    {
                        _participantSmsIdentifier = _stringRandomizer.Next();
                        _smsService.SendSms(Examination.Participant.Mobilephone, $"CoronaTest - Identcode: {_participantSmsIdentifier} !");
                        Message = "SMS Teilnehmeridentifikation versendet";
                    }
                    else
                    {
                        Message = "Der Identifier ist unbekannt";
                    }
                }
                ,
                canExecute: _ => ExaminationIdentifier != null
                );
            CmdParticipantIdentifier = new RelayCommand(
                execute: async _ =>
                {
                    if (ParticipantIdentifier != null && ParticipantIdentifier == _participantSmsIdentifier)
                    {
                        Message = $"Teilnehmer: {Examination.Participant.Firstname} {Examination.Participant.Lastname}";
                    }
                    else
                    {
                        Message = "Der Teilnehmer Code ist ungültig";
                    }
                }
                ,
                canExecute: _ => ParticipantIdentifier != null
                );
            CmdTestResult = new RelayCommand(
                execute: async _ =>
                {
                    if (SelectedTestResult == TestResult.Negative || SelectedTestResult == TestResult.Positive)
                    {
                        await using IUnitOfWork unitOfWork = new UnitOfWork();

                        var examinationInDb = await unitOfWork.Examinations
                            .GetExaminationByIdAsync(Examination.Id);

                        examinationInDb.Result = SelectedTestResult;
                        await unitOfWork.SaveChangesAsync();

                        _smsService.SendSms(Examination
                            .Participant.Mobilephone,
                            $"CoronaTest - Ergebnis: {examinationInDb.Result} !");
                        Message = "SMS mit Testergebnis versendet";
                    }
                }
                ,
                canExecute: _ => SelectedTestResult != TestResult.Unknown
                );
            CmdStartNewExamination = new RelayCommand(
                execute: async _ =>
                {
                    Message = string.Empty;
                    Examination = null;
                    _participantSmsIdentifier = string.Empty;
                    ExaminationIdentifier = null;
                    ParticipantIdentifier = null;
                    SelectedTestResult = TestResult.Unknown;
                    _stringRandomizer = new Randomizer();
                }
                , 
                canExecute: _ => true
                );
        }
        #endregion

        #region methods
        public static async Task<TeilnehmerViewModel> Create(IWindowController controller, Participant participant)
        {
            var model = new TeilnehmerViewModel(controller);
            await model.LoadParticipantAsync(participant);
            return model;
        }

        private async Task LoadParticipantAsync(Participant participant)
        {
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            yield return ValidationResult.Success;
        }
        #endregion
    }
}
