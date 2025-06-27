using System;
using System.ComponentModel.DataAnnotations;

namespace The_Scouts.Models

{
	public class Newsletter
	{
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}

