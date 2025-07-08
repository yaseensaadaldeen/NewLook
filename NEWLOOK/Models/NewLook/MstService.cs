using System;
using System.Collections.Generic;

namespace NEWLOOK.Models.NewLook;

public partial class MstService
{
    public int Id { get; set; }

    public string SerName { get; set; } = null!;

    public string SerDesc { get; set; } = null!;

    public string Active { get; set; } = null!;

    public virtual ICollection<MstServiceImage> MstServiceImages { get; set; } = new List<MstServiceImage>();

    public virtual ICollection<ServiceType> ServiceTypes { get; set; } = new List<ServiceType>();
}
