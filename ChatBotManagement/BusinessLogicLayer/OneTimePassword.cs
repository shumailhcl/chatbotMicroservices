using ChatBotManagement.Model;
using ChattbotManagment.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatBotManagement.BusinessLogicLayer
{
    public class OneTimePassword
    {
        ChatBotContext _chatBotContext;
        public OneTimePassword(ChatBotContext chatbotContext)
        {
            _chatBotContext = chatbotContext;
        }

        public string CreateOTP()
        {
            Random random = new Random();
            return random.Next(100001, 999999).ToString();
        }

        public tUserOtp InsertOtpData(int employeeId, string activationCode) 
        {
            tUserOtp objOTP = new tUserOtp();
            DateTime date = DateTime.Now;

            objOTP.otpNumber = activationCode;
            objOTP.otpFor = "Login";
            objOTP.isUsed = false;
            objOTP.expiryDate = date.AddMinutes(2); 
            objOTP.createdBy = employeeId;
            objOTP.createdDate = date;
            objOTP.usedDate = null;
            objOTP.isActive = true;

            _chatBotContext.tUserOtp.Add(objOTP);
            _chatBotContext.SaveChanges();

            return objOTP;
        }
        public bool CheckOtp(ValidateOTP data, int employeeId)
        {
            var otpRecords = _chatBotContext.tUserOtp.Where(x => (x.createdBy == employeeId && x.isActive == true && x.isUsed == false && x.otpFor.Trim().ToLower()== "login" && x.expiryDate > DateTime.Now));
            if (otpRecords.Any())
            {
                var otp = otpRecords.Where(x => x.createdDate == otpRecords.Max(r => r.createdDate)).FirstOrDefault();
                if (otp.otpNumber == data.otpNumber)
                {
                    otp.usedBy = employeeId;
                    otp.isUsed = true;
                    otp.isActive = false;
                    otp.usedDate = DateTime.Now;
                    _chatBotContext.SaveChanges();
                    return true;
                }
            }
            return false;
        }
    }
}
