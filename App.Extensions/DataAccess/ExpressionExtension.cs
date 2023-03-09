
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace App.Extensions.DataAccess
{
    /// <summary>
    /// 动态查询的扩展
    /// </summary>
    public static class ExpressionExtension
    {
        #region JSON序列化

        /// <summary>
        /// 实体对象转字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="ignoreNull"></param>
        /// <returns></returns>
        public static string ToJson(this object obj, bool ignoreNull = false)
        {
            if (obj==null)
                return null;

            return JsonConvert.SerializeObject(obj, Formatting.None, new JsonSerializerSettings
            {
                DateFormatString = "yyyy-MM-dd HH:mm:ss",
                NullValueHandling = ignoreNull ? NullValueHandling.Ignore : NullValueHandling.Include
            });
        }

        ///// <summary>
        ///// JSON字符串转实体对象
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="jsonStr"></param>
        ///// <returns></returns>
        //public static T FromJson<T>(this string jsonStr)
        //{
        //    return jsonStr.IsNullOrEmpty() ? default(T) : JsonConvert.DeserializeObject<T>(jsonStr);
        //}

        #endregion JSON序列化

        /// <summary>
        /// ToUtc
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long ToUtcNew(this DateTime value)
        {
            //DateTime date = TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1), TimeZoneInfo.loc4); //TimeZone.CurrentTimeZone.ToLocalTime();

            //return Convert.ToInt64((value - date).TotalSeconds);

            DateTimeOffset dto = new DateTimeOffset(value);
            return dto.ToUnixTimeSeconds();

            //return (value.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
            //var date = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            //return Convert.ToInt64((value - date).TotalSeconds);
        }

        /// <summary>
        /// UtcToDateTime
        /// </summary>
        /// <param name="utcTime"></param>
        /// <returns></returns>
        public static DateTime UtcToDateTimeNew(this long utcTime)
        {
            //DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            //TimeSpan toNow = new TimeSpan(utcTime);
            //return dtStart.Add(toNow);
            DateTime dtZone = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            dtZone = dtZone.AddSeconds(utcTime);
            return dtZone.ToLocalTime();
        }

        public static DateTime UtcToDateTimeNew2(this long utcTime)
        {
            //DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            //TimeSpan toNow = new TimeSpan(utcTime);
            //return dtStart.Add(toNow);
            DateTime dtZone = new DateTime(1970, 1, 1, 23, 59, 59, 0);
            dtZone = dtZone.AddSeconds(utcTime);
            return dtZone.ToLocalTime();
        }

        public static Expression<Func<T, bool>> True<T>()
        {
            return f => true;
        }

        public static Expression<Func<T, bool>> False<T>()
        {
            return f => false;
        }

        public static Expression<Func<T, bool>> And<T>(
       this Expression<Func<T, bool>> first,
       Expression<Func<T, bool>> second)
        {
            return first.AndAlso<T>(second, Expression.AndAlso);
        }

        public static Expression<Func<T, bool>> And1<T>(
       this Expression<Func<T, bool>> first,
       Expression<Func<T, bool>> second)
        {
            return first.AndAlso<T>(second, Expression.AndAlso);
        }

        public static Expression<Func<T, bool>> Or<T>(
            this Expression<Func<T, bool>> first,
            Expression<Func<T, bool>> second)
        {
            return first.AndAlso<T>(second, Expression.OrElse);
        }

        private static Expression<Func<T, bool>> AndAlso<T>(
        this Expression<Func<T, bool>> expr1,
        Expression<Func<T, bool>> expr2,
        Func<Expression, Expression, BinaryExpression> func)
        {
            var parameter = Expression.Parameter(typeof(T));

            var leftVisitor = new ReplaceExpressionVisitor(expr1.Parameters[0], parameter);
            var left = leftVisitor.Visit(expr1.Body);

            var rightVisitor = new ReplaceExpressionVisitor(expr2.Parameters[0], parameter);
            var right = rightVisitor.Visit(expr2.Body);

            return Expression.Lambda<Func<T, bool>>(
                func(left, right), parameter);
        }

        public static int UpdateAndSave<TEntity>(this DbContext context, TEntity entity)
        {
            context.Update(entity);
            return context.SaveChanges();
        }

        public static int AddAndSave<TEntity>(this DbContext context, TEntity entity)
        {
            context.Add(entity);
            return context.SaveChanges();
        }

        private class ReplaceExpressionVisitor
            : ExpressionVisitor
        {
            private readonly Expression _oldValue;
            private readonly Expression _newValue;

            public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
            {
                _oldValue = oldValue;
                _newValue = newValue;
            }

            public override Expression Visit(Expression node)
            {
                if (node == _oldValue)
                    return _newValue;
                return base.Visit(node);
            }
        }
    }
}
