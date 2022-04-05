using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Services;
using Newtonsoft.Json;

namespace DataHSMforWeb
{
    /// <summary>
    /// Сводное описание для WebService1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Чтобы разрешить вызывать веб-службу из скрипта с помощью ASP.NET AJAX, раскомментируйте следующую строку. 
    [System.Web.Script.Services.ScriptService]
    public class WebService1 : System.Web.Services.WebService
    {
        #region Measures
        [WebMethod]
        public string GetFirstTabData(string DateFrom, string DateTo)
        {
            _Default DefObj = new _Default();
            return DefObj.GetFirstTabData(DateFrom, DateTo);
        }

        [WebMethod]
        public string GetDataForChart(string Device ,string Piece_id)
        {
            _Default DefObj = new _Default();
            return DefObj.GetDataForChart(Device, Piece_id);
        }        
        
        [WebMethod]
        public string GetDataOnCorrelation(string DateFrom, string DateTo)
        {
            _Default DefObj = new _Default();
            return DefObj.GetDataOnCorrelation(DateFrom, DateTo);
        }
        #endregion

        #region AnalizeBlobs
        [WebMethod]
        public string GetDataAnalyzeBlobs(string DateFrom, string DateTo, string MinRange, string MaxRange, string Tail, string ChekedRB)
        {
            _Default DefObj = new _Default();
            return DefObj.GetDataAnalyzeBlobs(DateFrom, DateTo, MinRange, MaxRange, Tail, ChekedRB);
        }
        
        [WebMethod]
        public string GetExcelAnalyzeBlobs(string DateFrom, string DateTo, string MinRange, string MaxRange, string Tail, string ChekedRB)
        {
            _Default DefObj = new _Default();
            return DefObj.GetExcelAnalyzeBlobs(DateFrom, DateTo, MinRange, MaxRange, Tail, ChekedRB);
        }        
        
        [WebMethod]
        public string GetCSVAnalyzeBlobs(string DateFrom, string DateTo, string MinRange, string MaxRange, string Tail, string ChekedRB)
        {
            _Default DefObj = new _Default();
            return DefObj.GetCSVAnalyzeBlobs(DateFrom, DateTo, MinRange, MaxRange, Tail, ChekedRB);
        }
        #endregion

        #region Rolls
        [WebMethod]
        public string GetRollGraph(string DateObj, string Location, string RollId)
        {
            _Default DefObj = new _Default();
            return DefObj.BuildGraph(DateObj, Location, RollId);
        }

        [WebMethod]
        public string GetRollID(string DateObj, string Location)
        {
            _Default DefObj = new _Default();
            return DefObj.RollID(DateObj.ToString(), Location.ToString());
        }
        #endregion

        #region Analyze
        [WebMethod]
        public string GetColNames()
        {
            _Default DefObj = new _Default();
            return DefObj.GetColNames();
        } 
        
        [WebMethod]
        public string GetData_4(string DateFrom, string DateTo)
        {
            _Default DefObj = new _Default();
            return DefObj.GetData_4(DateFrom, DateTo);
        }        
        
        [WebMethod]
        public string GetDataTable_4(string DateFrom, string DateTo, string min, string max, string[] st_gradeArr, string count, string[] arr, string checkedRB)
        {
            _Default DefObj = new _Default();
            return DefObj.GetDataTable_4(DateFrom, DateTo, min, max, st_gradeArr, count, arr, checkedRB);
        }
        #endregion
    }
}
