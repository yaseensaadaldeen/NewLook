using System;
using System.Collections.Generic;

namespace NEWLOOK.Models.NewLook
{
    public partial class MstService
    {
        public MstService()
        {
            MstServiceImages = new HashSet<MstServiceImage>();
            ServiceTypes = new HashSet<ServiceType>();
        }

        public int Id { get; set; }
        public string SerName { get; set; } = null!;
        public string SerDesc { get; set; } = null!;
        public int TeamId { get; set; }

        public virtual Team Team { get; set; } = null!;
        public virtual ICollection<MstServiceImage> MstServiceImages { get; set; }
        public virtual ICollection<ServiceType> ServiceTypes { get; set; }
    }
}
