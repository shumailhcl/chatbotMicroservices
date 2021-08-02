using ChatBotManagement.Model;
using ChattbotManagment.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ChatBotManagement.Controllers
{
   
   [ApiController]
   [Authorize]
    public class CategoryController : ControllerBase
    {

        ChatBotContext _chatbotContext;
        public CategoryController(ChatBotContext chatbotContext)
        {
            _chatbotContext = chatbotContext;
        }

        [HttpGet("getCategoryList")] 
        public ActionResult<mCategory> Get()
        {
            try
            {
                var category = _chatbotContext.mCategory.Where(x => x.isActive == true);
                if (category.Any())
                {
                    return Ok(category.ToList());
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new Error { error = "No record Found" });
                }
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,new Error { error = ex.Message });
            }
        }
        
        [HttpGet("getSubCategory/{categoryId}")]
        public ActionResult<SubCategory> Get(int categoryId)
        {
            try
            {    
                if (categoryId != 0)
                {
                    var subCategory = _chatbotContext.mSubCategory.Where(x => (x.categoryId == categoryId && x.isActive == true));
                    if (subCategory.Any())
                        return Ok(subCategory.ToList());
                    else
                        return StatusCode(StatusCodes.Status404NotFound, new Error { error = "No record Found" });                  
                }
                else
                {
                    return BadRequest(new Error { error = "CategoryId is invalid" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Error { error = ex.Message });
            }
        }

        [HttpGet("getSubCategoryChild{categoryId}/{subCategoryId}")]
        public ActionResult<SubCategoryChild> Get(int categoryId,int subCategoryId)
        {
            try
            {
                if (categoryId != 0 && subCategoryId != 0)
                {
                    var subCategoryChild = _chatbotContext.mSubCategoryChild.Where(x => (x.subCategoryId == subCategoryId && x.categoryId == categoryId && x.isActive == true));
                    if (subCategoryChild.Any())
                        return Ok(subCategoryChild.ToList());
                    else
                        return StatusCode(StatusCodes.Status200OK ,new Message { Info = " No record Found " });
                      //  return StatusCode(StatusCodes.Status404NotFound, new Error { error = " No record Found " });
                }
                else
                {
                    return BadRequest(new Error { error = " Category Id or Sub Category Id is invalid" });
                }

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Error { error = ex.Message });
            }
        }

        [HttpGet("getNeedyUserType")]
        public ActionResult<mNeedyUser> GetNeedyUSeType()
        {
            try
            {
                var needyUser = _chatbotContext.mNeedyUser.Where(x => x.isActive == true);
                if (needyUser.Any())
                {
                    return Ok(needyUser.ToList());
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new Error { error = " No record Found " });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Error { error = ex.Message });
            }
        }

        [HttpGet("getResponseType")]
        public ActionResult<mResponseType> GetResponseType()
        {
            try
            {
                var response = _chatbotContext.mResponseType.Where(x => x.isActive == true);
                if(response.Any())
                {
                    return Ok(response.ToList());
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new Error { error = " No record Found " });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Error { error = ex.Message });
            }
        }

        [HttpGet("getLocationList")]
        public ActionResult<mResponseType> GetLocationList()
        {
            try
            {
                var location = _chatbotContext.mLocation.Where(x => x.isActive == true);
                if (location.Any())
                {
                    return Ok(location.ToList());
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new Error { error = " No record Found " });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Error { error = ex.Message });
            }
        }
    }
}

