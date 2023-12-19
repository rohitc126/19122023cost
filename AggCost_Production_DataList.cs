using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BusinessLayer.Entity
{
     public class AggCost_Production_DataList
    {
         public decimal? SC_ID { get; set; }
         public decimal? SCC_ID { get; set; }
         public string locationName { get; set; }

         public string productName { get; set; }


      public string Long_Name { get; set; }


      public string EFFECTIVE_DATE { get; set; }

  
    }
    
}
