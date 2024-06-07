using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagmentService.Models
{
    public class EmailConfigration
    {
        public string From { get; set; } = null;
        public string Smtpserver { get; set; } = null;
        public int port { get; set; }
        public string Password { get; set; } = null;
        public string UserName { get; set; } = null;


    }
}
