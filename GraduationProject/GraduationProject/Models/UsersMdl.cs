using System.ComponentModel.DataAnnotations;

namespace GraduationProject.Models
{
    public class UsersMdl
    {
        public int Id { get; set; }

        public string UserName { get; set; }
        public string Email { get; set; }
   
        public string Password { get; set; }
    }
}
