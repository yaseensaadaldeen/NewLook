using NEWLOOK.Models.NewLook;
using System.Data.Entity.Core.Metadata.Edm;

namespace NEWLOOK.Models
{
    public class HomeViewModel
    {
        public List<Gallery> Galleries { get; set; }
        public List<Team> Teams { get; set; }
        public List<ServiceType> ServicesType { get; set; }
    }
}
