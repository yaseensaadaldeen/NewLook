using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace NEWLOOK.Models.NewLook;

public partial class Gallery
{
    public int Id { get; set; }

    public string? ImageLink { get; set; }
    [NotMapped] // ✅ Tells EF to ignore this property
    public IFormFile ImageFile { get; set; }

}
