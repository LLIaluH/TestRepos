using DataHSMforWeb.App;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
//using Oracle.DataAccess.Client;
using Oracle.ManagedDataAccess.Types;
//using Oracle.DataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.UI;
//using System.DirectoryServices.ActiveDirectory;
using System.DirectoryServices;

namespace DataHSMforWeb
{
    public partial class _Default : Page
    {
        OracleConnection connect;
        OracleCommand cmd;

        string query;
        string pathCSVFiles = HttpContext.Current.Server.MapPath("~/CSVFiles/");

        const string PartNormalPath = "ExcelBook_";

        protected void Page_Load(object sender, EventArgs e)
        {
            //string strDomain = "D0";
            //string strUserId = "MIKHEEV_AV1";

            //string strPath = "LDAP://DC=" + strDomain.Trim() + ",DC=com";

            //DirectoryEntry de = new DirectoryEntry(strPath);
            //DirectorySearcher deSearch = new DirectorySearcher(de);

            //deSearch.Filter = "(&(objectClass=user)(SAMAccountName=" + strUserId.Trim() + "))";

            //SearchResult results = deSearch.FindOne();
            //if ((results == null))
            //{
            //    //No User Found
            //}
            //else
            //{
            //    //User Found
            //}
        }

        private void InitializeComponent()
        {

        }

        private static string ConvertToJson(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        private static int[] ArrStringToInt(params string[] param)
        {
            int[] result = new int[param.Length];
            for (int i = 0; i < param.Length; i++)
            {
                if (param[i].Length > 0)
                    result[i] = Convert.ToInt32(param[i]);
                else
                    result[i] = 0;
            }
            return result;
        }

        public bool GetCon()
        {
            dbConnection conn = new dbConnection();
            if (conn.OpenDbConnection(out connect))
            {
                return true;
            }
            return false;
        }

        private static DateTime StingToDateTime(string Date)
        {
            DateTime date = new DateTime();
            try
            {
                if (Date == "")
                    date = DateTime.Now;
                else
                    date = DateTime.ParseExact(Date, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
            }
            catch (Exception ex)
            {
                Logs.LogWriteError(ex.Message.ToString());
            }
            return date;
        }

        public static string[] ListToArrayStr<T>(List<T> list)
        {
            int c = list.Count;
            string[] arr = new string[c];
            for (int i = 0; i < c; i++)
            {
                arr[i] = list[i].ToString();
            }
            return arr;
        }

        private S_L2L2_FinisherData ByteArrayToStruct(byte[] bytes)
        {
            int size = Marshal.SizeOf(typeof(S_L2L2_FinisherData));
            IntPtr buffer = Marshal.AllocHGlobal(size);
            Marshal.Copy(bytes, 0, buffer, size);
            var myStruct = (S_L2L2_FinisherData)Marshal.PtrToStructure(buffer, typeof(S_L2L2_FinisherData));
            Marshal.FreeHGlobal(buffer);
            return myStruct;
        }

        #region Measures

        public string GetFirstTabData(string DateFrom, string DateTo)
        {
            dynamic reslult = new System.Dynamic.ExpandoObject();
            DateTime d1 = StingToDateTime(DateFrom);
            DateTime d2 = StingToDateTime(DateTo);
            
            return ReadList(d1, d2); ;
        }

        public string GetDataForChart(string device, string piece_id)
        {
            int deviceInt = Convert.ToInt32(device);
            dynamic result = CreatePlots(deviceInt, piece_id);

            return ConvertToJson(result);
        }

        public string GetDataOnCorrelation(string DateFrom, string DateTo)
        {
            DateTime d1 = StingToDateTime(DateFrom);
            DateTime d2 = StingToDateTime(DateTo);
            dynamic result = new System.Dynamic.ExpandoObject();
            try
            {
                string selList = "SELECT * FROM SC_LOOPER_MEASURE WHERE" +
                                 " LAST_UPD BETWEEN :datefrom and :dateto  ORDER BY LAST_UPD ASC";
                DataTable dataTable = new DataTable();
                GetCon();
                cmd = connect.CreateCommand();
                cmd.CommandText = selList;
                cmd.Parameters.Add("datefrom", d1);
                cmd.Parameters.Add("dateto", d2);
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    dataTable.Load(reader);
                }
                //DataView view = new DataView(dataTable);
                result.GridsData = dataTable;
            }
            catch (Exception ex)
            {
                Logs.LogWriteError(ex.Message.ToString());
                result.error = "Error " + ex.Message.ToString();
                return ConvertToJson(result);
            }
            finally
            {
                connect.Close();
            }
            return ConvertToJson(result);
        }

        public string ReadList(DateTime date1, DateTime date2)
        {
            dynamic result = new System.Dynamic.ExpandoObject();
            try
            {
                string selList = "SELECT PIECE_ID, COIL_ID, STEEL_GRADE, TARGET_THICK, TARGET_WIDTH, FINISHED FROM CM_COIL_OUTPUT WHERE" +
                                 " FINISHED BETWEEN :datefrom and :dateto and ARCHIVE_STATUS = 'PRODUCED' ORDER BY FINISHED ASC";
                //string selSteelGrade = "SELECT DISTINCT STEEL_GRADE FROM CM_COIL_OUTPUT WHERE FINISHED BETWEEN :datefrom and :dateto and ARCHIVE_STATUS = 'PRODUCED'";
                DataTable dataTable = new DataTable();
                GetCon();
                cmd = connect.CreateCommand();
                cmd.CommandText = selList;
                cmd.Parameters.Add("datefrom", date1);
                cmd.Parameters.Add("dateto", date2);
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    dataTable.Load(reader);
                }
                DataView view = new DataView(dataTable);
                DataTable distinctValues = view.ToTable(true, "STEEL_GRADE");

                string[] steel_gr = new string[distinctValues.Rows.Count + 1];
                steel_gr[0] = "";
                for (int i = 0; i < distinctValues.Rows.Count; i++)
                    steel_gr[i + 1] = (string)distinctValues.Rows[i][0];
                steel_gr.Distinct().ToArray();
                distinctValues = view.ToTable(true, "TARGET_THICK");

                string[] thick = new string[distinctValues.Rows.Count + 1];
                thick[0] = "";
                for (int i = 0; i < distinctValues.Rows.Count; i++)
                    thick[i + 1] = distinctValues.Rows[i][0].ToString();
                thick.Distinct().ToArray();
                Array.Sort(thick);

                result.steel_gr = steel_gr;
                result.thick = thick;
                result.GridsData = dataTable;
                return ConvertToJson(result);
            }
            catch (Exception ex)
            {
                Logs.LogWriteError(ex.Message.ToString());
                result.error = "Error " + ex.Message.ToString();
                return ConvertToJson(result);
            }
            finally
            {
                connect.Close();
            }
        }

        public dynamic CreatePlots(int device, string piece_id)
        {
            dynamic result;
            try
            {
                GetCon();
                cmd = connect.CreateCommand();
                cmd.Parameters.Add("piece_id", piece_id);                
                switch (device)
                {
                    case 19:                                                    //1
                    case 20:                                                    //2
                    case 21: result = CreateTempChart(device); break;           //3
                    case 22:                                                    //4
                    case 23: result = CreateWidthChart(device); break;          //5
                    case 24:                                                    //6
                    case 25: result = CreateThickChart(device); break;          //25
                    default: result = CreateMeasureCharts(device); break;       //...
                }
            }
            catch (Exception ex)
            {
                Logs.LogWriteError(ex.Message.ToString());
                result = new System.Dynamic.ExpandoObject();
                result.error = "Error " + ex.Message.ToString();
            }
            finally
            {
                connect.Close();
            }
            return result;
        }

        public dynamic CreateTempChart(int device)
        {
            dynamic result = new System.Dynamic.ExpandoObject();
            try
            {
                int pyrNum;
                string selDCPyr = "SELECT SELECTED_PYRO FROM COOLING_REPORT WHERE PIECE_ID = :piece_id";
                cmd.CommandText = selDCPyr;
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    reader.Read();
                    pyrNum = reader.GetInt32(0);
                }
                string selExitFMTemp = "SELECT TEMPERATURE,OFFSET,SAMPLE_NUMBER FROM SC_TEMPERATURE_MEASURE where PIECE_ID = :piece_id and DEVICE_ID = 6";
                string selEntryFMTemp = "SELECT TEMPERATURE,OFFSET,SAMPLE_NUMBER FROM SC_TEMPERATURE_MEASURE where PIECE_ID = :piece_id and DEVICE_ID = 11";
                string selDCTemp;
                if (pyrNum == 1)
                    selDCTemp = "SELECT TEMPERATURE,OFFSET,SAMPLE_NUMBER FROM SC_TEMPERATURE_MEASURE where PIECE_ID = :piece_id and DEVICE_ID = 8";
                else
                    selDCTemp = "SELECT TEMPERATURE,OFFSET,SAMPLE_NUMBER FROM SC_TEMPERATURE_MEASURE where PIECE_ID = :piece_id and DEVICE_ID = 9";

                string selTolEntryFM = "SELECT ENTRY_FM_TEMP,ENTRY_FM_POS_TOL, ENTRY_FM_NEG_TOL FROM CM_COIL_OUTPUT where PIECE_ID = :piece_id";
                string selTolExitFM = "SELECT EXIT_FM_TEMP,EXIT_FM_POS_TOL, EXIT_FM_NEG_TOL FROM CM_COIL_OUTPUT where PIECE_ID = :piece_id";
                string selTolDC = "SELECT EXIT_WW_TEMP,EXIT_WW_POS_TOL, EXIT_WW_NEG_TOL FROM CM_COIL_OUTPUT where PIECE_ID = :piece_id";
                
                if (device == 19)
                    cmd.CommandText = selEntryFMTemp;
                else if (device == 20)
                    cmd.CommandText = selExitFMTemp;
                else cmd.CommandText = selDCTemp;
                float[] measure;
                float[] offset;
                int sampleNum;
                var a = cmd.ExecuteReader();
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    reader.Read();
                    OracleBlob blobtemp = reader.GetOracleBlob(0);
                    OracleBlob bloboffset = reader.GetOracleBlob(1);
                    sampleNum = reader.GetInt32(2);
                    measure = new float[sampleNum];
                    offset = new float[sampleNum];
                    byte[] buffer = new byte[4 * sampleNum];
                    blobtemp.Read(buffer, 0, buffer.Length);
                    for (int i = 0; i < sampleNum; i++)
                        measure[i] = BitConverter.ToSingle(buffer, 4 * i);
                    bloboffset.Read(buffer, 0, buffer.Length);
                    for (int i = 0; i < sampleNum; i++)
                        offset[i] = BitConverter.ToSingle(buffer, 4 * i);
                }

                if (device == 19)
                    cmd.CommandText = selTolEntryFM;
                else if (device == 20)
                    cmd.CommandText = selTolExitFM;
                else cmd.CommandText = selTolDC;

                int target;
                int postol;
                int negtol;
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    reader.Read();
                    target = (int)reader.GetDouble(0);
                    postol = (int)reader.GetDouble(1);
                    negtol = (int)reader.GetDouble(2);
                    reader.Close();
                }
                if (postol < 0) postol = 15;
                if (negtol < 0) negtol = 15;

                result.target = target;
                result.postol = postol;
                result.negtol = negtol;
                result.measure = measure;
                result.offset = offset;
            }
            catch (Exception ex)
            {
                Logs.LogWriteError(ex.Message.ToString());
                result.error = "Error " + ex.Message.ToString();
                result.NoData = true;
            }
            return result;
        }

        public dynamic CreateWidthChart(int device)
        {
            dynamic result = new System.Dynamic.ExpandoObject();
            try
            {
                string selRMWidth = "SELECT WIDTH,OFFSET,SAMPLE_NUMBER FROM SC_WIDTH_MEASURE where PIECE_ID = :piece_id and DEVICE_ID = 2";
                string selFMWidth = "SELECT WIDTH,OFFSET,SAMPLE_NUMBER FROM SC_WIDTH_MEASURE where PIECE_ID = :piece_id and DEVICE_ID = 3";

                string selTolFMWidth = "SELECT ORDER_WIDTH,TARGET_WIDTH,WIDTH_POS_TOL FROM CM_COIL_OUTPUT where PIECE_ID = :piece_id";
                string selAvgTempFM = "SELECT AVERAGE FROM SC_TEMPERATURE_MEASURE WHERE PIECE_ID = :piece_id and DEVICE_ID = 6";
                string selAvgTempRM = "SELECT AVERAGE FROM SC_TEMPERATURE_MEASURE WHERE PIECE_ID = :piece_id and DEVICE_ID = 2";

                if (device == 22)
                    cmd.CommandText = selRMWidth;
                else
                    cmd.CommandText = selFMWidth;
                float[] measure;
                float[] offset;
                int sampleNum;
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    reader.Read();
                    OracleBlob blobtemp = reader.GetOracleBlob(0);
                    OracleBlob bloboffset = reader.GetOracleBlob(1);
                    sampleNum = reader.GetInt32(2);
                    measure = new float[sampleNum];
                    offset = new float[sampleNum];
                    byte[] buffer = new byte[4 * sampleNum];
                    blobtemp.Read(buffer, 0, buffer.Length);
                    for (int i = 0; i < sampleNum; i++)
                        measure[i] = BitConverter.ToSingle(buffer, 4 * i);
                    bloboffset.Read(buffer, 0, buffer.Length);
                    for (int i = 0; i < sampleNum; i++)
                        offset[i] = BitConverter.ToSingle(buffer, 4 * i);
                }

                cmd.CommandText = selTolFMWidth;
                int target;
                int postol;
                //int negtol;
                int order;
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    reader.Read();
                    order = (int)reader.GetDouble(0);
                    target = (int)reader.GetDouble(1);
                    postol = (int)reader.GetDouble(2);
                    reader.Close();
                }
                if (postol < 0) postol = 20;
                if (device == 22)
                    cmd.CommandText = selAvgTempFM;
                else
                    cmd.CommandText = selAvgTempRM;
                double avgtemp;
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    reader.Read();
                    avgtemp = reader.GetDouble(0);
                }

                double kudr_coeff = 1 / (1 + 0.000012 * (avgtemp - 15));

                result.target = target;
                result.postol = postol / 2;
                result.negtol = postol / 2;
                result.order = order;
                result.kudr_coeff = Math.Round(kudr_coeff, 4);
                result.measure = measure;
                result.offset = offset;
            }
            catch (Exception ex)
            {
                Logs.LogWriteError(ex.Message.ToString());
                result.error = "Error " + ex.Message.ToString();
                result.NoData = true;
            }
            return result;
        }

        public dynamic CreateThickChart(int device)
        {
            dynamic result = new System.Dynamic.ExpandoObject();
            try
            {
                string selThickness = "SELECT THICK,OFFSET,SAMPLE_NUMBER FROM SC_PROFILE_MEASURE where PIECE_ID = :piece_id";
                // string selThickness = "SELECT OFFSET, SAMPLE_NUMBER FROM SC_PROFILE_MEASURE where PIECE_ID = :piece_id";
                string selCLDeviation = "SELECT CENTRAL_DEV,OFFSET,SAMPLE_NUMBER FROM SC_PROFILE_MEASURE where PIECE_ID = :piece_id";
                string selTolThickness = "SELECT TARGET_THICK,THICK_POS_TOL, THICK_NEG_TOL FROM CM_COIL_OUTPUT where PIECE_ID = :piece_id";
                string selAvgTemp = "SELECT AVERAGE FROM SC_TEMPERATURE_MEASURE WHERE PIECE_ID = :piece_id and DEVICE_ID = 6";
                if (device == 24)
                    cmd.CommandText = selThickness;
                else
                    cmd.CommandText = selCLDeviation;
                float[] measure;
                float[] offset;
                int sampleNum;
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    reader.Read();
                    OracleBlob blobtemp = reader.GetOracleBlob(0);
                    OracleBlob bloboffset = reader.GetOracleBlob(1);
                    sampleNum = reader.GetInt32(2);
                    measure = new float[sampleNum];
                    offset = new float[sampleNum];
                    byte[] buffer = new byte[4 * sampleNum];
                    blobtemp.Read(buffer, 0, buffer.Length);
                    for (int i = 0; i < sampleNum; i++)
                        measure[i] = BitConverter.ToSingle(buffer, 4 * i);
                    bloboffset.Read(buffer, 0, buffer.Length);
                    for (int i = 0; i < sampleNum; i++)
                        offset[i] = BitConverter.ToSingle(buffer, 4 * i);
                }
                if (device == 24)
                {
                    cmd.CommandText = selTolThickness;
                    double target;
                    double postol;
                    double negtol;
                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        reader.Read();
                        target = reader.GetDouble(0);
                        postol = reader.GetDouble(1);
                        negtol = reader.GetDouble(2);
                        reader.Close();
                    }

                    cmd.CommandText = selAvgTemp;
                    double avgtemp;
                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        reader.Read();
                        avgtemp = reader.GetDouble(0);
                    }
                    double kudr_coeff = 1 / (1 + 0.000012 * (avgtemp - 15));

                    result.target = target;
                    result.postol = postol;
                    result.negtol = negtol;
                    result.kudr_coeff = Math.Round(kudr_coeff, 4);
                }
                result.offset = offset;
                result.measure = measure;

            }
            catch (Exception ex)
            {
                Logs.LogWriteError(ex.Message.ToString());
                result.error = "Error " + ex.Message.ToString();
                result.NoData = true;
            }
            return result;
        }

        public dynamic CreateMeasureCharts(int device)
        {
            dynamic result = new System.Dynamic.ExpandoObject();
            try
            {
                S_L2L2_FinisherData finisherData = new S_L2L2_FinisherData();
                string selThickness = "SELECT FINISHER_MESSAGE FROM SC_MEASURE_MESSAGE where PIECE_ID = :piece_id";
                cmd.CommandText = selThickness;
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    reader.Read();
                    OracleBlob blobtemp = reader.GetOracleBlob(0);
                    int sizestr = (int)blobtemp.Length;
                    byte[] buffer = new byte[sizestr];
                    blobtemp.Read(buffer, 0, sizestr);
                    finisherData = ByteArrayToStruct(buffer);
                    float[] a = finisherData.exitThick;
                    string[] str = new string[180];
                    for (int i = 0; i < 180; i++)
                        str[i] = a[i].ToString();
                    result.comboBox3 = str;
                    
                    switch (device)
                    {
                        case 1:  result.measure = finisherData.forceF1;        break;
                        case 2:  result.measure = finisherData.forceF2;        break;
                        case 3:  result.measure = finisherData.forceF3;        break;
                        case 4:  result.measure = finisherData.forceF4;        break;
                        case 5:  result.measure = finisherData.forceF5;        break;
                        case 6:  result.measure = finisherData.forceF6;        break;
                        case 7:  result.measure = finisherData.bendingF1;      break;
                        case 8:  result.measure = finisherData.bendingF2;      break;
                        case 9:  result.measure = finisherData.bendingF3;      break;
                        case 10: result.measure = finisherData.bendingF4;      break;
                        case 11: result.measure = finisherData.bendingF5;      break;
                        case 12: result.measure = finisherData.bendingF6;      break;
                        case 13: result.measure = finisherData.speedF1;        break;
                        case 14: result.measure = finisherData.speedF2;        break;
                        case 15: result.measure = finisherData.speedF3;        break;
                        case 16: result.measure = finisherData.speedF4;        break;
                        case 17: result.measure = finisherData.speedF5;        break;
                        case 18: result.measure = finisherData.speedF6;        break;
                        default: result.measure = finisherData.relativeOffset; break;
                    }
                    result.offset = finisherData.relativeOffset;
                }

            }
            catch (Exception ex)
            {
                Logs.LogWriteError(ex.Message.ToString());
                result.error = "Error " + ex.Message.ToString();
                result.NoData = true;
            }
            return result;
        }

        #endregion

        #region AnalyzeBlobs

        float[,] flatness = new float[300, 10];
        int sample;
        const int PROFILE_SCALE = 100;

        public string GetDataAnalyzeBlobs(string DateFrom, string DateTo, string minRange, string maxRange, string tail, string chekedRB)
        {
            return ConvertToJson(GetDataAnalyzeBlobsAsDataTable(DateFrom, DateTo, minRange, maxRange, tail, chekedRB));
        }
        private dynamic GetDataAnalyzeBlobsAsDataTable(string DateFrom, string DateTo, string minRange, string maxRange, string tail, string chekedRB)
        {
            dynamic result = new System.Dynamic.ExpandoObject();
            int[] ArrNums = ArrStringToInt(minRange, maxRange, tail, chekedRB);
            try
            {
                DateTime begin = StingToDateTime(DateFrom) ;
                DateTime last = StingToDateTime(DateTo);
                TimeSpan ts = new TimeSpan(0, 0, 0);
                begin = begin.Date - ts;
                DataTable dataTable = new DataTable();
                query = " SELECT A.PIECE_ID, COIL_ID, STEEL_GRADE, TARGET_THICK, TARGET_WIDTH, FINISHED FROM SC_FLAT_MEASURE A, CM_COIL_OUTPUT B" +
                        " WHERE A.PIECE_ID = B.PIECE_ID AND FINISHED BETWEEN :datefrom and :dateto and ARCHIVE_STATUS = 'PRODUCED'" +
                        "  ORDER BY FINISHED ASC";
                GetCon();
                cmd = connect.CreateCommand();
                cmd.CommandText = query;
                cmd.Parameters.Add("datefrom", begin);
                cmd.Parameters.Add("dateto", last);
                // cmd.Parameters.Add("steel_grade", comboBox1.SelectedText);
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    dataTable.Load(reader);
                    //dataGridView1.DataSource = dataTable;
                }
                //тут надо вызывать методы дополнения таблицы новыми колонками
                DataTable ResdataTable;
                ResdataTable = FlatnessCalc(dataTable, minRange, maxRange);
                ResdataTable = CenterLineCalc(ResdataTable, minRange, maxRange, tail, chekedRB);
                DataView view = new DataView(dataTable);

                DataTable distinctValues = view.ToTable(true, "STEEL_GRADE");

                //для комбобокса
                /////////////////////////////////////////////////////////////////////////
                    string[] steel_gr = new string[distinctValues.Rows.Count + 1];
                    steel_gr[0] = "";
                    for (int i = 0; i < distinctValues.Rows.Count; i++)
                        steel_gr[i + 1] = (string)distinctValues.Rows[i][0];
                    steel_gr.Distinct().ToArray();
                    //comboBox1.DataSource = steel_gr;
                /////////////////////////////////////////////////////////////////////////
                result.GridsData = ResdataTable;
                result.ComboboxData = steel_gr;
            }
            catch (Exception ex)
            {                
                Logs.LogWriteError(ex.Message.ToString());
                result.error = "Error " + ex.Message.ToString();
                return result;
            }
            finally
            {
                connect.Close();

            }
            return result;
        }

        private DataTable FlatnessCalc(DataTable dt, string minRange, string maxRange)
        {
            int RowCount = dt.Rows.Count;
            DataColumn maxFlat = new DataColumn("MAX_ELONGATION");
            dt.Columns.Add(maxFlat);
            DataColumn avgFlat = new DataColumn("AVG_ELONGATION");
            dt.Columns.Add(avgFlat);
            float[] coeffs = new float[100];
            float[] offset = new float[100];
            int sampleNum;
            double[] avgMassFlatness = new double[RowCount];
            double[] maxMassFlatness = new double[RowCount];

            for (int i = 0; i < RowCount - 1; i++)
            {
                // Clear parameters for the new charts //
                sample = 0;
                Array.Clear(flatness, 0, flatness.Length);
                //////////////////////////////////////////
                query = "SELECT SAMPLE_NUMBER, FLATNESS,OFFSET FROM SC_FLAT_MEASURE where PIECE_ID = :piece_id";
                cmd = connect.CreateCommand();
                cmd.CommandText = query;
                cmd.Parameters.Add("piece_id", dt.Rows[i].ItemArray[0].ToString());
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    reader.Read();
                    sampleNum = reader.GetInt32(0);
                    OracleBlob blobFlatCoeffs = reader.GetOracleBlob(1);
                    OracleBlob bloboffset = reader.GetOracleBlob(2);
                    byte[] buffer = new byte[100];
                    Array.Resize(ref buffer, (int)blobFlatCoeffs.Length);
                    Array.Resize(ref coeffs, sampleNum * 10);
                    Array.Resize(ref offset, sampleNum);
                    blobFlatCoeffs.Read(buffer, 0, buffer.Length);
                    for (int j = 0; j < sampleNum * 10; j++)
                        coeffs[j] = BitConverter.ToSingle(buffer, 4 * j);
                    bloboffset.Read(buffer, 0, buffer.Length);
                    for (int j = 0; j < sampleNum; j++)
                        offset[j] = BitConverter.ToSingle(buffer, 4 * j);
                }
                int k = 0;
                int startMeas = Convert.ToInt32(minRange ) * 1000;
                int endMeas = Convert.ToInt32(maxRange) * 1000;
                do
                {
                    if (offset[k] > startMeas)
                    {
                        for (int j = 0; j < 10; j++)
                            flatness[sample, j] = coeffs[10 * k + j];
                        sample++;
                    }
                    k++;
                } while (offset[k] < endMeas);

                for (int index = 0; index < sample; index++)
                {
                    double maxElongation = 0;
                    double avgElongation = 0;
                    for (int j = 0; j < PROFILE_SCALE; j++)
                    {
                        double x = -1 + 2 * (double)j / PROFILE_SCALE;
                        double y = (flatness[index, 0] + Math.Pow(x, 1) * flatness[index, 1] +
                                   Math.Pow(x, 2) * flatness[index, 2] + Math.Pow(x, 3) * flatness[index, 3] +
                                   Math.Pow(x, 4) * flatness[index, 4] + Math.Pow(x, 5) * flatness[index, 5]);
                        y /= 100; // flatness UI[m/m] = dL/L * 100000 -> [mm/m] dL/L = IU / 100
                        if (y > maxElongation) maxElongation = y;
                        avgElongation += y;
                    }
                    avgElongation /= PROFILE_SCALE;
                    if (maxElongation > maxMassFlatness[i]) maxMassFlatness[i] = maxElongation;
                    avgMassFlatness[i] += avgElongation;
                }
                avgMassFlatness[i] /= sample;

                dt.Rows[i]["MAX_ELONGATION"] = String.Format("{0:0.000}", maxMassFlatness[i]);
                dt.Rows[i]["AVG_ELONGATION"] = String.Format("{0:0.000}", avgMassFlatness[i]);
            }
            return dt;
        }

        private DataTable CenterLineCalc(DataTable dt, string minRange, string maxRange, string tail, string idRb)
        {
            int RowCount = dt.Rows.Count;
            DataColumn avgCL = new DataColumn("CL_DEV_AVG");
            dt.Columns.Add(avgCL);
            DataColumn stdCL = new DataColumn("CL_DEV_STD");
            dt.Columns.Add(stdCL);
            DataColumn maxCL = new DataColumn("CL_DEV_MAX");
            dt.Columns.Add(maxCL);
            DataColumn minCL = new DataColumn("CL_DEV_MIN");
            dt.Columns.Add(minCL);
            int sampleNum;
            int range;
            double[] measure;
            double[] offset;

            for (int i = 0; i < RowCount - 1; i++)
            {
                double avgCLDev = 0;
                double maxCLDev = 0;
                double minCLDev = 0;
                double stdCLDev = 0;
                int endIndex = 0;
                int startIndex = 0;
                //////////////////////////////////////////
                query = "SELECT CENTRAL_DEV,OFFSET,SAMPLE_NUMBER FROM SC_PROFILE_MEASURE where PIECE_ID = :piece_id";
                cmd = connect.CreateCommand();
                cmd.CommandText = query;
                cmd.Parameters.Add("piece_id", dt.Rows[i].ItemArray[0].ToString());
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    reader.Read();
                    OracleBlob blobtemp = reader.GetOracleBlob(0);
                    OracleBlob bloboffset = reader.GetOracleBlob(1);
                    sampleNum = reader.GetInt32(2);
                    measure = new double[sampleNum];
                    offset = new double[sampleNum];
                    byte[] buffer = new byte[4 * sampleNum];
                    blobtemp.Read(buffer, 0, buffer.Length);
                    for (int j = 0; j < sampleNum; j++)
                        measure[j] = BitConverter.ToSingle(buffer, 4 * j);
                    bloboffset.Read(buffer, 0, buffer.Length);
                    for (int j = 0; j < sampleNum; j++)
                        offset[j] = BitConverter.ToSingle(buffer, 4 * j);
                }
                double coilLength = offset[sampleNum - 1];
                int startMeas;
                int endMeas;
                int k = 0;
                // Define length range for statistic
                range = Convert.ToInt32(idRb);
                switch (range)
                {
                    case 1:
                        {
                            startMeas = Convert.ToInt32(minRange) * 1000;
                            endMeas = Convert.ToInt32(maxRange) * 1000; ;
                            do
                            {
                                if (offset[k] >= startMeas)
                                {
                                    if (endIndex == 0)
                                        startIndex = k;
                                    endIndex++;
                                }
                                k++;
                            } while ((offset[k - 1] < endMeas) || (k < sampleNum));
                            break;
                        }
                    case 2:
                        {
                            startIndex = 0;
                            endIndex = sampleNum;
                            break;
                        }
                    case 3:
                        {
                            startMeas = Convert.ToInt32(tail) * 1000;
                            k = sampleNum - 1;
                            while (offset[k] > coilLength - startMeas)
                            {
                                k--;
                            }
                            startIndex = k;
                            endIndex = sampleNum - 1;
                            break;
                        }
                    default: break;
                }
                //////////////////
                if (endIndex > startIndex)
                {
                    minCLDev = measure[startIndex];
                    maxCLDev = measure[startIndex];
                    double[] subMeasure = new double[endIndex - startIndex];
                    int subIdx = 0;
                    for (int index = startIndex; index < endIndex; index++)
                    {
                        subMeasure[subIdx] = measure[index];
                        subIdx++;
                        if (measure[index] > maxCLDev)
                            maxCLDev = measure[index];
                        if (measure[index] < minCLDev)
                            minCLDev = measure[index];
                        avgCLDev += measure[index];
                    }
                    avgCLDev /= (endIndex - startIndex);
                    stdCLDev = CalculateStandardDeviation(subMeasure);
                }

                dt.Rows[i]["CL_DEV_AVG"] = String.Format("{0:0.00}", avgCLDev);
                dt.Rows[i]["CL_DEV_MAX"] = String.Format("{0:0.00}", maxCLDev);
                dt.Rows[i]["CL_DEV_MIN"] = String.Format("{0:0.00}", minCLDev);
                dt.Rows[i]["CL_DEV_STD"] = String.Format("{0:0.00}", stdCLDev);
            }
            return dt;
        }

        public string GetCSVAnalyzeBlobs(string DateFrom, string DateTo, string minRange, string maxRange, string tail, string chekedRB)
        {
            dynamic result = new System.Dynamic.ExpandoObject();
            DataTable dt = GetDataAnalyzeBlobsAsDataTable(DateFrom, DateTo, minRange, maxRange, tail, chekedRB).GridsData;
            int ColCount = dt.Columns.Count;
            int RowCount = dt.Rows.Count;
            string stringCSV = "";
            if (RowCount > 0)
            {
                for (int j = 0; j < ColCount; j++)
                {
                    if (j == 0)
                        stringCSV += dt.Columns[j].ColumnName;
                    else
                        stringCSV += "," + dt.Columns[j].ColumnName;
                }
                stringCSV += "\n";
                for (int i = 1; i < RowCount; i++)
                {
                    for (int j = 0; j < ColCount; j++)
                    {
                        if (j == 0)
                            stringCSV += "\"" + dt.Rows[i].ItemArray[j] + "\"";
                        else
                            stringCSV += ",\"" + dt.Rows[i].ItemArray[j] + "\"";
                    }
                    stringCSV += "\n";
                }

                string FileName = PartNormalPath + DateTime.Now.ToString("ddMMyyyy-HH-mm-ss") + ".csv";

                try
                {
                    using (StreamWriter sw = new StreamWriter(pathCSVFiles + FileName, false, System.Text.Encoding.Default))
                    {
                        sw.WriteLine(stringCSV);
                    }
                    byte[] bytes = File.ReadAllBytes(pathCSVFiles + FileName);
                    result.File = Convert.ToBase64String(bytes, 0, bytes.Length);
                    result.FileName = FileName;
                    File.Delete(pathCSVFiles + FileName);
                }
                catch (Exception ex)
                {
                    Logs.LogWriteError(ex.Message.ToString());
                    result.error = "Error " + ex.Message.ToString();
                }
            }            
            return ConvertToJson(result);
        }

        public string GetExcelAnalyzeBlobs(string DateFrom, string DateTo, string minRange, string maxRange, string tail, string chekedRB)
        {
            dynamic result = new System.Dynamic.ExpandoObject();
            try
            {
                DataTable dt = GetDataAnalyzeBlobsAsDataTable(DateFrom, DateTo, minRange, maxRange, tail, chekedRB).GridsData;
                int ColCount = dt.Columns.Count;
                int RowCount = dt.Rows.Count;
                //Microsoft.Office.Interop.Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook ExcelWorkBook;
                Microsoft.Office.Interop.Excel.Worksheet ExcelWorkSheet;
                //Книга.
                ExcelWorkBook = ExcelApp.Workbooks.Add(System.Reflection.Missing.Value);
                //Таблица.
                ExcelWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)ExcelWorkBook.Worksheets.get_Item(1);

                ExcelWorkSheet.Columns.NumberFormat = "@";
                //ExcelWorkSheet.Columns.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                //ExcelWorkSheet.Columns.Borders.Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium;

                for (int j = 0; j < ColCount; j++)
                {
                    ExcelWorkSheet.Cells[1, j + 1] = dt.Columns[j].ColumnName;
                }
                for (int i = 1; i < RowCount; i++)
                {
                    for (int j = 0; j < ColCount; j++)
                    {
                        ExcelWorkSheet.Cells[i + 1, j + 1] = dt.Rows[i].ItemArray[j];
                    }
                }
                //Вызываем нашу созданную эксельку. (на сервере)
                //ExcelApp.Visible = true;
                //ExcelApp.UserControl = true;

                string FileName = PartNormalPath + DateTime.Now.ToString("ddMMyyyy-HH-mm-ss") /*+ ".xlsx"*/;
                string path = HttpContext.Current.Server.MapPath("~/ExcelFilesTemp/");
                ExcelWorkBook.Close(true, path + FileName);
                byte[] bytes = File.ReadAllBytes(path + FileName + ".xlsx");
                result.File = Convert.ToBase64String(bytes, 0, bytes.Length);
                result.FileName = FileName + ".xlsx";
                File.Delete(path + FileName + ".xlsx");
                ExcelApp.Quit();
            }
            catch (Exception ex)
            {
                Logs.LogWriteError(ex.Message.ToString());
                result.error = "Error " + ex.Message.ToString();
            }
            return ConvertToJson(result);
        }

        private double CalculateStandardDeviation(IEnumerable<double> values)
        {
            double standardDeviation = 0;

            if (values.Any())
            {
                // Compute the average.     
                double avg = values.Average();

                // Perform the Sum of (value-avg)_2_2.      
                double sum = values.Sum(d => Math.Pow(d - avg, 2));

                // Put it all together.      
                standardDeviation = Math.Sqrt((sum) / (values.Count() - 1));
            }

            return standardDeviation;
        }
    
    #endregion

        #region Rolls
        public string BuildGraph(string DateObj, string Location, string RollId)
        {
            query = "select offset,delta_radius,mount_num from ROLL_WR_WEAR_B_HIST " +
                    "where mount_num = (select mount_num from ROLL_WR_HIST " +
                    "where last_upd between :datefrom and :dateto and location = :loc and rolled_length>0 and roll_id = :roll_id )" +
                    " and roll_id = :roll_id ";
            float[] wear;
            float[] offset;
            DateTime date = StingToDateTime(DateObj);

            int res = ReadBlob(date, query, RollId, Location, out offset, out wear);
            dynamic reslult = new System.Dynamic.ExpandoObject();
            reslult.TextBoxValues = GetRollHist(date, RollId);
            if (res > 0)
            {
                //возвращаем объект для чарта
                reslult.a = (int)wear.Min();
                reslult.offset = offset;                
                reslult.wear = wear;
            }
            else
            {
                reslult.error = true;
            }
            return ConvertToJson(reslult);
        }

        public int ReadBlob(DateTime date, string query, string roll_id, string location, out float[] offset, out float[] wear)
        {
            try
            {
                GetCon();
                OracleCommand cmd =  connect.CreateCommand();
                cmd = connect.CreateCommand();
                cmd.CommandText = query;
                cmd.Parameters.Add("datefrom", date);
                cmd.Parameters.Add("dateto", date.AddDays(1));
                cmd.Parameters.Add("location", location);
                cmd.Parameters.Add("roll_id", roll_id);

                wear = new float[100];
                offset = new float[100];
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    reader.Read();
                    //int temp = reader.GetInt32(2);
                    OracleBlob blobwear = reader.GetOracleBlob(1);
                    OracleBlob bloboffset = reader.GetOracleBlob(0);
                    byte[] buffer = new byte[400];
                    blobwear.Read(buffer, 0, buffer.Length);
                    for (int i = 0; i < 100; i++)
                        wear[i] = BitConverter.ToSingle(buffer, 4 * i);
                    bloboffset.Read(buffer, 0, buffer.Length);
                    for (int i = 0; i < 100; i++)
                        offset[i] = BitConverter.ToSingle(buffer, 4 * i);
                }
                if (wear != null)
                    return 1;
                else
                    return 0;
            }
            catch (Exception ex)
            {
                Logs.LogWriteError(ex.Message.ToString());
                wear = null;
                offset = null;
                return -1;
            }
            finally
            {
                connect.Close();
            }
        }

        public string RollID(string DateObj, string Location)//название метода должно совпадать с ключом в объекте json
        {
            dynamic reslult = new System.Dynamic.ExpandoObject();
            query = "select roll_id from ROLL_WR_HIST where last_upd between :datefrom and :dateto and location = :location " +
                    "and rolled_length>0 order by location,last_upd asc";
            DateTime date = StingToDateTime(DateObj);

            try
            {
                GetCon();
                OracleCommand cmd = connect.CreateCommand();
                cmd = connect.CreateCommand();
                cmd.CommandText = query;
                cmd.Parameters.Add("datefrom", date);
                cmd.Parameters.Add("dateto", date.AddDays(1));
                cmd.Parameters.Add("location", Location);

                List<string> Res = new List<string>();
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Res.Add(reader.GetString(0));
                    }
                }
                //string[] ResArr = ListToArrayStr(Res);
                reslult.Arr = Res;
            }
            catch (Exception ex)
            {
                Logs.LogWriteError(ex.Message.ToString());
                reslult = "";
            }
            finally
            {
                connect.Close();
            }
            return ConvertToJson(reslult);
        }

        private dynamic GetRollHist(DateTime date, string roll_id)
        {
            dynamic reslult = new System.Dynamic.ExpandoObject();
            query = "select diameter,rolled_length,rolled_weight from ROLL_WR_HIST " +
                "where last_upd between :datefrom and :dateto and rolled_length>0 and roll_id = :roll_id";
            try
            {
                GetCon();
                OracleCommand cmd = connect.CreateCommand();
                cmd = connect.CreateCommand();
                cmd.CommandText = query;
                cmd.Parameters.Add("datefrom", date);
                cmd.Parameters.Add("dateto", date.AddDays(1));
                cmd.Parameters.Add("roll_id", roll_id);

                List<string> Res = new List<string>();
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Res.Add(reader.GetValue(0).ToString());
                        Res.Add(reader.GetValue(1).ToString());
                        Res.Add(reader.GetValue(2).ToString());
                    }
                }
                reslult.ArrTextValues = Res;
            }
            catch (Exception ex)
            {
                Logs.LogWriteError(ex.Message.ToString());
                reslult = "";
            }
            finally
            {
                connect.Close();
            }
            return reslult;
        }
        #endregion

        #region Analyze

        public string GetData_4(string dateFrom, string dateTo)
        {
            dynamic result = new System.Dynamic.ExpandoObject();
            DateTime date1 = StingToDateTime(dateFrom);
            DateTime date2 = StingToDateTime(dateTo);
            try
            {
                //string selList = "SELECT * FROM COOLING_REPORT WHERE" +
                //                 " LAST_UPD BETWEEN :datefrom and :dateto ORDER BY LAST_UPD DESC";
                string selSteelGrade = "SELECT DISTINCT STEEL_GRADE FROM CM_COIL_OUTPUT WHERE FINISHED BETWEEN :datefrom and :dateto ORDER BY STEEL_GRADE ASC";
                DataTable dataTable = new DataTable();
                GetCon();
                cmd = connect.CreateCommand();
                cmd.CommandText = selSteelGrade;
                cmd.Parameters.Add("datefrom", date1);
                cmd.Parameters.Add("dateto", date2);
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    dataTable.Load(reader);
                    result.GridsData = dataTable;
                }

                string[] steel_gr = new string[dataTable.Rows.Count];
                for (int i = 0; i < dataTable.Rows.Count; i++)
                    steel_gr[i] = (string)dataTable.Rows[i][0];
                result.steel_gr = steel_gr;
            }
            catch (Exception ex)
            {
                Logs.LogWriteError(ex.Message.ToString());
                result.error = "Error " + ex.Message.ToString();
            }
            finally
            {
                connect.Close();
            }
            return ConvertToJson(result);
        }

        public string GetColNames()
        {
            dynamic result = new System.Dynamic.ExpandoObject();
            try
            {
                string sel = "Select COLUMN_NAME from user_tab_columns where table_name = 'GENERAL_PARAMETERS'";
                DataTable dataTable = new DataTable();
                GetCon();
                cmd = connect.CreateCommand();
                cmd.CommandText = sel;
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    dataTable.Load(reader);
                }
                result.GridsData = dataTable;
            }
            catch (Exception ex)
            {
                Logs.LogWriteError(ex.Message.ToString());
                result.error = "Error " + ex.Message.ToString();
            }
            finally
            {
                connect.Close();
            }
            return ConvertToJson(result);
        }

        public string GetDataTable_4(string DateFrom, string DateTo, string min, string max, string[] st_gradeArr, string count, string[] arr, string checkedRB)
        {
            dynamic result = new System.Dynamic.ExpandoObject();
            DateTime d1 = StingToDateTime(DateFrom);
            DateTime d2 = StingToDateTime(DateTo);
            int Min = Convert.ToInt32(min);
            int Max = Convert.ToInt32(max);
            int Count = Convert.ToInt32(count);
            int CheckedRB = Convert.ToInt32(checkedRB);
            try
            {
                string columns = "";
                string st_grades = "";

                string percTol = "dc_perc_in_tol";
                if (CheckedRB == 2)
                {
                    percTol = "perc_in_tol_finish";
                }

                for (int i = 0; i < arr.Length; i++)
                {
                    if (i!=0)
                    {
                        columns += ", ";
                    }
                    columns += "a." + arr[i];
                }

                for (int i = 0; i < st_gradeArr.Length; i++)
                {
                    if (i != 0)
                    {
                        st_grades += ", ";
                    }
                    st_grades += "'" + st_gradeArr[i] + "'";
                }

                string AnalyzeSelect = " with MyTable" +
                                " AS (Select c.*,   CASE" +
                                                " WHEN " + percTol + ">= :good THEN  'Good'" +
                                                " END AS Good," +
                                                " CASE" +
                                                " WHEN " + percTol + "< :good and " + percTol + "> :bad THEN  'Normal'" +
                                                " END AS Normal," +
                                                " CASE" +
                                                " WHEN " + percTol + "<= :bad THEN  'Bad'" +
                                                " END AS Bad" +
                                    " from cooling_report c)" +

                              " select  " + columns +
                              ", ROUND(cast(count(Good) as real)/count(*),2) as Good,  ROUND(cast(count(Normal) as real)/count(*),2) as Normal," +
                              " ROUND(cast(count(Bad) as real)/count(*),2) as Bad, count(*) as Total" +
                              " from MyTable m, GENERAL_PARAMETERS a" +
                              " where m.piece_id = a.piece_id" +
                              " and m.last_upd between :date1 and :date2 and " +
                              "m.steel_family IN( " + st_grades + " )" +
                              " group by " +
                              " (" + columns + ")" +
                              " HAVING        Count (*) >= :count " +
                              " ORDER BY  Good DESC";
                DataTable dataTable = new DataTable();
                GetCon();
                cmd = connect.CreateCommand();
                cmd.CommandText = AnalyzeSelect;

                cmd.BindByName = true; //!!! it is necessary for multiple use one parameter inside query
                OracleParameter[] parameters =
                {
                    new OracleParameter("good",     OracleDbType.Varchar2, Max,         ParameterDirection.Input),
                    new OracleParameter("bad",      OracleDbType.Varchar2, Min,         ParameterDirection.Input),
                    new OracleParameter("date1",    OracleDbType.Date,     d1,          ParameterDirection.Input),
                    new OracleParameter("date2",    OracleDbType.Date,     d2,          ParameterDirection.Input),
                    new OracleParameter("count",    OracleDbType.Varchar2, Count,       ParameterDirection.Input),
                };

                cmd.Parameters.AddRange(parameters);
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    dataTable.Load(reader);
                    result.GridsData = dataTable;
                }
            }
            catch (Exception ex)
            {
                Logs.LogWriteError(ex.Message.ToString());
                result.error = "Error " + ex.Message.ToString();
            }
            finally
            {
                connect.Close();
            }
            return ConvertToJson(result);
        }
        #endregion

    }
}