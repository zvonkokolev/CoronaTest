using CoronaTest.Core.DTOs;
using CoronaTest.Core.Entities;
using CoronaTest.Core.Interfaces;
using CoronaTest.Persistence;
using CoronaTest.Wpf.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace CoronaTest.Wpf.ViewModels
{
    public class TeilnehmerViewModel : BaseViewModel
    {
        #region fields
        private Participant _teilnehmer;
        #endregion

        #region properies
        public Participant Teilnehmer 
        {
            get => _teilnehmer;
            set
            {
                _teilnehmer = value;
                OnPropertyChanged(nameof(Teilnehmer));
            }
        }
        #endregion

        #region constructor
        public TeilnehmerViewModel(IWindowController controller) : base(controller)
        {
            LoadCommandsAsync();
        }

        public TeilnehmerViewModel(IWindowController controller, TestsDto test) : base(controller)
        {
            LoadCommandsAsync();
        }
        #endregion

        #region commands
        private void LoadCommandsAsync()
        {
            throw new NotImplementedException();
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
            await using IUnitOfWork unitOfWork = new UnitOfWork();

            Teilnehmer = await unitOfWork.Participants
                .GetParticipantByIdAsync(participant.Id);

        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new System.NotImplementedException();
        }
        #endregion
    }
}
