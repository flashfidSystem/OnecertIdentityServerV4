//using System.Data;
//using System.Data.SqlClient;
//using System.Globalization;
//using System.Security.Cryptography;
//using System.Text;
//using System.Text.RegularExpressions;
//using System.Xml;

//namespace OnecertApiV1.Services
//{
//    public static class clsUtility
//    {
//        public static DataTable AddSpaceRec(DataTable SrcTB)
//        {
//            DataTable AddSpaceRecRet;
//            var Col = new DataColumn();
//            DataRow Dr;
//            var NewTb = new DataTable();

//            foreach (DataColumn currentCol in SrcTB.Columns)
//            {
//                Col = currentCol;
//                NewTb.Columns.Add(Col.ColumnName, Col.DataType);
//                NewTb.Columns[Col.Ordinal].MaxLength = Col.MaxLength;
//            }
//            Dr = NewTb.NewRow();

//            foreach (DataColumn currentCol1 in NewTb.Columns)
//            {
//                Col = currentCol1;
//                switch (Col.DataType.FullName ?? "")
//                {
//                    case "System.String":
//                        {
//                            Dr[Col.ColumnName] = "";
//                            break;
//                        }
//                    case "System.DateTime":
//                        {
//                            Dr[Col.ColumnName] = DBNull.Value;
//                            break;
//                        }

//                    default:
//                        {
//                            Dr[Col.ColumnName] = 0;
//                            break;
//                        }
//                }
//            }
//            NewTb.Rows.Add(Dr);

//            foreach (DataRow DrI in SrcTB.Rows)
//            {
//                DataRow NewDR;
//                NewDR = NewTb.NewRow();
//                foreach (DataColumn currentCol2 in NewTb.Columns)
//                {
//                    Col = currentCol2;
//                    NewDR[Col.ColumnName] = DrI[Col.ColumnName];
//                }
//                NewTb.Rows.Add(NewDR);
//            }

//            AddSpaceRecRet = NewTb;
//            SrcTB = null;
//            NewTb = null;
//            return AddSpaceRecRet;
//        }

//        public static DataTable AddSpaceRec(DataTable SrcTB, bool AddSpace = true)
//        {
//            DataTable AddSpaceRecRet;
//            var Col = new DataColumn();
//            DataRow Dr;
//            var NewTb = new DataTable();

//            foreach (DataColumn currentCol in SrcTB.Columns)
//            {
//                Col = currentCol;
//                NewTb.Columns.Add(Col.ColumnName, Col.DataType);
//                NewTb.Columns[Col.Ordinal].MaxLength = Col.MaxLength;
//            }

//            if (AddSpace)
//            {
//                Dr = NewTb.NewRow();
//                foreach (DataColumn currentCol1 in NewTb.Columns)
//                {
//                    Col = currentCol1;
//                    switch (Col.DataType.FullName ?? "")
//                    {
//                        case "System.String":
//                            {
//                                Dr[Col.ColumnName] = "";
//                                break;
//                            }
//                        case "System.DateTime":
//                            {
//                                Dr[Col.ColumnName] = DBNull.Value;
//                                break;
//                            }

//                        default:
//                            {
//                                Dr[Col.ColumnName] = 0;
//                                break;
//                            }
//                    }
//                }
//                NewTb.Rows.Add(Dr);

//            }

//            foreach (DataRow DrI in SrcTB.Rows)
//            {
//                DataRow NewDR;
//                NewDR = NewTb.NewRow();
//                foreach (DataColumn currentCol2 in NewTb.Columns)
//                {
//                    Col = currentCol2;
//                    NewDR[Col.ColumnName] = DrI[Col.ColumnName];
//                }
//                NewTb.Rows.Add(NewDR);
//            }

//            AddSpaceRecRet = NewTb;
//            SrcTB = null;
//            NewTb = null;
//            return AddSpaceRecRet;
//        }

//        public static DataTable AddSpaceRec(DataTable SrcTB, string DataValueField, string DataTextField, bool AddSpace = true)
//        {
//            DataTable AddSpaceRecRet;
//            var Col = new DataColumn();
//            DataRow Dr;
//            var NewTb = new DataTable();

//            foreach (DataColumn currentCol in SrcTB.Columns)
//            {
//                Col = currentCol;
//                NewTb.Columns.Add(Col.ColumnName, Col.DataType);
//                NewTb.Columns[Col.ColumnName].MaxLength = Col.MaxLength;
//            }

//            if (AddSpace)
//            {
//                Dr = NewTb.NewRow();
//                foreach (DataColumn currentCol1 in NewTb.Columns)
//                {
//                    Col = currentCol1;
//                    switch (Col.DataType.FullName ?? "")
//                    {
//                        case "System.String":
//                            {
//                                Dr[Col.ColumnName] = "";
//                                break;
//                            }
//                        case "System.DateTime":
//                            {
//                                Dr[Col.ColumnName] = DBNull.Value;
//                                break;
//                            }

//                        default:
//                            {
//                                Dr[Col.ColumnName] = 0;
//                                break;
//                            }
//                    }
//                }
//                NewTb.Rows.Add(Dr);
//            }

//            foreach (DataRow DrI in SrcTB.Rows)
//            {
//                DataRow NewDR;
//                NewDR = NewTb.NewRow();
//                foreach (DataColumn currentCol2 in NewTb.Columns)
//                {
//                    Col = currentCol2;
//                    if ((Col.ColumnName ?? "") == (DataTextField ?? ""))
//                    {
//                        NewDR[DataTextField] = DrI[DataValueField] + " | " + DrI[DataTextField];
//                    }
//                    else
//                    {
//                        NewDR[Col.ColumnName] = DrI[Col.ColumnName];
//                    }
//                }
//                NewTb.Rows.Add(NewDR);
//            }

//            AddSpaceRecRet = NewTb;
//            SrcTB = null;
//            NewTb = null;
//            return AddSpaceRecRet;
//        }

//        public static DataTable AddSpaceRecString(DataTable SrcTB, string DataValueField, string DataTextField, bool AddSpace = true)
//        {
//            DataTable AddSpaceRecStringRet;
//            var Col = new DataColumn();
//            DataRow Dr;
//            var NewTb = new DataTable();

//            foreach (DataColumn currentCol in SrcTB.Columns)
//            {
//                Col = currentCol;
//                NewTb.Columns.Add(Col.ColumnName, Type.GetType("System.String"));
//                // NewTb.Columns(Col.ColumnName).MaxLength = Col.MaxLength
//            }

//            if (AddSpace)
//            {
//                Dr = NewTb.NewRow();
//                foreach (DataColumn currentCol1 in NewTb.Columns)
//                {
//                    Col = currentCol1;
//                    switch (Col.DataType.FullName ?? "")
//                    {
//                        case "System.String":
//                            {
//                                Dr[Col.ColumnName] = "";
//                                break;
//                            }
//                        case "System.DateTime":
//                            {
//                                Dr[Col.ColumnName] = DBNull.Value;
//                                break;
//                            }
//                        case "System.Decimal":
//                            {
//                                Dr[Col.ColumnName] = 0.0d;
//                                break;
//                            }

//                        default:
//                            {
//                                Dr[Col.ColumnName] = 0;
//                                break;
//                            }
//                    }
//                }
//                NewTb.Rows.Add(Dr);
//            }

//            foreach (DataRow DrI in SrcTB.Rows)
//            {
//                DataRow NewDR;
//                NewDR = NewTb.NewRow();
//                foreach (DataColumn currentCol2 in NewTb.Columns)
//                {
//                    Col = currentCol2;
//                    if ((Col.ColumnName ?? "") == (DataTextField ?? ""))
//                    {
//                        NewDR[DataTextField] = DrI[DataValueField] + " | " + DrI[DataTextField];
//                    }
//                    else
//                    {
//                        NewDR[Col.ColumnName] = DrI[Col.ColumnName];
//                    }
//                }
//                NewTb.Rows.Add(NewDR);
//            }

//            AddSpaceRecStringRet = NewTb;
//            SrcTB = null;
//            NewTb = null;
//            return AddSpaceRecStringRet;
//        }

//        public static DataTable AddSpaceRec(DataTable SrcTB, string DataValueField, string DataTextField1, string DataTextField2, bool AddSpace = true)
//        {
//            DataTable AddSpaceRecRet;
//            var Col = new DataColumn();
//            DataRow Dr;
//            var NewTb = new DataTable();

//            foreach (DataColumn currentCol in SrcTB.Columns)
//            {
//                Col = currentCol;
//                NewTb.Columns.Add(Col.ColumnName, Col.DataType);
//                NewTb.Columns[Col.ColumnName].MaxLength = Col.MaxLength;
//            }
//            if (AddSpace)
//            {
//                Dr = NewTb.NewRow();
//                foreach (DataColumn currentCol1 in NewTb.Columns)
//                {
//                    Col = currentCol1;
//                    switch (Col.DataType.FullName ?? "")
//                    {
//                        case "System.String":
//                            {
//                                Dr[Col.ColumnName] = "";
//                                break;
//                            }
//                        case "System.DateTime":
//                            {
//                                Dr[Col.ColumnName] = DBNull.Value;
//                                break;
//                            }

//                        default:
//                            {
//                                Dr[Col.ColumnName] = 0;
//                                break;
//                            }
//                    }
//                }
//                NewTb.Rows.Add(Dr);
//            }

//            foreach (DataRow DrI in SrcTB.Rows)
//            {
//                DataRow NewDR;
//                NewDR = NewTb.NewRow();
//                foreach (DataColumn currentCol2 in NewTb.Columns)
//                {
//                    Col = currentCol2;
//                    if ((Col.ColumnName ?? "") == (DataTextField2 ?? ""))
//                    {
//                        NewDR[DataTextField2] = DrI[DataValueField] + " | " + DrI[DataTextField1] + " | " + DrI[DataTextField2];
//                    }
//                    else
//                    {
//                        NewDR[Col.ColumnName] = DrI[Col.ColumnName];
//                    }
//                }
//                NewTb.Rows.Add(NewDR);
//            }

//            AddSpaceRecRet = NewTb;
//            SrcTB = null;
//            NewTb = null;
//            return AddSpaceRecRet;
//        }

//        public static DataTable AddALLRecToCompanyList(DataTable SrcTB)
//        {
//            DataTable AddALLRecToCompanyListRet;
//            var Col = new DataColumn();
//            DataRow Dr;
//            var NewTb = new DataTable();

//            foreach (DataColumn currentCol in SrcTB.Columns)
//            {
//                Col = currentCol;
//                NewTb.Columns.Add(Col.ColumnName, Col.DataType);
//                NewTb.Columns[Col.Ordinal].MaxLength = Col.MaxLength;
//            }
//            Dr = NewTb.NewRow();

//            Dr[0] = "9999999";
//            Dr[1] = "----- ALL Companies -----";

//            NewTb.Rows.Add(Dr);

//            foreach (DataRow DrI in SrcTB.Rows)
//            {
//                DataRow NewDR;
//                NewDR = NewTb.NewRow();
//                foreach (DataColumn currentCol1 in NewTb.Columns)
//                {
//                    Col = currentCol1;
//                    NewDR[Col.ColumnName] = DrI[Col.ColumnName];
//                }
//                NewTb.Rows.Add(NewDR);
//            }

//            AddALLRecToCompanyListRet = NewTb;
//            SrcTB = null;
//            NewTb = null;
//            return AddALLRecToCompanyListRet;
//        }

//        public static void alertbox(Page page, string msg)
//        {
//            string s = "<script language=JavaScript>";
//            s += "alert(\"" + msg + "\");";
//            s += "</script>";
//            page.ClientScript.RegisterClientScriptBlock(page.GetType(), "script1", s);
//        }

//        public static void year(DropDownList yearList, int StartYear)
//        {
//            var time = DateTime.Now;
//            int currentYear = time.Year;
//            yearList.Items.Clear();

//            for (int I = currentYear, loopTo = StartYear; I >= loopTo; I -= 1)
//            {
//                var itm = new ListItem(I.ToString().Trim(), I.ToString().Trim());
//                yearList.Items.Add(itm);
//            }
//            var mitm = new ListItem("", "");
//            yearList.Items.Insert(0, mitm);
//        }

//        public static void yearArr(DropDownList yearList, int[] YrArr)
//        {
//            var time = DateTime.Now;
//            int currentYear = time.Year;
//            yearList.Items.Clear();

//            for (int I = YrArr.GetLowerBound(0), loopTo = YrArr.GetUpperBound(0); I <= loopTo; I++)
//            {
//                var itm = new ListItem(YrArr[I].ToString(), YrArr[I].ToString());
//                yearList.Items.Add(itm);
//            }
//            var mitm = new ListItem("year", "");
//            yearList.Items.Insert(0, mitm);
//        }

//        public static void yearFuture(DropDownList yearList, int StartYear)
//        {
//            var time = DateTime.Now;
//            int currentYear = time.Year + 1;
//            yearList.Items.Clear();

//            for (int I = currentYear, loopTo = StartYear; I >= loopTo; I -= 1)
//            {
//                var itm = new ListItem(I.ToString().Trim(), I.ToString().Trim());
//                yearList.Items.Add(itm);
//            }
//            var mitm = new ListItem("year", "");
//            yearList.Items.Insert(0, mitm);
//        }

//        // * method to create drop down List control for month
//        public static void month(DropDownList monthList)
//        {
//            var months = new[] { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12" };
//            monthList.Items.Clear();

//            for (int a = 0, loopTo = months.Length - 1; a <= loopTo; a++)
//            {
//                var itm = new ListItem(monthDescription(months[a]), months[a]);
//                monthList.Items.Add(itm);
//            }
//            var mitm = new ListItem("month", "");
//            monthList.Items.Insert(0, mitm);
//        }
//        public static string monthDescription(string mnth)
//        {
//            string name = "";
//            switch (mnth ?? "")
//            {
//                case "01":
//                    {
//                        name = "January";
//                        break;
//                    }
//                case "02":
//                    {
//                        name = "February";
//                        break;
//                    }
//                case "03":
//                    {
//                        name = "March";
//                        break;
//                    }
//                case "04":
//                    {
//                        name = "April";
//                        break;
//                    }
//                case "05":
//                    {
//                        name = "May";
//                        break;
//                    }
//                case "06":
//                    {
//                        name = "June";
//                        break;
//                    }
//                case "07":
//                    {
//                        name = "July";
//                        break;
//                    }
//                case "08":
//                    {
//                        name = "August";
//                        break;
//                    }
//                case "09":
//                    {
//                        name = "September";
//                        break;
//                    }
//                case "10":
//                    {
//                        name = "October";
//                        break;
//                    }
//                case "11":
//                    {
//                        name = "November";
//                        break;
//                    }
//                case "12":
//                    {
//                        name = "December";
//                        break;
//                    }
//            }
//            return name;
//        }

//        // * method to create drop down List control for day
//        public static void day(DropDownList dayList)
//        {
//            dayList.Items.Clear();

//            for (int a = 1; a <= 31; a++)
//            {
//                if (a < 10)
//                {
//                    dayList.Items.Add("0" + a);
//                }
//                else
//                {
//                    dayList.Items.Add("" + a);
//                }
//            }
//            var mitm = new ListItem("day", "");
//            dayList.Items.Insert(0, mitm);
//        }

//        public static bool ValidateEmail(string strEmail)
//        {
//            bool ValidateEmailRet;
//            string strTmp;
//            var n = default(long);
//            string sEXT;
//            ValidateEmailRet = true; // Assume true on init 

//            sEXT = strEmail;
//            while ((sEXT.IndexOf(".") + 1) != 0)
//                //while (Strings.InStr(1, sEXT, ".") != 0)
//                sEXT = sEXT.Substring(sEXT.Length - (sEXT.Length - (sEXT.IndexOf(".") + 1)));
//            //sEXT = Strings.Right(sEXT, sEXT.Length - sEXT.IndexOf("."));
//            //sEXT = Strings.Right(sEXT, sEXT.Length - Strings.InStr(1, sEXT, "."));

//            if (string.IsNullOrEmpty(strEmail))
//            {
//                ValidateEmailRet = false;
//            }
//            else if ((strEmail.IndexOf("@") + 1) == 0)
//            {
//                ValidateEmailRet = false;
//            }
//            else if ((strEmail.IndexOf("@") + 1) == 1)
//            {
//                ValidateEmailRet = false;
//            }
//            else if ((strEmail.IndexOf("@") + 1) == strEmail.Length)
//            {
//                ValidateEmailRet = false;
//            }
//            else if (EXTisOK(sEXT) == false)
//            {
//                ValidateEmailRet = false;
//            }
//            else if (strEmail.Length < 6)
//            {
//                ValidateEmailRet = false;
//            }
//            strTmp = strEmail;
//            while ((strTmp.IndexOf("@") + 1) != 0)
//            {
//                n = 1L;
//                strTmp = strTmp.Substring(strTmp.Length - (strTmp.Length - (strTmp.IndexOf("@") + 1)));
//                //strTmp = Strings.Right(strTmp, strTmp.Length - strTmp.IndexOf("@"));
//            }
//            if (n > 1L)
//            {
//                ValidateEmailRet = false; // found more than one @ sign 
//            }

//            return ValidateEmailRet;
//        }


//        public static bool EXTisOK(string sEXT)
//        {
//            bool EXTisOKRet;
//            string EXT = "";
//            long X = 0L;
//            EXTisOKRet = false;
//            if (sEXT.Substring(0, 1) != ".")
//                //if (Strings.Left(sEXT, 1) != ".")
//                sEXT = "." + sEXT;
//            sEXT = sEXT.ToUpper(); // just to avoid errors 
//            EXT = EXT + ".COM.EDU.GOV.NET.BIZ.ORG.TV";
//            EXT = EXT + ".AF.AL.DZ.As.AD.AO.AI.AQ.AG.AP.AR.AM.AW.AU.AT.AZ.BS.BH.BD.BB.BY";
//            EXT = EXT + ".BE.BZ.BJ.BM.BT.BO.BA.BW.BV.BR.IO.BN.BG.BF.MM.BI.KH.CM.CA.CV.KY";
//            EXT = EXT + ".CF.TD.CL.CN.CX.CC.CO.KM.CG.CD.CK.CR.CI.HR.CU.CY.CZ.DK.DJ.DM.DO";
//            EXT = EXT + ".TP.EC.EG.SV.GQ.ER.EE.ET.FK.FO.FJ.FI.CS.SU.FR.FX.GF.PF.TF.GA.GM.GE.DE";
//            EXT = EXT + ".GH.GI.GB.GR.GL.GD.GP.GU.GT.GN.GW.GY.HT.HM.HN.HK.HU.IS.IN.ID.IR.IQ";
//            EXT = EXT + ".IE.IL.IT.JM.JP.JO.KZ.KE.KI.KW.KG.LA.LV.LB.LS.LR.LY.LI.LT.LU.MO.MK.MG";
//            EXT = EXT + ".MW.MY.MV.ML.MT.MH.MQ.MR.MU.YT.MX.FM.MD.MC.MN.MS.MA.MZ.NA";
//            EXT = EXT + ".NR.NP.NL.AN.NT.NC.NZ.NI.NE.NG.NU.NF.KP.MP.NO.OM.PK.PW.PA.PG.PY";
//            EXT = EXT + ".PE.PH.PN.PL.PT.PR.QA.RE.RO.RU.RW.GS.SH.KN.LC.PM.ST.VC.SM.SA.SN.SC";
//            EXT = EXT + ".SL.SG.SK.SI.SB.SO.ZA.KR.ES.LK.SD.SR.SJ.SZ.SE.CH.SY.TJ.TW.TZ.TH.TG.TK";
//            EXT = EXT + ".TO.TT.TN.TR.TM.TC.TV.UG.UA.AE.UK.US.UY.UM.UZ.VU.VA.VE.VN.VG.VI";
//            EXT = EXT + ".WF.WS.EH.YE.YU.ZR.ZM.ZW";
//            EXT = (EXT).ToUpper(); // just to avoid errors 
//            int a = EXT.IndexOf(sEXT) + 1;
//            //int a = Strings.InStr(1, EXT, sEXT, CompareMethod.Binary);
//            if (a != 0)
//            {

//                EXTisOKRet = true;
//            }
//            return EXTisOKRet;
//        }

//        public static object NullToEmpty(object obj)
//        {
//            if (Convert.IsDBNull(obj))
//            {
//                return "";
//            }
//            else if (string.IsNullOrEmpty(Convert.ToString(obj)))
//                return "";
//            else
//                return obj;
//        }

//        public static object EmptyDtToNull(string obj)
//        {
//            // Dim culture As New System.Globalization.CultureInfo("en-US")

//            if (string.IsNullOrEmpty(obj))
//            {
//                return DBNull.Value;
//            }
//            else
//            {
//                // Return Date.Parse(obj, culture)
//                return Convert.ToDateTime(obj);
//            }
//        }

//        public static string GetFileName(object FilePath)
//        {
//            string GetFileNameRet;
//            object StrLen;
//            int I;
//            object flag;
//            GetFileNameRet = Convert.ToString(FilePath);

//            StrLen = FilePath.ToString().Length;
//            flag = false;

//            for (I = Convert.ToInt32(StrLen); I >= 1; I -= 1)
//            {
//                string a = FilePath.ToString().Substring(I - 1, 1);
//                if (a == @"\" | a == "/")
//                {
//                    GetFileNameRet = FilePath.ToString().Substring(I);
//                    //GetFileNameRet = Strings.Mid(Convert.ToString(FilePath), Convert.ToInt32(Operators.AddObject(I, 1)));
//                    flag = true;
//                    break;
//                }
//            }

//            return GetFileNameRet;
//        }

//        public static void LogError(string errSrc, string errDesc)
//        {
//            try
//            {
//                string a = "0" + DateTime.Today.Day.ToString();
//                string b = "0" + DateTime.Today.Month.ToString();

//                string fileName = a.Substring(a.Length - 2) + "_" + b.Substring(b.Length - 2) + "_" + DateTime.Today.Year.ToString() + ".txt";
//                string logPath = HttpContext.Current.Server.MapPath(@"~\errorlog") + @"\";

//                var logFile = new StreamWriter(logPath + fileName, true);

//                logFile.WriteLine("Error Time: " + DateTime.Now.ToString());
//                logFile.WriteLine("Error Source: " + errSrc);
//                logFile.WriteLine("Error Description: " + errDesc);
//                logFile.WriteLine("");

//                logFile.Close();
//            }
//            catch (Exception ex)
//            {

//            }
//        }

//        public static string GetPortalPath()
//        {
//            string GetPortalPathRet;
//            string myPath = HttpContext.Current.Request.MapPath("app_code").Replace(@"\app_code", @"\");

//            GetPortalPathRet = myPath;
//            return GetPortalPathRet;
//        }

//        public static void OpenPopUp(WebControl opener, string PagePath, string windowName, int width, int height, string SQLDirect)
//        {
//            string clientScript;
//            string windowAttribs;

//            // Building Client side window attributes with width and height.
//            // Also the the window will be positioned to the middle of the screen
//            windowAttribs = "width=" + width + "px," + "height=" + height + "px," + "left='+((screen.width -" + width + ") / 2)+'," + "top='+ (screen.height - " + height + ") / 2+'";


//            // Building the client script- window.open, with additional parameters
//            clientScript = "window.open('" + PagePath + "','" + windowName + "','" + windowAttribs + "'); hdPop = '" + SQLDirect + "';return false;";
//            // regiter the script to the clientside click event of the 'opener' control
//            opener.Attributes.Add("onClick", clientScript);
//        }

//        public static void OpenPopUp(WebControl opener, string PagePath, string windowName, int width, int height)
//        {
//            string clientScript;
//            string windowAttribs;

//            // Building Client side window attributes with width and height.
//            // Also the the window will be positioned to the middle of the screen
//            windowAttribs = "width=" + width + "px," + "height=" + height + "px," + "left='+((screen.width -" + width + ") / 2)+'," + "top='+ (screen.height - " + height + ") / 2+'";


//            // Building the client script- window.open, with additional parameters
//            clientScript = "window.open('" + PagePath + "','" + windowName + "','" + windowAttribs + "');return false;";
//            // regiter the script to the clientside click event of the 'opener' control
//            opener.Attributes.Add("onClick", clientScript);
//        }
    
//        public static Control FindControlRecursive(Control Root, string Id)
//        {
//            if ((Root.ID ?? "") == (Id ?? ""))
//            {
//                return Root;
//            }
//            foreach (Control Ctl in Root.Controls)
//            {
//                var FoundCtl = FindControlRecursive(Ctl, Id);
//                if (FoundCtl != null)
//                {
//                    return FoundCtl;
//                }
//            }
//            return null;
//        }

//        public static string GetFileExtension(string Filename)
//        {
//            int Pos = Filename.LastIndexOf(".");
//            //int Pos = Strings.InStrRev(Filename, ".");
//            return Filename.Substring(Pos);
//        }
//        // Doyin
//        public static bool isNumeric(this string text) => double.TryParse(text, out _);
//        public static string GetFirstName(string strName)
//        {
//            int Pos = strName.Trim().IndexOf(" ") + 1;
//            string strRet = "";

//            if (Pos == 0)
//            {
//                strRet = strName;
//            }
//            else
//            {
//                strRet = strName.Trim().Substring(0, (Pos - 1));
//                //strRet = Strings.Left(strName.Trim(), Pos - 1);
//            }

//            return strRet.Substring(0, 1).ToUpper() + strRet.Substring(1).ToLower();
//            //return Strings.Left(strRet, 1).ToUpper() + Strings.Mid(strRet, 2).ToLower();
//        }

//        public static string GetSetupDescription(string Category, string Code)
//        {
//            var DT = new DataTable();
//            string RetStr = "";

//            DT = clsSetup.listSomeSetup("[CATEGORY]='" + Category + "' AND [CODE]='" + Code + "'");

//            if (DT.Rows.Count > 0)
//            {
//                RetStr = Convert.ToString(DT.Rows[0]["Description"]);
//            }
//            DT = null;

//            return RetStr;
//        }

//        public static string GetSetupRemarks(string Category, string Code)
//        {
//            var DT = new DataTable();
//            string RetStr = "";

//            DT = clsSetup.listSomeSetup("[CATEGORY]='" + Category + "' AND [CODE]='" + Code + "'");

//            if (DT.Rows.Count > 0)
//            {
//                RetStr = Convert.ToString(DT.Rows[0]["Remarks"]);
//            }
//            DT = null;

//            return RetStr;
//        }

//        public static void SaveBytesAsImage(string ImagePath, byte[] ByteArr)
//        {
//            if (ByteArr == null == true)
//                return;
//            try
//            {
//                var fs = new FileStream(ImagePath, FileMode.OpenOrCreate, FileAccess.Write);
//                var bw = new BinaryWriter(fs);
//                bw.Write(ByteArr);
//                bw.Flush();
//                bw.Close();
//                fs.Close();
//                bw = null;
//                fs.Dispose();
//            }
//            catch (Exception ex)
//            {

//            }
//        }

//        public static bool ValidateNIBSSDate(string DateStr)
//        {
//            // Valid Date Example: 12-DEC-1970
//            try
//            {
//                DateTime dt;
//                var culture = new CultureInfo("en-US");

//                dt = Convert.ToDateTime(DateStr, culture);

//                // If Not IsDate(DateStr) Then Return False
//                if (DateStr.Trim().Length < 11)
//                    return false;

//                string monthStr = DateStr.Substring(2, 5); // i.e. -DEC-
//                //string monthStr = Strings.Mid(DateStr, 3, 5); // i.e. -DEC-

//                if ("-JAN-FEB-MAR-APR-MAY-JUN-JUL-AUG-SEP-OCT-NOV-DEC-".IndexOf(monthStr.ToUpper()) + 1 == 0)
//                {
//                    return false;
//                }
//                //if (Strings.InStr("-JAN-FEB-MAR-APR-MAY-JUN-JUL-AUG-SEP-OCT-NOV-DEC-", monthStr.ToUpper()) == 0)
//                return true;
//            }
//            catch (Exception ex)
//            {
//                return false;
//            }
//        }

//        public static string Dlookup(string SourceTable, string ReturnField, string Criteria)
//        {
//            string DlookupRet;
//            var objData = new clsData();
//            string sqlStr = "Select Top 1 " + ReturnField + " from [" + SourceTable + "] Where " + Criteria;
//            var mySQLCommmand = new SqlCommand(sqlStr, objData.Connection());

//            mySQLCommmand.CommandType = CommandType.Text;

//            var resDS = new DataSet();
//            var myDA = new SqlDataAdapter(mySQLCommmand);
//            myDA.Fill(resDS);

//            if (resDS.Tables[0].Rows.Count > 0)
//            {
//                DlookupRet = resDS.Tables[0].Rows[0][0].ToString();
//            }
//            else
//            {
//                DlookupRet = "";
//            }

//            myDA = null;
//            resDS = null;
//            mySQLCommmand = null;
//            objData.CloseConnection();
//            objData = null;
//            return DlookupRet;
//        }

//        public static string GenerateRandomPassword(int passwordLength)
//        {
//            string s = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789?!#$*><";
//            var r = new Random();
//            var passwordChars = new char[passwordLength];
//            int charIndex;

//            for (int i = 0, loopTo = passwordLength - 1; i <= loopTo; i++)
//            {
//                charIndex = r.Next(s.Length);
//                passwordChars[i] = s[charIndex];
//            }

//            string password = new string(passwordChars);

//            return password;
//        }
//        //test
//        public static string getRequestID(string sortCode)
//        {
//            string getRequestIDRet;
//            // Dim sortCode As String = ConfigurationManager.AppSettings("banksortcode")
//            string dateTimeStr = DateTime.Now.ToString("yyyyMMddhhmmss");
//            var random = new Random();
//            string a = "0000000" + random.Next(0, 9999999).ToString().Trim();
//            getRequestIDRet = sortCode + dateTimeStr + a.Substring(a.Length - 7);
//            //getRequestIDRet = sortCode + dateTimeStr + Strings.Right(, 7);
//            return getRequestIDRet;
//        }

//        public static string GetProfilePhoto(string photoName)
//        {
//            string GetProfilePhotoRet;
//            if (HttpContext.Current.Session["LoggedUser"] is null)
//                return "";

//            string PhotoPath = HttpContext.Current.Server.MapPath("~/img/profile-photos");
//            var photoDir = new DirectoryInfo(PhotoPath);
//            string photoNameOld = photoName;

//            if (!photoDir.Exists)
//                photoDir.Create();
//            photoDir = null;

//            if (!string.IsNullOrEmpty(photoName))
//                photoName = PhotoPath.Substring(PhotoPath.Length - 1) == @"\" ? PhotoPath : PhotoPath + @"\" + photoName;
//            //photoName = Strings.Right(PhotoPath, 1) == @"\" ? PhotoPath : PhotoPath + @"\" + photoName; 

//            if (!string.IsNullOrEmpty(photoName))
//            {
//                var filePhoto = new FileInfo(photoName);
//                if (!filePhoto.Exists)
//                    photoName = "";
//                filePhoto = null;
//            }

//            if (string.IsNullOrEmpty(photoName))
//            {
//                photoName = @"..\img\nophoto.jpg";
//            }
//            else
//            {
//                photoName = @"..\img\profile-photos\" + photoNameOld;
//            }

//            GetProfilePhotoRet = photoName;
//            return GetProfilePhotoRet;
//        }

//        public static string RestructureDate(string uploadDate)
//        {
//            string RestructureDateRet;
//            var myArray = uploadDate.Split('-');
//            var myMonthsArray = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
//            int index = Array.IndexOf(myMonthsArray, myArray[1]);
//            string myMonth = index < 9 ? "0" + (index + 1) : "" + (index + 1);
//            RestructureDateRet = myArray[2] + myMonth + myArray[0];
//            return RestructureDateRet;
//        }

//        public static string Encrypt(string input)
//        {
//            // px
//            string Key = "7?v9rd2BtTwiN2fG";
//            // rx
//            string IV = "~nv^M3(L30*6Nubk";
//            if (input is null || input.Length <= 0)
//            {
//                throw new ArgumentNullException("plainText");
//            }

//            byte[] result;
//            var wordBytes = Encoding.UTF8.GetBytes(input);
//            var ms = new MemoryStream();
//            var AES = new RijndaelManaged();
//            AES.KeySize = 256;
//            AES.BlockSize = 128;
//            AES.Key = Encoding.UTF8.GetBytes(Key);
//            AES.IV = Encoding.UTF8.GetBytes(IV);
//            AES.Mode = CipherMode.CBC;
//            var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write);
//            cs.Write(wordBytes, 0, wordBytes.Length);
//            cs.Close();
//            var encryptedBytes = ms.ToArray();
//            result = encryptedBytes;

//            return ByteArrayToString(encryptedBytes).ToUpper();
//        }

//        public static string ByteArrayToString(byte[] ba)
//        {
//            var hex = new StringBuilder(ba.Length * 2);
//            foreach (byte b in ba)
//                hex.AppendFormat("{0:x2}", b);
//            return hex.ToString();
//        }

//        public static short SafeAsc(string str)
//        {
//            return Convert.ToInt16(Encoding.Default.GetBytes(str)[0]);
//        }
//        public static string SafeChr(int CharCode)
//        {
//            if (CharCode > 255)
//            {
//                throw new ArgumentOutOfRangeException("CharCode", CharCode, "CharCode must be between 0 and 255.");
//            }

//            return Encoding.Default.GetString(new[] { (byte)CharCode });
//        }
//        public static string Generatepwd(int minLength)
//        {
//            string pwdStr = "";
//            int indx = 0;
//            int value1;

//            var Rnd = new Random();

//            while (indx < minLength)
//            {
//                //    value1 = CInt(Int(((Asc("Z") - Asc("A") + 1) * Rnd()))) + Asc("A")
//                //pwdStr += Chr(value1)
//                //indx += 1
//                value1 = Convert.ToInt32((((SafeAsc("Z") - SafeAsc("A") + 1) * Rnd.Next(1)))) + SafeAsc("A");
//                pwdStr += SafeChr(value1);
//                indx += 1;

//                value1 = Convert.ToInt32((((SafeAsc("z") - SafeAsc("a") + 1) * Rnd.Next(2)))) + SafeAsc("a");
//                pwdStr += SafeChr(value1);
//                indx += 1;

//                value1 = Convert.ToInt32((((SafeAsc("9") - SafeAsc("0") + 1) * Rnd.Next(3)))) + SafeAsc("0");
//                pwdStr += SafeChr(value1);
//                indx += 1;

//                value1 = Convert.ToInt32((((SafeAsc("z") - SafeAsc("a") + 1) * Rnd.Next(4)))) + SafeAsc("a");
//                pwdStr += SafeChr(value1);
//                indx += 1;

//                string specialChrStr = @"!@#$%^&*()_+=-{}][|\:;?/>.<,~`";
//                var specialChr = specialChrStr.ToCharArray();
//                var rand = new Random();
//                value1 = rand.Next(30);
//                pwdStr += Convert.ToString(specialChr[value1]);
//                indx += 1;

//            }
//            return pwdStr;
//        }


//        // * method to create drop down List control for month

//        public static string ConvertStringToMoney(string MoneyStr)
//        {
//            string ConvertStringToMoneyRet;

//            // Developed by Oluwole Olojede

//            string tempstr = "";
//            string tempstr2 = "";
//            string ReturnedMoney = "";
//            var TempMoney = MoneyStr.Split('.');
//            Console.WriteLine(TempMoney[0].Length);
//            int a = TempMoney[0].Length;
//            int b = 0;
//            int d = 0;
//            Math.DivRem(a, 3, out b);

//            if (b > 0)
//            {

//                d = (int)Math.Round((a - b) / 3d + 1d);
//            }
//            else
//            {
//                d = (int)Math.Round(a / 3d);
//            }

//            for (int i = 0, loopTo = d - 1; i <= loopTo; i++)
//            {

//                if (a > 3)
//                {
//                    tempstr = "," + TempMoney[0].Substring(a - 3, 3) + tempstr;
//                }
//                else
//                {
//                    tempstr2 += TempMoney[0].Substring(0, a);
//                }
//                a = a - 3;

//            }

//            if (TempMoney.Length > 1)
//            {
//                ReturnedMoney = tempstr2 + tempstr + "." + TempMoney[1];
//            }
//            else
//            {
//                ReturnedMoney = tempstr2 + tempstr;
//            }

//            ConvertStringToMoneyRet = ReturnedMoney;
//            return ConvertStringToMoneyRet;

//        }

//        public static void LogError(string errSrc, string errDesc, string errEncounteredBy)
//        {
//            try
//            {
//                // Modified by Oluwole Olojede
//                string a = "0" + DateTime.Today.Day.ToString();
//                string b = "0" + DateTime.Today.Month.ToString();
//                string fileName = a.Substring(a.Length - 2) + "_" + b.Substring(b.Length - 2) + "_" + DateTime.Today.Year.ToString() + ".txt";
//                string logPath = HttpContext.Current.Server.MapPath("errorlog") + @"\";

//                var logFile = new StreamWriter(logPath + fileName, true);

//                logFile.Write("[ Error Encountered By : " + errEncounteredBy + " ]");
//                logFile.Write("[ Error Time: " + DateTime.Now.ToString() + " ]");
//                logFile.Write("[ Error Source: " + errSrc + " ]");
//                logFile.Write("[ Error Description: " + errDesc + " ]");
//                logFile.WriteLine("");

//                logFile.Close();
//            }

//            catch (Exception ex)
//            {

//            }
//        }

//        // * method to create drop down List control for day

//        public static string GetRegTypeDesc(int RegType)
//        {
//            string GetRegTypeDescRet;
//            switch (RegType)
//            {
//                case 1:
//                    {
//                        GetRegTypeDescRet = "LSHSCE REGULAR";
//                        break;
//                    }
//                case 2:
//                    {
//                        GetRegTypeDescRet = "LJHSCE REGULAR";
//                        break;
//                    }
//                case 4:
//                    {
//                        GetRegTypeDescRet = "WASSCE";
//                        break;
//                    }

//                default:
//                    {
//                        GetRegTypeDescRet = "";
//                        break;
//                    }
//            }

//            return GetRegTypeDescRet;
//        }

//        public static DataTable ReturnBVNSingleTable(string XMLStr)
//        {
//            try
//            {
//                var XMLDoc = new XmlDocument();

//                XMLStr = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>" + "<SearchResult>" + "<ResultStatus>00</ResultStatus>" + "<BvnSearchResult>" + "<Bvn>22222222222</Bvn>" + "<FirstName>CHIJIOKE</FirstName>" + "<MiddleName>JJC</MiddleName>" + "<LastName>OKECHUKWU</LastName>" + "<DateOfBirth>22-OCT-70</DateOfBirth>" + "<PhoneNumber>08146703440</PhoneNumber>" + "<RegistrationDate>16-NOV-14</RegistrationDate>" + "<EnrollmentBank>068</EnrollmentBank>" + "<EnrollmentBranch>Victoria Island</EnrollmentBranch>" + "<ImageBase64>" + "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" + "</ImageBase64>" + "</BvnSearchResult>" + "</SearchResult>";

//                // Adhoc fix ... to be taken out when PGP decryption is sorted!
//                if (XMLStr.ToUpper().Trim().Substring(XMLStr.ToUpper().Trim().Length, -15) != "</SEARCHRESULT>")
//                //if (Strings.Right(XMLStr.ToUpper().Trim(), 15) != "</SEARCHRESULT>")
//                {
//                    XMLStr = XMLStr + "</ImageBase64>" + "</BvnSearchResult>" + "</SearchResult>";
//                }

//                XMLDoc.LoadXml(XMLStr);

//                var BVNSingleDT = new DataTable();

//                // Creating Table Columns
//                BVNSingleDT.Columns.Add("ResultStatus", typeof(string));
//                BVNSingleDT.Columns.Add("BVN", typeof(string));
//                BVNSingleDT.Columns.Add("Firstname", typeof(string));
//                BVNSingleDT.Columns.Add("Middlename", typeof(string));
//                BVNSingleDT.Columns.Add("Lastname", typeof(string));
//                BVNSingleDT.Columns.Add("DateOfBirth", typeof(string));
//                BVNSingleDT.Columns.Add("PhoneNumber", typeof(string));
//                BVNSingleDT.Columns.Add("RegistrationDate", typeof(string));
//                BVNSingleDT.Columns.Add("EnrollmentBank", typeof(string));
//                BVNSingleDT.Columns.Add("EnrollmentBranch", typeof(string));
//                // BVNSingleDT.Columns.Add("ImageBase64", GetType(System.String))
//                BVNSingleDT.Columns.Add("ImageBase64", typeof(byte[]));

//                // Assigning Values
//                var RW = BVNSingleDT.NewRow();

//                RW["ResultStatus"] = XMLDoc.DocumentElement.ChildNodes[0].InnerText;

//                if (XMLDoc.DocumentElement.ChildNodes[0].InnerText == "00")
//                {
//                    foreach (XmlNode xmlNode in XMLDoc.DocumentElement.ChildNodes[1])
//                    {
//                        if (xmlNode.LocalName == "ImageBase64")
//                        {
//                        }
//                        // Dim ByteArr As Byte() = Convert.FromBase64String(xmlNode.InnerText)
//                        // RW.Item(xmlNode.LocalName) = ByteArr
//                        else
//                        {
//                            RW[xmlNode.LocalName] = xmlNode.InnerText;
//                        }
//                    }
//                }
//                BVNSingleDT.Rows.Add(RW);
//                XMLDoc = null;

//                return BVNSingleDT;
//            }
//            catch (Exception ex)
//            {
//                return null;
//            }
//        }

//        public static string ReturnBVNUpdateStr(string XMLStr)
//        {
//            try
//            {
//                var XMLDoc = new XmlDocument();

//                XMLStr = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>" + "<UpdateResult>" + "<ResultStatus>00</ResultStatus>" + "</UpdateResult>";

//                XMLDoc.LoadXml(XMLStr);

//                string RetStr;

//                RetStr = XMLDoc.DocumentElement.ChildNodes[0].InnerText;

//                XMLDoc = null;

//                return RetStr;
//            }
//            catch (Exception ex)
//            {
//                return "";
//            }
//        }

//        public static bool ValidateNumeric(string Acct, int length = 11)
//        {
//            bool ValidateNumericRet;
//            string pattern = @"^(\d{" + length + "}$)";
//            var regex = new Regex(pattern);
//            ValidateNumericRet = regex.IsMatch(Acct);
//            return ValidateNumericRet;
//        }

//        public static bool ValidatePhone(string Acct)
//        {
//            bool ValidatePhoneRet;
//            string pattern = @"^(\d{9,11}$)";
//            var regex = new Regex(pattern);
//            ValidatePhoneRet = regex.IsMatch(Acct);
//            return ValidatePhoneRet;
//        }

//        public static string GetDateString(DateTime uploadDate)
//        {
//            string GetDateStringRet;
//            int day = uploadDate.Day;
//            int month = uploadDate.Month;
//            int year = uploadDate.Year;
//            var myMonthsArray = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
//            string myMonth = myMonthsArray[month - 1];

//            GetDateStringRet = day < 10 ? ("0" + day + "-") : ("" + day + "-") + myMonth + "-" + year;
//            return GetDateStringRet;
//        }

//        public static string RearrangeDate(string dt)
//        {
//            string RearrangeDateRet;
//            // dt = dt.Trim(dt, "'")
//            dt = dt.Replace("'", "");
//            // Dim myDt As DateTime = DateTime.Parse(dt)
//            var myDt = DateTime.Parse(dt);
//            RearrangeDateRet = "'" + myDt.Year + myDt.Month + myDt.Day + "'";
//            return RearrangeDateRet;
//            // RearrangeDate = "'" & myDt.Year & "-" & myDt.Day & "-" & myDt.Month & "'"
//        }

//        public static bool CheckAuthorizeAccess1(string Branch, string NextFlowState)
//        {
//            var objUser = new clsUser();
//            bool validAccess = false;

//            objUser = (clsUser)HttpContext.Current.Session["LoggedUser"];

//            var rowArr = objUser.arrayFilter.Split('|');
//            //var rowArr = Strings.Split(objUser.arrayFilter, "|");

//            for (int I = rowArr.GetLowerBound(0), loopTo = rowArr.GetUpperBound(0); I <= loopTo; I++)
//            {
//                var colArr = rowArr[I].Split('#');

//                if (colArr[2] == "1" & (colArr[0] ?? "") == (NextFlowState ?? ""))
//                {
//                    validAccess = true;
//                    break;
//                }
//                if (colArr[2] == "0" & (colArr[0] ?? "") == (NextFlowState ?? "") & (colArr[1] ?? "") == (Branch ?? ""))
//                {
//                    validAccess = true;
//                    break;
//                }
//            }
//            return validAccess;
//        }

//        public static bool CheckAuthorizeAccess(string Status, string FlowState, string NextFlowState)
//        {
//            var objUser = new clsUser();
//            bool validAccess = false;
//            var objCon = new clsData();

//            objUser = (clsUser)HttpContext.Current.Session["LoggedUser"];

//            var resDS = new DataSet();
//            var sqlCommand = new SqlCommand();
//            var myTB = new DataTable();
//            string arrFilter = "";

//            sqlCommand.Connection = objCon.Connection();
//            sqlCommand.CommandType = CommandType.StoredProcedure;
//            sqlCommand.CommandText = "spRoleAccessCheck";
//            sqlCommand.Parameters.Add(new SqlParameter("mUserID", DbType.String)).Value = objUser.UserID;
//            sqlCommand.Parameters.Add(new SqlParameter("mStatus", DbType.String)).Value = Status;
//            sqlCommand.Parameters.Add(new SqlParameter("mFlowState", DbType.String)).Value = FlowState;
//            sqlCommand.Parameters.Add(new SqlParameter("mNextFlowState", DbType.String)).Value = NextFlowState;

//            var DA = new SqlDataAdapter(sqlCommand);
//            DA.Fill(resDS);

//            myTB = resDS.Tables[0];
//            sqlCommand.Dispose();
//            resDS.Dispose();
//            DA.Dispose();
//            objCon.CloseConnection();
//            if (myTB.Rows.Count > 0)
//            {
//                validAccess = true;
//            }
//            return validAccess;

//        }

//        public static string GetBranchFilter()
//        {
//            var objUser = new clsUser();
//            string recFilter;

//            objUser = (clsUser)HttpContext.Current.Session["LoggedUser"];

//            var rowArr = objUser.arrayFilter.Split('|');
//            recFilter = "([BranchCode] = '" + objUser.Branch + "')";

//            // Test
//            // Get list of branch with super powers find branch in list and use super power condition.
//            var dtBP = new DataTable();
//            dtBP = clsSetup.listSomeSetup("[CATEGORY] = 'BRANCH_POWER' AND CODE = '" + objUser.Branch + "'");
//            if (dtBP.Rows.Count > 0)
//            {
//                recFilter = Convert.ToString(dtBP.Rows[0]["Description"]);
//            }

//            for (int I = rowArr.GetLowerBound(0), loopTo = rowArr.GetUpperBound(0); I <= loopTo; I++)
//            {
//                var colArr = rowArr[I].Split('#');

//                if (colArr[2] == "1")
//                {
//                    recFilter = "(1=1)";
//                    break;
//                }
//            }
//            return recFilter;
//        }

//        public static string GetRoleFilter()
//        {
//            var objUser = new clsUser();

//            objUser = (clsUser)HttpContext.Current.Session["LoggedUser"];

//            var rowArr = objUser.arrayFilter.Split('#');

//            // Dim colArr() As String = Split(rowArr(0), "#")
//            return rowArr[3];
//        }

//        public static DataTable GetUserRoleConditions()
//        {
//            var objUser = new clsUser();
//            bool validAccess = false;
//            var objCon = new clsData();

//            objUser = (clsUser)HttpContext.Current.Session["LoggedUser"];

//            var resDS = new DataSet();
//            var sqlCommand = new SqlCommand();
//            var myTB = new DataTable();
//            string arrFilter = "";

//            sqlCommand.Connection = objCon.Connection();
//            sqlCommand.CommandType = CommandType.StoredProcedure;
//            sqlCommand.CommandText = "spGetUserRoleFilterConditions";
//            sqlCommand.Parameters.Add(new SqlParameter("mUserID", DbType.String)).Value = objUser.UserID;

//            var DA = new SqlDataAdapter(sqlCommand);
//            DA.Fill(resDS);

//            myTB = resDS.Tables[0];
//            sqlCommand.Dispose();
//            resDS.Dispose();
//            DA.Dispose();
//            objCon.CloseConnection();

//            return myTB;
//        }

//        public static string GetEmails(ref string ccEmails, string nextFlow, string authStatus, string branch, string maker, string authoriser, string form)
//        {
//            string toEmails = "";
//            var objUser = new clsUser();
//            objUser = (clsUser)HttpContext.Current.Session["LoggedUser"];

//            string userid = objUser.UserID;
//            string role = objUser.arrayFilter;

//            switch (nextFlow ?? "")
//            {
//                case "10":
//                    {
//                        switch (authStatus ?? "")
//                        {
//                            case "U":
//                                {
//                                    // Get emails for RMC
//                                    toEmails = GetUserEmails("RMC", branch);

//                                    // copy maker
//                                    ccEmails = Convert.ToString(clsUser.listSomeUsers("userID = '" + maker + "'").Rows[0]["email"]);
//                                    break;
//                                }

//                            case "V":
//                                {
//                                    // Get emails for RMC
//                                    toEmails = GetUserEmails("RMC", branch);

//                                    // copy maker
//                                    ccEmails = Convert.ToString(clsUser.listSomeUsers("userID = '" + maker + "'").Rows[0]["email"]);
//                                    // copy userID
//                                    ccEmails = ccEmails + "," + objUser.Email;
//                                    break;
//                                }

//                            case "R":
//                            case "C":
//                                {
//                                    // mail maker
//                                    toEmails = Convert.ToString(clsUser.listSomeUsers("userID = '" + maker + "'").Rows[0]["email"]);

//                                    // copy userid
//                                    ccEmails = ccEmails + "," + objUser.Email;
//                                    break;
//                                }

//                        }

//                        break;
//                    }
//                case "20":
//                    {
//                        if (form == "INVISIBLE" | form == "FORMQ")
//                        {
//                            // Mail CMO
//                            toEmails = GetUserEmails("CMO", "");
//                        }
//                        else
//                        {
//                            // Mail TRADE
//                            toEmails = GetUserEmails("TRO", "");
//                        }
//                        // copy maker
//                        ccEmails = Convert.ToString(clsUser.listSomeUsers("userID = '" + maker + "'").Rows[0]["email"]);
//                        break;
//                    }

//                case "30":
//                    {
//                        switch (authStatus ?? "")
//                        {
//                            case "A":
//                                {
//                                    // Mail FM
//                                    toEmails = GetUserEmails("FM", "");

//                                    // Copy maker, userid
//                                    ccEmails = Convert.ToString(clsUser.listSomeUsers("userID = '" + maker + "'").Rows[0]["email"]);
//                                    ccEmails = ccEmails + "," + objUser.Email;
//                                    break;
//                                }


//                            case "B":
//                            case "S":
//                            case "E":
//                                {
//                                    // mail maker
//                                    toEmails = Convert.ToString(clsUser.listSomeUsers("userID = '" + maker + "'").Rows[0]["email"]);

//                                    // copy authoriser, userid
//                                    ccEmails = Convert.ToString(clsUser.listSomeUsers("userID = '" + authoriser + "'").Rows[0]["email"]);
//                                    ccEmails = ccEmails + "," + objUser.Email;
//                                    break;
//                                }

//                        }

//                        break;
//                    }
//            }

//            return toEmails;
//        }

//        public static string GetUserEmails(string role, string branch)
//        {

//            var resDS = new DataSet();
//            var sqlCommand = new SqlCommand();
//            var myTB = new DataTable();
//            string emails = "";
//            var objCon = new clsData();

//            sqlCommand.Connection = objCon.Connection();
//            sqlCommand.CommandType = CommandType.StoredProcedure;
//            sqlCommand.CommandText = "spGetUserEmails";
//            sqlCommand.Parameters.Add(new SqlParameter("mRole", DbType.String)).Value = role;
//            sqlCommand.Parameters.Add(new SqlParameter("mBranch", DbType.String)).Value = branch;

//            var DA = new SqlDataAdapter(sqlCommand);
//            DA.Fill(resDS);

//            myTB = resDS.Tables[0];
//            sqlCommand.Dispose();
//            resDS.Dispose();
//            DA.Dispose();
//            objCon.CloseConnection();

//            for (int I = 0, loopTo = myTB.Rows.Count - 1; I <= loopTo; I++)
//                emails += myTB.Rows[I]["email"].ToString() + ",";
//            if (!string.IsNullOrEmpty(emails.Trim()))
//            {
//                emails = emails.Substring(0, emails.Length - 1);
//            }
//            return emails;

//        }
 
//        public static long Autonumber(string prefix, long offset)
//        {
//            var objCon = new clsData();

//            var resDS = new DataSet();
//            var sqlCommand = new SqlCommand();
//            var myTB = new DataTable();
//            string arrFilter = "";

//            sqlCommand.Connection = objCon.Connection();
//            sqlCommand.CommandType = CommandType.StoredProcedure;
//            sqlCommand.CommandText = "spAutonumber";
//            sqlCommand.Parameters.Add(new SqlParameter("@mFieldName", DbType.String)).Value = prefix;
//            sqlCommand.Parameters.Add(new SqlParameter("@mIncrement", DbType.String)).Value = offset;

//            var DA = new SqlDataAdapter(sqlCommand);
//            DA.Fill(resDS);

//            myTB = resDS.Tables[0];
//            sqlCommand.Dispose();
//            resDS.Dispose();
//            DA.Dispose();
//            objCon.CloseConnection();
//            return Convert.ToInt64(myTB.AsEnumerable().ElementAtOrDefault(0)[0]);

//        }

//        public static double Autonumber(string prefix, double offset, bool increment)
//        {
//            var objCon = new clsData();

//            var resDS = new DataSet();
//            var sqlCommand = new SqlCommand();
//            var myTB = new DataTable();
//            string arrFilter = "";

//            sqlCommand.Connection = objCon.Connection();
//            sqlCommand.CommandType = CommandType.StoredProcedure;
//            sqlCommand.CommandText = "spAutonumberFloat";
//            sqlCommand.Parameters.Add(new SqlParameter("@mFieldName", DbType.String)).Value = prefix;
//            sqlCommand.Parameters.Add(new SqlParameter("@mValue", DbType.Double)).Value = offset;
//            sqlCommand.Parameters.Add(new SqlParameter("@mIncrement", DbType.Boolean)).Value = increment == true ? 1 : 0;

//            var DA = new SqlDataAdapter(sqlCommand);
//            DA.Fill(resDS);

//            myTB = resDS.Tables[0];
//            sqlCommand.Dispose();
//            resDS.Dispose();
//            DA.Dispose();
//            objCon.CloseConnection();
//            return Convert.ToDouble(myTB.AsEnumerable().ElementAtOrDefault(0)[0]);

//        }

//    }
//}