using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Entity
{
   public class Agg_CostProd_View
    {
        
            public decimal? SC_ID { get; set; }

            public string locationName { get; set; }
            public string productName { get; set; }
            public string Long_Name { get; set; }
            //public SelectList PLANT_LIST { get; set; }

            public DateTime EFFECTIVE_DATE { get; set; }
            public string hdnAGGCOSTPROD_DATE { get; set; }

            public List<AggCostProdView_Dtl> AggCostView_List { get; set; }

          
    }
   public class AggCostProdView_Dtl
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
