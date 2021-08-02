using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ChatBotManagement.Model
{
	public class mCategory
	{
		[Key]
		public int categoryId { get; set; }
		public string categoryType { get; set; }

		public bool isActive { get; set; }
		
		}

	public class Error
    {
		public string error { get; set; }
    }
	public class Message
    {
		public string Info { get; set; }
    }
}

