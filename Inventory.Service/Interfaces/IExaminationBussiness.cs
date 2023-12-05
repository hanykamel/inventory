
using Inventory.Data.Entities;
using Inventory.Data.Models.ExaminationVM;
using Inventory.Data.Models.PrintTemplateVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Service.Interfaces
{
    public interface IExaminationBusiness
    {
        ExaminationCommitte GetExaminationById(Guid examinationId);

        IQueryable<ExaminationCommitte> GetAllExamination();
        IQueryable<ExaminationCommitte> PrintExamination();

        Task<Guid> AddNewExamination(ExaminationCommitte _examinationCommitte);
        string GetCode(int max);
        string GetCode();        
        void updateExaminationForAddition(Guid? examinationCommitteId, List<Guid> list1, List<string> list2);
        Task<ExaminationCommitte> EditeExamination(ExaminationViewModel _examinationViewModel, ExaminationCommitte _examinationCommitte, List<ExaminationItemResult> Massage);
        List<ExaminationItemResult> ValidateEditExaminationItems(ExaminationViewModel _examinationViewModel, ExaminationCommitte _oldExaminationCommittee);
        Task<int> saveExamination();

        bool ValidateEditExaminationBudget(ExaminationViewModel _examinationViewModel, ExaminationCommitte __ExaminationCommitteDBCommitte);

    }
}
