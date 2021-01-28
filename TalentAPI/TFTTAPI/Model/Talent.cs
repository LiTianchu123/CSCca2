using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace TFTTAPI.Model
{
    public partial class Talent
    {
        public int Id { get; set; }
        [MaxLength(255)]
        [Required]
        public string Name { get; set; }
        [MaxLength(255)]
        [Required]
        public string ShortName { get; set; }
        [MaxLength(4000)]
        [Required]
        public string Reknown { get; set; }
        [MaxLength(4000)]
        [Required]
        public string Bio { get; set; }
        [MaxLength(255)]
        [Required]
        public string Profile { get; set; }
    }
}
