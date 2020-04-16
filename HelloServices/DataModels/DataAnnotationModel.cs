using System.ComponentModel.DataAnnotations;

namespace HelloServices.DataModels
{
    public class DataAnnotationModel
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [StringLength(10, MinimumLength =1)]
        public string Email { get; set; }
    }
}