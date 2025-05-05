using System;
using System.Collections.Generic;

namespace NEWLOOK.Models.NewLook
{
    public partial class BookDet
    {
        public int Id { get; set; }
        public string InvNo { get; set; } = null!;
        public int Price { get; set; }
        public int ServTypeId { get; set; }

        public virtual BookHdr InvNoNavigation { get; set; } = null!;
        public virtual ServiceType ServType { get; set; } = null!;
    }
}
