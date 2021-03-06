﻿using CoronaTest.Core.DTOs;
using CoronaTest.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoronaTest.Core.Interfaces
{
    public interface IExaminationRepository
    {
        Task<IList<Examination>> GetAllExaminationsAsync();
        Task AddExaminationAsync(Examination examination);
        Task<Examination> GetExaminationByIdAsync(int id);
        void RemoveExamination(Examination examination);
        bool IsExistingExamination(int id);
        void UpdateExamination(Examination examination);
        Task<int> GetCountAsync();
        Task<List<TestsDto>> GetAllExaminationsDtosAsync();
        Task<List<TestsDto>> GetFilteredTests(DateTime selectedDateFilterFrom, DateTime selectedDateFilterTo);
        Task<List<Examination>> GetExaminationsByDateTimeAsync(DateTime dt);
        Task<Examination> GetExaminationByIdentifierAsync(string examinationIdentifier);
        Task<TestsDto> GetExaminationDtoByIdAsync(int id);
        Task<IEnumerable<Examination>> GetExaminationsWithFilterAsync(string postalCode = null, DateTime? from = null, DateTime? to = null);
		Task<IList<TestsDto>> GetExaminationDtoForParticipantByIdAsync(int id);
	}
}
