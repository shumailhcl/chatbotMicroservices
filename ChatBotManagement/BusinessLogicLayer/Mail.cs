using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using ChatBotManagement.Model;
using ChattbotManagment.Context;
using Grpc.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Web;

namespace ChatBotManagement.BusinessLogicLayer
{
    public class Mail
    {
       
        ChatBotContext _chatBotContext;
        private readonly string imagepath;
        public Mail(IConfiguration _obj)
        {
            imagepath = _obj["Filepath"];
        }
        public Mail(ChatBotContext chatbotContext)
        {
            _chatBotContext = chatbotContext;
        }

        public void SendEmail(string activationCode, string employeeName,string employeeEmailId) //IHostingEnvironment env
        {
            
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.Credentials = new System.Net.NetworkCredential("hcl.helpinghand@gmail.com", "Hcl@12345");
            smtp.EnableSsl = true;
            MailMessage msg = new MailMessage();
            msg.Subject = "Activation code to verify Email";
            msg.Body = "Dear " + employeeName + ", <br/> Your activation Code is  " + activationCode + "\n\n Thanks  \n IT Team";
            string toAddress = employeeEmailId;
            msg.To.Add(toAddress);
            string fromAddress = "IT Team <hcl.helpinghand@gmail.com>";
            msg.From = new MailAddress(fromAddress);
            smtp.Send(msg);        
        }
        public void WelcomeEmail(string employeeName, string employeeEmailId)
        {
            
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.Credentials = new System.Net.NetworkCredential("hcl.helpinghand@gmail.com", "Hcl@12345");
            smtp.EnableSsl = true;
            MailMessage msg = new MailMessage();
            msg.Subject = "Welcome Email !! ";
            msg.IsBodyHtml = true;
            msg.Body = "<html><body><p> Dear "+ employeeName + " Welcome to Chat Bot ! <br/><center> <img src='https://chatbotapihcl.azurewebsites.net/Images/Welcome-Img.gif'/></center> <br/> By Login, You have taken your first step towards healthier life. <br/> We HCLites do everything we can to help and support others. <br/> <br/> <br/> Thanks  <br/> Chat Bot !!  </p></body></html>";  //lr.ContentId;
            string toAddress = employeeEmailId;
            msg.To.Add(toAddress);
            string fromAddress = "Chat Bot <hcl.helpinghand@gmail.com>";
            msg.From = new MailAddress(fromAddress);
            smtp.Send(msg);
        }
        public void BroadcastEmail(List<mEmployeeDetails> employeeDetails, TEmployeeRequest data)
        {
            Helper helper = new Helper(_chatBotContext);
            var emp=helper.GetEmployeeDetails(data.needyUserSapId);
            string requirement = string.Empty;
            if (data.subCategoryChildOther != null && data.subCategoryChildOther.Trim() != string.Empty && data.subCategoryId==5)
                requirement = data.subCategoryChildOther;
            else
                requirement =helper.GetSubCategoryName(data.subCategoryId) + " " + helper.GetSubCategoryChildName(data.subCategoryChildId);
            MailMessage msg = new MailMessage();
            string fromAddress = "IT Team <hcl.helpinghand@gmail.com>";
            msg.From = new MailAddress(fromAddress);
            msg.Subject = "Urgent Help Chat Bot Alert !! ";
            msg.IsBodyHtml = true;
            msg.Body = "<html><body><p>Name - "+emp.employeeName+"<br/> SapId - "+emp.employeeSapId+"<br/> Contact Number -  "+emp.employeeContactNo+"<br/>"+"EmailId - "+emp.employeeEmailId+"<br/> Requirement - "+requirement+ "</p><br/><a href='https://helpingbot.azurewebsites.net/'>Click Here</a></body></html>"; 

            foreach (var employee in employeeDetails)
            {
                msg.To.Add(new MailAddress(employee.employeeEmailId));
            }
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.Credentials = new System.Net.NetworkCredential("hcl.helpinghand@gmail.com", "Hcl@12345");
            smtp.EnableSsl = true;
            smtp.Send(msg);
        }
    }
}
