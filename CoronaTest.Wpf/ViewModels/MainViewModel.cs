using CoronaTest.Core.DTOs;
using CoronaTest.Core.Entities;
using CoronaTest.Core.Interfaces;
using CoronaTest.Persistence;
using CoronaTest.Wpf.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using static CoronaTest.Core.Enums.Enums;

namespace CoronaTest.Wpf.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        #region fields
        private ObservableCollection<TestsDto> _tests;
        private DateTime _selectedDateFilterFrom;
        private DateTime _selectedDateFilterTo;
        private int _testCount;
        private int _testNeg;
        private int _testPos;
        private string _title;
        private TestsDto _selectedTest;
        #endregion

        #region properties
        public ObservableCollection<TestsDto> Tests
        {
            get => _tests;
            set
            {
                _tests = value;
                OnPropertyChanged(nameof(Tests));
            }
        }

        public TestsDto SelectedTest
        {
            get => _selectedTest;
            set
            {
                _selectedTest = value;
                OnPropertyChanged(nameof(SelectedTest));
            }
        }

        public DateTime SelectedDateFilterFrom
        {
            get => _selectedDateFilterFrom;
            set
            {
                _selectedDateFilterFrom = value;
                OnPropertyChanged(nameof(SelectedDateFilterFrom));
                LoadCommandsAsync();
            }
        }

        public DateTime SelectedDateFilterTo
        {
            get => _selectedDateFilterTo;
            set
            {
                _selectedDateFilterTo = value;
                OnPropertyChanged(nameof(SelectedDateFilterTo));
                LoadCommandsAsync();
            }
        }
        public int TestCount
        {
            get => _testCount;
            set
            {
                _testCount = value;
                OnPropertyChanged(nameof(TestCount));
            }
        }

        public int TestNeg
        {
            get => _testNeg;
            set
            {
                _testNeg = value;
                OnPropertyChanged(nameof(TestNeg));
            }
        }

        public int TestPos
        {
            get => _testPos;
            set
            {
                _testPos = value;
                OnPropertyChanged(nameof(TestPos));
            }
        }
        #endregion

        #region constructor
        public MainViewModel(IWindowController controller) : base(controller)
        {
            SelectedDateFilterFrom = DateTime.UtcNow;
            SelectedDateFilterTo = DateTime.UtcNow;
            LoadCommandsAsync();
        }
        #endregion

        #region commands
        public ICommand CmdDateFilter { get; set; }
        public ICommand CmdTestsRes { get; set; }
        public ICommand CmdFilterReset { get; set; }

        private void LoadCommandsAsync()
        {
            CmdDateFilter = new RelayCommand(
                execute: async _ =>
                {
                    await using IUnitOfWork unitOfWork = new UnitOfWork();
                    List<TestsDto> a = await unitOfWork.Examinations
                    .GetFilteredTests(SelectedDateFilterFrom, SelectedDateFilterTo);

                    Tests = new ObservableCollection<TestsDto>(a);
                }
                ,
                canExecute: _ => SelectedDateFilterFrom != null
                    || SelectedDateFilterTo != null
                )
                ;
            CmdFilterReset = new RelayCommand(
                execute: async _ =>
                {
                    SelectedDateFilterFrom = DateTime.UtcNow;
                    SelectedDateFilterTo = DateTime.UtcNow;
                    await LoadProducts();
                }
                ,
                canExecute: _ => true
                )
                ;
            CmdTestsRes = new RelayCommand(
                execute: async _ =>
                {
                    await using IUnitOfWork unitOfWork = new UnitOfWork();
                    _title = "Testen";
                    var controller = new WindowController();
                    TeilnehmerViewModel viewModel = await TeilnehmerViewModel.Create(controller, null);
                    controller.ShowWindow(viewModel);
                }
                ,
                canExecute: _ => true
                )
                ;
        }
        #endregion

        #region methods
        public static async Task<MainViewModel> Create(IWindowController controller)
        {
            var model = new MainViewModel(controller);
            await model.LoadProducts();
            return model;
        }

        private async Task LoadProducts()
        {
            await using IUnitOfWork unitOfWork = new UnitOfWork();

            List<TestsDto> examinations = await unitOfWork.Examinations.GetAllExaminationsDtosAsync();
            Tests = new ObservableCollection<TestsDto>(examinations);
            SelectedTest = Tests.FirstOrDefault();

            TestCount = Tests.Count;
            TestNeg = Tests.Count(_ => _.TestResult == TestResult.Negative);
            TestPos = Tests.Count(_ => _.TestResult == TestResult.Positive);
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            yield return ValidationResult.Success;
        }
        #endregion
    }
}
