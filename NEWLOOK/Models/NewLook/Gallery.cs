using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace NEWLOOK.Models.NewLook;

public partial class Gallery
{
    public int Id { get; set; }

    public string? ImageLink { get; set; }
    [NotMapped]
    public IFormFile? ImageFile { get; set; }
}
