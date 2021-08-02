using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatBotManagement.Model
{
    public class TEmployeeRequest
    {
        [Key]
        public int requestId { get; set; }

        [NotMapped]
        public int needyUserSapId { get; set; }
        public int categoryId { get; set; }
        public int subCategoryId { get; set; }
        public int subCategoryChildId { get; set; }
        public string subCategoryChildOther { get; set; }
        public int employeeId { get; set; }
        public int createdBy { get; set; }
        public int locationId { get; set; }
        public int needyUserId { get; set; }
        public DateTime? createdDate { get; set; }
    }

    public class TEmployeeGraphRequest
    {
        [Key]
        //public int requestId { get; set; }
        public int employeeId { get; set; }
        public string Month { get; set; }

        public int TotalRequest { get; set; }

        public int ResolvedRequest { get; set; }

        public int NotResolveRequest { get; set; }

        public int CancelRequest { get; set; }
    }

}
