using AutoMapper;
using inventory.Engines.CodeGenerator;
using Inventory.CrossCutting.ExceptionHandling;
using Inventory.CrossCutting.Resources;
using Inventory.CrossCutting.Tenant;
using Inventory.Data.Entities;
using Inventory.Data.Enums;
using Inventory.Data.Models.AdditionVM;
using Inventory.Data.Models.PrintTemplateVM;
using Inventory.Data.Models.StoreItemVM;
using Inventory.Repository;
using Inventory.Service.Entities.AdditionRequest.Commands;
using Inventory.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Service.Implementation
{
    public class AdditionBussiness : IAdditionBussiness
    {
        readonly private IRepository<Addition, Guid> _additionRepository;
        private readonly ICodeGenerator codeGenerator;
        private readonly ITenantProvider tenantProvider;
        private readonly IStringLocalizer<SharedResource> _Localizer;
        readonly private IRepository<Attachment, Guid> _AttachmentRepository;
        readonly private IRepository<AdditionAttachment, Guid> additionAttachment;
        private readonly IMapper _mapper;
        public AdditionBussiness(IRepository<Addition, Guid> additionRepository,
            ITenantProvider tenantProvider,
            ICodeGenerator codeGenerator,
            IStringLocalizer<SharedResource> Localizer,
            IRepository<Attachment, Guid> AttachmentRepository,
            IRepository<AdditionAttachment, Guid> _additionAttachment,
            IMapper mapper
            )
        {
            _additionRepository = additionRepository;
            this.codeGenerator = codeGenerator;
            this.tenantProvider = tenantProvider;
            _Localizer = Localizer;
            _AttachmentRepository = AttachmentRepository;
            additionAttachment = _additionAttachment;
            _mapper = mapper;
        }
        public async Task<Guid> AddNewAddition(Addition _Addition)
        {
            _Addition.Id = Guid.NewGuid();
            _Addition.OperationId = (int)OperationEnum.Addition;
            _Addition.Serial = GetMax();
            _Addition.Code = this.GetCode(_Addition.Serial);

            _additionRepository.Add(_Addition);
            int added = await _additionRepository.SaveChanges();
            if (added > 0)
                return await Task.FromResult(_Addition.Id);
            else
                throw new NotSavedException(_Localizer["NotSavedException"]);
        }

        public IQueryable<Addition> GetAllAddition()
        {
            var AdditionList = _additionRepository.GetAll();
            return AdditionList;
        }




        public IQueryable<Addition> GetAdditionView()
        {
            var Addition = _additionRepository.GetAll(true);
            return Addition;
        }
        public IQueryable<AdditionListVM> PrintAdditionList()
        {
            return _additionRepository.GetAll()
                .Select(s => new AdditionListVM()
                {
                    Code = s.Code,
                    AdditionNumber = s.AdditionNumber,
                    Date = s.RequestDate,
                    TenantId = s.TenantId,
                    CreationDate = s.CreationDate
                });
        }

        public Addition GetAdditionById(Guid additionId)
        {
            return _additionRepository.GetAll(true)
               .Include(a => a.Budget)
               .Include(a => a.AdditionDocumentType)
               .Include(a => a.ExaminationCommitte)
               .Include(a => a.StoreItem)
               .Where(e => e.Id == additionId )
                .FirstOrDefault();
        }
        public bool Activate(Guid AdditionId)
        {
            var AdditionEntity = _additionRepository.Get(AdditionId);
            AdditionEntity.IsActive = AdditionEntity.IsActive == true ? false : true;
            return true;
        }

        public async Task<bool> UpdateAddition(EditAdditionCommand request)
        {
            Addition oldAddition = GetAdditionById(request.AdditionId);

            oldAddition.AdditionDocumentDate = request.AdditionDocumentDate;
            oldAddition.AdditionDocumentNumber = request.AdditionDocumentNumber;
            oldAddition.AdditionDocumentTypeId = request.AdditionDocumentTypeId;
            oldAddition.BudgetId = request.BudgetId;
            oldAddition.Note = request.Note;
            oldAddition.RequestDate = request.RequestDate;
            oldAddition.RequesterName = request.RequesterName;
            oldAddition.AdditionNumber = request.AdditionNumber;
            oldAddition.Date = request.Date;
            for (int i = 0; i < oldAddition.StoreItem.Count; i++)
            {
                StoreItem storeItem = oldAddition.StoreItem.ElementAt(i);
                AdditionItemVM item = request.Items.FirstOrDefault(s => s.BaseItemId == storeItem.BaseItemId);
                storeItem.Price = item.Price;
                storeItem.BookId = item.BookId;
                storeItem.BookPageNumber = item.BookPageNumber;
                storeItem.StoreItemStatusId = item.StoreItemStatusId;
                storeItem.Note = item.Note;
                storeItem.CurrencyId = item.CurrencyId;
            }
            if (request.FileDelete != null && request.FileDelete.Any())
            {
                var Attachment = additionAttachment.GetAll()
                    .Where(a => request.FileDelete.Contains(a.AttachmentId))
                    .Include(aa=>aa.Attachment);
                //var Attachment = _AttachmentRepository.GetAll().Where(x => request.FileDelete.Contains(x.Id));
                foreach (var item in Attachment)
                {
                    item.IsActive = false;
                    item.Attachment.IsActive = false;
                }
            }

            if (request.AdditionAttachment != null && request.AdditionAttachment.Any())
                foreach (var attachment in request.AdditionAttachment)
                {
                    Data.Entities.Attachment obj = _mapper.Map<Data.Entities.Attachment>(attachment);
                    oldAddition.AdditionAttachment.Add(new AdditionAttachment() { Id = Guid.NewGuid(), Attachment = obj });
                }


            _additionRepository.Update(oldAddition);
            int added = await _additionRepository.SaveChanges();
            if (added > 0)
                return await Task.FromResult(true);
            else
                return await Task.FromResult(false);

        }

        public Addition ViewAddition(Guid AdditionId)
        {
            var AdditionEntity = _additionRepository.Get(AdditionId);
            return AdditionEntity;
        }
        public int GetMax()
        {
            return _additionRepository.GetMax(null, x => x.Serial) + 1;
        }
        public int GetMaxAdditionNumber()
        {
            return _additionRepository.GetMax(null, x => Convert.ToInt32(x.AdditionNumber))+1;
        }
        public string GetCode(int serial)
        {

            return codeGenerator.Generate(serial);
        }

        public string GetCode()
        {
            var serial = GetMax();
            return codeGenerator.Generate(serial);
        }

        public string GetLastCode()
        {
            var lastAddedObj = _additionRepository.GetAll().OrderByDescending(o => o.CreationDate).FirstOrDefault();
            if (lastAddedObj != null)
            {
                return lastAddedObj.Code;
            }
            return "";
        }
    }
}
