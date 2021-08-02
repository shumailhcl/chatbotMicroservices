using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatBotManagement.Model
{
    public class RequestSummaryForLocation
    {
        public int LocationId { get; set; }
        public string Location { get; set; }

      
        public int PendingRequestByLocation { get; set; }

        public int ResolvedRequestByLocation { get; set; }

        public int CancelRequestByLocation { get; set; }
    }
}
