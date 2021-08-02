using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ChatBotManagement.Model
{
    public class TEmployeeRequestBroadcast
    {
		[Key]
		public int broadcastId { get; set; }
		public int requestId { get; set; }
		public int requestedToEmployeeId { get; set; }
		public bool isRequestSeen { get; set; }
		public bool isResponded { get; set; }
		public int requestedBy { get; set; }
		public DateTime? requestedDate { get; set; }
		public int respondedBy { get; set; }
		public DateTime? respondedDate { get; set; }
		public bool isActive { get; set; }

		[NotMapped]
		public int SapId { get; set; }
	}
}
