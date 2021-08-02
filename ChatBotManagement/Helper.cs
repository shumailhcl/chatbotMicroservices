using ChatBotManagement.Model;
using ChattbotManagment.Context;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatBotManagement
{
    public class Helper
    {
        ChatBotContext _chatBotContext;
        public Helper(ChatBotContext chatbotContext)
        {
            _chatBotContext = chatbotContext;
        }
        public int GetEmployeeId(int sapId)
        {
           var data= _chatBotContext.mEmployeeDetails.Where(r => (r.employeeSapId == sapId && r.isActive==true));
            if (data.Any())
                return Convert.ToInt32(data.SingleOrDefault().employeeId);
            else
                return 0;
        }

        public mEmployeeDetails GetEmployeeDetails(int sapId)
        {
            var data=_chatBotContext.mEmployeeDetails.Where(r => (r.employeeSapId == sapId && r.isActive == true));
                return data.SingleOrDefault();           
        }

        public string GetLocation(int locationId)
        {
            var location = _chatBotContext.mLocation.Where(r => (r.locationId == locationId && r.isActive == true));
            if (location.Any())
                return Convert.ToString(location.SingleOrDefault().locationName);
            else
                return null;

        }

        public string GetCategoryName(int categoryId)
        {
            var category = _chatBotContext.mCategory.Where(r => (r.categoryId == categoryId && r.isActive == true));
            if (category.Any())
                return Convert.ToString(category.SingleOrDefault().categoryType);
            else
                return null;
        }
        public string GetSubCategoryName(int SubcategoryId)
        {
            var category = _chatBotContext.mSubCategory.Where(r => (r.subCategoryId == SubcategoryId && r.isActive == true));
            if (category.Any())
                return Convert.ToString(category.SingleOrDefault().subCategoryName);
            else
                return null;
        }
        public string GetSubCategoryChildName(int SubcategoryChildId)
        {
            var category = _chatBotContext.mSubCategoryChild.Where(r => (r.subCategoryChildID == SubcategoryChildId && r.isActive == true));
            if (category.Any())
                return Convert.ToString(category.SingleOrDefault().subCategoryChildName);
            else
                return null;
        }

        public int GetSAPId(int employeeId)
        {
            var data = _chatBotContext.mEmployeeDetails.Where(r => r.employeeId == employeeId);
            if (data.Any())
                return Convert.ToInt32(data.SingleOrDefault().employeeSapId);
            else
                return 0;
        }
    }
}
