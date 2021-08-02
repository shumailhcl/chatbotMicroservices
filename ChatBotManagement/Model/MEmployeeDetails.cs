using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatBotManagement.Model
{
    public class mEmployeeDetails
    {
        [Key]
        public int employeeId { get; set; }
       
        public int employeeSapId { get; set; }
        public string employeeName { get; set; }
        public string employeeContactNo { get; set; }
       
        public string employeeEmailId { get; set; }
        public string employeeAddress { get; set; }
        public int locationId { get; set; }
        public bool isActive { get; set; }
        [NotMapped]
        public string locationName { get; set; }

        public string gender { get; set; }

        [NotMapped]

        public string Token { get; set; }
        
        
    }
}
