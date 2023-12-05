using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.CrossCutting.FinancialYear
{
    public class FinancialYearProvider
    {

        static public DateTime CurrentYearStartDate
        { 
            get {
                if (DateTime.Now <= new DateTime(DateTime.Now.Year, 6, 30))
                    return new DateTime(DateTime.Now.Year - 1, 7, 1);
                else
                    return new DateTime(DateTime.Now.Year, 7, 1);
                }
        }
        static public DateTime CurrentYearEndDate
        {
            get
            {
                if (DateTime.Now <= new DateTime(DateTime.Now.Year, 6, 30))
                    return new DateTime(DateTime.Now.Year, 6, 30);
                else
                    return new DateTime(DateTime.Now.Year + 1, 6, 30);
            }
        }
        static public DateTime LastYearStartDate
        {
            get
            {
                if (DateTime.Now <= new DateTime(DateTime.Now.Year, 6, 30))
                    return new DateTime(DateTime.Now.Year - 2, 7, 1);
                else
                    return new DateTime(DateTime.Now.Year - 1, 7, 1);
            }
        }
        static public DateTime LastYearEndDate
        {
            get
            {
                if (DateTime.Now <= new DateTime(DateTime.Now.Year, 6, 30))
                    return new DateTime(DateTime.Now.Year - 1, 6, 30);
                else
                    return new DateTime(DateTime.Now.Year, 6, 30);
            }
        }
        static public int CurrentYear
        {
            get
            {
                if (DateTime.Now <= new DateTime(DateTime.Now.Year, 6, 30))
                    return DateTime.Now.Year - 1;
                else
                    return DateTime.Now.Year;
            }
        }
        static public int LastYear
        {
            get
            {
                if (DateTime.Now <= new DateTime(DateTime.Now.Year, 6, 30))
                    return DateTime.Now.Year - 2;
                else
                    return DateTime.Now.Year-1;
            }
        }
       

    }
}
