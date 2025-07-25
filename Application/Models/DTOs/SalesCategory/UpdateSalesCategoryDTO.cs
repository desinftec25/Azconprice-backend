﻿using System.ComponentModel.DataAnnotations;

namespace Application.Models.DTOs.SalesCategory
{
    public class UpdateSalesCategoryDTO
    {
        [Required]
        [MinLength(2, ErrorMessage = "Name must be at least 2 characters.")]
        public string Name { get; set; }
    }
}
