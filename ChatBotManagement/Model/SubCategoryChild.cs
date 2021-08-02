using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ChatBotManagement.Model
{
    [Table("mSubCategoryChild")]
    public class SubCategoryChild
    {
        [Key]
        public int subCategoryChildID { get; set; }
        public int categoryId { get; set; }
        public int subCategoryId { get; set; }
        public string subCategoryChildName { get; set; }

        public bool isActive { get; set; }
    }
}
