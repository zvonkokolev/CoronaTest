using CoronaTest.Core.DTOs;
using CoronaTest.Core.Entities;
using CoronaTest.Wpf.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace CoronaTest.Wpf.ViewModels
{
    public class TeilnehmerViewModel : BaseViewModel
    {
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
            await model.LoadParticipant(participant);
            return model;
        }

        private Task LoadParticipant(Participant participant)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new System.NotImplementedException();
        }
        #endregion
    }
}
