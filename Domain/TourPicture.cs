//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Domain
{
    using System;
    using System.Collections.Generic;
    
    public partial class TourPicture
    {
        public long ID { get; set; }
        public int TourID { get; set; }
        public bool IsActive { get; set; }
        public int Order { get; set; }
        public string Alt { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public string Extension { get; set; }
        public long ContentLength { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public bool IsAvartar { get; set; }
        public string BackLink { get; set; }
    
        public virtual Tour Tour { get; set; }
    }
}