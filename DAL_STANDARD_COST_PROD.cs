using BusinessLayer.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace BusinessLayer.DAL
{
    public class DAL_STANDARD_COST_PROD
    {
        public List<AggCost_Production_Dtl> GET_Standard_CostProd_DTLS(string Comp_Code, string Branch_Code, string EFFECTIVE_DATE)
        {
            SqlParameter[] param = {
                                       new SqlParameter("@COMP_CODE", Comp_Code),
                                       new SqlParameter("@BRANCH_CODE", Branch_Code),  
                                       new SqlParameter("@Effective_Date", EFFECTIVE_DATE),  
                                          
                                           //new SqlParameter("@LocationCode", LocationCode), 
                                           // new SqlParameter("@productCode", ProductCode),  
                                           //  new SqlParameter("@ProductMine_Code", ProductMine_code),  
                                    
                                   };
            DataTable dt = new DataAccess(sqlConnection.GetConnectionString_SGX()).GetDataTable("[AGG].[USP_SELECT_STANDARD_COST_PROD_DTLS]", CommandType.StoredProcedure, param);
            List<AggCost_Production_Dtl> list = new List<AggCost_Production_Dtl>();
            AggCost_Production_Dtl dtl = null;


            foreach (DataRow row in dt.Rows)
            {
                dtl = new AggCost_Production_Dtl();
                dtl.PCSG_ID = Convert.ToInt32(row["PCSG_ID"] == DBNull.Value ? 0 : row["PCSG_ID"]);
                dtl.PCSG_NAME = Convert.ToString(row["PCSG_NAME"]);
                dtl.SC_ID = Convert.ToDecimal(row["SC_ID"] == DBNull.Value ? 0 : row["SC_ID"]);
                dtl.SCC_ID = Convert.ToDecimal(row["SCC_ID"] == DBNull.Value ? 0 : row["SCC_ID"]);
                dtl.PCG_ID = Convert.ToInt32(row["PCG_ID"] == DBNull.Value ? 0 : row["PCG_ID"]);
                dtl.PCG_NAME = Convert.ToString(row["PCG_NAME"]);
                dtl.LocationCode = Convert.ToString(row["LocationCode"]);
                dtl.ProductCode = Convert.ToString(row["productCode"]);
                dtl.ProductMine_code = Convert.ToInt32(row["ProductMine_Code"] == DBNull.Value ? 0 : row["ProductMine_Code"]);
                //dtl.EFFECTIVE_DATE = Convert.ToString(row["Effective_Date"]);
                dtl.StandardCost = Convert.ToInt32(row["StandardCost"] == DBNull.Value ? 0 : row["StandardCost"]);
                list.Add(dtl);
            }
            return list;
        }
        public string INSERT_Standard_CostProd(string Comp_Code, string Branch_Code, string Emp_Code, AggCost_Production Aggcost)
        {
            string errorMsg = "";

            using (var connection = new SqlConnection(sqlConnection.GetConnectionString_SGX()))
            {
                connection.Open();
                SqlCommand command;
                SqlTransaction transactionScope = null;
                transactionScope = connection.BeginTransaction(IsolationLevel.ReadCommitted);
                try
                {
                    SqlParameter[] param =
                    { 
                      new SqlParameter("@ERRORSTR", SqlDbType.VarChar, 200)
                     ,new SqlParameter("@SC_ID", SqlDbType.Decimal)  
                     ,new SqlParameter("@COMP_CODE", Comp_Code)
                     ,new SqlParameter("@BRANCH_CODE", Branch_Code)
  
                             ,new SqlParameter("@LocationCode",( Aggcost.LocationCode == null)?(object)DBNull.Value : Aggcost.LocationCode)
                     ,new SqlParameter("@Effective_Date", (Aggcost.EFFECTIVE_DATE == null)?(object)DBNull.Value : Aggcost.EFFECTIVE_DATE)
                     ,new SqlParameter("@productCode", (Aggcost.ProductCode == null)?(object)DBNull.Value : Aggcost.ProductCode)
                     ,new SqlParameter("@ProductMine_Code", (Aggcost.ProductMine_code == null)?(object)DBNull.Value : Aggcost.ProductMine_code) 
                     ,new SqlParameter("@ADDED_BY", Emp_Code)  
                    };
  
                    param[0].Direction = ParameterDirection.Output; 
                    param[1].Direction = ParameterDirection.Output;

                    new DataAccess().InsertWithTransaction("[AGG].[USP_INSERT_STANDARD_COST_PROD_HDR]", CommandType.StoredProcedure, out command, connection, transactionScope, param);
                    decimal SC_ID = (decimal)command.Parameters["@SC_ID"].Value;
                    string error_1 = (string)command.Parameters["@ERRORSTR"].Value;
                    if (SC_ID == -1) { errorMsg = error_1; }

                    if (SC_ID > 0)
                    {
                        if (Aggcost.AggCost_Production_Dtl_List != null)
                        {
                            foreach (AggCost_Production_Dtl item in Aggcost.AggCost_Production_Dtl_List)
                            {
                                if (item.StandardCost.HasValue && item.StandardCost.Value != 0)
                                {
                                    SqlParameter[] param2 =
                                    {
                                       new SqlParameter("@ERRORSTR", SqlDbType.VarChar, 200)
                                      ,new SqlParameter("@SCC_ID", SqlDbType.Decimal)  
                                      ,new SqlParameter("@SC_ID", SC_ID)  
                                      ,new SqlParameter("@PCG_ID", (item.PCG_ID == null) ? 0  : item.PCG_ID)
                                      ,new SqlParameter("@PCSG_ID", (item.PCSG_ID == null) ? 0  : item.PCSG_ID)
                                      ,new SqlParameter("@StandardCost", (item.StandardCost == null) ? (object)DBNull.Value : item.StandardCost) 


                                    };
                                    param2[0].Direction = ParameterDirection.Output;
                                    param2[1].Direction = ParameterDirection.Output;
                                    new DataAccess().InsertWithTransaction("[AGG].[USP_INSERT_STANDARD_COST_PROD_dtl]", CommandType.StoredProcedure, out command, connection, transactionScope, param2);
                                    decimal SCC_ID = (decimal)command.Parameters["@SCC_ID"].Value;
                                    string error_2 = (string)command.Parameters["@ERRORSTR"].Value;
                                    if (SCC_ID == -1) { errorMsg = error_2; }

                                }
                            }
                        }
                    }
                    if (errorMsg == "")
                    {
                        transactionScope.Commit();
                    }
                    else
                    {
                        transactionScope.Rollback();
                    }
                }
                catch (Exception ex)
                {
                    try
                    {
                        transactionScope.Rollback();
                    }
                    catch (Exception ex1)
                    {
                        errorMsg = "Error: Exception occured. " + ex1.StackTrace.ToString();
                    }
                }
                finally
                {
                    connection.Close();
                }
            }
            return errorMsg;
        }
        public List<AggCost_Production_DataList> Select_Standard_Cost_Production_List(AggCost_Production Aggcost)
        {
            SqlParameter[] param = {    new SqlParameter("@Effective_Date", Aggcost.EFFECTIVE_DATE),
                          
                                   };

            DataSet ds = new DataAccess(sqlConnection.GetConnectionString_SGX()).GetDataSet("[AGG].[USP_GET_StandardCost_List]", CommandType.StoredProcedure, param);

            List<AggCost_Production_DataList> _list = new List<AggCost_Production_DataList>();
            DataTable dt = ds.Tables[0];
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow Rows in ds.Tables[0].Rows)
                {
                    _list.Add(new AggCost_Production_DataList
                    {
                        SC_ID = Convert.ToDecimal(Rows["SC_ID"] == DBNull.Value ? 0 : Rows["SC_ID"]),
                        //SCC_ID = Convert.ToDecimal(Rows["SCC_ID"] == DBNull.Value ? 0 : Rows["SCC_ID"]),
                        EFFECTIVE_DATE = Convert.ToString(Rows["Effective_Date"]),
                        locationName = Convert.ToString(Rows["locationName"]),
                        productName = Convert.ToString(Rows["productName"]),
                        Long_Name = Convert.ToString(Rows["Long_Name"]),

                    });
                }
            }

            return _list;
        }
        public Agg_CostProd_View VIEW_STANDARD_COSTPROD(decimal SC_ID)
        {
            SqlParameter[] param = { new SqlParameter("@SC_ID", SC_ID) };

            DataSet ds = new DataAccess(sqlConnection.GetConnectionString_SGX()).GetDataSet("[AGG].[USP_STANDARD_COST_PROD_VIEW]", CommandType.StoredProcedure, param);

            Agg_CostProd_View costProd = new Agg_CostProd_View();
            costProd.AggCostView_List = new List<AggCostProdView_Dtl>(); // Initialize the list

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    AggCostProdView_Dtl dtl = new AggCostProdView_Dtl();
                    dtl.PCG_NAME = Convert.ToString(row["PCG_NAME"] == DBNull.Value ? "" : row["PCG_NAME"]);
                    dtl.PCSG_NAME = Convert.ToString(row["PCSG_NAME"] == DBNull.Value ? "" : row["PCSG_NAME"]);
                    dtl.StandardCost = Convert.ToInt32(row["StandardCost"] == DBNull.Value ? 0 : row["StandardCost"]);

                    costProd.AggCostView_List.Add(dtl); // Add the object to the list
                }

                DataRow firstRow = ds.Tables[0].Rows[0];
                costProd.SC_ID = Convert.ToDecimal(firstRow["SC_ID"]);
                costProd.locationName = Convert.ToString(firstRow["locationName"]);
                costProd.productName = Convert.ToString(firstRow["productName"]);
                costProd.Long_Name = Convert.ToString(firstRow["Long_Name"]);
                costProd.EFFECTIVE_DATE = Convert.ToDateTime(firstRow["Effective_Date"]);
            }

            return costProd;
        }

     
    }
}
