using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ChatBotManagement.Model;
using ChattbotManagment.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using ChatBotManagement.BusinessLogicLayer;
using Microsoft.AspNetCore.Hosting;



// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ChatBotManagement.Controllers
{
   
    [ApiController]
    public class EmployeeDetailsController : ControllerBase
    {
        // GET: api/<EmployeeDetailsController>

        private ChatBotContext _chatbotContext;
        private IConfiguration _config;
       // private readonly IHostingEnvironment _webHostEnvironment;
        public EmployeeDetailsController (ChatBotContext chatbotContext, IConfiguration config)
        {
            _chatbotContext = chatbotContext;
            _config = config;
           // _webHostEnvironment = webHostEnvironment;
        }  //, IHostingEnvironment webHostEnvironment

        [HttpPost("VerifyCredential")]
        public IActionResult Post([FromBody] Login data)
        {
            try
            {           
                if (data.employeeSapId!=0 && data.employeeEmailId?.ToString()!=null)
                {
                    var val = _chatbotContext.mEmployeeDetails.Where(x => (x.employeeEmailId == data.employeeEmailId && x.employeeSapId==data.employeeSapId && x.isActive == true));
                    if (val.Any())
                    {
                        var employeeDetails = val.SingleOrDefault();
                        OneTimePassword OTP = new OneTimePassword(_chatbotContext);
                        Mail mail = new Mail(_config);

                        string activationCode = OTP.CreateOTP();
                        mail.SendEmail(activationCode, employeeDetails.employeeName,employeeDetails.employeeEmailId);  // _webHostEnvironment
                        tUserOtp objOTP = OTP.InsertOtpData(employeeDetails.employeeId, activationCode);
                        AuthenticateUserStatus user = new AuthenticateUserStatus();
                        user.message = "User is Authorized";
                        user.expiryDate = objOTP.expiryDate;
                        user.status = true;                     
                        return Ok(user); 
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status401Unauthorized, new Error { error = "Invalid Sap or EmailId" });
                    }
                }
                else
                    return BadRequest(new Error { error = "SapId and EmailId Both are required" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Error { error = ex.Message});
            }         
        }

        [HttpPost("ValidateOTP")]
        public IActionResult ValidateOTP([FromBody] ValidateOTP data)
        {
            try
            {
                if (data.employeeSapId != 0 && data.otpNumber != null)
                {
                    if (data.otpNumber.Length == 6)
                    {
                        OneTimePassword otp = new OneTimePassword(_chatbotContext);
                        Mail mail = new Mail(_config);
                        Helper helper = new Helper(_chatbotContext);
                        var empDetail = helper.GetEmployeeDetails(data.employeeSapId);
                        int empId = helper.GetEmployeeId(data.employeeSapId);
                        if (empId == 0)
                            return StatusCode(StatusCodes.Status401Unauthorized, "Invalid Sap Id");
                        if (otp.CheckOtp(data,empId))
                        {
                            var ID = _chatbotContext.lEmpSession.Where(o => o.employeeId == empId);
                            if (!ID.Any())
                            {
                                mail.WelcomeEmail(empDetail.employeeName, empDetail.employeeEmailId);
                               
                            }
                            var employeesDetails = _chatbotContext.mEmployeeDetails.Where(x => (x.employeeSapId == data.employeeSapId && x.isActive == true));
                            if (employeesDetails.Any())
                            {
                                DateTime loginTime = DateTime.Now;
                                DateTime sessionExpireTime = loginTime.AddDays(2);
                                var employeeDetails = employeesDetails.SingleOrDefault();
                                TokenGenerator token = new TokenGenerator(_config);
                                var tokenNumber = token.GenerateJSONWebToken(employeeDetails, sessionExpireTime);
                                employeeDetails.Token = tokenNumber;
                                LEmpSession empSession = new LEmpSession();
                                empSession.employeeId = employeeDetails.employeeId;
                                empSession.employeeIp = data.employeeIp;
                                empSession.browserDetail = data.browserDetail;
                                empSession.loginDateTime = loginTime;
                                empSession.logoutDateTime = sessionExpireTime;
                                empSession.sessionId = tokenNumber;
                                LoginSession loginSession = new LoginSession(_chatbotContext);
                                loginSession.SaveSession(empSession);
                                return Ok(employeeDetails);
                            }

                        }
                        return StatusCode(StatusCodes.Status401Unauthorized, new Error { error = "otp is not validated" });
                    }
                    else
                    {
                        return BadRequest(new Error { error = "otp length should be six" });
                    }
                }
                else
                {
                    return BadRequest(new Error { error = "EmployeeId and otpNumber  are required" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Error { error = ex.Message });
            }
        }
       
        [HttpPost("getEmployeeDetails")]
        [Authorize]
        public ActionResult<mEmployeeDetails> Post([FromBody] int sapId)
        {
            try
            {            
                    var Detail = _chatbotContext.mEmployeeDetails.Join(_chatbotContext.mLocation,
                                  mEmployeeDetails => mEmployeeDetails.locationId,
                                  mLocation => mLocation.locationId,
                                  (mEmployeeDetails, mLocation) => new
                                  {
                                      employeeId = mEmployeeDetails.employeeId,
                                      employeeSapId = mEmployeeDetails.employeeSapId,
                                      employeeEmailId = mEmployeeDetails.employeeEmailId,
                                      gender = mEmployeeDetails.gender,
                                      locationId = mEmployeeDetails.locationId,
                                      locationName = mLocation.locationName,
                                      isActive = mEmployeeDetails.isActive
                                  }).Where(r => (r.isActive == true && r.employeeSapId == sapId));

                if (Detail.Any())
                {
                    return Ok(Detail.ToList());
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new Error { error = "No record Found" });
                }
 
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Error { error = ex.Message });
            }
        }

        [HttpPost("Logout")]
        [Authorize]
        public IActionResult Logout([FromBody] Logout data)
        {
            try
            {
                if (data.SAPId != 0)  //&& data.logoutDateTime != null
                {
                    Helper helper = new Helper(_chatbotContext);
                    var empId = helper.GetEmployeeId(data.SAPId);
                    
                    var a = DateTime.UtcNow.AddHours(5);
                    var b = a.AddMinutes(30);

                    var updatedytable = _chatbotContext.lEmpSession.Where(a => (a.employeeId == empId && a.employeeIp == data.employeeIp &&
                    a.browserDetail == data.browserDetail && a.sessionId == data.sessionId));
                    if(updatedytable.Any())
                    {
                        var updaterecord = updatedytable.FirstOrDefault();
                        updaterecord.logoutDateTime = b;
                        _chatbotContext.SaveChanges();

                        return Ok(true);
                    }
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Error { error = ex.Message });
            }
        }

        [HttpPost("Session")]
        [Authorize]
        public IActionResult CheckSession([FromBody] Logout data)
        {
            try
            {
                Helper helper = new Helper(_chatbotContext);
                var currentDate = DateTime.UtcNow;
                var t = currentDate.AddHours(5);
                var finaltime = t.AddMinutes(30);
                var empId = helper.GetEmployeeId(data.SAPId);

                var result = _chatbotContext.lEmpSession.Where(a => (a.employeeId == empId && a.employeeIp == data.employeeIp && 
                a.sessionId == data.sessionId && a.browserDetail == data.browserDetail && a.logoutDateTime > finaltime));
                if(result.Any())                                        
                {
                    return Ok(true);
                }
                else
                {
                    return Ok(false);
                        //StatusCode(StatusCodes.Status404NotFound, new Error { error = "No record Found" });
                }
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Error { error = ex.Message });
            }
        }

    }
}
