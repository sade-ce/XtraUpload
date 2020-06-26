﻿using System.ComponentModel.DataAnnotations;

namespace XtraUpload.WebApp.Common
{
    public class EditExtensionViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(10)]
        [RegularExpression(@"^\.[a-zA-Z0-9]*$")]
        public string NewExt { get; set; }
    }
}
