using ChatBotManagement.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChattbotManagment.Context
{
    public class ChatBotContext : DbContext
    {
        public ChatBotContext(DbContextOptions<ChatBotContext> options) : base(options)
        {

        }
        public DbSet<mCategory> mCategory { get; set; }
        public DbSet<SubCategory> mSubCategory { get; set; }
        public DbSet<SubCategoryChild> mSubCategoryChild { get; set; }
        public DbSet<mEmployeeDetails> mEmployeeDetails { get; set; }
        public DbSet<mLocation> mLocation { get; set; }
        public DbSet<mNeedyUser> mNeedyUser { get; set; }
        public DbSet<mResponseType> mResponseType { get; set; }
        public DbSet<TEmployeeRequest> tEmployeeRequest { get; set; }
        public DbSet<tUserOtp> tUserOtp { get; set; }
        public DbSet<LEmpSession> lEmpSession { get; set; }
        public DbSet<TEmployeeRequestBroadcast> temployeeRequestBroadcast { get; set; }
        public DbSet<TRequestResponse> trequestResponse { get; set; }
        public DbSet<TEmployeeGraphRequest> TEmployeeGraphRequest { get; set; }


    }
}

