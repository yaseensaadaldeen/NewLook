using System;
using System.Collections.Generic;

namespace NEWLOOK.Models.NewLook
{
    public partial class BookHdr
    {
        public BookHdr()
        {
            BookDets = new HashSet<BookDet>();
        }

        public string InvNo { get; set; } = null!;
        public DateTime BookDate { get; set; }
        public int TotalPrice { get; set; }
        public int Discount { get; set; }
        public int CustmId { get; set; }
        public DateTime CreatedDt { get; set; }

        public virtual Customer Custm { get; set; } = null!;
        public virtual ICollection<BookDet> BookDets { get; set; }
    }
}
