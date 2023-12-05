namespace Inventory.Data.Enums
{
    public enum NotificationTemplateEnum
    {
        NTF_Addition=1,//01 تم إضافة أصناف جديدة للمخزن بإذن إضافة رقم  ]رقم إذن الإضافة [
        NTF_Transformation_RequestFrom = 2,//تم نقل أصناف جديدة  من المخزن  بطلب نقل رقم  
        NTF_Transformation_RequestTo = 3, //تم إرسال طلب نقل جديد ]رقم طلب نقل العهدة[.يرجى إتمام عملية إضافة الأصناف إالى المخزن
        NTF_Transformation_Addition = 4,//تم نقل أصناف جديدة للمخزن بطلب نقل رقم  ]رقم طلب النقل[
        NTF_RobbingOrder_RequestFrom = 5,//تم تكهين أصناف جديدة بطلب تكهين رقم  ]رقم طلب التكهين[
        NTF_RobbingOrder_RequestTo = 6,//تم إرسال طلب تكهين جديد ]رقم طلب التكهين[. يرجى إتمام عملية إضافة الأصناف إالى المخزن
        NTF_RobbingOrder_Addition = 7,//تم اضافة أصناف جديدة للمخزن بطلب تكهين رقم  ]رقم طلب التكهين[
        NTF_Invoice = 8,//تم صرف أصناف إيصال إستلام رقم  ]رقم أمر الصرف[
        NTF_Create_ExchangeOrder = 9,//لديك أمر صرف جديد  ]رقم أمر الصرف[
        NTF_Review_ExchangeOrder = 10,//لديك أمر صرف جديد  ]رقم أمر الصرف[
        NTF_Create_RefundOrder = 11,//لديك أمر إرتجاع جديد  ]رقم أمر الإرتجاع[
        NTF_Review_RefundOrder = 12,//لديك أمر إرتجاع جديد  ]رقم أمر الإرتجاع
        NTF_Invoice_Edit = 13,// تم تعديل إيصال إستلام رقم [رقم إيصال ألإستلام[ من مخزن [اسم المخزن ] ن
        NTF_Transformation_Addition_To_Sender = 14,//تم إضافة الأصناف من مخزن إلي مخزن 
        NTF_RobbingOrder_Addition_To_Sender = 15,//تم تكهين الأصناف إلي مخزن 
        NTF_Delegation_Store = 16,//تم تفويض أمين مخزن عن اسم المخزن
        NTF_Delegated_User= 17,
        NTF_Addition_Edit = 18,
        NTF_ExchangeOrder_Review_TechManager = 19,
        NTF_RefundOrder_Review_TechManager = 20,
        NTF_DirectOrder_ExchangeOrder = 21,
        NTF_Cancel_Transformation_RequestFrom = 22,
        NTF_Cancel_Transformation_RequestTo = 23,
        NTF_Cancel_RobbingOrder_RequestFrom = 24,
        NTF_Cancel_RobbingOrder_RequestTo = 25,
        NTF_Deduction = 26,
        NTF_Cancel_Delegation_Store = 27,
        NTF_Cancel_Delegated_User = 28,
        NTF_Create_Execution_Request = 29,
        NTF_Review_Execution_Request = 30,
        NTF_Cancel_Execution_Request = 31,
        NTF_Create_Execution_After_Review_Request = 32,
        NTF_Delegation_Technician = 33,
        NTF_DirectOrder_RefundOrder = 34,
    }
}
