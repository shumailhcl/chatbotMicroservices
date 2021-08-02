using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatBotManagement.Model
{
    public class RequestSummary
    {
        public int MyRequest { get; set; }

        public int PendingRequest { get; set; }

        public int ResolvedRequest { get; set; }

        public int CancelRequest { get; set; }

        public int TotalNeedyUser { get; set; }

        public int TotalUser { get; set; }

        public string Month { get; set; }

    }
}
