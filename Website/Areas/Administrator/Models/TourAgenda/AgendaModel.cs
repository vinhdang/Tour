using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Domain;

namespace Website.Areas.Administrator.Models.TourAgenda
{
    public class AgendaModel
    {
        public int ID { get; set; }
        public int TourID { get; set; }

        [Display(Name = "Bản đồ Lat")]
        public string Lat { get; set; }

        [Display(Name = "Bản đồ Lng")]
        public string Lng { get; set; }
        
        //[Required]
        [Display(Name = "Mô tả")]
        public string Description { get; set; }
        
        [Required]
        [Display(Name = "Tiêu đề")]
        public string Name { get; set; }
        
        [Display(Name = "Quốc gia")]
        public int NationalID { get; set; }
        
        [Display(Name = "Tỉnh thành")]
        public int ProvinceID { get; set; }
        
        [Display(Name = "Quận huyện")]
        public Nullable<int> DistrictID { get; set; }
        
        [Display(Name = "Khu vực")]
        public Nullable<int> AreaID { get; set; }

        [Display(Name = "Từ Ngày")]
        public System.DateTime? FromDate { get; set; }

        [Display(Name = "Đến Ngày")]
        public System.DateTime? ToDate { get; set; }

        [Required]
        [Display(Name = "Thứ tự hiển thị")]
        public Nullable<int> Sequence { get; set; }

        [Display(Name = "Duyệt?")]
        public bool IsActivate { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> Nationals { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> Provinces { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> Districts { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> Areas { get; set; }
    }

    public class AgendaDisplay
    {
        public int ID { get; set; }
        public int TourID { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public int NationalID { get; set; }
        public int ProvinceID { get; set; }
        public int DistrictID { get; set; }
        public Nullable<int> AreaID { get; set; }
        public System.DateTime FromDate { get; set; }
        public System.DateTime ToDate { get; set; }
        public Nullable<int> Sequence { get; set; }
        public bool IsActivate { get; set; }

        public virtual Domain.National National { get; set; }
        public virtual Domain.Province Province { get; set; }
        public virtual Domain.District District { get; set; }
        public virtual Domain.Area Area { get; set; }
    }
}