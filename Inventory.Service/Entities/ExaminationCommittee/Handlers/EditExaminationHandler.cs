using AutoMapper;
using Inventory.CrossCutting.ExceptionHandling;
using Inventory.CrossCutting.Tenant;
using Inventory.Data.Entities;
using Inventory.Data.Enums;
using Inventory.Data.Models.ExaminationVM;
using Inventory.Data.Models.NotificationVM;
using Inventory.Service.Entities.ExaminationCommittee.Commands;
using Inventory.Service.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Service.Entities.ExaminationCommittee.Handlers
{
  public  class EditExaminationHandler : IRequestHandler<EditExaminationCommand, EditExaminationCommandVM>
    {
        private readonly IExaminationBusiness _examinationBussiness;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly ITenantProvider _tenantProvider;
        private readonly INotificationBussiness _notificationBussiness;
        public EditExaminationHandler(IExaminationBusiness examinationBussiness,
            IMapper mapper, IStringLocalizer<SharedResource> localizer,
            ITenantProvider tenantProvider,
            INotificationBussiness notificationBussiness)
        {
            _tenantProvider = tenantProvider;
            _examinationBussiness = examinationBussiness;
            _mapper = mapper;
            _localizer = localizer;
            _notificationBussiness = notificationBussiness;
        }

        public async Task<EditExaminationCommandVM> Handle(EditExaminationCommand request, CancellationToken cancellationToken)
        {
            EditExaminationCommandVM modeValidation = new EditExaminationCommandVM();
            modeValidation.massage = new List<ValidationMassage>();
            

            ExaminationViewModel newExamination = _mapper.Map<ExaminationViewModel>(request);

            var _oldExaminationCommittee = _examinationBussiness. GetExaminationById(request.Id);
            List <ExaminationItemResult> ResultMessage = new List<ExaminationItemResult>();

            var isValidToEditBudget = _examinationBussiness.ValidateEditExaminationBudget(newExamination, _oldExaminationCommittee);

            if (!isValidToEditBudget)
                throw new InvalidEditExaminationBudget(_localizer["InvalidEditExaminationBudget"]);

            //message.Message = _Localizer["budgetChange"]

            //if (!BudgetValidation.IsSuccess)
            //{
            //    //throw exception
            //    modeValidation.massage.Add(new ValidationMassage()
            //    {  Isbudget = !BudgetValidation.IsSuccess ,message= BudgetValidation.Message });

            //    return await Task.FromResult(modeValidation);
            //}


            //check if examination items are valid to be deleted or updated and return list of error of success messages
           
                ResultMessage  =  _examinationBussiness.ValidateEditExaminationItems(newExamination, _oldExaminationCommittee);


            if (ResultMessage.Any(r => r.IsSuccess == false)&& !request.SaveData)
            {
                var massageResult = ResultMessage.GroupBy(x => new { BaseItemName = x.BaseItemName, Id = x.Id, IsSucess = x.IsSuccess });

                foreach (var item in massageResult)
                {
                    var res = new ValidationMassage() { Id = item.Key.Id, isSuccess = item.Key.IsSucess, BaseItemName = item.Key.BaseItemName };
                    res.message = new List<string>();
                    res.message.AddRange(item.Select(a => a.Message));
                    modeValidation.massage.Add(res);
                }

                modeValidation.isEditSuccess = false;
                modeValidation.Id = request.Id;
            }
            else
            {
                var Result = await _examinationBussiness.EditeExamination(newExamination, _oldExaminationCommittee, ResultMessage);
                modeValidation.Id = Result.Id;
                modeValidation.isEditSuccess = true;
                //NotificationVM notification = new NotificationVM();
                //notification.notificationTemplateEnum = NotificationTemplateEnum.NTF_Addition_Edit;
                //notification.storeId = Convert.ToString(_tenantProvider.GetTenant());
                //notification.FromStore = _tenantProvider.GetTenantName();
                //for (int i = 0; i < 5; i++)
                //{
                    //notification.Id = result.AdditionId.ToString();
                    //notification.code = request.Code;
                //    await _notificationBussiness.SendNotification(notification);
                //}
            }


            return await Task.FromResult(modeValidation);





        }
    }
}
