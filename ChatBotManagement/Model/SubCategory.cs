using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ChatBotManagement.Model
{
    [Table("mSubCategory")]
    public class SubCategory
    {
        [Key]
        public int subCategoryId { get; set; }
        public int categoryId { get; set; }
        public string subCategoryName { get; set; }

        public bool isActive { get; set; }
    }
}
