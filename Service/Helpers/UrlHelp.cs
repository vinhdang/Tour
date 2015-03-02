using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Service.Helpers
{
    public class UrlHelp
    {
        public static string NormalizeStringForUrl(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                String normalizedString = name.Normalize(NormalizationForm.FormD);
                StringBuilder stringBuilder = new StringBuilder();
                foreach (char c in normalizedString)
                {
                    switch (CharUnicodeInfo.GetUnicodeCategory(c))
                    {
                        case UnicodeCategory.LowercaseLetter:
                        case UnicodeCategory.UppercaseLetter:
                        case UnicodeCategory.DecimalDigitNumber:
                            stringBuilder.Append(c);
                            break;
                        case UnicodeCategory.SpaceSeparator:
                        case UnicodeCategory.ConnectorPunctuation:
                        case UnicodeCategory.DashPunctuation:
                            stringBuilder.Append('-');
                            break;
                    }
                }
                string result = stringBuilder.ToString();
                string str = String.Join("-", result.Split(new char[] { '-' }
                    , StringSplitOptions.RemoveEmptyEntries));
                str = str.ToLower().Replace("đ", "d");
                return str;
            }
            return "";
            // remove duplicate underscores 
        }
        public static string getProductUrl(string catName, string name, int catID, int id)
        {
            return string.Format("/{0}/{1}/{2}/{3}/p/", NormalizeStringForUrl(catName), NormalizeStringForUrl(name), id, catID);
        }
        public static string getOrderUrl(string national, string provinceName, string hotelName, int hotelID, int provinceID)
        {
            return string.Format("/{0}/{1}/{2}/{3}/{4}/booking", NormalizeStringForUrl(national), NormalizeStringForUrl(provinceName), NormalizeStringForUrl(hotelName), hotelID, provinceID).ToLower();
        }
        public static string getCategoryUrl(string url, string name, int id)
        {
            if (!string.IsNullOrEmpty(url))
            {
                return url;
            }
            return string.Format("/{0}/{1}/c/", NormalizeStringForUrl(name), id);
        }
        public static string getCategoryUrl(string url, string name, int id, string last)
        {
            if (!string.IsNullOrEmpty(url))
            {
                return url;
            }
            return string.Format("/{0}/{1}/{2}/", NormalizeStringForUrl(name), id, last);
        }
        public static string getHotelUrl(string nationalName, string provinceName, string hotelName, int hotelID, int provinceID)
        {
            return string.Format("/{0}/{1}/{2}/t/", NormalizeStringForUrl(nationalName), NormalizeStringForUrl(provinceName), NormalizeStringForUrl(hotelName)).ToLower();
        }

        public static string getHotelMap(string name, int id, int nationalID, int provinceID)
        {
            return string.Format("/{0}/{1}/{2}/{3}/{4}/m", NormalizeStringForUrl(name), id, nationalID, provinceID, CultureInfo.CurrentCulture.TwoLetterISOLanguageName).ToLower();
        }

        public static string getProvinceUrl(string nationalName, string provinceName, int provinceID)
        {
            return string.Format("/{0}/{1}/p/", NormalizeStringForUrl(nationalName), NormalizeStringForUrl(provinceName)).ToLower();
        }

        public static string getForeignUrl(string nationalName, string provinceName, int provinceID)
        {
            return string.Format("/{0}/{1}/f/", NormalizeStringForUrl(nationalName), NormalizeStringForUrl(provinceName)).ToLower();
        }

        public static string getNewUrl(string name, string catname, int catID, int newID)
        {

            return string.Format("/{0}/{1}/{2}/{3}/n/", NormalizeStringForUrl(catname), NormalizeStringForUrl(name), catID, newID);
        }
        public static string getBuyUrl(string name, int productid)
        {
            return string.Format("/{0}/{1}/b/", NormalizeStringForUrl(name), productid);
        }

        public static string getRecruimentUrl()
        {
            return string.Format("/tuyen-dung");
        }

        public static string getPolicyUrl()
        {
            return string.Format("/noi-quy");
        }
    }
}
