using System;
using System.Collections.Generic;

namespace NEWLOOK.Models.NewLook;

public partial class Customer
{
    public int Id { get; set; }

    public string CustmName { get; set; } = null!;

    public string CustmPhone { get; set; } = null!;

    public string? CustmEmail { get; set; }

    public string CustmAddress { get; set; } = null!;

    public DateTime BirthDt { get; set; }

    public virtual ICollection<BookHdr> BookHdrs { get; set; } = new List<BookHdr>();
}
