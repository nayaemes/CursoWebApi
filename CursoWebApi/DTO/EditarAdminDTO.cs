using System.ComponentModel.DataAnnotations;

namespace CursoWebApi.DTO
{
    public class EditarAdminDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } 
       
    }
}
