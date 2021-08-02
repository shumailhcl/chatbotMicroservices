using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ChatBotManagement.Model
{
    public class mResponseType
    { 
        [Key]
        public int responseTypeId { get; set; }
        public string responseType { get; set; }
        public bool isActive { get; set; }
    }
}
