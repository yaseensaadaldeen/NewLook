using System;
using System.Collections.Generic;

namespace NEWLOOK.Models.NewLook;

public partial class Team
{
    public int Id { get; set; }

    public string EmpName { get; set; } = null!;

    public string EmpSkills { get; set; } = null!;

    public string Nationality { get; set; } = null!;

    public string Experiances { get; set; } = null!;

    public string Languages { get; set; } = null!;

    public string CountryWork { get; set; } = null!;

    public string ImageLink { get; set; } = null!;

    public string Active { get; set; } = null!;
}
