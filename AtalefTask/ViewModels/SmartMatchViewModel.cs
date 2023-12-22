using System.ComponentModel.DataAnnotations;

namespace AtalefTask.ViewModels
{
    public class SmartMatchViewModel
    {
        [Required(ErrorMessage = "The UserId field is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a UserId bigger than {0}")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "The UniqueValue field is required.")]
        public string UniqueValue { get; set; }
    }
}
