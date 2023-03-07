using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace App.Contracts
{
    [KnownType(typeof(BaseResponse<string>))]
    public class BaseResponse<T>
    {
        public string code { get; set; }

        public decimal count { get; set; }
        public string msg { get; set; }
        public T data { get; set; }
        public BaseResponse()
        {
            code = "SUCCESS";
        }


    }
}
