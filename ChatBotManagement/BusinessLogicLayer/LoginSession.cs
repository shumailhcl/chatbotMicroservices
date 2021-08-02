using ChatBotManagement.Model;
using ChattbotManagment.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatBotManagement.BusinessLogicLayer
{
    public class LoginSession
    {
        ChatBotContext _chatBotContext;
        public LoginSession(ChatBotContext chatBotContext)
        {
                _chatBotContext = chatBotContext;
        }

        public void SaveSession(LEmpSession data)
        {
            _chatBotContext.lEmpSession.Add(data);
            _chatBotContext.SaveChanges();

        }
    }
}
