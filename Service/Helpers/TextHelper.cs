using System;
using System.Web.Security;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;

namespace Service.Helpers
{
    public static class TextHelper
    {
        public static string Hash(string originalText, HashMethod hashMethod)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(originalText, hashMethod.ToString());
        }
        public static List<SelectListItem> LoadType(int Type, List<SelectListItem> listType)
        {
            foreach (SelectListItem selectedId in listType)
            {
                if ((Protector.Int(selectedId.Value, 0) & Type) == Protector.Int(selectedId.Value, 0))
                {
                    selectedId.Selected = true;
                }
                else { selectedId.Selected = false; }
            }
            return listType;
        }
        public static int UpdateType(string[] LType)
        {
            int type = 0;
            if (LType != null && LType.Length > 0)
            {
                foreach (string t in LType)
                {
                    type += Protector.Int(t, 0);
                }
            }
            return type;
        }
        public static string Selected(bool selected)
        {
            if (selected)
            { return "checked='checked'"; }
            return "";
        }
        public static List<SelectListItem> LoadPosition(int Type)
        {
            List<SelectListItem> cat_Type = GlobalVariables.ListPosition;
            foreach (SelectListItem selectedId in cat_Type)
            {
                if ((Protector.Int(selectedId.Value, 0) & Type) == Protector.Int(selectedId.Value, 0))
                {
                    selectedId.Selected = true;
                }
                else { selectedId.Selected = false; }
            }
            return cat_Type;
        }
        public static List<SelectListItem> LoadBannerPosition(int Type)
        {
            List<SelectListItem> cat_Type = GlobalVariables.ListBannerPosition;
            foreach (SelectListItem selectedId in cat_Type)
            {
                if ((Protector.Int(selectedId.Value, 0) & Type) == Protector.Int(selectedId.Value, 0))
                {
                    selectedId.Selected = true;
                }
                else { selectedId.Selected = false; }
            }
            return cat_Type;
        }
        public static List<SelectListItem> LoadType(int Type)
        {
            List<SelectListItem> cat_Type = GlobalVariables.TypeList;
            foreach (SelectListItem selectedId in cat_Type)
            {
                if ((Protector.Int(selectedId.Value, 0) & Type) == Protector.Int(selectedId.Value, 0))
                {
                    selectedId.Selected = true;
                }
                else { selectedId.Selected = false; }
            }
            return cat_Type;
        }
        static string PostData(string url)
        {
            HttpWebRequest request = null;
            Uri uri = new Uri(url);
            request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            string result = "";
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    using (StreamReader readStream = new StreamReader(responseStream, Encoding.UTF8))
                    {
                        result = readStream.ReadToEnd();
                    }
                }
            }
            return result;
        }

        public static string CheckYahoo(string Username)//yahoo can hien
        {
            // kiểm tra username
            string isonval = PostData("http://mail.opi.yahoo.com/online?u=" + Username.Trim() + "&m=t&t=1");
            if (isonval == "00") return "offline-yahoo";
            else if (isonval == "01") return "online-yahoo";
            else return "online-yahoo";

        }
        public static string CheckKype(string url)
        {
            HttpWebRequest objRequest1 = (HttpWebRequest)WebRequest.Create("http://mystatus.skype.com/" + url + ".num");//bat buoc phai co .num o phia sau
            HttpWebResponse objResponse1 = (HttpWebResponse)objRequest1.GetResponse();
            StreamReader objReader1 = new StreamReader(objResponse1.GetResponseStream());
            string msgReturn1 = objReader1.ReadToEnd();
            objReader1.Close();
            objResponse1.Close();
            msgReturn1 = msgReturn1.ToLower().Trim();
            if (msgReturn1 == "2")
                return "online-skype";
            else
                return "offline-skype";

        }

        public static string SaleOff(Decimal originalPrice, Decimal salePrice)
        {
            if (originalPrice > 0 && salePrice > 0)
            {
                decimal sale = 100 - (salePrice / originalPrice) * 100;
                return @String.Format("{0:#,###}%", sale);
            }
            return "0%";
        }
        public static string Save(Decimal originalPrice, Decimal salePrice)
        {
            if (originalPrice > 0 && salePrice > 0 && originalPrice > salePrice)
            {
                decimal sale = originalPrice - salePrice;
                return @String.Format("{0:#,###}đ", sale);
            }
            return "0 đ";
        }
        public static bool CheckEmail(string str)
        {
            Regex re = new Regex(@"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$");
            if (re.IsMatch(str))
                return true;
            else
                return false;
        }
        public static string GetIPAddress()
        {
            try
            {
                string accountIP = "";
                if (HttpContext.Current.Request.ServerVariables["HTTP_CITRIX"] != null)
                {
                    accountIP = HttpContext.Current.Request.ServerVariables["HTTP_CITRIX"];
                }

                if (string.IsNullOrEmpty(accountIP) && HttpContext.Current.Request.Headers["CITRIX_CLIENT_HEADER"] != null)
                {
                    accountIP = HttpContext.Current.Request.Headers["CITRIX_CLIENT_HEADER"];
                }

                if (string.IsNullOrEmpty(accountIP) && HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
                {
                    accountIP = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                }
                if (string.IsNullOrEmpty(accountIP) && HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"] != null)
                {
                    accountIP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }

                return accountIP;
            }
            catch
            {
                return null;
            }
        }
        const string HTML_TAG_PATTERN = "<.*?>";
        static string StripHTML(string inputString)
        {
            return Regex.Replace
              (inputString, HTML_TAG_PATTERN, string.Empty);
        }
        public static string Substring(string str, int startIndex, int lenght)
        {
            str = StripHTML(str);
            if (startIndex > lenght && str == "")
            {
                return "";
            }
            else
            {
                if (str.Length > lenght)
                {
                    char item = str[lenght];
                    if (item.Equals(' '))
                    {
                        str = str.Substring(startIndex, lenght);
                    }
                    else
                    {
                        for (int i = lenght; i < str.Length; i++)
                        {
                            item = str[i];
                            if (item.Equals(' '))
                            {
                                str = str.Substring(startIndex, i) + "...";
                                return str;
                            }
                        }
                    }

                }
                else
                {
                    return str;
                }
            }
            return str;
        }
        public static string HandleTitleStringForImg(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }
            str = str.Trim();
            string result = "";
            result = RemoveSign4VietnameseString(HttpUtility.HtmlDecode(str));
            result = result.Replace("`", "");
            result = result.Replace("~", "");
            result = result.Replace("!", "");
            result = result.Replace("@", "");
            result = result.Replace("#", "");
            result = result.Replace("$", "");
            result = result.Replace("%", "");
            result = result.Replace("^", "");
            result = result.Replace("&", "");
            result = result.Replace("*", "");
            result = result.Replace("(", "");
            result = result.Replace(")", "");
            //result = result.Replace("-", "");
            result = result.Replace("_", "");
            result = result.Replace("+", "");
            result = result.Replace("=", "");
            result = result.Replace("{", "");
            result = result.Replace("[", "");
            result = result.Replace("}", "");
            result = result.Replace("]", "");
            result = result.Replace("|", "");
            result = result.Replace("\\", "");
            result = result.Replace(":", "");
            result = result.Replace(";", "");
            result = result.Replace("\"", "");
            result = result.Replace("'", "");
            result = result.Replace("<", "");
            result = result.Replace(",", "");
            result = result.Replace(">", "");
            //result = result.Replace(".", "");
            result = result.Replace("?", "");
            result = result.Replace("/", "");
            result = result.Replace(" ", "-");
            result = result.Replace("“", "");
            result = result.Replace("”", "");
            result = result.Replace("__", "-");
            return result.ToLower();
        }
        public static string RemoveSign4VietnameseString(string str)
        {
            for (int i = 1; i < VietnameseSigns.Length; i++)
            {
                for (int j = 0; j < VietnameseSigns[i].Length; j++)
                    str = str.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);
            }
            return str;
        }
        public static string TwoLetterISOLanguage(int id)
        {
            string str = "";
            switch (id)
            {
                case 1:
                    str = "vi";
                    break;
                case 2:
                    str = "en";
                    break;

            }
            return str;
        }
        private static readonly string[] VietnameseSigns = new string[]
        {
            "aAeEoOuUiIdDyY",
            "áàạảãâấầậẩẫăắằặẳẵ",
            "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
            "éèẹẻẽêếềệểễ",
            "ÉÈẸẺẼÊẾỀỆỂỄ",
            "óòọỏõôốồộổỗơớờợởỡ",
            "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
            "úùụủũưứừựửữ",
            "ÚÙỤỦŨƯỨỪỰỬỮ",
            "íìịỉĩ",
            "ÍÌỊỈĨ",
            "đ",
            "Đ",
            "ýỳỵỷỹ",
            "ÝỲỴỶỸ"
        };
    }
    public enum HashMethod
    {
        MD5,
        SHA1
    }
}
