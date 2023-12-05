using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Data.Models.ReportVM
{
    public class DailyStoreItemsVM
    {
     //   public string Currency { get; set; }
        public Guid Id { get; set; }
        public string Code { get; set; }
        public DateTime Date { get; set; } 
        public string Note { get; set; }
        private string _requester;

        public string Requester {
            get {
                if(IsAddition)
                    return ExaminationRequester + TransRequester + RobbingRequester;
                return _requester;
            }
            set { _requester = value; }
        }
        public decimal _CountStoreAddition { get; set; }
        public decimal CountStoreAddition {
            get {
                if (CountStoreAdditionlist != null)
                {
                    _CountStoreAddition = 0;
                    foreach (var item in CountStoreAdditionlist)
                    {
                        _CountStoreAddition += (item.price * item.count);

                    }
                }
            
                return _CountStoreAddition;
            }
            set { _CountStoreAddition = value; }
        }
        public string _Currency { get; set; }
        public string Currency
        {
            get
            {
                if (CountStoreAdditionlist != null)
                {
                    foreach (var item in CountStoreAdditionlist)
                    {
                        _Currency =item.Currency;

                    }
                }
                else if(CountStoreAdditionOutgoinglist !=null)
                {
                    foreach (var item in CountStoreAdditionOutgoinglist)
                    {
                        _Currency = item.Currency;

                    }
                }

                return _Currency;
            }
            set { _Currency = value; }
        }
        public IEnumerable<priceItem> CountStoreAdditionlist { get; set; }

        private decimal _CountStoreOutgoing = 0;
        public decimal CountStoreOutgoing
        {
            get
            {
                _CountStoreOutgoing = 0;
                if (CountStoreAdditionOutgoinglist != null)
                {
                    foreach (var item in CountStoreAdditionOutgoinglist)
                    {
                        _CountStoreOutgoing += (item.price * item.count);

                    }
                }

                return _CountStoreOutgoing;
            }
            set { _CountStoreOutgoing = value; }
        }

        public IEnumerable<priceItem> CountStoreAdditionOutgoinglist { get; set; }
        public int? BudgetId { get; set; }
        public int TenantId { get; set; }

        public DateTime CreationDate { get; set; }
        public string ExaminationRequester { get; set; }
        public string TransRequester { get; set; }
        public string RobbingRequester { get; set; }
        public bool IsAddition { get; set; }
        //public string Currency { get; set; }
    }


    public class priceItem
    {
        public decimal price { get; set; }
        public int count { get; set; }
        public string Currency { get; set; }

    }

    public class LastYearBalanceVM
    {
        public decimal Price { get; set; }
        public string Currency { get; set; }
    }
}
