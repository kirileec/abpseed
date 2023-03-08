﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace App.Contracts
{
    [KnownType(typeof(BaseResponse<string>))]
    public class BaseResponse<T>:IActionResult
    {
        public string code { get; set; }

        public decimal count { get; set; }
        public string msg { get; set; }
        public T data { get; set; }
        public BaseResponse()
        {
            code = "SUCCESS";
        }

        public Task ExecuteResultAsync(ActionContext context)
        {
            return Task.CompletedTask;
        }
    }
}
