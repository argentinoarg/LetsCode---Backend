using BACK.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Diagnostics;

namespace BACK.Filters
{
    public class TrackExecutionTime : ActionFilterAttribute, IExceptionFilter
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            //<datetime> - Card <id> - <titulo> - <Remover|Alterar>

            string message = DateTime.Now + " - Card ";
            var result = (OkObjectResult) context.Result;

            try
            {
                CardModel card = (CardModel) result.Value;
                message += card.Id + " - " + card.Title + " - " + context.RouteData.Values["action"] + "\n";

            }
            catch 
            {

            }

            Debug.WriteLine(message);
        }

        public void OnException(ExceptionContext context)
        {
            Debug.WriteLine(context.Exception.Message);
        }
    }
}
