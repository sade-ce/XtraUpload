﻿using System.ComponentModel.DataAnnotations;

namespace XtraUpload.WebApp.Common
{
    public class FileAvailabilityViewModel
    {
        [Required]
        [RegularExpression("^[a-zA-Z0-9]*$")]
        public string Fileid { get; set; }
        public bool Available { get; set; }
    }
}
