using AutoMapper;
using Inventory.CrossCutting.Tenant;
using Inventory.Data.Entities;
using Inventory.Data.Enums;
using Inventory.Service.Entities.ExaminationCommittee.Commands;
using Inventory.Service.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Service.Entities.ExaminationCommittee.Handlers
{
    public class AddExaminationHandler : IRequestHandler<AddExaminationCommand, Guid>
    {
        private readonly IExaminationBusiness _iExaminationBussiness;
        private readonly IMapper _mapper;

        public AddExaminationHandler(IExaminationBusiness iExaminationBussiness, IMapper mapper)
        {
            _iExaminationBussiness = iExaminationBussiness;
            _mapper = mapper;
        }
        public async Task<Guid> Handle(AddExaminationCommand request, CancellationToken cancellationToken)
        {
            ExaminationCommitte model = new ExaminationCommitte();

            model.Date = request.ExaminationDate;
            model.BudgetId = request.Budget;
            model.DecisionNumber = request.SupplyApprovalNumber;
            model.DecisionDate = request.SupplyApprovalDate;
            model.SupplyOrderNumber = request.SupplyOrderNumber;
            model.SupplyOrderDate = request.SupplyOrderDate;
            model.ContractNumber = request.ContractNum;
            model.ContractDate = request.ContractDate;
            model.ForConsumedItems = request.Examinationtype;
            model.CurrencyId = request.Currency;
            model.SupplyType = request.SupplierType;
            if (request.SupplierType ==(int) SupplierTypeEnum.External)
            {
                if(request.ExternalEntity != 0)
                {
                    model.ExternalEntityId = request.ExternalEntity;

                }
            }
            else  if(request.SupplierType == (int)SupplierTypeEnum.Supplier)
            {
                if (request.Supplier != 0)
                {
                    model.SupplierId = request.Supplier;

                }

            }

            if (request.Members != null)
            {
                foreach (var itemmember in request.Members)
                {
                    CommitteeEmployee emp = new CommitteeEmployee();
                    emp.Id = Guid.NewGuid();
                    emp.EmplyeeId = itemmember.EmpId;
                    emp.IsHead = itemmember.IsHead;
                    emp.JobTitleId = itemmember.JobId;
                    model.CommitteeEmployee.Add(emp);
                }

            }
            //code Review
            foreach (var itemCategory in request.AllCategory)
            {
                CommitteeItem item = new CommitteeItem();
                item.Id = Guid.NewGuid();
                item.BaseItemId = itemCategory.CategoryId;
                item.Accepted = itemCategory.Accepted;
                item.Reasons = itemCategory.Reasons;
                item.Rejected = itemCategory.Refused;
                item.UnitId = itemCategory.UniId;
                item.Quantity = itemCategory.Quantity;
                item.ExaminationPercentage = itemCategory.ExaminationPercentage;
                model.CommitteeItem.Add(item);
            }
            model.CommitteeAttachment = new List<CommitteeAttachment>();

            if (request.Attachments != null && request.Attachments.Count > 0)
                foreach (var attachment in request.Attachments)
                {
                    //code Review
                    Data.Entities.Attachment obj = _mapper.Map<Data.Entities.Attachment>(attachment);
                    model.CommitteeAttachment.Add(new CommitteeAttachment() { Id = Guid.NewGuid(), Attachment = obj });
                }

            return await _iExaminationBussiness.AddNewExamination(model);
            

        }
    }
}
