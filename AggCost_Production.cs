using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BusinessLayer.Entity
{
     public class AggCost_Production
    {
         public string ErrMsg { get; set; }
         public decimal SC_ID { get; set; }
      [Required(ErrorMessage = "Select Location")]
         public string LocationCode { get; set; }
      public SelectList Location_List { get; set; }

      [Required(ErrorMessage = "Select Product")]
      public string ProductCode { get; set; }
      public SelectList PRODUCT_LIST { get; set; }

      [Required(ErrorMessage = "Select Plant/Mine")]
      public int? ProductMine_code { get; set; }
      public SelectList PLANT_LIST { get; set; }

      public string EFFECTIVE_DATE { get; set; }
      public string hdnAGGCOSTPROD_DATE { get; set; }

      public List<AggCost_Production_Dtl> AggCost_Production_Dtl_List { get; set; }

      public int? PCSG_ID { get; set; }
      public int? PCG_ID { get; set; }
      public int? StandardCost { get; set; }
    }
     public class AggCost_Production_Dtl
     {

         public decimal SCC_ID { get; set; }
         public decimal SC_ID { get; set; }
         public int? PCG_ID { get; set; }
         public string PCG_NAME { get; set; }
         public string PCSG_NAME { get; set; }
         public int? PCSG_ID { get; set; }
         public int? StandardCost { get; set; }

         public string BRANCH_CODE { get; set; }
         public string COMP_CODE { get; set; }
         public string LocationCode { get; set; }
         public string ProductCode { get; set; }
         public int? ProductMine_code { get; set; }
         public string EFFECTIVE_DATE { get; set; }
     }
}
