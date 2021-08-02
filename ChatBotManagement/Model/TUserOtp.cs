using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ChatBotManagement.Model
{
    public class tUserOtp
    {
        [Key]
        public int otpId { get; set; }
        public string otpNumber { get; set; }
        public string otpFor { get; set; }
        public bool isUsed { get; set; }
        public DateTime? expiryDate { get; set; }

      
        public int createdBy { get; set; } // employee ID will come under this
        public DateTime? createdDate { get; set; }
        public bool isActive { get; set; }

        public int usedBy { get; set; }
        public DateTime? usedDate { get; set; }
    }
}
