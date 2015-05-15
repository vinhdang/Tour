using System.Collections.Generic;
using System.Web.Mvc;

namespace Service.Helpers
{
    public class GlobalVariables
    {

        public static List<SelectListItem> ListPosition = new List<SelectListItem>() 
        { 
            new SelectListItem { Text ="Menu Chính", Value = "1", Selected = false }
         
        };
        public static List<SelectListItem> ListBannerPosition = new List<SelectListItem>() 
        { 
            new SelectListItem { Text ="Trang chủ Right", Value = "1", Selected = false },
            new SelectListItem { Text ="Kết quả tìm kiếm", Value = "2", Selected = false },
            new SelectListItem { Text ="Popup trang chủ", Value = "4", Selected = false },
            new SelectListItem { Text ="Trang tỉnh thành / phố", Value = "8", Selected = false }
          
        };
        public static List<SelectListItem> ListHotelPromotion = new List<SelectListItem>() 
        { 
            new SelectListItem { Text ="Khuyến mãi", Value = "1", Selected = false }
          
        };
        public static List<SelectListItem> ListStar = new List<SelectListItem>() 
        { 
            new SelectListItem { Text ="1", Value = "1", Selected = false },
             new SelectListItem { Text ="2", Value = "2", Selected = false },
              new SelectListItem { Text ="3", Value = "3", Selected = false },
               new SelectListItem { Text ="4", Value = "4", Selected = false },
                new SelectListItem { Text ="5", Value = "5", Selected = false },

          
        };
        public static List<SelectListItem> PriceList = new List<SelectListItem>() 
        { 
            new SelectListItem { Text ="< 800.000", Value = "1", Selected = false },
             new SelectListItem { Text ="800.000 - 1.500.000", Value = "2", Selected = false },
              new SelectListItem { Text ="1.500.000 - 2.500.000", Value = "3", Selected = false },
               new SelectListItem { Text ="2.500.000 - 3.500.000", Value = "4", Selected = false },
                new SelectListItem { Text ="> 3.500.000", Value = "5", Selected = false },

          
        };
        public static List<SelectListItem> ListCompareType = new List<SelectListItem>() 
        { 
            new SelectListItem { Text ="Vietrip.vn", Value = "1", Selected = false },
             new SelectListItem { Text ="Agoda.com", Value = "2", Selected = false },
               new SelectListItem { Text ="Chudu24.com", Value = "3", Selected = false },
                  new SelectListItem { Text ="iViVu.com", Value = "4", Selected = false }
          
        };
        public static List<SelectListItem> DateList = new List<SelectListItem>() 
        { 
            new SelectListItem { Text ="Thứ 6", Value = "Friday", Selected = false },
             new SelectListItem { Text ="Thứ 7", Value = "Saturday", Selected = false },
               new SelectListItem { Text ="Chủ Nhật", Value = "Sunday", Selected = false }
                
          
        };
        public static List<SelectListItem> BannerType = new List<SelectListItem>() 
        { 
            new SelectListItem { Text = "Top", Value = "1", Selected = false }
           
        };
        public static List<SelectListItem> Active = new List<SelectListItem>() 
        { 
            new SelectListItem { Text = "Đã Duyệt", Value = "1", Selected = false },
            new SelectListItem { Text ="Chưa Duyệt", Value = "0", Selected = false }
           
        };
        public static List<SelectListItem> SupportType = new List<SelectListItem>() 
        { 
            new SelectListItem { Text = "Yahoo", Value = "1", Selected = false },
            new SelectListItem { Text ="Skype", Value = "2", Selected = false },
            new SelectListItem { Text ="Phone", Value = "4", Selected = false },
             new SelectListItem { Text ="Email", Value = "8", Selected = false }
        };
        public static List<SelectListItem> TypeList = new List<SelectListItem>() 
        { 
            new SelectListItem { Text ="Trang chủ", Value = "1", Selected = false }
        };
        public static List<SelectListItem> PictureType = new List<SelectListItem>() 
        { 
            new SelectListItem { Text ="Ảnh Đại Diện", Value = "1", Selected = false }
        };
        public static List<SelectListItem> ConfigType = new List<SelectListItem>() 
        { 
            new SelectListItem { Text = "Int", Value = "1", Selected = false },
            new SelectListItem { Text ="String", Value = "2", Selected = false },
            new SelectListItem { Text ="Html", Value = "3", Selected = false } ,
              new SelectListItem { Text ="Image", Value = "4", Selected = false }
        };





        public static List<SelectListItem> ProvincePosition = new List<SelectListItem>() 
        { 
            new SelectListItem { Text = "Tiêu biểu", Value = "1", Selected = false },
            new SelectListItem { Text ="Bình thường", Value = "0", Selected = false },               
        };

        public static List<SelectListItem> TourType = new List<SelectListItem>() 
        { 
             new SelectListItem { Text ="Tour tiêu biểu trang chủ", Value = "1", Selected = false }
        };
        public static List<SelectListItem> Star = new List<SelectListItem>() 
        { 
            new SelectListItem { Text = "1 Sao", Value = "1", Selected = false },
            new SelectListItem { Text ="2 Sao", Value = "2", Selected = false },
            new SelectListItem { Text ="3 Sao", Value = "3", Selected = false },
             new SelectListItem { Text ="4 Sao", Value = "4", Selected = false },
              new SelectListItem { Text ="5 Sao", Value = "5", Selected = false }
        };
        public static List<SelectListItem> RoomList = new List<SelectListItem>() 
        { 
            new SelectListItem { Text = "1 phòng", Value = "1", Selected = true },  
            new SelectListItem { Text = "2 phòng", Value = "2", Selected = false }    ,  
            new SelectListItem { Text = "3 phòng", Value = "3", Selected = false }, 
            new SelectListItem { Text = "4 phòng", Value = "4", Selected = false },
            new SelectListItem { Text = "5 phòng", Value = "5", Selected = false },  
            new SelectListItem { Text = "6 phòng", Value = "6", Selected = false },
            new SelectListItem { Text = "7 phòng", Value = "7", Selected = false }, 
            new SelectListItem { Text = "8 phòng", Value = "8", Selected = false },
            new SelectListItem { Text = "9 phòng", Value = "9", Selected = false },
            new SelectListItem { Text = "10 phòng", Value = "10", Selected = false }
        };

    }
}
