﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;

namespace CarHub.Web.Utils.Alerts
{
    public class AlertDecoratorResult : IActionResult
    {
        public AlertDecoratorResult(IActionResult result, string type, string title, string body)
        {
            Result = result;
            Type = type;
            Title = title;
            Body = body;
        }

        public IActionResult Result { get; }

        public string Type { get; }

        public string Title { get; }

        public string Body { get; }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            var factory = context.HttpContext.RequestServices.GetService<ITempDataDictionaryFactory>();

            var tempData = factory.GetTempData(context.HttpContext);
            tempData["_alert.type"] = Type;
            tempData["_alert.title"] = Title;
            tempData["_alert.body"] = Body;

            await Result.ExecuteResultAsync(context);
        }
    }
}

