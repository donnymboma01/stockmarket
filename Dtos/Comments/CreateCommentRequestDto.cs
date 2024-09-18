using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Comments
{
    public class CreateCommentRequestDto
    {
        [Required]
        [MinLength(5, ErrorMessage = "Le titre doit avoir au moins 5 lettres ")]
        [MaxLength(255, ErrorMessage = "Le titre ne peut pas avoir plus 255 lettres")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MinLength(5, ErrorMessage = "Le contenu doit avoir au moins 5 lettres ")]
        [MaxLength(280, ErrorMessage = "Le contenu ne peut pas dépasser 280 caracteres")]
        public string Content { get; set; } = string.Empty;
    }
}