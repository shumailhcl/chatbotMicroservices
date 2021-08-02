using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ChatBotManagement.Model
{
    public class LEmpSession
    {
        [Key]
        public int empSessionId { get; set; }

        public int employeeId { get; set; }

        public DateTime? loginDateTime { get; set; }

        public DateTime? logoutDateTime { get; set; }

        public string employeeIp { get; set; }

        public string browserDetail { get; set; }

        public string sessionId { get; set; }

        [NotMapped]
        public int SAPId { get; set; }
    }
}
