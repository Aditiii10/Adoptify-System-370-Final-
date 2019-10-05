using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdoptifyWebsite.Models
{
    public class AdopterFileUpload
    {
        public string FileId { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public IEnumerable<AdopterFileUpload> FileList { get; set; }
    }
}