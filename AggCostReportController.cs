using BusinessLayer;
using BusinessLayer.DAL;
using BusinessLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eSGBIZ.Controllers
{
    public class AggCostReportController : BaseController
    {
        //
        // GET: /AggCostReport/
  [Authorize]
        public ActionResult AggCostProduction()
        {
            return View();
        }

  public ActionResult AggCostProduction_Entry(string EFFECTIVE_DATE)
  {
       ViewBag.Title = "Standard Cost Of Production";
      AggCost_Production Aggcost = new AggCost_Production();

      SelectDate _selectDate = new DAL_Common().GetSelect_Date(emp.Comp_Code, emp.BranchCode, "EFFECTIVE_DATE");
      Aggcost.EFFECTIVE_DATE = _selectDate.SELECT_DATE;
      Aggcost.hdnAGGCOSTPROD_DATE = _selectDate.SELECT_DATE;

      DateTime dt;

      if (string.IsNullOrWhiteSpace(EFFECTIVE_DATE))
      {
          dt = DateTime.Now;
      }
      else
      {

          if (DateTime.TryParse(EFFECTIVE_DATE, out dt) == false)
          {
              dt = DateTime.Now;
          }
      }

      Aggcost.EFFECTIVE_DATE = dt.ToString("dd/MM/yyyy");



      List<CostLocation_Mater> LocList = new DAL_Common().GetLocationList(emp.Employee_Code);
      Aggcost.Location_List = new SelectList(LocList, "locationCode", "locationName");

      List<CostProduct_Master> ProdList = new DAL_Common().GetProductList("L0009");
      Aggcost.PRODUCT_LIST = new SelectList(ProdList, "ProductCode", "productName", "ProductDesc");

      List<CostPlantMine_Master> PlantMineList = new DAL_Common().GetPlantMineList("L0011");
      Aggcost.PLANT_LIST = new SelectList(PlantMineList, "ProductMine_code", "Long_Name");

      List<AggCost_Production_Dtl> dtl = new DAL_STANDARD_COST_PROD().GET_Standard_CostProd_DTLS(emp.Comp_Code, emp.BranchCode,Aggcost.EFFECTIVE_DATE);
      Aggcost.AggCost_Production_Dtl_List = dtl;

      return View(Aggcost);
  }

          [HttpPost]
          [SubmitButtonSelector(Name = "Save")]
          [ActionName("AggCostProduction_Entry")]
          public ActionResult STANDARD_COSTPROD_SAVE(AggCost_Production Aggcost)
          {
              ViewBag.Header = "Standard Cost Of Production";

              var errors = ModelState.Where(x => x.Value.Errors.Count > 0).Select(x => new { x.Key, x.Value.Errors }).ToArray();

              if (ModelState.IsValid)
              {
                  try
                  {

                      string result = new DAL_STANDARD_COST_PROD().INSERT_Standard_CostProd(emp.Comp_Code, emp.BranchCode, emp.Employee_Code, Aggcost);

                      if (result == "")
                      {
                          Success(string.Format("<b>Standard Cost Of Production Inserted Successfully</b>"), true);
                          return RedirectToAction("AggCostProduction", "AggCostReport");
                      }
                      else
                      {
                          Danger(string.Format("<b>Error:</b>" + result), true);
                      }

                  }
                  catch (Exception ex)
                  {
                      Danger(string.Format("<b>Error:</b>" + ex.Message), true);
                  }
              }
              else
              {
                  Danger(string.Format("<b>Error:102 :</b>" + string.Join("; ", ModelState.Values.SelectMany(z => z.Errors).Select(z => z.ErrorMessage))), true);
              }

              //if ((Aggcost.ErrMsg ?? "").Trim() != "")
              //{
              //    Aggcost.EFFECTIVE_DATE = "";
              //    Danger(string.Format("<b>" + Aggcost.ErrMsg + "</b>"), true);
              //}

              return View(Aggcost);
          }
        
          public ActionResult _AggCostProductionList()
          {
              return PartialView("_AggCostProductionList");
          }
          [HttpPost]
          public ActionResult _StandardCost_Prod_Data_List(string E_DATE)
          {
              // Server Side Processing
              int start = Convert.ToInt32(Request["start"]);
              int length = Convert.ToInt32(Request["length"]);
              string searchValue = Request["search[value]"];
              string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
              string sortDirection = Request["order[0][dir]"];
              int totalRow = 0;

              AggCost_Production Aggcost = new AggCost_Production();
              List<AggCost_Production_DataList> AggcostData = new List<AggCost_Production_DataList>();
              try
              {
                  //Aggcost.From_DT = fDate;
                  //Aggcost.To_DT = tDate;

                  Aggcost.EFFECTIVE_DATE = Convert.ToString( E_DATE);

                  AggcostData = new DAL_STANDARD_COST_PROD().Select_Standard_Cost_Production_List(Aggcost);

                  totalRow = AggcostData.Count();

              }
              catch (Exception ex)
              {
                  Danger(string.Format("<b>Exception occured.</b>"), true);
              }

              if (!string.IsNullOrEmpty(searchValue)) // Filter Operation
              {
                  AggcostData = AggcostData.
                      Where(x => x.EFFECTIVE_DATE.ToLower().Contains(searchValue.ToLower())
                      || x.locationName.ToLower().Contains(searchValue.ToLower()) ||

                          x.productName.ToLower().Contains(searchValue.ToLower()) || x.Long_Name.ToString().Contains(searchValue.ToLower())
                           ).ToList<AggCost_Production_DataList>();

              }
              int totalRowFilter = AggcostData.Count();

              if (length == -1)
              {
                  length = totalRow;
              }
              AggcostData = AggcostData.Skip(start).Take(length).ToList<AggCost_Production_DataList>();

              var jsonResult = Json(new { data = AggcostData, draw = Request["draw"], recordsTotal = totalRow, recordsFiltered = totalRowFilter }, JsonRequestBehavior.AllowGet);
              jsonResult.MaxJsonLength = int.MaxValue;
              return jsonResult;
          }
      

          public ActionResult AggCostProductionView(decimal SC_ID)
          {
              Agg_CostProd_View _objStCost = new Agg_CostProd_View();
              _objStCost = new DAL_STANDARD_COST_PROD().VIEW_STANDARD_COSTPROD(SC_ID);

              return PartialView("AggCostProductionView", _objStCost);

          }

    }
}
