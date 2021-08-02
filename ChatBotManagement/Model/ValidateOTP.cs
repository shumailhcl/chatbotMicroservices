using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatBotManagement.Model
{
    public class ValidateOTP
    {
        public int employeeSapId{ get; set; }
      
        public string otpNumber { get; set; }

        public string employeeIp { get; set; }

        public string browserDetail { get; set; }
    }
}
