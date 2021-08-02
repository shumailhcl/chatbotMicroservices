using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ChatBotManagement.Model
{
    public class TRequestResponse
    {
		[Key]
		public int reqResponseId { get; set; }
		public int responseTypeId { get; set; }
		public int requestId { get; set; }
		public string query { get; set; }
		public string reply { get; set; }
		public int responseBy { get; set; }
		public DateTime? responseDate { get; set; }
		public int repliedBy { get; set; }
		public DateTime? repliedDate { get; set; }
		public bool isActive { get; set; }
		public int requestStatus { get; set; }
	}
}
