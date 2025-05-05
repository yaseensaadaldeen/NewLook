using System;
using System.Collections.Generic;

namespace NEWLOOK.Models.NewLook
{
    public partial class ServiceType
    {
        public ServiceType()
        {
            BookDets = new HashSet<BookDet>();
        }

        public int Id { get; set; }
        public string SerTypeName { get; set; } = null!;
        public string? SerTypeDesc { get; set; }
        public int Price { get; set; }
        public string SerTime { get; set; } = null!;
        public string Active { get; set; } = null!;
        public int MstSerId { get; set; }

        public virtual MstService MstSer { get; set; } = null!;
        public virtual ICollection<BookDet> BookDets { get; set; }
    }
}
