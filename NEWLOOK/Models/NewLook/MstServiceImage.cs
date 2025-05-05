using System;
using System.Collections.Generic;

namespace NEWLOOK.Models.NewLook
{
    public partial class MstServiceImage
    {
        public int Id { get; set; }
        public int SerId { get; set; }
        public string? ImageLink { get; set; }
        public string Active { get; set; } = null!;

        public virtual MstService Ser { get; set; } = null!;
    }
}
