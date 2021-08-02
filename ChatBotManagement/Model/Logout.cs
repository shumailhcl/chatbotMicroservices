using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ChatBotManagement.Model
{
    public class Logout
    {
        [NotMapped]
        public int SAPId { get; set; }

        public string employeeIp { get; set; }

        public string browserDetail { get; set; }

        public string sessionId { get; set; }

      //  public DateTime? logoutDateTime { get; set; }

    }
}
