using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatBotManagement.Model
{
    public class ReceiveRequest
    {
        public int RequestId { get; set; }

        public int RequestByEmpId { get; set; }

        public int RequestToEmpId { get; set; }

        public int EmpSapId { get; set; }

        public string EmpName { get; set; }

        public string EmpEmailId { get; set; }

        public string EmpContactNumber { get; set; }

        public string EmpRequirement { get; set; }
    }
}
