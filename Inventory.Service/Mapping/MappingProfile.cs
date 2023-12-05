using AutoMapper;
using Inventory.Data.Entities;
using Inventory.Data.Models.AdditionVM;
using Inventory.Data.Models.AttachmentVM;
using Inventory.Data.Models.ExaminationVM;
using Inventory.Data.Models.Inquiry;
using Inventory.Data.Models.PrintTemplateVM;
using Inventory.Data.Models.Shared;
using Inventory.Service.Entities.AdditionRequest.Commands;
using Inventory.Service.Entities.DeductionRequest.Commands;
using Inventory.Service.Entities.DelegationRequest.Commands;
using Inventory.Service.Entities.ExaminationCommittee.Commands;
using Inventory.Service.Entities.ExecutionOrderRequest.Commands;
using Inventory.Service.Entities.InquiryRequest.Commands;
using Inventory.Service.Entities.RefundOrderRequest.Commands;
using Inventory.Service.Entities.ReportRequest.Commands;
using Inventory.Service.Entities.RobbingOrderRequest.Commands;
using Inventory.Service.Entities.TransformationRequest.Commands;
using System;

namespace Inventory.Service.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Add as many of these lines as you need to map your objects
            CreateMap<BaseItem, BaseItemVm>();
            CreateMap<BaseItemVm, BaseItem>();
            CreateMap<StoreItemStatus, LookupVM<int>>();
            CreateMap<LookupVM<int>, StoreItemStatus>();


            CreateMap<StoreItem, StoreItemVM>()
                .ForMember(e => e.Notes, o => o.MapFrom(e => e.Note))
                .ForMember(e => e.Name, o => o.MapFrom(e => e.BaseItem.Name))
                .ForMember(e => e.BaseItem, o => o.MapFrom(e => e.BaseItem))
                .ForMember(e => e.ContractNumber, o => o.MapFrom(e => e.Addition.ExaminationCommitte.ContractNumber))
                .ForMember(e => e.CurrencyId, o => o.MapFrom(e => e.CurrencyId))
                .ForMember(e => e.CurrencyName, o => o.MapFrom(e => e.Currency.Name))
                .ForMember(e => e.PageNumber, o => o.MapFrom(e => e.BookPageNumber))
                .ForMember(e => e.StoreItemStatusValue, o => o.MapFrom(e => e.StoreItemStatus))
             .ForMember(e => e.UnitName, o => o.MapFrom(e => e.Unit.Name));
            CreateMap<StoreItemVM, StoreItem>()
                .ForMember(e => e.Note, o => o.MapFrom(e => e.Notes));

            CreateMap<Attachment, AttachmentVM>();
            CreateMap<AttachmentVM, Attachment>();

            CreateMap<CreateAdditionCommand, Addition>()
                .ForMember(e => e.TransformationId, o => o.MapFrom(e => e.RequestId))
                .ForMember(e => e.RobbingOrderId, o => o.MapFrom(e => e.RobbingOrderId))
                .ForMember(e => e.AdditionAttachment, o => o.Ignore());

            //CreateMap<EditAdditionCommand, Addition>()
            //    .ForMember(e => e.Id, o => o.MapFrom(e => e.AdditionId))
            //    .ForMember(e => e.AdditionDocumentDate, o => o.MapFrom(e => e.AdditionDocumentDate))
            //    .ForMember(e => e.AdditionDocumentNumber, o => o.MapFrom(e => e.AdditionDocumentNumber))
            //    .ForMember(e => e.AdditionDocumentTypeId, o => o.MapFrom(e => e.AdditionDocumentTypeId))
            //    .ForMember(e => e.BudgetId, o => o.MapFrom(e => e.BudgetId))
            //    .ForMember(e => e.Code, o => o.MapFrom(e => e.Code))
            //    .ForMember(e => e.RequestDate, o => o.MapFrom(e => e.RequestDate))
            //    .ForMember(e => e.RequesterName, o => o.MapFrom(e => e.RequesterName))
            //    .ForMember(e => e.AdditionAttachment, o => o.Ignore());
            CreateMap<RobbingStoreItemVm, RobbedStoreItem>();
            CreateMap<AdditionItemVM, StoreItem>();
            CreateMap<CreateAdditionCommand, StoreItem>();

            CreateMap<AddTransforemationCommand, Transformation>();
            CreateMap<AddRobbingOrderCommand, RobbingOrder>();
            CreateMap<RobbingExecutionOrderRemainsCommand, RobbingOrder>();
            CreateMap<CreateRefundOrderCommand, RefundOrder>();

            CreateMap<InquiryStoreItemsCommand, InquiryStoreItemsRequest>();
            CreateMap<Invoice, InvoiceReportVM>()
                .ForMember(s => s.TenantId, d => d.MapFrom(o => o.TenantId))
                .ForMember(s => s.Date, d => d.MapFrom(o => o.Date))
                .ForMember(s => s.Code, d => d.MapFrom(o => o.Code));
            //CreateMap<InvoiceStoreItem, InvoiceStoreItemsReportVM>()
            //    .ForMember(s => s.Code, d => d.MapFrom(o => o.StoreItem.Code))
            //    .ForMember(s => s.StoreItem, d => d.MapFrom(o => o.StoreItem.BaseItem.Name))
            //    .ForMember(s => s.Unit, d => d.MapFrom(o => o.StoreItem.Unit.Name))
            //    .ForMember(s => s.Price, d => d.MapFrom(o => o.StoreItem.Price))
            //    .ForMember(s => s.Quantity, d => d.MapFrom(o => 1));

            CreateMap<PrintStagnantCommand, StagnantModelVM>()
                .ForMember(s => s.DateFrom, d => d.MapFrom(o => o.DateFrom))
                .ForMember(s => s.StoreItems, d => d.MapFrom(o => o.StoreItems));


            CreateMap<Category, ItemsVM>();
            CreateMap<members, membersVM>();
            CreateMap<AttachmentVM, EditAttachment>();
            CreateMap<EditExaminationCommand, ExaminationViewModel>()
              .ForMember(s => s.AllItems, d => d.MapFrom(o => o.AllCategory))
              .ForMember(s => s.Allmembers, d => d.MapFrom(o => o.Members));






            CreateMap<Category, CommitteeItem>()
             .ForMember(s => s.BaseItemId, d => d.MapFrom(o => o.CategoryId))
         .ForMember(s => s.Accepted, d => d.MapFrom(o => o.Accepted))
        .ForMember(s => s.Reasons, d => d.MapFrom(o => o.Reasons))
         .ForMember(s => s.Rejected, d => d.MapFrom(o => o.Refused))
        .ForMember(s => s.UnitId, d => d.MapFrom(o => o.UniId))
         .ForMember(s => s.Quantity, d => d.MapFrom(o => o.Quantity))
        .ForMember(s => s.ExaminationPercentage, d => d.MapFrom(o => o.ExaminationPercentage));

            CreateMap<members, CommitteeEmployee>()
              .ForMember(s => s.EmplyeeId, d => d.MapFrom(o => o.EmpId))
         .ForMember(s => s.IsHead, d => d.MapFrom(o => o.IsHead))
        .ForMember(s => s.JobTitleId, d => d.MapFrom(o => o.JobId));

            CreateMap<membersVM, CommitteeEmployee>()
         .ForMember(s => s.EmplyeeId, d => d.MapFrom(o => o.EmpId))
    .ForMember(s => s.IsHead, d => d.MapFrom(o => o.IsHead))
   .ForMember(s => s.JobTitleId, d => d.MapFrom(o => o.JobId));


            CreateMap<ItemsVM, CommitteeItem>()
         .ForMember(s => s.BaseItemId, d => d.MapFrom(o => o.CategoryId))
         .ForMember(s => s.Accepted, d => d.MapFrom(o => o.Accepted))
        .ForMember(s => s.Reasons, d => d.MapFrom(o => o.Reasons))
         .ForMember(s => s.Rejected, d => d.MapFrom(o => o.Refused))
        .ForMember(s => s.UnitId, d => d.MapFrom(o => o.UniId))
         .ForMember(s => s.Quantity, d => d.MapFrom(o => o.Quantity))
        .ForMember(s => s.ExaminationPercentage, d => d.MapFrom(o => o.ExaminationPercentage));

            CreateMap<EditExaminationCommand, ExaminationCommitte>()
                  .ForMember(s => s.Id, d => d.MapFrom(o => o.Id))
           .ForMember(s => s.Date, d => d.MapFrom(o => o.ExaminationDate))
           .ForMember(s => s.BudgetId, d => d.MapFrom(o => o.Budget))
          .ForMember(s => s.DecisionNumber, d => d.MapFrom(o => o.SupplyApprovalNumber))
           .ForMember(s => s.DecisionDate, d => d.MapFrom(o => o.SupplyApprovalDate))
          .ForMember(s => s.SupplyOrderNumber, d => d.MapFrom(o => o.SupplyOrderNumber))
           .ForMember(s => s.SupplyOrderDate, d => d.MapFrom(o => o.SupplyOrderDate))
          .ForMember(s => s.ContractNumber, d => d.MapFrom(o => o.ContractNum))
          .ForMember(s => s.ContractDate, d => d.MapFrom(o => o.ContractDate))
            .ForMember(s => s.ForConsumedItems, d => d.MapFrom(o => o.Examinationtype))
            .ForMember(s => s.ExternalEntityId, d => d.MapFrom(o => o.ExternalEntity))
            .ForMember(s => s.Code, d => d.MapFrom(o => o.ExaminationNum))
            .ForMember(s => s.SupplierId, d => d.MapFrom(o => o.Supplier))
             .ForMember(s => s.CommitteeItem, d => d.MapFrom(o => o.AllCategory))
              .ForMember(s => s.CommitteeAttachment, d => d.MapFrom(o => o.Attachments))
            .ForMember(s => s.CommitteeEmployee, d => d.MapFrom(o => o.Members));

            CreateMap<EditAttachment, Attachment>();
            CreateMap<AddDelegationCommand, Delegation>();


            CreateMap<CreateExecutionOrderCommand, ExecutionOrder>();

            CreateMap<ExecutionOrderResultItemModel, ExecutionOrderResultItem>();

            CreateMap<ExecutionOrderResultRemainModel, ExecutionOrderResultRemain>();

            CreateMap<AddTransforemationCommand, Subtraction>();
            CreateMap<AddRobbingOrderCommand, Subtraction>();
            CreateMap<CreateExecutionOrderCommand, Subtraction>();
            CreateMap<RobbingExecutionOrderRemainsCommand, Subtraction>();
            CreateMap<SaveDeductionCommand, Subtraction>();

            CreateMap<AddDelegationCommand, Delegation>()
                  .ForMember(s => s.ShiftId, d => d.MapFrom(o => o.ShiftId))
                   .ForMember(s => s.DateFrom, d => d.MapFrom(o => DateTime.Parse(o.DateFrom)))
                    .ForMember(s => s.DateTo, d => d.MapFrom(o => DateTime.Parse(o.DateTo)))
                     .ForMember(s => s.UserName, d => d.MapFrom(o => o.UserName))
                      .ForMember(s => s.DelegationTypeId, d => d.MapFrom(o => o.DelegationType))
                        .ForMember(s => s.TimeFrom, d => d.MapFrom(o => new TimeSpan(o.TimeFrom.Houre, o.TimeFrom.Minute, o.TimeFrom.Secand)))
                          .ForMember(s => s.TimeTo, d => d.MapFrom(o => new TimeSpan(o.TimeTo.Houre, o.TimeTo.Minute, o.TimeTo.Secand)))
             .ForMember(c => c.TenantId, option => option.Ignore())
              .ForMember(c => c.DelegationStore, option => option.Ignore())
               .ForMember(c => c.Shift, option => option.Ignore())
                .ForMember(c => c.DelegationType, option => option.Ignore())
                 .ForMember(c => c.IsSuspended, option => option.Ignore())
                  .ForMember(c => c.IsActive, option => option.Ignore());


        }
    }
}
