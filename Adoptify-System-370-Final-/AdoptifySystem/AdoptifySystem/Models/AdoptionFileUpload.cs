using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdoptifySystem.Models
{
    public class AdoptionFileUpload
    {
        public string FileId { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public IEnumerable<AdoptionFileUpload> FileList { get; set; }
    }
}