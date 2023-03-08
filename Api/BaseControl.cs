using App.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Api
{
    [Route("api")]
    public abstract class BaseController : Controller
    {

        #region 返回
        public const string CODE_SUCCESS = "SUCCESS";
        public const string CODE_FAIL = "FAIL";
        public const string CODE_UNAUTH = "401";
        public const string CODE_UNPERMISSION = "403";
        public const string CODE_ERROR = "ERROR";
        protected BaseResponse<object> JsonSuccess()
        {
            return new BaseResponse<object>
            {
                code = CODE_SUCCESS,
                data = null,
            };
        }
        /// <summary>
        /// 返回成功
        /// </summary>
        /// <returns></returns>
        protected BaseResponse<T> JsonSuccess<T>(T data=default(T))
        {
            return new BaseResponse<T>
            {
                code = CODE_SUCCESS,
                data = data,
            };
        }
        /// <summary>
        /// 返回string的json
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        protected string JsonString(object result)
        {
            return JsonConvert.SerializeObject(result);
        }
        /// <summary>
        /// 返回数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected BaseResponse<T> JsonSuccess<T>(T data,string message = "")
        {
            return new BaseResponse<T>
            {
                code = CODE_SUCCESS,
                msg = message,
                data = data
            };
        }

        protected BaseResponse<T> Ok<T>(T data)
        {
            return new BaseResponse<T>
            {
                code = CODE_SUCCESS,
                data = data
            };
        }
        protected BaseResponse<T> Fail<T>(string msg)
        {
            return new BaseResponse<T>
            {
                code = CODE_FAIL,
                msg = msg,
                data = default(T)
            };
        }
        protected BaseResponse<T> Error<T>(string msg)
        {
            return new BaseResponse<T>
            {
                code = CODE_ERROR,
                msg = msg,
                data = default(T)
            };
        }



        /// <summary>
        /// 返回单数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected BaseResponse<T> JsonData<T>(T data)
        {
            return JsonSuccess(data);
        }
        /// <summary>
        /// 返回列表
        /// </summary>
        /// <param name="count"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        protected JsonResult JsonList(decimal count, IEnumerable list)
        {
            return Json(new BaseResponse<IEnumerable>
            {
                code = CODE_SUCCESS,
                count = count,
                data = list
            });
        }
        protected JsonResult JsonList(int count, IEnumerable list)
        {
            return Json(new BaseResponse<IEnumerable>
            {
                code = CODE_SUCCESS,
                count = count,
                data = list
            });
        }
        /// <summary>
        /// 返回列表
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        protected JsonResult JsonList(IEnumerable list)
        {
            return Json(new BaseResponse<IEnumerable>
            {
                code = CODE_SUCCESS,
                count = 0,
                data = list
            });
        }



        


        /// <summary>
        /// 返回失败
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        protected BaseResponse<object> JsonFail(string msg)
        {
            return new BaseResponse<object>
            {
                code = CODE_FAIL,
                msg = msg
            };
        }
        private static int GetLineNum()
        {
            System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace(1, true);
            return st.GetFrame(0).GetFileLineNumber();
        }
        private static string GetFileName()
        {
            System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace(1, true);
            return st.GetFrame(0).GetFileName();
        }

        protected string GetStack()
        {
            return $"{GetFileName()}:{GetLineNum()} - ";
        }

        /// <summary>
        /// 返回错误
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        protected BaseResponse<object> JsonError(string msg)
        {
            return new BaseResponse<object>
            {
                code = CODE_ERROR,
                msg = msg
            };
        }
        /// <summary>
        /// 返回异常
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        protected BaseResponse<string> JsonError(Exception ex)
        {
            return new BaseResponse<string>
            {
                code = CODE_ERROR,
                msg = ex.Message,
                data = ex.StackTrace
            };
        }
        /// <summary>
        /// 返回失败信息
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        protected BaseResponse<object> JsonMessage(string msg)
        {
            return new BaseResponse<object>
            {
                code = CODE_SUCCESS,
                msg = msg
            };
        }
        #endregion

   
        /// <summary>
        /// 获取客户端IP
        /// </summary>
        /// <returns></returns>
        protected string GetClientIP()
        {
            string remoteIpAddress = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                remoteIpAddress = Request.Headers["X-Forwarded-For"];
            return remoteIpAddress;
        }

    }

}
