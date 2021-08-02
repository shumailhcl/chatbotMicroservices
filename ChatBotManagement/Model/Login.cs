using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ChatBotManagement.Model
{
    public class Login
    {
        public int employeeSapId { get; set; }
        public string employeeEmailId { get; set; }
    }

    public class AuthenticateUserStatus
    {
        public bool status { get; set; }
        public DateTime? expiryDate { get; set; }
        public string message { get; set; }
    }
}
