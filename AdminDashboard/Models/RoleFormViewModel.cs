using System.ComponentModel.DataAnnotations;

namespace AdminDashboard.Models
{
    public class RoleFormViewModel
    {
        [Required(ErrorMessage ="Name Is Required !")]
        public string Name { get; set; }
    }
}
