using System;
using System.Collections.Generic;

namespace NEWLOOK.Models.NewLook;

public partial class MstUser
{
    public int Id { get; set; }

    public string UserName { get; set; } = null!;

    public string Pswd { get; set; } = null!;

    public string Lvl { get; set; } = null!;

    public string Active { get; set; } = null!;
}
