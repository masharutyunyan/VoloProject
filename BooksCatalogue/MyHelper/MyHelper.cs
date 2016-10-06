using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BooksCatalogue.MyHelper
{
    public static  class MyHelper
    {
        public static DateTime GetShortDate( DateTime date)
        {
           
            DateTime shortDate = Convert.ToDateTime( date.Day.ToString() + "/"+ date.Month.ToString() + "/"+ date.Year.ToString());
            return shortDate;
        }

       
    }
}