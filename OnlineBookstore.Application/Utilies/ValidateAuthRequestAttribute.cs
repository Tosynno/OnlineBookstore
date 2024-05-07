using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using OnlineBookstore.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace OnlineBookstore.Application.Utilies
{
    public class ValidateAuthRequestAttribute : ActionFilterAttribute
    {
        public ValidateAuthRequestAttribute()
        {

        }
        private readonly BookdbContext _bookdbContext;
        private JwtHandler _jwtHandler;
        public ValidateAuthRequestAttribute(JwtHandler jwtHandler, BookdbContext bookdbContext)
        {
            _jwtHandler = jwtHandler;
            _bookdbContext = bookdbContext;
        }
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var descriptor = context.ActionDescriptor as ControllerActionDescriptor;
            if (descriptor != null)
            {
                var _logger = context.HttpContext.RequestServices.GetService<ILogger<ValidateAuthRequestAttribute>>();
                try
                {
                    string[] auth = context.HttpContext.Request.Headers["Authorization"].ToString().Split(':');

                    //Validate session
                    var res = await _bookdbContext.Admin_Users.FirstOrDefaultAsync(c => c.Password.Equals(auth[0].ToString()));
                    if (res != null)
                    {
                        var result = JwtHandler.CheckTokenIsValid("pass Token");

                        _logger.LogDebug($"Action: {descriptor.ActionName} * Access: {res.APIkey} ");
                        //using (var _dataAccessLayer = new VendorDataContext())
                        //{

                        //}]
                        if (!result)
                            context.Result = new JsonResult("You are not authorized to call this endpoint");

                    }
                    else
                    {
                        context.Result = new JsonResult("Access Denied!!!");
                    }

                    

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message, ex);
                    context.Result = new JsonResult("Sorry, the application is unavailable at the moment. Please try again later.");

                }
                await base.OnActionExecutionAsync(context, next);
            }

        }
    }
}
