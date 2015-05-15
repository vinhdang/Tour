using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;

namespace Service.Helpers
{
    public static class FileHelper
    {
        public static string SaveProduct_Picture(HttpPostedFileBase postedFile, string folder, int id)
        {
            if (postedFile == null || postedFile.FileName.Trim() == string.Empty)
                return string.Empty;
            string storageFileName = postedFile.FileName.Trim();
            var path = Path.Combine(HttpContext.Current.Server.MapPath(folder), id.ToString());
            CreateDirectory(path);
            postedFile.SaveAs(Path.Combine(path, storageFileName));
            return storageFileName;
        }
        public static string GetProduct_Picture(string filename, int id)
        {
            if (!string.IsNullOrEmpty(filename) && id > 0)
            {
                return StorageElement.Products_PictureFolder + id + "/" + HttpUtility.UrlDecode(filename);
            }
            return string.Empty;
        }
        public static string SaveApartment_Picture(HttpPostedFileBase postedFile, string fileName, string folder, int id)
        {
            if (postedFile == null || postedFile.FileName.Trim() == string.Empty)
                return string.Empty;
            string storageFileName = fileName;
            var path = Path.Combine(HttpContext.Current.Server.MapPath(folder), id.ToString());
            CreateDirectory(path);
            postedFile.SaveAs(Path.Combine(path, storageFileName));
            return storageFileName;
        }

        public static string GetApartment_Picture(string filename, int id)
        {
            if (!string.IsNullOrEmpty(filename) && id > 0)
            {
                return StorageElement.Tour_PictureFolder + id + "/" + HttpUtility.UrlDecode(filename);
            }
            return string.Empty;
        } 
        
        public static string GetProvider_Picture(string filename, int id)
        {
            if (!string.IsNullOrEmpty(filename) && id > 0)
            {
                return StorageElement.Provider_PictureFolder + id + "/" + HttpUtility.UrlDecode(filename);
            }
            return string.Empty;
        }
        
        public static string GetTourService_Picture(string filename, int id)
        {
            if (!string.IsNullOrEmpty(filename) && id > 0)
            {
                return StorageElement.SupportService_PictureFolder + id + "/" + HttpUtility.UrlDecode(filename);
            }
            return string.Empty;
        }
        public static string GetProvince_Picture(string filename, int id)
        {
            if (!string.IsNullOrEmpty(filename) && id > 0)
            {
                return  StorageElement.Province_PictureFolder + id + "/" + HttpUtility.UrlDecode(filename);
            }
            return string.Empty;
        }
        public static string GetSocial_Picture(string filename, int id)
        {
            if (!string.IsNullOrEmpty(filename) && id > 0)
            {
                return  StorageElement.Social_PictureFolder + id + "/" + HttpUtility.UrlDecode(filename);
            }
            return string.Empty;
        }
        public static string GetCompareType_Picture(string filename, int id)
        {
            if (!string.IsNullOrEmpty(filename) && id > 0)
            {
                return  StorageElement.CompareType_PictureFolder + id + "/" + HttpUtility.UrlDecode(filename);
            }
            return string.Empty;
        }
        public static string GetAirline_Picture(string filename, int id)
        {
            if (!string.IsNullOrEmpty(filename) && id > 0)
            {
                return StorageElement.Airline_PictureFolder + id + "/" + HttpUtility.UrlDecode(filename);
            }
            return string.Empty;
        }
        public static string GetBanner_Picture(string filename, int id)
        {
            if (!string.IsNullOrEmpty(filename) && id > 0)
            {
                return  StorageElement.Banner_PictureFolder + id + "/" + HttpUtility.UrlDecode(filename);
            }
            return string.Empty;
        }

        public static string GetCategory_Picture(string filename, int id)
        {
            if (!string.IsNullOrEmpty(filename) && id > 0)
            {
                return  StorageElement.Category_PictureFolder + id + "/" + HttpUtility.UrlDecode(filename);
            }
            return string.Empty;
        }
        public static string GetNew_Picture(string filename, int id)
        {
            if (!string.IsNullOrEmpty(filename) && id > 0)
            {
                return StorageElement.News_PictureFolder + id + "/" + HttpUtility.UrlDecode(filename);
            }
            return string.Empty;
        }
        public static string GetConfig_Picture(string filename, int id)
        {
            if (!string.IsNullOrEmpty(filename) && id > 0)
            {
                return StorageElement.Config_PictureFolder + id + "/" + HttpUtility.UrlDecode(filename);
            }
            return string.Empty;
        }
        public static void DeleteConfig_Picture(string filename, int id)
        {
            try
            {
                var fullFilePath = Path.Combine(HttpContext.Current.Server.MapPath(StorageElement.Config_PictureFolder), id.ToString(), HttpUtility.UrlDecode(filename));
                if (System.IO.File.Exists(fullFilePath))
                {
                    System.IO.File.Delete(fullFilePath);
                }
            }
            catch { }
        }
        public static bool SaveConfig_Picture(string filename, string oldpath, string newpath, int id)
        {
            var _newpath = Path.Combine(HttpContext.Current.Server.MapPath(newpath), id.ToString());
            try
            {
                CreateDirectory(_newpath);
                File.Copy(HttpUtility.UrlDecode(oldpath), HttpUtility.UrlDecode(_newpath) + "/" + HttpUtility.UrlDecode(filename), true);
                return true;
            }
            catch { }
            return false;
        }
        public static void DeleteApartment_Picture(string filename, int id)
        {
            try
            {
                var fullFilePath = Path.Combine(HttpContext.Current.Server.MapPath(StorageElement.Tour_PictureFolder), id.ToString(), HttpUtility.UrlDecode(filename));
                if (System.IO.File.Exists(fullFilePath))
                {
                    System.IO.File.Delete(fullFilePath);
                }
            }
            catch { }
        }
        public static void CreateDirectory(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                try
                {
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);
                }
                catch { }
            }
        }
        public static void DeleteDirectory(string folder, int id)
        {
            string path = "";
            switch (folder)
            {

                case "Product":
                    path = Path.Combine(HttpContext.Current.Server.MapPath(StorageElement.Products_PictureFolder), id.ToString());
                    break;
                case "New":
                    path = Path.Combine(HttpContext.Current.Server.MapPath(StorageElement.News_PictureFolder), id.ToString());
                    break;
                case "Airline":
                    path = Path.Combine(HttpContext.Current.Server.MapPath(StorageElement.Airline_PictureFolder), id.ToString());
                    break;
            }
            if (Directory.Exists(path))
                Directory.Delete(path, true);
        }
        public static bool imageFileAvailable(string fullFilePath)
        {
            return System.IO.File.Exists(HttpUtility.UrlDecode(fullFilePath));
        }
        public static string getFullFilePath(string folder, int id, string file)
        {
            string path = "";
            switch (folder)
            {
                case "Tour":
                    path = StorageElement.Tour_PictureFolder + id;
                    break;
                case "New":
                    path = StorageElement.News_PictureFolder + id;
                    break;
                case "Banner":
                    path = StorageElement.Banner_PictureFolder + id;
                    break;
                case "Category":
                    path = StorageElement.Category_PictureFolder + id;
                    break;
                case "Provinces":
                    path = StorageElement.Province_PictureFolder + id;
                    break;

                case "Airlines":
                    path = StorageElement.Airline_PictureFolder + id;
                    break;
                case "CompareTypes":
                    path = StorageElement.CompareType_PictureFolder + id;
                    break;
            }
            return string.Format("{0}/{1}", HttpContext.Current.Server.MapPath(path), file);
        }
        public static string instantiate404ErrorResult(string file)
        {
            return string.Format("Tệp tin không hiện hữu vui lòng chọn file khác.", file);
        }
    }
}
