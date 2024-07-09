using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using api.Models.Repositories;


namespace api.Filters
{
    public class FilterBookID: ActionFilterAttribute
    {
        public override async void OnActionExecuting(ActionExecutingContext context){
            
            var id = context.ActionArguments["id"] as int?;
            if (id.HasValue){
                if (id.Value < 0){
                    context.ModelState.AddModelError("BookID","Book id must be a positive integer");
                    context.Result = new BadRequestObjectResult(context.ModelState);
                }

            }

        }
    }
}