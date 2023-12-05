using Inventory.Data.Entities;
using Inventory.Data.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inventory.Data
{
    public class DbInitializer
    {
        public static void Initialize(InventoryContext context)
        {
            // context.Database.EnsureCreated();
            context.Database.Migrate();
            //AttachmentType 
            if (!context.AttachmentType.Any())
            {
                var attachmentTypes = new AttachmentType[]
                {
                new AttachmentType {Id = (int)AttachmentTypeEnum.Template_12 , Name="نموذج حكومة 12" ,IsActive=true},
                new AttachmentType {Id = (int)AttachmentTypeEnum.Template_2 , Name="نموذج 2 مخازن حكومة" ,IsActive=true},
                new AttachmentType {Id = (int)AttachmentTypeEnum.Template_8 , Name="نموذج 8 مخازن حكومة" ,IsActive=true},
                new AttachmentType {Id = (int)AttachmentTypeEnum.Template_6 , Name="نموذج حكومة 6" ,IsActive=true},
                new AttachmentType {Id = (int)AttachmentTypeEnum.Template_9 , Name="نموذج حكومة 9" ,IsActive=true},
                new AttachmentType {Id = (int)AttachmentTypeEnum.Contract , Name="العقد" ,IsActive=true},
                new AttachmentType {Id = (int)AttachmentTypeEnum.SupplyOrder , Name="امر التوريد" ,IsActive=true},
                new AttachmentType {Id = (int)AttachmentTypeEnum.Receipt , Name="اصل الفاتورة" ,IsActive=true},
                new AttachmentType {Id = (int)AttachmentTypeEnum.AdministrativeCertificate , Name="شهادة ادارية" ,IsActive=true},
                new AttachmentType {Id = (int)AttachmentTypeEnum.Others , Name="اخرى" ,IsActive=true},
                   new AttachmentType {Id = (int)AttachmentTypeEnum.TechnicalExamination , Name="فحص فنى" ,IsActive=true},
                    new AttachmentType {Id = (int)AttachmentTypeEnum.StockTaking ,Name ="محضرالجرد" ,IsActive=true}



                };
                context.AttachmentType.AddRange(attachmentTypes);
                context.SaveChanges();
            }
            if (!context.ExchangeOrderStatus.Any())
            {
                var ExchangeOrderStatus = new ExchangeOrderStatus[]
                {
                new ExchangeOrderStatus {Id = (int)ExchangeOrderStatusEnum.Pending ,IsActive=true, Name="قيد الانتظار" },
                new ExchangeOrderStatus {Id = (int)ExchangeOrderStatusEnum.Reviewed ,IsActive=true, Name="تم المراجعة" },
                new ExchangeOrderStatus {Id = (int)ExchangeOrderStatusEnum.PaidOff ,IsActive=true, Name="تم الصرف" },
                new ExchangeOrderStatus {Id = (int)ExchangeOrderStatusEnum.Canceled ,IsActive=true, Name="ملغي" },
                    new ExchangeOrderStatus {Id = (int)ExchangeOrderStatusEnum.ItemsAvailable ,IsActive=true, Name="تم إتاحةالاصناف" }
                };
                context.ExchangeOrderStatus.AddRange(ExchangeOrderStatus);
                context.SaveChanges();
            }
            if (!context.ExecutionOrderStatus.Any())
            {
                var ExecutionOrderStatus = new ExecutionOrderStatus[]
                {
                new ExecutionOrderStatus {Id = (int)ExecutionOrderStatusEnum.Pending ,IsActive=true, Name="قيد الانتظار" },
                new ExecutionOrderStatus {Id = (int)ExecutionOrderStatusEnum.Reviewed ,IsActive=true, Name="تم المراجعة" },
                new ExecutionOrderStatus {Id = (int)ExecutionOrderStatusEnum.Resulted ,IsActive=true, Name="تم الإعدام" },
                new ExecutionOrderStatus {Id = (int)ExecutionOrderStatusEnum.Canceled ,IsActive=true, Name="ملغي" },
                };
                context.ExecutionOrderStatus.AddRange(ExecutionOrderStatus);
                context.SaveChanges();
            }
            //if (!context.RobbingStoreItemStatus.Any())
            //{
            //    var RobbingStoreItemStatus = new RobbingStoreItemStatus[]
            //    {
            //    new RobbingStoreItemStatus {Id = (int)RobbingStoreItemStatusEnum.NewStagnant , Name="راكد جديد" },
            //    new RobbingStoreItemStatus {Id = (int)RobbingStoreItemStatusEnum.UsedStagnant , Name="راكد مستعمل" },
            //    new RobbingStoreItemStatus {Id = (int)RobbingStoreItemStatusEnum.Robbing , Name="كهنة" },
               
            //    };
            //    context.RobbingStoreItemStatus.AddRange(RobbingStoreItemStatus);
            //    context.SaveChanges();
            //}
            if (!context.Currency.Any())
            {
                var Currency = new Currency[]
                {
                new Currency {Id = (int)CurrencyEnum.Pound ,IsActive=true, Name="جنية مصرى",IsDefault=true },
                new Currency {Id = (int)CurrencyEnum.Dollar ,IsActive=true, Name="دولار امريكي" },
                new Currency {Id = (int)CurrencyEnum.Reyal ,IsActive=true, Name="ريال سعودي" }
                };
                context.Currency.AddRange(Currency);
                context.SaveChanges();
            }

            //StoreType 
            if (!context.StoreType.Any())
            {
                var storeTypes = new StoreType[]
                {
                new StoreType {Id = (int)StoreTypeEnum.Store , Name="مخزن" },
                new StoreType {Id = (int)StoreTypeEnum.Custody , Name="عهدة" },
                new StoreType {Id = (int)StoreTypeEnum.Robbing , Name="كهنة" },

                };
                context.StoreType.AddRange(storeTypes);
                context.SaveChanges();
            }
            //Operation 
            if (!context.Operation.Any())
            {
                var operations = new Operation[]
                {
                new Operation {Id = (int)OperationEnum.ExaminationCommittee ,
                    Name ="لجنة فحص" },
                new Operation {Id = (int)OperationEnum.Addition , Name="اضافة" },
                new Operation {Id = (int)OperationEnum.Robbing , Name="كهنة" },
                new Operation {Id = (int)OperationEnum.ExchangeOrder , Name="امر صرف" },
                new Operation {Id = (int)OperationEnum.Transformation , Name="نقل عهدة" },
                new Operation {Id = (int)OperationEnum.RefundOrder , Name="امر ارتجاع" },
                new Operation {Id = (int)OperationEnum.StockTaking , Name="محضرالجرد" },
                new Operation {Id = (int)OperationEnum.Stagnant , Name="الاصناف الراكدة" },
                new Operation {Id = (int)OperationEnum.Deduction , Name="خصم الاصناف التالفة" },
                new Operation {Id = (int)OperationEnum.Execution , Name="الاعدام" },
                new Operation {Id = (int)OperationEnum.Subtraction , Name="الخصم" } };


                context.Operation.AddRange(operations);
                context.SaveChanges();
            }
            if (!context.Budget.Any())
            {
                var budgets = new Budget[]
                {
                new Budget {Id = (int)BudgetNamesEnum.Staff ,IsActive=true, Name="هيئة" },
                new Budget {Id = (int)BudgetNamesEnum.Presidency  ,IsActive=true, Name="رئاسة" },
                new Budget {Id = (int)BudgetNamesEnum.ArmedForces  , IsActive=true,Name="قوات مسلحة" },
                new Budget {Id = (int)BudgetNamesEnum.Housing  ,IsActive=true, Name="إسكان" },
                };
                context.Budget.AddRange(budgets);
                context.SaveChanges();
            }
            if (!context.AdditionDocumentType.Any())
            {
                var additionDocumentTypes = new AdditionDocumentType[]
                {
                new AdditionDocumentType {Id = (int)AdditionDocumentTypeEnum.Receipt , Name="أصل الفاتورة",IsActive=true },
                new AdditionDocumentType {Id = (int)AdditionDocumentTypeEnum.AdministrativeCertificate , Name="شهادة إدارية" ,IsActive=true},

                };
                context.AdditionDocumentType.AddRange(additionDocumentTypes);
                context.SaveChanges();
            }
            if (!context.ItemStatus.Any())
            {
                var itemStatus = new ItemStatus[]
                {
                new ItemStatus {Id = (int)ItemStatusEnum.Available , Name="متاح" },
                new ItemStatus {Id = (int)ItemStatusEnum.Reserved , Name="محجوز" },
                new ItemStatus {Id = (int)ItemStatusEnum.Expenses , Name="مصروف" }
                };
                context.ItemStatus.AddRange(itemStatus);
                context.SaveChanges();
            }

            if (!context.StoreItemStatus.Any())
            {
                var storeItemStatus = new StoreItemStatus[]
                {
                new StoreItemStatus {Id =(int)StoreItemStatusEnum.New , Name="جديد" },
                new StoreItemStatus {Id = (int)StoreItemStatusEnum.Used , Name="مستعمل" },
                new StoreItemStatus {Id = (int)StoreItemStatusEnum.Robbing , Name="كهنة" },
                new StoreItemStatus {Id = (int)StoreItemStatusEnum.Tainted , Name="تالف" },
                new StoreItemStatus {Id = (int)StoreItemStatusEnum.NewStagnant , Name="راكد جديد" },
                new StoreItemStatus {Id = (int)StoreItemStatusEnum.UsedStagnant , Name="راكد مستعمل" }
                ,
                };
                context.StoreItemStatus.AddRange(storeItemStatus);
                context.SaveChanges();
            }


            if (!context.ExaminationStatus.Any())
            {
                var examinationStatus = new ExaminationStatus[]
                {
                new ExaminationStatus {Id = (int)ExaminationStatusEnum.DoneExamination , Name="تم الفحص" },
                new ExaminationStatus {Id =  (int)ExaminationStatusEnum.DoneAddition , Name="تم الإضافة" },

                };
                context.ExaminationStatus.AddRange(examinationStatus);
                context.SaveChanges();
            }
            if (!context.DelegationType.Any())
            {
                var delegationType = new DelegationType[]
                {
                new DelegationType {Id = (int)DelegationTypeEnum.Technican , Name="تفويضات مديرين الإدارة الفنية" },
                new DelegationType {Id =  (int)DelegationTypeEnum.StoreAdmin ,
                    Name="تفويضات أمناء المخازن" },

                };
                context.DelegationType.AddRange(delegationType);
                context.SaveChanges();
            }
            if (!context.RefundOrderStatus.Any())
            {
                var refundOrderStatuses = new RefundOrderStatus[]
                {
                new RefundOrderStatus
                { Id = (int)RefundOrderStatusEnum.Pending ,IsActive=true, Name="قيد الانتظار" },
                new RefundOrderStatus
                { Id =  (int)RefundOrderStatusEnum.Reviewed ,IsActive=true, Name="تمت المراجعة" },

                new RefundOrderStatus
                { Id =  (int)RefundOrderStatusEnum.Refunded ,IsActive=true, Name="تم الإرجاع" },
                new RefundOrderStatus
                { Id = (int)RefundOrderStatusEnum.Cancelled, IsActive = true, Name = "تم الإلغاء" }
                };
            context.RefundOrderStatus.AddRange(refundOrderStatuses);
                context.SaveChanges();
            }

            if (!context.TransformationStatus.Any())
            {
                var transformationStatuses = new TransformationStatus[]
                {
                new TransformationStatus
                { Id = (int)TransformationOrderStatusEnum.Requested , Name="تم الطلب" },
                new TransformationStatus
                { Id =  (int)TransformationOrderStatusEnum.Added , Name="تم الإضافة" },
                new TransformationStatus
                { Id = (int)TransformationOrderStatusEnum.Cancel, Name = "تم الإلغاء" } };

            context.TransformationStatus.AddRange(transformationStatuses);
                context.SaveChanges();
            }


            if (!context.RobbingOrderStatus.Any())
            {
                var RobbingOrderStatuses = new RobbingOrderStatus[]
                {
                new RobbingOrderStatus
                { Id = (int)RobbingOrderStatusEnum.Requested ,IsActive=true, Name="تم الطلب" },
                new RobbingOrderStatus
                { Id =  (int)RobbingOrderStatusEnum.Added ,IsActive=true, Name="تم الإضافة" } ,
                new RobbingOrderStatus
                { Id =  (int)RobbingOrderStatusEnum.Cancel ,IsActive=true, Name="تم الإلغاء" }};

                context.RobbingOrderStatus.AddRange(RobbingOrderStatuses);
                context.SaveChanges();
            }
            if (!context.OperationAttachmentType.Any())
            {
                var operationAttachmentTypes = new OperationAttachmentType[]
                {
                new OperationAttachmentType {AttachmentTypeId = (int)AttachmentTypeEnum.Contract ,
                    OperationId=(int)OperationEnum.ExaminationCommittee,
                    BudgetId =(int)BudgetNamesEnum.Staff,IsActive=true },

                //
                new OperationAttachmentType {AttachmentTypeId = (int)AttachmentTypeEnum.SupplyOrder
                , OperationId=(int)OperationEnum.ExaminationCommittee,
                BudgetId=(int)BudgetNamesEnum.Presidency,IsActive=true },

                new OperationAttachmentType {AttachmentTypeId = (int)AttachmentTypeEnum.SupplyOrder  ,
                    OperationId =(int)OperationEnum.ExaminationCommittee,
                BudgetId=(int)BudgetNamesEnum.ArmedForces,IsActive=true },

                new OperationAttachmentType {AttachmentTypeId = (int)AttachmentTypeEnum.SupplyOrder
                ,OperationId=(int)OperationEnum.ExaminationCommittee,
                BudgetId=(int)BudgetNamesEnum.Housing,IsActive=true },


                new OperationAttachmentType {AttachmentTypeId = (int)AttachmentTypeEnum.Template_12 ,
                    OperationId=(int)OperationEnum.ExaminationCommittee,
                    BudgetId =(int)BudgetNamesEnum.Staff,IsActive=true },

                new OperationAttachmentType {AttachmentTypeId = (int)AttachmentTypeEnum.Template_12 ,
                    OperationId=(int)OperationEnum.ExaminationCommittee,
                    BudgetId =(int)BudgetNamesEnum.Presidency,IsActive=true },

                new OperationAttachmentType {AttachmentTypeId = (int)AttachmentTypeEnum.Template_12 ,
                    OperationId=(int)OperationEnum.ExaminationCommittee,
                    BudgetId =(int)BudgetNamesEnum.ArmedForces,IsActive=true },

                new OperationAttachmentType {AttachmentTypeId = (int)AttachmentTypeEnum.Template_12 ,
                    OperationId=(int)OperationEnum.ExaminationCommittee,
                    BudgetId =(int)BudgetNamesEnum.Housing,IsActive=true },


                new OperationAttachmentType {AttachmentTypeId = (int)AttachmentTypeEnum.Others ,
                    OperationId=(int)OperationEnum.ExaminationCommittee,
                    BudgetId =(int)BudgetNamesEnum.Staff,IsActive=true },
                 new OperationAttachmentType {AttachmentTypeId = (int)AttachmentTypeEnum.Others ,
                    OperationId=(int)OperationEnum.ExaminationCommittee,
                    BudgetId =(int)BudgetNamesEnum.Presidency,IsActive=true },
                 new OperationAttachmentType {AttachmentTypeId = (int)AttachmentTypeEnum.Others ,
                    OperationId=(int)OperationEnum.ExaminationCommittee,
                    BudgetId =(int)BudgetNamesEnum.ArmedForces,IsActive=true },
                  new OperationAttachmentType {AttachmentTypeId = (int)AttachmentTypeEnum.Others ,
                    OperationId=(int)OperationEnum.ExaminationCommittee,
                    BudgetId =(int)BudgetNamesEnum.Housing,IsActive=true },



                new OperationAttachmentType {AttachmentTypeId = (int)AttachmentTypeEnum.Receipt ,
                    OperationId =(int)OperationEnum.Addition,
                    AdditionDocumentTypeId =(int)AdditionDocumentTypeEnum.Receipt, IsActive=true },
                 new OperationAttachmentType {AttachmentTypeId = (int)AttachmentTypeEnum.Template_2 ,
                    OperationId =(int)OperationEnum.Addition,
                    AdditionDocumentTypeId =(int)AdditionDocumentTypeEnum.Receipt, IsActive=true },
                 new OperationAttachmentType {AttachmentTypeId = (int)AttachmentTypeEnum.Others ,
                    OperationId =(int)OperationEnum.Addition,
                    AdditionDocumentTypeId =(int)AdditionDocumentTypeEnum.Receipt, IsActive=true },

                      new OperationAttachmentType {AttachmentTypeId = (int)AttachmentTypeEnum.AdministrativeCertificate ,
                    OperationId =(int)OperationEnum.Addition,
                    AdditionDocumentTypeId =(int)AdditionDocumentTypeEnum.AdministrativeCertificate, IsActive=true },
                 new OperationAttachmentType {AttachmentTypeId = (int)AttachmentTypeEnum.Template_2 ,
                    OperationId =(int)OperationEnum.Addition,
                    AdditionDocumentTypeId =(int)AdditionDocumentTypeEnum.AdministrativeCertificate, IsActive=true },
                 new OperationAttachmentType {AttachmentTypeId = (int)AttachmentTypeEnum.Others ,
                    OperationId =(int)OperationEnum.Addition,
                    AdditionDocumentTypeId =(int)AdditionDocumentTypeEnum.AdministrativeCertificate, IsActive=true },

                 //transformation operation
                  new OperationAttachmentType {AttachmentTypeId = (int)AttachmentTypeEnum.Template_2 ,
                    OperationId =(int)OperationEnum.Transformation, IsActive=true },
                 new OperationAttachmentType {AttachmentTypeId = (int)AttachmentTypeEnum.Template_8 ,
                    OperationId =(int)OperationEnum.Transformation, IsActive=true },
                 new OperationAttachmentType {AttachmentTypeId = (int)  AttachmentTypeEnum.Others ,
                    OperationId =(int)OperationEnum.Transformation, IsActive=true},

                 //Robbing operation
                  new OperationAttachmentType {AttachmentTypeId = (int)AttachmentTypeEnum.Template_2 ,
                    OperationId =(int)OperationEnum.Robbing, IsActive=true },
                 new OperationAttachmentType {AttachmentTypeId = (int)AttachmentTypeEnum.Template_8 ,
                    OperationId =(int)OperationEnum.Robbing, IsActive=true },
                 new OperationAttachmentType {AttachmentTypeId = (int)  AttachmentTypeEnum.Others ,
                    OperationId =(int)OperationEnum.Robbing, IsActive=true},

                 //RefundOrder
                  new OperationAttachmentType {AttachmentTypeId = (int)AttachmentTypeEnum.TechnicalExamination ,
                    OperationId =(int)OperationEnum.RefundOrder, IsActive=true },
                  //StockTaking
                  
                  new OperationAttachmentType {AttachmentTypeId = (int)AttachmentTypeEnum.StockTaking ,
                    OperationId =(int)OperationEnum.StockTaking, IsActive=true },

                   //Execution
                  
                  new OperationAttachmentType {AttachmentTypeId = (int)AttachmentTypeEnum.Others ,
                    OperationId =(int)OperationEnum.Execution, IsActive=true },

                };
                context.OperationAttachmentType.AddRange(operationAttachmentTypes);
                context.SaveChanges();
            }

            if (!context.NotificationTemplate.Any(o => o.Id == (int)NotificationTemplateEnum.NTF_Addition))
            {
                context.NotificationTemplate.Add(new NotificationTemplate()
                {
                    Id = (int)NotificationTemplateEnum.NTF_Addition,
                    Body = string.Format("تم إضافة أصناف جديدة إلي [{0}]  بإذن إضافة رقم [{1}]", (int)NotificationValuesEnum.FromStore, (int)NotificationValuesEnum.Code),
                    URL = "",
                    IsActive = true,
                    CreationDate = DateTime.Now,
                    NotificationHandler = new List<NotificationHandler>()
                {
                    new NotificationHandler()
                    {
                        HandlerId = (int)NotificationHandlersEnum.SendPushNotification,
                        IsActive=true,
                        CreationDate = DateTime.Now
                    }
                }
                });
            }
            if (!context.NotificationTemplate.Any(o => o.Id == (int)NotificationTemplateEnum.NTF_Transformation_RequestFrom))
            {
                context.NotificationTemplate.Add(new NotificationTemplate()
                {
                    Id = (int)NotificationTemplateEnum.NTF_Transformation_RequestFrom,
                    Body = string.Format("تم إرسال طلب نقل عهدة رقم [{1}] من [{0}] إلي [{2}]", 
                    (int)NotificationValuesEnum.FromStore,(int)NotificationValuesEnum.Code,(int)NotificationValuesEnum.ToStore),
                    URL = "",
                    IsActive = true,
                    CreationDate = DateTime.Now,
                    NotificationHandler = new List<NotificationHandler>()
                {
                    new NotificationHandler()
                    {
                        HandlerId = (int)NotificationHandlersEnum.SendPushNotification,
                        IsActive=true,
                        CreationDate = DateTime.Now
                    }
                }
                });
            }
            if (!context.NotificationTemplate.Any(o => o.Id == (int)NotificationTemplateEnum.NTF_Transformation_RequestTo))
            {
                context.NotificationTemplate.Add(new NotificationTemplate()
                {
                    Id = (int)NotificationTemplateEnum.NTF_Transformation_RequestTo,
                    Body = string.Format("تم إرسال طلب نقل عهدة جديد برقم [{0}] من [{1}]. يرجى إتمام عملية إضافة الأصناف إلى [{2}]  ",
                    (int)NotificationValuesEnum.Code, (int)NotificationValuesEnum.FromStore,(int)NotificationValuesEnum.ToStore),
                    URL = "",
                    IsActive = true,
                    CreationDate = DateTime.Now,
                    NotificationHandler = new List<NotificationHandler>()
                {
                    new NotificationHandler()
                    {
                        HandlerId = (int)NotificationHandlersEnum.SendPushNotification,
                        IsActive=true,
                        CreationDate = DateTime.Now
                    }
                }
                });
            }
            if (!context.NotificationTemplate.Any(o => o.Id == (int)NotificationTemplateEnum.NTF_Transformation_Addition))
            {
                context.NotificationTemplate.Add(new NotificationTemplate()
                {
                    Id = (int)NotificationTemplateEnum.NTF_Transformation_Addition,
                    Body = string.Format("تم نقل أصناف جديدة إلي [{0}] بإذن إضافة رقم [{1}]", (int)NotificationValuesEnum.FromStore,(int)NotificationValuesEnum.Code),
                    URL = "",
                    IsActive = true,
                    CreationDate = DateTime.Now,
                    NotificationHandler = new List<NotificationHandler>()
                {
                    new NotificationHandler()
                    {
                        HandlerId = (int)NotificationHandlersEnum.SendPushNotification,
                        IsActive=true,
                        CreationDate = DateTime.Now
                    }
                }
                });
            }
            if (!context.NotificationTemplate.Any(o => o.Id == (int)NotificationTemplateEnum.NTF_RobbingOrder_RequestFrom))
            {
                context.NotificationTemplate.Add(new NotificationTemplate()
                {
                    Id = (int)NotificationTemplateEnum.NTF_RobbingOrder_RequestFrom,
                    Body = string.Format("تم إرسال طلب تكهين رقم [{0}] من [{1}] إلي [{2}]",
                    (int)NotificationValuesEnum.Code,(int)NotificationValuesEnum.FromStore,(int)NotificationValuesEnum.ToStore),
                    URL = "",
                    IsActive = true,
                    CreationDate = DateTime.Now,
                    NotificationHandler = new List<NotificationHandler>()
                {
                    new NotificationHandler()
                    {
                        HandlerId = (int)NotificationHandlersEnum.SendPushNotification,
                        IsActive=true,
                        CreationDate = DateTime.Now
                    }
                }
                });
            }
            if (!context.NotificationTemplate.Any(o => o.Id == (int)NotificationTemplateEnum.NTF_RobbingOrder_RequestTo))
            {
                context.NotificationTemplate.Add(new NotificationTemplate()
                {
                    Id = (int)NotificationTemplateEnum.NTF_RobbingOrder_RequestTo,
                    Body = string.Format("تم إرسال طلب تكهين جديد برقم [{0}] من [{1}]. يرجى إتمام عملية إضافة الأصناف إلى [{2}]  ",
                    (int)NotificationValuesEnum.Code, (int)NotificationValuesEnum.FromStore, (int)NotificationValuesEnum.ToStore),
                    URL = "",
                    IsActive = true,
                    CreationDate = DateTime.Now,
                    NotificationHandler = new List<NotificationHandler>()
                {
                    new NotificationHandler()
                    {
                        HandlerId = (int)NotificationHandlersEnum.SendPushNotification,
                        IsActive=true,
                        CreationDate = DateTime.Now
                    }
                }
                });
            }
            if (!context.NotificationTemplate.Any(o => o.Id == (int)NotificationTemplateEnum.NTF_RobbingOrder_Addition))
            {
                context.NotificationTemplate.Add(new NotificationTemplate()
                {
                    Id = (int)NotificationTemplateEnum.NTF_RobbingOrder_Addition,
                    Body = string.Format("تم اضافة أصناف جديدة إلي [{0}] بطلب تكهين رقم [{1}]", (int)NotificationValuesEnum.FromStore,(int)NotificationValuesEnum.Code),
                    URL = "",
                    IsActive = true,
                    CreationDate = DateTime.Now,
                    NotificationHandler = new List<NotificationHandler>()
                {
                    new NotificationHandler()
                    {
                        HandlerId = (int)NotificationHandlersEnum.SendPushNotification,
                        IsActive=true,
                        CreationDate = DateTime.Now
                    }
                }
                });
            }
            if (!context.NotificationTemplate.Any(o => o.Id == (int)NotificationTemplateEnum.NTF_Invoice))
            {
                context.NotificationTemplate.Add(new NotificationTemplate()
                {
                    Id = (int)NotificationTemplateEnum.NTF_Invoice,
                    Body = string.Format("تم صرف أصناف من [{0}] بإيصال إستلام رقم [{1}]", (int)NotificationValuesEnum.FromStore,(int)NotificationValuesEnum.Code),
                    URL = "",
                    IsActive = true,
                    CreationDate = DateTime.Now,
                    NotificationHandler = new List<NotificationHandler>()
                {
                    new NotificationHandler()
                    {
                        HandlerId = (int)NotificationHandlersEnum.SendPushNotification,
                        IsActive=true,
                        CreationDate = DateTime.Now
                    }
                }
                });
            }
            if (!context.NotificationTemplate.Any(o => o.Id == (int)NotificationTemplateEnum.NTF_Invoice_Edit))
            {
                context.NotificationTemplate.Add(new NotificationTemplate()
                {
                    Id = (int)NotificationTemplateEnum.NTF_Invoice_Edit,
                    Body = string.Format("تم تعديل إيصال إستلام رقم [{0}] من [{1}]", (int)NotificationValuesEnum.Code, (int)NotificationValuesEnum.FromStore),
                    URL = "",
                    IsActive = true,
                    CreationDate = DateTime.Now,
                    NotificationHandler = new List<NotificationHandler>()
                {
                    new NotificationHandler()
                    {
                        HandlerId = (int)NotificationHandlersEnum.SendPushNotification,
                        IsActive=true,
                        CreationDate = DateTime.Now
                    }
                }
                });
            }
            if (!context.NotificationTemplate.Any(o => o.Id == (int)NotificationTemplateEnum.NTF_Review_ExchangeOrder))
            {
                context.NotificationTemplate.Add(new NotificationTemplate()
                {
                    Id = (int)NotificationTemplateEnum.NTF_Review_ExchangeOrder,
                    Body = string.Format("لديك أمر صرف جديد من [{0}] برقم [{1}]", (int)NotificationValuesEnum.FromStore,(int)NotificationValuesEnum.Code),
                    URL = "",
                    IsActive = true,
                    CreationDate = DateTime.Now,
                    NotificationHandler = new List<NotificationHandler>()
                {
                    new NotificationHandler()
                    {
                        HandlerId = (int)NotificationHandlersEnum.SendPushNotification,
                        IsActive=true,
                        CreationDate = DateTime.Now
                    }
                }
                });
            }
            if (!context.NotificationTemplate.Any(o => o.Id == (int)NotificationTemplateEnum.NTF_Create_ExchangeOrder))
            {
                context.NotificationTemplate.Add(new NotificationTemplate()
                {
                    Id = (int)NotificationTemplateEnum.NTF_Create_ExchangeOrder,
                    Body = string.Format("لديك أمر صرف جديد من [{0}] برقم [{1}]", (int)NotificationValuesEnum.FromStore, (int)NotificationValuesEnum.Code),
                    URL = "",
                    IsActive = true,
                    CreationDate = DateTime.Now,
                    NotificationHandler = new List<NotificationHandler>()
                {
                    new NotificationHandler()
                    {
                        HandlerId = (int)NotificationHandlersEnum.SendPushNotification,
                        IsActive=true,
                        CreationDate = DateTime.Now
                    }
                }
                });
            }
            if (!context.NotificationTemplate.Any(o => o.Id == (int)NotificationTemplateEnum.NTF_Create_RefundOrder))
            {
                context.NotificationTemplate.Add(new NotificationTemplate()
                {
                    Id = (int)NotificationTemplateEnum.NTF_Create_RefundOrder,
                    Body = string.Format("لديك أمر ارتجاع جديد إلي [{0}] برقم [{1}]", (int)NotificationValuesEnum.FromStore, (int)NotificationValuesEnum.Code),
                    URL = "",
                    IsActive = true,
                    CreationDate = DateTime.Now,
                    NotificationHandler = new List<NotificationHandler>()
                {
                    new NotificationHandler()
                    {
                        HandlerId = (int)NotificationHandlersEnum.SendPushNotification,
                        IsActive=true,
                        CreationDate = DateTime.Now
                    }
                }
                });
            }
            if (!context.NotificationTemplate.Any(o => o.Id == (int)NotificationTemplateEnum.NTF_Review_RefundOrder))
            {
                context.NotificationTemplate.Add(new NotificationTemplate()
                {
                    Id = (int)NotificationTemplateEnum.NTF_Review_RefundOrder,
                    Body = string.Format("لديك أمر ارتجاع جديد إلي [{0}] برقم [{1}]", (int)NotificationValuesEnum.FromStore, (int)NotificationValuesEnum.Code),
                    URL = "",
                    IsActive = true,
                    CreationDate = DateTime.Now,
                    NotificationHandler = new List<NotificationHandler>()
                {
                    new NotificationHandler()
                    {
                        HandlerId = (int)NotificationHandlersEnum.SendPushNotification,
                        IsActive=true,
                        CreationDate = DateTime.Now
                    }
                }
                });
            }
            if (!context.NotificationTemplate.Any(o => o.Id == (int)NotificationTemplateEnum.NTF_RobbingOrder_Addition_To_Sender))
            {
                context.NotificationTemplate.Add(new NotificationTemplate()
                {
                    Id = (int)NotificationTemplateEnum.NTF_RobbingOrder_Addition_To_Sender,
                    Body = string.Format("تم تكهين الأصناف من [{0}] إلي [{1}] بأمر تكهين رقم [{2}]",
                    (int)NotificationValuesEnum.FromStore,(int)NotificationValuesEnum.ToStore,(int)NotificationValuesEnum.Code),
                    URL = "",
                    IsActive = true,
                    CreationDate = DateTime.Now,
                    NotificationHandler = new List<NotificationHandler>()
                {
                    new NotificationHandler()
                    {
                        HandlerId = (int)NotificationHandlersEnum.SendPushNotification,
                        IsActive=true,
                        CreationDate = DateTime.Now
                    }
                }
                });
            }
            if (!context.NotificationTemplate.Any(o => o.Id == (int)NotificationTemplateEnum.NTF_Transformation_Addition_To_Sender))
            {
                context.NotificationTemplate.Add(new NotificationTemplate()
                {
                    Id = (int)NotificationTemplateEnum.NTF_Transformation_Addition_To_Sender,
                    Body = string.Format("تم إضافة الأصناف من [{0}] إلي [{1}] بأمر نقل رقم [{2}]",
                    (int)NotificationValuesEnum.FromStore, (int)NotificationValuesEnum.ToStore, (int)NotificationValuesEnum.Code),
                    URL = "",
                    IsActive = true,
                    CreationDate = DateTime.Now,
                    NotificationHandler = new List<NotificationHandler>()
                {
                    new NotificationHandler()
                    {
                        HandlerId = (int)NotificationHandlersEnum.SendPushNotification,
                        IsActive=true,
                        CreationDate = DateTime.Now
                    }
                }
                });
            }
            if (!context.NotificationTemplate.Any(o => o.Id == (int)NotificationTemplateEnum.NTF_Delegated_User))
            {
                context.NotificationTemplate.Add(new NotificationTemplate()
                {
                    Id = (int)NotificationTemplateEnum.NTF_Delegated_User,
                    Body = string.Format("تم تفويضك للعمل علي إدارة [{0}] يرجي إعادة تسجيل الدخول لتتمكن من العمل عليه",
                    (int)NotificationValuesEnum.FromStore),
                    URL = "",
                    IsActive = true,
                    CreationDate = DateTime.Now,
                    NotificationHandler = new List<NotificationHandler>()
                {
                    new NotificationHandler()
                    {
                        HandlerId = (int)NotificationHandlersEnum.SendPushNotification,
                        IsActive=true,
                        CreationDate = DateTime.Now
                    }
                }
                });
            }
            if (!context.NotificationTemplate.Any(o => o.Id == (int)NotificationTemplateEnum.NTF_Delegation_Store))
            {
                context.NotificationTemplate.Add(new NotificationTemplate()
                {
                    Id = (int)NotificationTemplateEnum.NTF_Delegation_Store,
                    Body = string.Format("تم تفويض [{0}] للعمل علي إدارة [{1}]",
                    (int)NotificationValuesEnum.FromStoreAdmin, (int)NotificationValuesEnum.ToStore),
                    URL = "",
                    IsActive = true,
                    CreationDate = DateTime.Now,
                    NotificationHandler = new List<NotificationHandler>()
                {
                    new NotificationHandler()
                    {
                        HandlerId = (int)NotificationHandlersEnum.SendPushNotification,
                        IsActive=true,
                        CreationDate = DateTime.Now
                    }
                }
                });
            }
            if (!context.NotificationTemplate.Any(o => o.Id == (int)NotificationTemplateEnum.NTF_Addition_Edit))
            {
                context.NotificationTemplate.Add(new NotificationTemplate()
                {
                    Id = (int)NotificationTemplateEnum.NTF_Addition_Edit,
                    Body = string.Format("تم تعديل إذن إضافة رقم [{0}] من [{1}]",
                    (int)NotificationValuesEnum.Code, (int)NotificationValuesEnum.FromStore),
                    URL = "",
                    IsActive = true,
                    CreationDate = DateTime.Now,
                    NotificationHandler = new List<NotificationHandler>()
                {
                    new NotificationHandler()
                    {
                        HandlerId = (int)NotificationHandlersEnum.SendPushNotification,
                        IsActive=true,
                        CreationDate = DateTime.Now
                    }
                }
                });
            }
            if (!context.NotificationTemplate.Any(o => o.Id == (int)NotificationTemplateEnum.NTF_ExchangeOrder_Review_TechManager))
            {
                context.NotificationTemplate.Add(new NotificationTemplate()
                {
                    Id = (int)NotificationTemplateEnum.NTF_ExchangeOrder_Review_TechManager,
                    Body = string.Format("تم مراجعة أمر صرف رقم [{0}] من [{1}]",
                    (int)NotificationValuesEnum.Code, (int)NotificationValuesEnum.FromStore),
                    URL = "",
                    IsActive = true,
                    CreationDate = DateTime.Now,
                    NotificationHandler = new List<NotificationHandler>()
                {
                    new NotificationHandler()
                    {
                        HandlerId = (int)NotificationHandlersEnum.SendPushNotification,
                        IsActive=true,
                        CreationDate = DateTime.Now
                    }
                }
                });
            }
            if (!context.NotificationTemplate.Any(o => o.Id == (int)NotificationTemplateEnum.NTF_RefundOrder_Review_TechManager))
            {
                context.NotificationTemplate.Add(new NotificationTemplate()
                {
                    Id = (int)NotificationTemplateEnum.NTF_RefundOrder_Review_TechManager,
                    Body = string.Format("تم مراجعة أمر ارتجاع رقم [{0}] من [{1}]",
                    (int)NotificationValuesEnum.Code, (int)NotificationValuesEnum.FromStore),
                    URL = "",
                    IsActive = true,
                    CreationDate = DateTime.Now,
                    NotificationHandler = new List<NotificationHandler>()
                {
                    new NotificationHandler()
                    {
                        HandlerId = (int)NotificationHandlersEnum.SendPushNotification,
                        IsActive=true,
                        CreationDate = DateTime.Now
                    }
                }
                });
            }
            if (!context.NotificationTemplate.Any(o => o.Id == (int)NotificationTemplateEnum.NTF_DirectOrder_ExchangeOrder))
            {
                context.NotificationTemplate.Add(new NotificationTemplate()
                {
                    Id = (int)NotificationTemplateEnum.NTF_DirectOrder_ExchangeOrder,
                    Body = string.Format("تم إنشاء أمر صرف رقم [{0}] من [{1}] بالأمر المباشر",
                    (int)NotificationValuesEnum.Code, (int)NotificationValuesEnum.FromStore),
                    URL = "",
                    IsActive = true,
                    CreationDate = DateTime.Now,
                    NotificationHandler = new List<NotificationHandler>()
                {
                    new NotificationHandler()
                    {
                        HandlerId = (int)NotificationHandlersEnum.SendPushNotification,
                        IsActive=true,
                        CreationDate = DateTime.Now
                    }
                }
                });
            }
            if (!context.NotificationTemplate.Any(o => o.Id == (int)NotificationTemplateEnum.NTF_DirectOrder_RefundOrder))
            {
                context.NotificationTemplate.Add(new NotificationTemplate()
                {
                    Id = (int)NotificationTemplateEnum.NTF_DirectOrder_RefundOrder,
                    Body = string.Format("تم إنشاء أمر ارتجاع رقم [{0}] من [{1}] بالأمر المباشر",
                    (int)NotificationValuesEnum.Code, (int)NotificationValuesEnum.FromStore),
                    URL = "",
                    IsActive = true,
                    CreationDate = DateTime.Now,
                    NotificationHandler = new List<NotificationHandler>()
                {
                    new NotificationHandler()
                    {
                        HandlerId = (int)NotificationHandlersEnum.SendPushNotification,
                        IsActive=true,
                        CreationDate = DateTime.Now
                    }
                }
                });
            }

            if (!context.NotificationTemplate.Any(o => o.Id == (int)NotificationTemplateEnum.NTF_Cancel_Transformation_RequestFrom))
            {
                context.NotificationTemplate.Add(new NotificationTemplate()
                {
                    Id = (int)NotificationTemplateEnum.NTF_Cancel_Transformation_RequestFrom,
                    Body = string.Format("تم إلغاء طلب نقل العهدة رقم [{0}] من [{1}] إلي [{2}]",
                    (int)NotificationValuesEnum.Code, (int)NotificationValuesEnum.FromStore, (int)NotificationValuesEnum.ToStore),
                    URL = "",
                    IsActive = true,
                    CreationDate = DateTime.Now,
                    NotificationHandler = new List<NotificationHandler>()
                {
                    new NotificationHandler()
                    {
                        HandlerId = (int)NotificationHandlersEnum.SendPushNotification,
                        IsActive=true,
                        CreationDate = DateTime.Now
                    }
                }
                });
            }
            if (!context.NotificationTemplate.Any(o => o.Id == (int)NotificationTemplateEnum.NTF_Cancel_Transformation_RequestTo))
            {
                context.NotificationTemplate.Add(new NotificationTemplate()
                {
                    Id = (int)NotificationTemplateEnum.NTF_Cancel_Transformation_RequestTo,
                    Body = string.Format("تم إلغاء طلب نقل العهدة رقم [{0}] من [{1}] إلي [{2}]",
                    (int)NotificationValuesEnum.Code, (int)NotificationValuesEnum.FromStore, (int)NotificationValuesEnum.ToStore),
                    URL = "",
                    IsActive = true,
                    CreationDate = DateTime.Now,
                    NotificationHandler = new List<NotificationHandler>()
                {
                    new NotificationHandler()
                    {
                        HandlerId = (int)NotificationHandlersEnum.SendPushNotification,
                        IsActive=true,
                        CreationDate = DateTime.Now
                    }
                }
                });
            }
            if (!context.NotificationTemplate.Any(o => o.Id == (int)NotificationTemplateEnum.NTF_Cancel_RobbingOrder_RequestFrom))
            {
                context.NotificationTemplate.Add(new NotificationTemplate()
                {
                    Id = (int)NotificationTemplateEnum.NTF_Cancel_RobbingOrder_RequestFrom,
                    Body = string.Format("تم إلغاء طلب التكهين رقم [{0}] من [{1}] إلي [{2}]",
                    (int)NotificationValuesEnum.Code, (int)NotificationValuesEnum.FromStore, (int)NotificationValuesEnum.ToStore),
                    URL = "",
                    IsActive = true,
                    CreationDate = DateTime.Now,
                    NotificationHandler = new List<NotificationHandler>()
                {
                    new NotificationHandler()
                    {
                        HandlerId = (int)NotificationHandlersEnum.SendPushNotification,
                        IsActive=true,
                        CreationDate = DateTime.Now
                    }
                }
                });
            }
            if (!context.NotificationTemplate.Any(o => o.Id == (int)NotificationTemplateEnum.NTF_Cancel_RobbingOrder_RequestTo))
            {
                context.NotificationTemplate.Add(new NotificationTemplate()
                {
                    Id = (int)NotificationTemplateEnum.NTF_Cancel_RobbingOrder_RequestTo,
                    Body = string.Format("تم إلغاء طلب التكهين رقم [{0}] من [{1}] إلي [{2}]",
                    (int)NotificationValuesEnum.Code, (int)NotificationValuesEnum.FromStore, (int)NotificationValuesEnum.ToStore),
                    URL = "",
                    IsActive = true,
                    CreationDate = DateTime.Now,
                    NotificationHandler = new List<NotificationHandler>()
                {
                    new NotificationHandler()
                    {
                        HandlerId = (int)NotificationHandlersEnum.SendPushNotification,
                        IsActive=true,
                        CreationDate = DateTime.Now
                    }
                }
                });
            }
            if (!context.NotificationTemplate.Any(o => o.Id == (int)NotificationTemplateEnum.NTF_Deduction))
            {
                context.NotificationTemplate.Add(new NotificationTemplate()
                {
                    Id = (int)NotificationTemplateEnum.NTF_Deduction,
                    Body = string.Format("تم خصم الأصناف التالفة من [{0}] بسند خصم رقم [{1}]",
                    (int)NotificationValuesEnum.FromStore, (int)NotificationValuesEnum.Code),
                    URL = "",
                    IsActive = true,
                    CreationDate = DateTime.Now,
                    NotificationHandler = new List<NotificationHandler>()
                {
                    new NotificationHandler()
                    {
                        HandlerId = (int)NotificationHandlersEnum.SendPushNotification,
                        IsActive=true,
                        CreationDate = DateTime.Now
                    }
                }
                });
            }
            if (!context.NotificationTemplate.Any(o => o.Id == (int)NotificationTemplateEnum.NTF_Cancel_Delegated_User))
            {
                context.NotificationTemplate.Add(new NotificationTemplate()
                {
                    Id = (int)NotificationTemplateEnum.NTF_Cancel_Delegated_User,
                    Body = string.Format("تم إيقاف تفويضك من إدارة [{0}]",
                    (int)NotificationValuesEnum.FromStore),
                    URL = "",
                    IsActive = true,
                    CreationDate = DateTime.Now,
                    NotificationHandler = new List<NotificationHandler>()
                {
                    new NotificationHandler()
                    {
                        HandlerId = (int)NotificationHandlersEnum.SendPushNotification,
                        IsActive=true,
                        CreationDate = DateTime.Now
                    }
                }
                });
            }
            if (!context.NotificationTemplate.Any(o => o.Id == (int)NotificationTemplateEnum.NTF_Cancel_Delegation_Store))
            {
                context.NotificationTemplate.Add(new NotificationTemplate()
                {
                    Id = (int)NotificationTemplateEnum.NTF_Cancel_Delegation_Store,
                    Body = string.Format("تم إيقاف تفويض [{0}] من إدارة [{1}]",
                    (int)NotificationValuesEnum.FromStoreAdmin, (int)NotificationValuesEnum.ToStore),
                    URL = "",
                    IsActive = true,
                    CreationDate = DateTime.Now,
                    NotificationHandler = new List<NotificationHandler>()
                {
                    new NotificationHandler()
                    {
                        HandlerId = (int)NotificationHandlersEnum.SendPushNotification,
                        IsActive=true,
                        CreationDate = DateTime.Now
                    }
                }
                });
            }
            if (!context.NotificationTemplate.Any(o => o.Id == (int)NotificationTemplateEnum.NTF_Create_Execution_Request))
            {
                context.NotificationTemplate.Add(new NotificationTemplate()
                {
                    Id = (int)NotificationTemplateEnum.NTF_Create_Execution_Request,
                    Body = string.Format("تم إنشاء طلب إعدام برقم [{0}] من [{1}]",
                    (int)NotificationValuesEnum.Code, (int)NotificationValuesEnum.FromStore),
                    URL = "",
                    IsActive = true,
                    CreationDate = DateTime.Now,
                    NotificationHandler = new List<NotificationHandler>()
                {
                    new NotificationHandler()
                    {
                        HandlerId = (int)NotificationHandlersEnum.SendPushNotification,
                        IsActive=true,
                        CreationDate = DateTime.Now
                    }
                }
                });
            }
            if (!context.NotificationTemplate.Any(o => o.Id == (int)NotificationTemplateEnum.NTF_Review_Execution_Request))
            {
                context.NotificationTemplate.Add(new NotificationTemplate()
                {
                    Id = (int)NotificationTemplateEnum.NTF_Review_Execution_Request,
                    Body = string.Format("تم مراجعة طلب الإعدام رقم [{0}] من [{1}]",
                    (int)NotificationValuesEnum.Code, (int)NotificationValuesEnum.FromStore),
                    URL = "",
                    IsActive = true,
                    CreationDate = DateTime.Now,
                    NotificationHandler = new List<NotificationHandler>()
                {
                    new NotificationHandler()
                    {
                        HandlerId = (int)NotificationHandlersEnum.SendPushNotification,
                        IsActive=true,
                        CreationDate = DateTime.Now
                    }
                }
                });
            }
            if (!context.NotificationTemplate.Any(o => o.Id == (int)NotificationTemplateEnum.NTF_Cancel_Execution_Request))
            {
                context.NotificationTemplate.Add(new NotificationTemplate()
                {
                    Id = (int)NotificationTemplateEnum.NTF_Cancel_Execution_Request,
                    Body = string.Format("تم إلغاء طلب الإعدام رقم [{0}] من [{1}]",
                    (int)NotificationValuesEnum.Code, (int)NotificationValuesEnum.FromStore),
                    URL = "",
                    IsActive = true,
                    CreationDate = DateTime.Now,
                    NotificationHandler = new List<NotificationHandler>()
                {
                    new NotificationHandler()
                    {
                        HandlerId = (int)NotificationHandlersEnum.SendPushNotification,
                        IsActive=true,
                        CreationDate = DateTime.Now
                    }
                }
                });
            }
            if (!context.NotificationTemplate.Any(o => o.Id == (int)NotificationTemplateEnum.NTF_Create_Execution_After_Review_Request))
            {
                context.NotificationTemplate.Add(new NotificationTemplate()
                {
                    Id = (int)NotificationTemplateEnum.NTF_Create_Execution_After_Review_Request,
                    Body = string.Format("تم إنشاء محضر إعدام لطلب الإعدام رقم [{0}] من [{1}]",
                    (int)NotificationValuesEnum.Code, (int)NotificationValuesEnum.FromStore),
                    URL = "",
                    IsActive = true,
                    CreationDate = DateTime.Now,
                    NotificationHandler = new List<NotificationHandler>()
                {
                    new NotificationHandler()
                    {
                        HandlerId = (int)NotificationHandlersEnum.SendPushNotification,
                        IsActive=true,
                        CreationDate = DateTime.Now
                    }
                }
                });
            }
            if (!context.NotificationTemplate.Any(o => o.Id == (int)NotificationTemplateEnum.NTF_Delegation_Technician))
            {
                context.NotificationTemplate.Add(new NotificationTemplate()
                {
                    Id = (int)NotificationTemplateEnum.NTF_Delegation_Technician,
                    Body = string.Format("تم تفويضك للعمل علي إدارة [{0}] يرجي إعادة تسجيل الدخول لتتمكن من العمل عليه",
                    (int)NotificationValuesEnum.FromStore),
                    URL = "",
                    IsActive = true,
                    CreationDate = DateTime.Now,
                    NotificationHandler = new List<NotificationHandler>()
                {
                    new NotificationHandler()
                    {
                        HandlerId = (int)NotificationHandlersEnum.SendPushNotification,
                        IsActive=true,
                        CreationDate = DateTime.Now
                    }
                }
                });
            }
            context.SaveChanges();


            if (!context.Shift.Any())
            {
                var shifts = new Shift[]
                {
                new Shift
                { Id = (int)ShiftEnum.MorningShift ,IsActive=true, Name="الورديه الصباحية"
                ,TimeFrom=new TimeSpan(9,0,0),TimeTo=new TimeSpan(15,0,0)},
                 new Shift
                { Id = (int)ShiftEnum.EveningShift ,IsActive=true, Name="الورديه المسائية"
                ,TimeFrom=new TimeSpan(15,0,0),TimeTo=new TimeSpan(21,0,0)},
                  new Shift
                { Id = (int)ShiftEnum.AllDayShift ,IsActive=true, Name="اليوم كامل"
                ,TimeFrom=new TimeSpan(9,0,0),TimeTo=new TimeSpan(21,0,0)},
                   new Shift
                { Id = (int)ShiftEnum.CustomShift ,IsActive=true, Name="وردية معدلة"
                    ,TimeFrom=new TimeSpan(0,0,0),TimeTo=new TimeSpan(23,59,0)}
      
                };

                context.Shift.AddRange(shifts);
                context.SaveChanges();
            }


        }

    }
}
