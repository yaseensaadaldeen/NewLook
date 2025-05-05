using System;
using System.Collections.Generic;

namespace NEWLOOK.Models.NewLook
{
    public partial class TeamType
    {
        public TeamType()
        {
            Teams = new HashSet<Team>();
        }

        public int Id { get; set; }
        public string TmTypeName { get; set; } = null!;

        public virtual ICollection<Team> Teams { get; set; }
    }
}
