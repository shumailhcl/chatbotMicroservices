using ChatBotManagement.BusinessLogicLayer;
using ChatBotManagement.Model;
using ChattbotManagment.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
//using System.Data.SqlClient;
using Microsoft.Data.SqlClient;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ChatBotManagement.Controllers
{
    [ApiController]
    [Authorize]
    public class HelpRequestController : ControllerBase
    {
        // GET: api/<HelpRequestController>
        

        private ChatBotContext _chatbotContext;
        
        public HelpRequestController(ChatBotContext chatbotContext)
        {
            _chatbotContext = chatbotContext;          
        }

        [HttpPost("SendHelpRequest")]   
        public ActionResult<TEmployeeRequest> EmployeeRequest([FromBody] TEmployeeRequest data)
        {
            try
            {
                if (data.createdBy != 0)
                {

                    if (data.needyUserId == 2)
                    {
                        var needyEmployeeDetail = _chatbotContext.mEmployeeDetails.Where(r => (r.employeeSapId == data.needyUserSapId && r.isActive == true));
                        if (needyEmployeeDetail.Any())
                        {
                            var needyEmployee = needyEmployeeDetail.SingleOrDefault();
                            data.employeeId = needyEmployee.employeeId;
                        }
                        else
                        {
                            return StatusCode(StatusCodes.Status401Unauthorized, new Error { error = "Needy User is unauthorized" });
                        }
                    }
                    else
                    {
                        data.employeeId = data.createdBy;
                    }

                    _chatbotContext.tEmployeeRequest.Add(data);

                    var val = _chatbotContext.SaveChanges();
                    int requestId = data.requestId;
                    int requestedBy = data.createdBy;
                    DateTime? requestedDate = data.createdDate;
                    if (val == 1)
                    {
                        var employeeListForSendingHelp = _chatbotContext.mEmployeeDetails.Where(r => (r.locationId == data.locationId && r.isActive == true));
                        if (employeeListForSendingHelp.Any())
                        {
                            List<mEmployeeDetails> listOfEmployees = employeeListForSendingHelp.ToList();
                            var removeNeedyEmployee = listOfEmployees.Where(r => (r.employeeId == data.employeeId || r.employeeId == data.createdBy));
                            if (removeNeedyEmployee.Any())
                            {
                                List<mEmployeeDetails> listOfRemoveEmployee = removeNeedyEmployee.ToList();
                                listOfEmployees = listOfEmployees.Except(removeNeedyEmployee).ToList();
                            }
                            Mail mail = new Mail(_chatbotContext);
                            mail.BroadcastEmail(listOfEmployees, data);
                            List<TEmployeeRequestBroadcast> lisOfTEmployeeRequestBroadcasts = new List<TEmployeeRequestBroadcast>();
                            foreach (var emp in listOfEmployees)
                            {
                                TEmployeeRequestBroadcast temployeeRequestBroadcast = new TEmployeeRequestBroadcast();
                                temployeeRequestBroadcast.requestId = requestId;
                                temployeeRequestBroadcast.requestedToEmployeeId = emp.employeeId;
                                temployeeRequestBroadcast.requestedBy = requestedBy;
                                temployeeRequestBroadcast.requestedDate = requestedDate;
                                temployeeRequestBroadcast.isActive = true;
                                lisOfTEmployeeRequestBroadcasts.Add(temployeeRequestBroadcast);
                            }
                            _chatbotContext.BulkInsert(lisOfTEmployeeRequestBroadcasts);
                            return Ok(data);

                        }
                        else
                        {
                            return StatusCode(StatusCodes.Status404NotFound, new Error { error = "No Employee found at needy location" });
                        }

                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, new Error { error = "Something Went Wrong" });
                    }
                }

                else
                    return BadRequest(new Error { error = "Employee Id is incorrect" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Error { error = ex.Message });
            }
        }

        [HttpPost("UpdateHelpRequest")]
        public IActionResult UpdateBroadcast([FromBody] TEmployeeRequestBroadcast data)
        {
            try
            {
                if (data.SapId != 0)
                {
                    Helper helper = new Helper(_chatbotContext);
                    var employeeId = helper.GetEmployeeId(data.SapId);
                    var updaterecords = _chatbotContext.temployeeRequestBroadcast.Where(x => (x.requestedToEmployeeId == employeeId && x.requestId == data.requestId && x.isActive == true));
                    if (updaterecords.Any())
                    {
                        var updaterecord = updaterecords.FirstOrDefault();
                        updaterecord.isRequestSeen = data.isRequestSeen;
                        updaterecord.isResponded = data.isResponded;                      
                        updaterecord.respondedBy = employeeId;
                        updaterecord.respondedDate = data.respondedDate;
                        updaterecord.isActive = false;
                        _chatbotContext.SaveChanges();

                        return Ok(updaterecord);
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status401Unauthorized, new Error { error = "Employee ID is unauthorized" });
                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, new Error { error = "Employee ID is unauthorized" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Error { error = ex.Message });
            }
        }

        [HttpPost("ListOfHelpingPeople")]      
        public IActionResult ListOfHelpingPeople([FromBody] TEmployeeRequestBroadcast data)
        {
            try
            {
                if (data.requestId != 0 && data.requestedBy != 0)
                {
                    var datalist = _chatbotContext.temployeeRequestBroadcast.Where(x => (x.requestId == data.requestId && x.requestedBy == data.requestedBy && x.isResponded == true));
                    if (datalist.Any())
                    {
                        var listofreadytohelpemployees = datalist.ToList();
                        List<int> empids = new List<int>();
                        foreach (var emp in listofreadytohelpemployees)
                        {
                            empids.Add(emp.requestedToEmployeeId);
                        }
                        var result = _chatbotContext.mEmployeeDetails.Where(r => empids.Contains(r.employeeId));
                        if (result.Any())
                        {
                            return Ok(result.ToList());
                        }
                        else
                        {
                            return StatusCode(StatusCodes.Status404NotFound, new Error { error = " No record Found " });
                        }
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status404NotFound, new Error { error = " No record Found " });
                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new Error { error = "Please enter valid ID" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Error { error = ex.Message });
            }
        }

        [HttpPost("GetNotifications")]
        public ActionResult<ReceiveRequest> ReceiveRequest([FromBody] SAP data)
        {
            try
            {
                Helper helper = new Helper(_chatbotContext);
                int requestToEmpdId = helper.GetEmployeeId(data.employeeSapId);
               var receiveRequest= _chatbotContext.temployeeRequestBroadcast.Where(r => (r.requestedToEmployeeId == requestToEmpdId && r.isActive == true));
                if(receiveRequest.Any())
                {

                    List<int> requestIds = new List<int>();
                    foreach(var request in receiveRequest.ToList())
                    {
                        requestIds.Add(request.requestId);
                    }

                    var sendRequest = _chatbotContext.tEmployeeRequest.Where(r => requestIds.Contains(r.requestId));
                    if(sendRequest.Any())
                    {
                        List<ReceiveRequest> receiveRequests = new List<ReceiveRequest>();
                        foreach(var request in sendRequest.ToList())
                        {
                            int empsapid = helper.GetSAPId(request.employeeId);
                            mEmployeeDetails empdetails = helper.GetEmployeeDetails(empsapid);
                            ReceiveRequest rr = new ReceiveRequest();
                            rr.RequestId = request.requestId;
                            rr.EmpName = empdetails.employeeName;
                            rr.EmpContactNumber = empdetails.employeeContactNo;
                            rr.EmpEmailId = empdetails.employeeEmailId;
                            rr.RequestToEmpId = requestToEmpdId;
                            rr.EmpSapId = empsapid;
                            rr.RequestByEmpId = request.createdBy;
                            string requirement = string.Empty;
                            if (request.subCategoryChildOther != null && request.subCategoryChildOther.Trim() != string.Empty && request.subCategoryId == 5)
                                requirement = request.subCategoryChildOther;
                            else
                                requirement = helper.GetSubCategoryName(request.subCategoryId) + " " + helper.GetSubCategoryChildName(request.subCategoryChildId);

                            rr.EmpRequirement = requirement;
                            receiveRequests.Add(rr);
                        }
                        return Ok(receiveRequests);
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status404NotFound, new Error { error = "No Notifications found" });
                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new Error { error = "No Notifications found"});
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Error { error = ex.Message });
            }
        }

        [HttpPost("EmployeeResponse")]
        public ActionResult<TRequestResponse> EmployeeResponse([FromBody] TRequestResponse data)
        {
            try
            {
                if (data.requestId != 0 && data.responseBy != 0 && data.requestStatus != 0)
                {
                    var updaterecords = _chatbotContext.trequestResponse.Where(x => (x.requestId == data.requestId && x.responseBy == data.responseBy && x.isActive == true));
                    if (updaterecords.Any())
                    {
                        var updaterecord = updaterecords.FirstOrDefault();
                        updaterecord.responseTypeId = data.responseTypeId;
                        updaterecord.query = data.query;
                        updaterecord.reply = data.reply;
                        updaterecord.responseDate = data.responseDate;
                        updaterecord.repliedBy = data.repliedBy;
                        updaterecord.requestStatus = data.requestStatus;

                        _chatbotContext.SaveChanges();
                        return Ok(updaterecord);
                    }
                    else
                    {
                        _chatbotContext.trequestResponse.Add(data);
                        int val = _chatbotContext.SaveChanges();
                        if (val == 1)
                         return Ok(); 
                        else
                        {
                            return StatusCode(StatusCodes.Status404NotFound, new Error { error = " No Data Inserted " });
                        }
                    }            
                }
                else
                {
                    return BadRequest(new Error { error = "Request id , response by and request status is required" });
                }
            }
            catch(Exception Ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Error { error = Ex.Message });
            }

        }

        /// <summary>
        /// Dashboard API
        /// </summary>
        /// <returns></returns>
        [HttpPost("RequestSummaryForUser")]
        public IActionResult RequestSummary([FromBody] SAP data)
        {
            try
            {
                RequestSummary rs = new RequestSummary();
                Helper helper = new Helper(_chatbotContext);
                int empid = helper.GetEmployeeId(data.employeeSapId);
                rs.TotalUser = _chatbotContext.lEmpSession.Select(r => r.employeeId).Distinct().Count();

                var totalRequests = _chatbotContext.tEmployeeRequest.Select(r => r.createdBy);
                if (totalRequests.Any())
                {
                    rs.TotalNeedyUser = totalRequests.Distinct().Count();
                    var requestdata = _chatbotContext.tEmployeeRequest.Where(r => (r.createdBy == empid));
                    if (requestdata.Any())
                    {
                        rs.MyRequest = requestdata.Count();
                        rs.PendingRequest = rs.MyRequest;
                         var responsedata = _chatbotContext.trequestResponse.Where(r => (r.responseBy == empid));
                        if (responsedata.Any())
                        {
                            var ResolveRequest = responsedata.Where(r => (r.requestStatus == 1));
                            if (ResolveRequest.Any())
                                rs.ResolvedRequest = ResolveRequest.Count();
                            var CancelRequest = responsedata.Where(r => (r.requestStatus == 3));
                            if (CancelRequest.Any())
                                rs.CancelRequest = CancelRequest.Count();
                            rs.PendingRequest = rs.MyRequest - (rs.ResolvedRequest + rs.CancelRequest);
                        }
                    }
                    return Ok(rs);
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new Error { error = "No Record Found" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Error { error = ex.Message });
            }
        }

        [HttpGet("RequestSummaryForLocation")]
        public IActionResult RequestSummaryForLocation()
        {
            try
            {

                var requestData = _chatbotContext.tEmployeeRequest;
                if (requestData.Any())
                {
                    Helper helper = new Helper(_chatbotContext);
                    List<mLocation> locations = _chatbotContext.mLocation.Where(r => r.isActive == true).ToList();
                    List<RequestSummaryForLocation> rsfl = new List<RequestSummaryForLocation>();
                    var response = _chatbotContext.trequestResponse;
                    if (response.Any())
                    {
                        List<int> resolveRequestids = new List<int>();
                        List<int> cancelRequestids = new List<int>();
                        var resloveData = _chatbotContext.trequestResponse.Where(r => r.requestStatus == 1);
                        if (resloveData.Any())
                        {
                            resolveRequestids = resloveData.Select(r => r.requestId).ToList();
                            var data = from e in _chatbotContext.tEmployeeRequest.Where(r=>resolveRequestids.Contains(r.requestId)) group e.locationId by e.locationId into g select new { request = g.Count(), location = g.Key };


                            foreach (var d in data)
                            {
                                RequestSummaryForLocation rs = new RequestSummaryForLocation();
                                rs.LocationId = d.location;
                                rs.Location = helper.GetLocation(d.location);             rs.ResolvedRequestByLocation = d.request;
                                rsfl.Add(rs);
                            }
                        }

                        var cancelData = _chatbotContext.trequestResponse.Where(r => r.requestStatus == 3);
                        if (cancelData.Any())
                        {
                            cancelRequestids = cancelData.Select(r => r.requestId).ToList();

                            var data = from e in _chatbotContext.tEmployeeRequest.Where(r => cancelRequestids.Contains(r.requestId)) group e.locationId by e.locationId into g select new { request = g.Count(), location = g.Key };


                            foreach (var d in data)
                            {
                                RequestSummaryForLocation rs = new RequestSummaryForLocation();
                                rs.LocationId = d.location;
                                rs.Location = helper.GetLocation(d.location);                     
                                rs.CancelRequestByLocation = d.request;
                                rsfl.Add(rs);
                            }
                        }

                        var allIds = _chatbotContext.tEmployeeRequest.Select(r => r.requestId).ToList();
                        var pendingIds = allIds.Except(resolveRequestids);
                        pendingIds = pendingIds.Except(cancelRequestids);
                        if(pendingIds.Any())
                        {
                            var data = from e in _chatbotContext.tEmployeeRequest.Where(r => pendingIds.Contains(r.requestId)) group e.locationId by e.locationId into g select new { request = g.Count(), location = g.Key };


                            foreach (var d in data)
                            {
                                RequestSummaryForLocation rs = new RequestSummaryForLocation();
                                rs.LocationId = d.location;
                                rs.Location = helper.GetLocation(d.location);
                                rs.PendingRequestByLocation = d.request;
                                rsfl.Add(rs);
                            }
                        }                                           
                    }
                    else
                    {
                        var data = from e in _chatbotContext.tEmployeeRequest group e.locationId by e.locationId into g select new { request = g.Count(), location = g.Key };
                        foreach (var d in data)
                        {
                            RequestSummaryForLocation rs = new RequestSummaryForLocation();
                            rs.LocationId = d.location;
                            rs.Location = helper.GetLocation(d.location);                     
                            rs.PendingRequestByLocation = d.request;
                            rsfl.Add(rs);
                        }
                    }

                    var dataresult = from e in rsfl group e by e.LocationId into g select new { LocationName = g.First().Location, locationId = g.Key, PendingRequestByLocation = g.Sum(r=>r.PendingRequestByLocation), ResolvedRequestByLocation = g.Sum(r=>r.ResolvedRequestByLocation), CancelRequestByLocation = g.Sum(r=>r.CancelRequestByLocation) };
                    return Ok(dataresult);

                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new Error { error = "No Record Found" });
                }
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Error { error = ex.Message });
            }

        }
    }
}
