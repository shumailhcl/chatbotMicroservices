using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ChatBotManagement.Model
{
    public class mNeedyUser
    {
        [Key]
        public int needyUserId { get; set; }
        public string needyUserType { get; set; }
        public bool isActive { get; set; }
    }
}
