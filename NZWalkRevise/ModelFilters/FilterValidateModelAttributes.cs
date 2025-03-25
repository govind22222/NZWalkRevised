using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace NZWalkRevise.ModelFilters
{
    public class FilterValidateModelAttributes : ActionFilterAttribute
    {
        // Added By Raghevendra to validate the model attributes.
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid is false)
            {
                context.Result = new BadRequestResult();
            }
        }

    }
}
