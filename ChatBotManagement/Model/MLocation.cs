using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ChatBotManagement.Model
{
    public class mLocation
    {
        [Key]
        public int locationId { get; set; }
        public string locationName { get; set; }
        public bool isActive { get; set; }
    }
}
