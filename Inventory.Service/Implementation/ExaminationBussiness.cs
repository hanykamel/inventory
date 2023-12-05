using AutoMapper;
using inventory.Engines.CodeGenerator;
using Inventory.CrossCutting.ExceptionHandling;
using Inventory.CrossCutting.Tenant;
using Inventory.Data.Entities;
using Inventory.Data.Enums;
using Inventory.Data.Models.ExaminationVM;
using Inventory.Data.Models.PrintTemplateVM;
using Inventory.Repository;
using Inventory.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Service.Implementation
{
    public class ExaminationBusiness : IExaminationBusiness
    {
        readonly private IRepository<ExaminationCommitte, Guid> _examinationRepository;
        readonly private IRepository<StoreItem, Guid> _storeItemRepository;
        readonly private IRepository<Addition, Guid> _additionRepository;

        readonly private IRepository<Attachment, Guid> _AttachmentRepository;
        readonly private IRepository<ExchangeOrderStoreItem, Guid> _exchangeOrderStoreItemRepository;
        readonly private IRepository<TransformationStoreItem, Guid> _transformationStoreItemRepository;
        readonly private IRepository<RobbingOrderStoreItem, Guid> _robbingOrderStoreItemRepository;
        readonly private ICodeGenerator _codeGenerator;
        readonly private ITenantProvider _tenantProvider;
        private readonly ILogger<ExaminationBusiness> _logger;
        private readonly IStringLocalizer<SharedResource> _Localizer;
        private readonly IMapper _mapper;
        private readonly IStoreItemBussiness _storeItemBussiness;

        public ExaminationBusiness(IRepository<ExaminationCommitte, Guid> examinationRepository,
            ICodeGenerator codeGenerator,
            ITenantProvider tenantProvider,
            ILogger<ExaminationBusiness> logger,
           IRepository<StoreItem, Guid> StoreItemRepository,
        IRepository<ExchangeOrderStoreItem, Guid> exchangeOrderStoreItemRepository,
              IStringLocalizer<SharedResource> Localizer,
              IRepository<Attachment, Guid> AttachmentRepository,
        IMapper mapper, IRepository<Addition, Guid> additionRepository
            , IStoreItemBussiness storeItemBussiness,
        IRepository<TransformationStoreItem, Guid> transformationStoreItemRepository,
        IRepository<RobbingOrderStoreItem, Guid> robbingOrderStoreItemRepository
            )
        {
            _examinationRepository = examinationRepository;
            _codeGenerator = codeGenerator;
            _tenantProvider = tenantProvider;
            _logger = logger;
            _storeItemRepository = StoreItemRepository;
            _exchangeOrderStoreItemRepository = exchangeOrderStoreItemRepository;
            _Localizer = Localizer;
            _AttachmentRepository = AttachmentRepository;
            _mapper = mapper;
            _additionRepository=additionRepository;
            _storeItemBussiness = storeItemBussiness;
            _transformationStoreItemRepository = transformationStoreItemRepository;
            _robbingOrderStoreItemRepository = robbingOrderStoreItemRepository;
        }

        public object _ExaminationRepository { get; private set; }

        public IQueryable<ExaminationCommitte> GetAllExamination()
        {
            var ExaminationList = _examinationRepository.GetAll();
            return ExaminationList;
        }

        IQueryable<ExaminationCommitte> IExaminationBusiness.PrintExamination()
        {
            return _examinationRepository.GetAll()
                .Include(e => e.ExaminationStatus);
                //.Select(s => new ExaminationListVM()
                //{
                //    Code = s.Code,
                //    Date = s.Date,
                //    ExaminationStatusName = s.ExaminationStatus.Name,
                //    ExaminationStatusId =s.ExaminationStatusId,
                //    TenantId = s.TenantId,
                //    CreationDate = s.CreationDate
                //});
        }

        //public ExaminationCommitte GetExaminationByCodeForAddition(string code)
        //{
        //    var examination = _examinationRepository.GetAll(o=>o.Code == code)
        //        .Include(o=>o.CommitteeItem)
        //        .ThenInclude(o=>o.Unit)
        //        .Include(o => o.CommitteeItem)
        //        .ThenInclude(o => o.BaseItem)
        //        .Include(o=>o.Budget)
        //        .Include(o=>o.Store)
        //        .ThenInclude(o=>o.TechnicalDepartment)
        //        .FirstOrDefault();
        //    if(examination != null)
        //    {

        //    }
        //}
        public ExaminationCommitte GetExaminationById(Guid examinationId)
        {
            return _examinationRepository.GetAll().Include(x => x.Budget)
                .Include(x => x.CommitteeEmployee).ThenInclude(e => e.Emplyee)
                .Include(x => x.CommitteeEmployee).ThenInclude(e => e.JobTitle)
                .Include(x => x.ExternalEntity).Include(x => x.Supplier)
                .Include(x => x.CommitteeItem).ThenInclude(i => i.BaseItem)
                .Include(x => x.CommitteeItem).ThenInclude(i => i.Unit)
                .Include(x=>x.CommitteeAttachment).Include(x=>x.Addition)
                .Where(e => e.Id == examinationId).FirstOrDefault();
        }
        //get max serial of the store and year
        public int GetMax()
        {
            return _examinationRepository.GetMax(null, x => x.Serial) + 1;
        }
        public string GetCode(int serial)
        {

            return _codeGenerator.Generate(serial);
        }
        public async Task<Guid> AddNewExamination(ExaminationCommitte _examinationCommitte)
        {
            
            _examinationCommitte.OperationId = (int)OperationEnum.ExaminationCommittee;
            _examinationCommitte.ExaminationStatusId = (int)ExaminationStatusEnum.DoneExamination;
            _examinationCommitte.Id = Guid.NewGuid();
            _examinationCommitte.StoreId = _tenantProvider.GetTenant();
            _examinationCommitte.Serial = GetMax();
            _examinationCommitte.Code = GetCode(_examinationCommitte.Serial);
            _examinationRepository.Add(_examinationCommitte);
            int added = await _examinationRepository.SaveChanges();
            if (added > 0)
                return await Task.FromResult(_examinationCommitte.Id);
            else
                throw new Exception("No records saved to database");


        }

        public async Task<int> saveExamination()
        {

            int added = await _examinationRepository.SaveChanges();
            return added;
        }
        public string GetCode()
        {
            var serial = GetMax();
            return _codeGenerator.Generate(serial);
        }
        private bool AreAllCommitteeItemsAdded(ExaminationCommitte examinationObj, List<Guid> currentCommitteeItemids)
        {
            //set examination status done addition when all committee items are added
            //get base items of all committe items 
            var committeeItemBaseItems = examinationObj.CommitteeItem.Where(x=>x.Rejected==null||x.Rejected==0).Select(o => o.BaseItemId).Distinct().ToList();
            //get base items of added store items 
            var storeItemBaseItemsList = examinationObj.Addition.Select(o => o.StoreItem.Select(s => s.BaseItemId).ToList()).ToList();
            var storeItemBaseItems = new List<long>();
            foreach (var item in storeItemBaseItemsList)
            {
                storeItemBaseItems.AddRange(item.Distinct());
            }
            //get current committee items base items 
            storeItemBaseItems.AddRange(examinationObj.CommitteeItem.Where(o => currentCommitteeItemids.Contains(o.Id)).Select(o => o.BaseItemId).Distinct().ToList());

            storeItemBaseItems = storeItemBaseItems.Distinct().ToList();
            //check if all base items of committee items exists in all base items of store items 

            return committeeItemBaseItems.All(o => storeItemBaseItems.Contains(o));
        }
        public void updateExaminationForAddition(Guid? examinationCommitteId,
            List<Guid> committeeItemids, List<string> notes)
        {

            var examinationObj = _examinationRepository
                .GetAll(o => o.Id == examinationCommitteId)
                .Include(o => o.CommitteeItem)
                .Include(o => o.Addition)
                .ThenInclude(o => o.StoreItem)
                .FirstOrDefault();

            if (committeeItemids != null&& notes.Any())
                for (int i = 0; i < committeeItemids.Count; i++)
                {
                    if (string.IsNullOrEmpty(notes[i]) || committeeItemids[i] == null || committeeItemids[i] == Guid.Empty)
                        continue;
                    var commiteeItem = examinationObj.CommitteeItem.FirstOrDefault(o => o.Id == committeeItemids[i]);
                    commiteeItem.AdditionNotes = notes[i];
                }

            if (AreAllCommitteeItemsAdded(examinationObj, committeeItemids))
            {
                examinationObj.ExaminationStatusId = (int)Data.Enums.ExaminationStatusEnum.DoneAddition;
            }
            _examinationRepository.Update(examinationObj);

        }
        
        public async Task<ExaminationCommitte> EditeExamination(ExaminationViewModel _examinationViewModel,ExaminationCommitte _examinationCommitte, List<ExaminationItemResult> Massage)
        {
            var _ExaminationCommitteDB = _examinationCommitte;
            #region Update Main Data Examination
            _ExaminationCommitteDB.Date = _examinationViewModel.ExaminationDate;
            _ExaminationCommitteDB.DecisionNumber = _examinationViewModel.SupplyApprovalNumber;
            _ExaminationCommitteDB.DecisionDate = _examinationViewModel.SupplyApprovalDate;
            _ExaminationCommitteDB.SupplyOrderNumber = _examinationViewModel.SupplyOrderNumber;
            _ExaminationCommitteDB.SupplyOrderDate = _examinationViewModel.SupplyOrderDate;
            _ExaminationCommitteDB.ContractNumber = _examinationViewModel.ContractNum;
            _ExaminationCommitteDB.ContractDate = _examinationViewModel.ContractDate;
            _ExaminationCommitteDB.ForConsumedItems = _examinationViewModel.Examinationtype;
          
            _ExaminationCommitteDB.SupplyType = _examinationViewModel.SupplierType;
            if (_examinationViewModel.SupplierType ==(int) SupplierTypeEnum.External)
            {
                if (_examinationViewModel.ExternalEntity != 0)
                {
                    _ExaminationCommitteDB.ExternalEntityId = _examinationViewModel.ExternalEntity;

                }
                else
                {
                    _ExaminationCommitteDB.ExternalEntityId = null;
                }
               
                _ExaminationCommitteDB.SupplierId = null;
            }
            else if(_examinationViewModel.SupplierType == (int)SupplierTypeEnum.Supplier)
            {
                if (_examinationViewModel.Supplier != 0)
                {
                    _ExaminationCommitteDB.SupplierId = _examinationViewModel.Supplier;
                }
                else
                {
                    _ExaminationCommitteDB.SupplierId = null;
                }
                _ExaminationCommitteDB.ExternalEntityId = null;
            }
            else
            {
                _ExaminationCommitteDB.SupplierId = null;
                _ExaminationCommitteDB.ExternalEntityId = null;
            }
            #region UbdateCurrancy
            if(_ExaminationCommitteDB.CurrencyId!= _examinationViewModel.Currency)
            {
                EditStoreItemCurrancy(_ExaminationCommitteDB.Id, _examinationViewModel.Currency);
            }
            _ExaminationCommitteDB.CurrencyId = _examinationViewModel.Currency;
            #endregion

            #region EditAddationBudget
            if (_ExaminationCommitteDB.BudgetId != _examinationViewModel.Budget)
            {
                _ExaminationCommitteDB.BudgetId = _examinationViewModel.Budget;
                foreach (var item in _ExaminationCommitteDB.Addition)
                {
                    item.BudgetId = _examinationViewModel.Budget;
                }
            }


            #endregion
            #endregion

            _ExaminationCommitteDB = EditeExaminationEmployee(_examinationViewModel, _ExaminationCommitteDB);

            _ExaminationCommitteDB = EditeExaminationItem(_examinationViewModel, _ExaminationCommitteDB, Massage);
            //Get New Item Added
            List<ItemsVM> AddItems = _examinationViewModel.AllItems.Where(c => !(_ExaminationCommitteDB.CommitteeItem.Select(x => x.BaseItemId).Contains(c.CategoryId))).ToList();
            //Get New Member Added
            List<membersVM> Addmembers = _examinationViewModel.Allmembers.Where(c => !(_ExaminationCommitteDB.CommitteeEmployee.Select(x => x.EmplyeeId).Contains(c.EmpId))).ToList();

            #region Add new Member
            foreach (var item in Addmembers)
            {
                CommitteeEmployee CommitteeEmployee = new CommitteeEmployee();
                CommitteeEmployee = _mapper.Map<CommitteeEmployee>(item);
                CommitteeEmployee.Id = Guid.NewGuid();
                _ExaminationCommitteDB.CommitteeEmployee.Add(CommitteeEmployee);
            }
            #endregion
            #region Add New Item
            foreach (var item in AddItems)
            {
                CommitteeItem Committeeitem = new CommitteeItem();
                Committeeitem = _mapper.Map<CommitteeItem>(item);
                Committeeitem.Rejected = item.Refused;
                Committeeitem.Id = Guid.NewGuid();
                _ExaminationCommitteDB.CommitteeItem.Add(Committeeitem);
            }
            #endregion
            #region Delete file
            //  Delete file Attachment from CommitteeAttachment and Attachment
            if (_examinationViewModel.FileDelete.Any())
            {

             var Attachment =   _AttachmentRepository.GetAll().Where(x => _examinationViewModel.FileDelete.Contains(x.Id));
                foreach (var item in Attachment)
                {
                    item.IsActive = false;
                    item.IsDelete = true;
                    _ExaminationCommitteDB.CommitteeAttachment.FirstOrDefault(x => x.AttachmentId == item.Id).IsDelete = true;
                    _ExaminationCommitteDB.CommitteeAttachment.FirstOrDefault(x => x.AttachmentId == item.Id).IsActive = false;
                }
            }
            #endregion
            #region Add File
            // Add new  file Attachment in CommitteeAttachment and Attachment
            if (_examinationViewModel._Attachments != null && _examinationViewModel. _Attachments.Any())
                foreach (var attachment in _examinationViewModel._Attachments)
                {
                    Data.Entities.Attachment obj = _mapper.Map<Data.Entities.Attachment>(attachment);
                    _ExaminationCommitteDB.CommitteeAttachment.Add(new CommitteeAttachment() { Id = Guid.NewGuid(), Attachment = obj });
                }

            #endregion
            #region Check Examination State 
            // update Examination Status 
                UpdateExaminationStatus(_ExaminationCommitteDB);
            #endregion
            int added = await _examinationRepository.SaveChanges();
            if (added > 0)
                return  _ExaminationCommitteDB;
            else
                throw new NoChangesInEdit(_Localizer["NoChangesInEdit"]);

        }


        // Check Examination Status and Update Status
        private bool UpdateExaminationStatus(ExaminationCommitte _examinationCommitte)
        {
            var BaseItemId = _examinationCommitte.CommitteeItem.Where(x => x.IsActive != false && (x.Rejected==null ||x.Rejected==0)).Select(x => x.BaseItemId);
            foreach (var item in BaseItemId)
            {
                if (!(_storeItemRepository.GetAll().Include(x=>x.Addition).Where(x=>x.Addition.ExaminationCommitteId == _examinationCommitte.Id && x.BaseItemId == item).Any()))
                {
                    _examinationCommitte.ExaminationStatusId = (int)ExaminationStatusEnum.DoneExamination;
                    return true;

                }
            }

            _examinationCommitte.ExaminationStatusId = (int)ExaminationStatusEnum.DoneAddition;

            return true;
        }


        // Edit Examination Menber if UPDATE Member(Change IsHead) or Delete Member  
        public ExaminationCommitte EditeExaminationEmployee(ExaminationViewModel _examinationViewModel, ExaminationCommitte __ExaminationCommitteDBCommitte)
        {

           var  updateOldEmployee= __ExaminationCommitteDBCommitte.CommitteeEmployee.Where(c => _examinationViewModel.Allmembers.Select(a => a.EmpId).Contains(c.EmplyeeId));
           var  DeleteOldEmployee = __ExaminationCommitteDBCommitte.CommitteeEmployee.Where(c => !(_examinationViewModel.Allmembers.Select(a => a.EmpId).Contains(c.EmplyeeId)));

            if (updateOldEmployee.Any())
            {
                foreach (var item in updateOldEmployee)
                {
                    var emp = _examinationViewModel.Allmembers.FirstOrDefault(x => x.EmpId == item.EmplyeeId);
                    if (emp.IsHead != item.IsHead)
                    {
                        item.IsHead = emp.IsHead;
                    }
                   
                }
            }

            if (DeleteOldEmployee.Any())
            {
                foreach (var item in DeleteOldEmployee)
                {
                 
                    item.IsActive = false;
                    item.IsDelete = true;
                }
            }

            return __ExaminationCommitteDBCommitte;


        }


        //  Delete CommitteeItem from Examination
        void DeleteCommitteeItem(ExaminationCommitte _examinationCommitte,List<long> baseItemIds)
        {
             var deletedCommitteeItems=_examinationCommitte.CommitteeItem
                      .Where(e => baseItemIds.Contains( e.BaseItemId));
            if(deletedCommitteeItems !=null && deletedCommitteeItems.Count()>0)
            {
                foreach (CommitteeItem _CommitteeItem in deletedCommitteeItems)
                {

                    _CommitteeItem.IsDelete = true;
                    _CommitteeItem.IsActive = false;
                }
            }

        }

        // Edit Examination CommitteeItem if UPDATE Item(Change Count,Reasons,UNIT ,ExaminationPercentage) or Delete Item
        public ExaminationCommitte EditeExaminationItem(ExaminationViewModel _examinationViewModel,
            ExaminationCommitte _oldExaminationCommittee, List<ExaminationItemResult> massage)
        {
            //get deleted items
            var deletedBaseItemIds = massage.Where(x => x.IsDelete && x.IsSuccess)
                .Select(x=>x.Id).ToList();

            DeleteCommitteeItem(_oldExaminationCommittee, deletedBaseItemIds);
            DeleteExaminationStoreItem(deletedBaseItemIds, _oldExaminationCommittee.Id);


            var updatedBaseItemIds = massage.Where(x => x.IsUpdate && x.IsSuccess)
                .Select(x => x.Id).ToList();

            //get updated items
            foreach (var updateBaseItemId in updatedBaseItemIds)
            {
                var newCommitteeItem = _examinationViewModel.AllItems
                      .FirstOrDefault(x => x.CategoryId == updateBaseItemId);

                var oldCommitteItem = _oldExaminationCommittee.CommitteeItem
                    .FirstOrDefault(e => e.BaseItemId == updateBaseItemId);

               
                //update accepted and rejected quantity
                oldCommitteItem.ExaminationPercentage = newCommitteeItem.ExaminationPercentage;
                oldCommitteItem.Reasons = newCommitteeItem.Reasons;
                oldCommitteItem.UnitId = newCommitteeItem.UniId;

              
                if (newCommitteeItem.Accepted==0)
                {
                    List<long> baseitemid = new List<long>();
                    baseitemid.Add(updateBaseItemId);
                    DeleteExaminationStoreItem(baseitemid, _oldExaminationCommittee.Id);
                }
                else
                {
                    var diffQuantity = oldCommitteItem.Accepted - newCommitteeItem.Accepted;
                         if (diffQuantity > 0)
                    {
                        //Delete Store Item If not use inExchange Order
                        EditStoreItem(updateBaseItemId, _oldExaminationCommittee.Id, diffQuantity);

                    }

                    else if (diffQuantity < 0)
                    {
                        //Add new Store Item with same Addation Id 
                        var AddQuantity = newCommitteeItem.Quantity - oldCommitteItem.Quantity;
                        AddAdditionStoreItem(AddQuantity, updateBaseItemId, _oldExaminationCommittee);
                    }
                }
           
      
                //add store items

                oldCommitteItem.Quantity = newCommitteeItem.Quantity;
                oldCommitteItem.Accepted = newCommitteeItem.Accepted;
                oldCommitteItem.Rejected = newCommitteeItem.Refused;
            }


            return _oldExaminationCommittee;

        }


        public void DeleteExaminationStoreItem(List<long> BaseItemIds, Guid ExaminationId)
        {
            var storeItems = _storeItemRepository.GetAll()
                .Where(s => BaseItemIds.Contains(s.BaseItemId))
                .Include(s => s.Addition)
                .Include(s => s.ExchangeOrderStoreItem)
                 .Where(s => s.Addition.ExaminationCommitteId == ExaminationId);
            if (storeItems.Any())
            {
                foreach (var storeItem in storeItems)
                {
                    storeItem.IsActive = false;
                    storeItem.IsDelete = true;
                    storeItem.Addition.IsActive = false;
                    storeItem.Addition.IsDelete = true;
                }
                  
              
            }

            //var Addation = _additionRepository.GetAll().Include(x=>x.ExaminationCommitte)
            //    .Where(x => x.ExaminationCommitteId == ExaminationId
            //            && BaseItemIds.Contains(x.ExaminationCommitte.BudgetId));
            //if (Addation.Any())
            //{
            //    foreach (var item in Addation)
            //    {
            //        item.IsActive = false;
            //    }
            //}
        
        }

       
        public void EditStoreItem(long BaseitemId, Guid ExaminationId, int Quantity)
        {
            var storeItems = _storeItemRepository.GetAll().Where(s => s.BaseItemId == BaseitemId)
                                                   .Include(s => s.Addition)
                                                   .Include(s => s.ExchangeOrderStoreItem)
                                              .Where(s => s.Addition.ExaminationCommitteId == ExaminationId
                                              && s.ExchangeOrderStoreItem.Count() == 0
                                              && s.UnderDelete!=true&&s.UnderExecution!=true);

            if (storeItems.Any() && storeItems.Count() >= Quantity)
            {
                    var deletedStoreItems = storeItems.Take(Quantity);
                    foreach (var storeItem in deletedStoreItems)
                {
                    storeItem.IsActive = false;
                    storeItem.IsDelete = true;
                    //if(Quantity== storeItems.Count())
                    //{
                    //    storeItem.Addition.IsActive = false;
                    //}
                }
                       
            }
            else
            {
                //throw exception 
            }
        }



        public void EditStoreItemCurrancy( Guid ExaminationId,int NewCurrencyId)
        {
            var storeItems = _storeItemRepository.GetAll()
                                                   .Include(s => s.Addition)
                                              .Where(s => s.Addition.ExaminationCommitteId == ExaminationId);


                foreach (var storeItem in storeItems)
                {
                storeItem.CurrencyId = NewCurrencyId;
       
                }

      
        }



        public bool ValidateEditExaminationBudget(ExaminationViewModel _examinationViewModel, ExaminationCommitte _oldExaminationCommittee)
        {
            //ExaminationItemResult message = new ExaminationItemResult();
            //message.IsSuccess = true;
            if (_examinationViewModel.Budget!= _oldExaminationCommittee.BudgetId)
            {
                //get the examination committee items 
                var itemIds=_oldExaminationCommittee.CommitteeItem.Select(b => b.BaseItemId);

                //check if any of them has exchange order so we c
                if (_exchangeOrderStoreItemRepository.GetAll()
                        .Include(x => x.StoreItem).ThenInclude(x => x.Addition)
                                   .Where(s => itemIds.Contains(s.StoreItem.BaseItemId) && s.StoreItem.Addition.ExaminationCommitteId == _oldExaminationCommittee.Id).Any())
                {
                    return false;
                } else if (_robbingOrderStoreItemRepository.GetAll()
                  .Include(x => x.StoreItem).ThenInclude(x => x.Addition)
                             .Where(s => itemIds.Contains(s.StoreItem.BaseItemId) && s.StoreItem.Addition.ExaminationCommitteId == _oldExaminationCommittee.Id).Any()) {

                    return false;

                } else if (_transformationStoreItemRepository.GetAll()
                  .Include(x => x.StoreItem).ThenInclude(x => x.Addition)
                             .Where(s => itemIds.Contains(s.StoreItem.BaseItemId) && s.StoreItem.Addition.ExaminationCommitteId == _oldExaminationCommittee.Id).Any())
                {

                    return false;

                };



                //if (result.Any())
                //    return false;


                //{
                //    //message.IsSuccess = false;
                //    //message.Message = _Localizer["budgetChange"];
                //    //return message;
                //}


            }
            return true;


        }



        public List<ExaminationItemResult> ValidateEditExaminationItems(ExaminationViewModel _examinationViewModel, ExaminationCommitte _oldExaminationCommittee)
        {

            List<ExaminationItemResult> resultMessages = new List<ExaminationItemResult>();
            IEnumerable<CommitteeItem> deletedItems;
            IEnumerable<CommitteeItem> updatedItems;

            PrepareData(_examinationViewModel, _oldExaminationCommittee, out deletedItems, out updatedItems);
            if (deletedItems.Any())
            {
                foreach (var item in deletedItems)
                {
                    //check if the base items has any exchange orders

                    var ExchangeOrderMess = CheckExchangeOrderExist_delete(item);
                    var TransformationMess = CheckOperationExist_delete(item);
                    //var RobbingOrderMess = CheckRobbingExist_delete(item);
                    if(!ExchangeOrderMess.IsSuccess|| !TransformationMess.IsSuccess)
                    {
                     if(!ExchangeOrderMess.IsSuccess)
                        resultMessages.Add(ExchangeOrderMess);
                        if (!TransformationMess.IsSuccess)
                            resultMessages.Add(TransformationMess);
                        //if (!RobbingOrderMess.IsSuccess)
                        //    resultMessages.Add(RobbingOrderMess);

                    }
                    else
                    {
                        resultMessages.Add(ExchangeOrderMess);

                    }

                }
            }

            if (updatedItems.Any())
            {
                foreach (var oldItem in updatedItems)
                {
                    var newItem = _examinationViewModel.AllItems.FirstOrDefault(x => x.itemId == oldItem.Id);
                    if(newItem.Accepted< oldItem.Accepted)
                    {
                        var ExchangeOrderCount = CheckExchangeOrderExist_update(_oldExaminationCommittee.Id, newItem);
                        var OperationCount = CheckOperationExist_update(_oldExaminationCommittee.Id, newItem);
                        //var RobbingOrderMess = CheckrobbingOrderExist_update(_oldExaminationCommittee.Id, newItem);
                        // check Result Item Count
                        var ResultCheck = CheckResultStoreItem_update(ExchangeOrderCount, OperationCount, newItem);
                        if (!ResultCheck.IsSuccess)
                        {
                            if (!ResultCheck.IsSuccess)
                                resultMessages.Add(ResultCheck);
                          //  if (!ResultCheck.IsSuccess)
                             //  resultMessages.Add(ResultCheck);
                            //if (!RobbingOrderMess.IsSuccess)
                            //    resultMessages.Add(RobbingOrderMess);

                        }
                        else
                        {
                            resultMessages.Add(ResultCheck);

                        }
                    }
                    //in case quantity the same or greater than it is valid to update
                    else 
                    { 
                        resultMessages.Add(new ExaminationItemResult()
                        { Id = oldItem.BaseItemId, IsUpdate = true, IsSuccess=true, BaseItemName = oldItem.BaseItem.Name, Message = _Localizer["EditSuccess"] });
                    }
                }
            }

            return resultMessages;
        }



        private void PrepareData(ExaminationViewModel _ExaminationViewModel, ExaminationCommitte _oldExaminationCommittee ,out IEnumerable<CommitteeItem> deleteItems,out IEnumerable<CommitteeItem> updateItems)
        {
            deleteItems = _oldExaminationCommittee.CommitteeItem.Where
                (c => !(_ExaminationViewModel.AllItems.Select(a => a.CategoryId).Contains(c.BaseItemId)));
            updateItems = _oldExaminationCommittee.CommitteeItem.Where
                (c => _ExaminationViewModel.AllItems.Select(a => a.CategoryId).Contains(c.BaseItemId));
           
        }

        private ExaminationItemResult CheckExchangeOrderExist_delete(CommitteeItem item)
        {
            ExaminationItemResult validationMassage = new ExaminationItemResult();

            var result = _exchangeOrderStoreItemRepository.GetAll()
                .Include(x => x.StoreItem).ThenInclude(x => x.Addition)
                .Where(s => s.StoreItem.BaseItemId == item.BaseItemId
                && s.StoreItem.Addition.ExaminationCommitteId == item.ExaminationCommitteId);
            if (result.Any())
            {
                validationMassage.BaseItemName = item.BaseItem.Name;
                validationMassage.IsDelete = false;
                validationMassage.Id = item.BaseItemId;
                validationMassage.IsSuccess = false;
                validationMassage.Message =  _Localizer["NotdeleteItem"];

            }
            else
            {
                validationMassage.BaseItemName = item.BaseItem.Name;
                validationMassage.IsDelete = true;
                validationMassage.IsSuccess = true;
                validationMassage.Id = item.BaseItemId;
                validationMassage.Message = _Localizer["EditSuccess"];
            }
            
            return validationMassage;



        }


        private ExaminationItemResult CheckOperationExist_delete(CommitteeItem item)
        {
            ExaminationItemResult validationMassage = new ExaminationItemResult();

            //var result = _transformationStoreItemRepository.GetAll()
            //    .Include(x => x.StoreItem).ThenInclude(x => x.Addition)
            //    .Where(s => s.StoreItem.BaseItemId == item.BaseItemId
            //    && s.StoreItem.Addition.ExaminationCommitteId == item.ExaminationCommitteId);

            var result = _storeItemRepository.GetAll(true)
                .Include(x => x.Addition)
                .Where(s => s.BaseItemId == item.BaseItemId
                && s.Addition.ExaminationCommitteId == item.ExaminationCommitteId
                && s.UnderDelete==true&& s.UnderExecution==true);
            if (result.Any())
            {
                validationMassage.BaseItemName = item.BaseItem.Name;
                validationMassage.IsDelete = false;
                validationMassage.Id = item.BaseItemId;
                validationMassage.IsSuccess = false;
                validationMassage.Message = _Localizer["NotDeletetemOperation"];

            }
            else 
            {
                validationMassage.BaseItemName = item.BaseItem.Name;
                validationMassage.IsDelete = true;
                validationMassage.IsSuccess = true;
                validationMassage.Id = item.BaseItemId;
                validationMassage.Message = _Localizer["EditSuccess"];
            }

            return validationMassage;



        }


        private ExaminationItemResult CheckRobbingExist_delete(CommitteeItem item)
        {
            ExaminationItemResult validationMassage = new ExaminationItemResult();

            var result = _robbingOrderStoreItemRepository.GetAll()
                .Include(x => x.StoreItem).ThenInclude(x => x.Addition)
                .Where(s => s.StoreItem.BaseItemId == item.BaseItemId
                && s.StoreItem.Addition.ExaminationCommitteId == item.ExaminationCommitteId);
            if (result.Any())
            {
                validationMassage.BaseItemName = item.BaseItem.Name;
                validationMassage.IsDelete = false;
                validationMassage.Id = item.BaseItemId;
                validationMassage.IsSuccess = false;
                validationMassage.Message = _Localizer["NotdeleteItemRobbing"];

            }
            else 
            {
                validationMassage.BaseItemName = item.BaseItem.Name;
                validationMassage.IsDelete = true;
                validationMassage.IsSuccess = true;
                validationMassage.Id = item.BaseItemId;
                validationMassage.Message = _Localizer["EditSuccess"];
            }

            return validationMassage;



        }


        private int CheckExchangeOrderExist_update(Guid ExaminationId,ItemsVM item)
        {
            ExaminationItemResult validationMassage = new ExaminationItemResult();

            var result = _exchangeOrderStoreItemRepository.GetAll()
                .Include(x => x.StoreItem)
                .ThenInclude(x => x.Addition)
                .Where(s => s.StoreItem.BaseItemId == item.CategoryId 
                && s.StoreItem.Addition.ExaminationCommitteId == ExaminationId)
                .Select(s=>s.StoreItemId).Distinct().Count();

            //if (result> item.Accepted)
            //{
            //    validationMassage.BaseItemName = item.CategoryName;
            //    validationMassage.IsUpdate = false;
            //    validationMassage.Id = item.CategoryId;
            //    validationMassage.IsSuccess = false;
            //    validationMassage.Message = _Localizer["NotUpdateItem"];
            //}
            //else 
            //{
            //    validationMassage.BaseItemName = item.CategoryName;
            //    validationMassage.IsUpdate = true;
            //    validationMassage.IsSuccess = true;
            //    validationMassage.Id = item.CategoryId;
            //    validationMassage.Message = _Localizer["EditSuccess"];
            //}
            return result;
        }

        // Check Result StoreItem with new Accepted Item
        private ExaminationItemResult CheckResultStoreItem_update(int ExchangeOrderCount, int OperationCount, ItemsVM item) {

            ExaminationItemResult validationMassage = new ExaminationItemResult();

            if (ExchangeOrderCount+ OperationCount > item.Accepted)
            {
                validationMassage.BaseItemName = item.CategoryName;
                validationMassage.IsUpdate = false;
                validationMassage.Id = item.CategoryId;
                validationMassage.IsSuccess = false;
                validationMassage.Message = ExchangeOrderCount >0&& OperationCount>0? _Localizer["ResultStoreItem"]: OperationCount > 0? _Localizer["NotUpdateItemOperation"] : _Localizer["NotUpdateItem"];
            }
            else
            {
                validationMassage.BaseItemName = item.CategoryName;
                validationMassage.IsUpdate = true;
                validationMassage.IsSuccess = true;
                validationMassage.Id = item.CategoryId;
                validationMassage.Message = _Localizer["EditSuccess"];
            }
            return validationMassage;


        }




        private int CheckOperationExist_update(Guid ExaminationId, ItemsVM item)
        {
            ExaminationItemResult validationMassage = new ExaminationItemResult();
            var result = _storeItemRepository.GetAll(true)
                .Include(x => x.Addition)
                .Where(s => s.BaseItemId == item.CategoryId
               && s.Addition.ExaminationCommitteId == ExaminationId && (s.UnderDelete==true || s.UnderExecution==true))
               .Select(s => s.Id).Distinct().Count();

            //if (result > item.Accepted)
            //{
            //    validationMassage.BaseItemName = item.CategoryName;
            //    validationMassage.IsUpdate = false;
            //    validationMassage.Id = item.CategoryId;
            //    validationMassage.IsSuccess = false;
            //    validationMassage.Message = _Localizer["NotUpdateItemOperation"];
            //}
            //else 
            //{
            //    validationMassage.BaseItemName = item.CategoryName;
            //    validationMassage.IsUpdate = true;
            //    validationMassage.IsSuccess = true;
            //    validationMassage.Id = item.CategoryId;
            //    validationMassage.Message = _Localizer["EditSuccess"];
            //}
            return result;
        }


        //private ExaminationItemResult CheckrobbingOrderExist_update(Guid ExaminationId, ItemsVM item)
        //{
        //    ExaminationItemResult validationMassage = new ExaminationItemResult();

        //    var result = _robbingOrderStoreItemRepository.GetAll()
        //        .Include(x => x.StoreItem)
        //        .ThenInclude(x => x.Addition)
        //        .Where(s => s.StoreItem.BaseItemId == item.CategoryId
        //        && s.StoreItem.Addition.ExaminationCommitteId == ExaminationId)
        //        .Select(s => s.StoreItemId).Distinct().Count();

        //    if (result > item.Accepted)
        //    {
        //        validationMassage.BaseItemName = item.CategoryName;
        //        validationMassage.IsUpdate = false;
        //        validationMassage.Id = item.CategoryId;
        //        validationMassage.IsSuccess = false;
        //        validationMassage.Message = _Localizer["NotUpdateItemRobbing"];
        //    }
        //    else 
        //    {
        //        validationMassage.BaseItemName = item.CategoryName;
        //        validationMassage.IsUpdate = true;
        //        validationMassage.IsSuccess = true;
        //        validationMassage.Id = item.CategoryId;
        //        validationMassage.Message = _Localizer["EditSuccess"];
        //    }
        //    return validationMassage;
        //}


        private void AddAdditionStoreItem(int Quantity, long BaseItemId, ExaminationCommitte OldExaminationCommitte)
        {
            var storeItems = _storeItemRepository.GetAll().Where(s => s.BaseItemId == BaseItemId)
                                                  .Include(s => s.Addition)
                                             .Where(s => s.Addition.ExaminationCommitteId == OldExaminationCommitte.Id);

            if (storeItems.Any())
            {

                var FriststoreItems = storeItems.FirstOrDefault();

                var addedStorItems = new List<StoreItem>();
                for (int i = 0; i < Quantity; i++)
                {
                    FriststoreItems.Addition.StoreItem.Add(new StoreItem()
                    {

                        BaseItemId = BaseItemId,
                        Id = Guid.NewGuid(),
                        BookId = FriststoreItems.BookId,
                        BookPageNumber = FriststoreItems.BookPageNumber,
                        Note = FriststoreItems.Note,
                        StoreId = FriststoreItems.StoreId,
                        CurrentItemStatusId = (int)ItemStatusEnum.Available,
                        CurrencyId = FriststoreItems.CurrencyId,
                        StoreItemStatusId = FriststoreItems.StoreItemStatusId,
                        UnitId= FriststoreItems.UnitId,
                        Price= FriststoreItems.Price
                    });
                }

                _storeItemBussiness.GenerateBarcode(FriststoreItems.Addition.StoreItem
                    .TakeLast(Quantity).ToList(),
                                FriststoreItems.Addition.BudgetId, BaseItemId);

            }
           
        }

    }
}
