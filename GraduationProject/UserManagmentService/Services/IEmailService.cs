using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagmentService.Models;
using NETCore.MailKit.Core;

namespace UserManagmentService.Services
{
    public interface IEmailService
    {
        void SendEmail(Message message);
    }
}
