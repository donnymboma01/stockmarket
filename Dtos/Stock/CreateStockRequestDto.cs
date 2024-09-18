using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Stock
{
    public class CreateStockRequestDto
    {
        [Required]
        [MaxLength(10, ErrorMessage = "Le symbole ne peut pas avoir plus de 10 caracteres")]
        public string Symbol { get; set; } = string.Empty;

        [Required]
        [MaxLength(10, ErrorMessage = "Le nom de la company ne peut pas avoir plus de 10 caracteres")]
        public string CompanyName { get; set; } = string.Empty;
        
        [Required]
        [Range(1,1000000000)]
        public decimal Purchase { get; set; }

        [Required]
        [Range(0.001,100)]
        public decimal LastDividend { get; set; }
        
        [Required]
        [MaxLength(10, ErrorMessage = "Le nom de l'industrie ne peut pas avoir plus de 10 caracteres")]
        public string Industry { get; set; } = string.Empty;
        
        [Range(1,5000000000)]
        public long MarketCap { get; set; }
    }
}