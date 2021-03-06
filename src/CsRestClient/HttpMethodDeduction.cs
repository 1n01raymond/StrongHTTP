﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;

namespace CsRestClient
{
    using CsRestClient.Attributes.Request;

    /// <summary>
    /// 메소드 이름, 또는 Attribute를 통해 메소드 추론을 수행하는 클래스
    /// </summary>
    /// <see cref="AutoHttpMethod"/>
    /// <see cref="Method"/>
    internal static class HttpMethodDeduction
    {
        internal static Dictionary<string, HttpMethod> deductionTable { get; } =
            new Dictionary<string, HttpMethod>()
            {
                /* GET */
                ["get"] = HttpMethod.Get,
                ["query"] = HttpMethod.Get,

                /* POST */
                ["create"] = HttpMethod.Post,

                /* PUT */
                ["update"] = HttpMethod.Put,
                ["modify"] = HttpMethod.Put,

                /* DELETE */
                ["remove"] = HttpMethod.Delete,
                ["delete"] = HttpMethod.Delete,
            };

        private static HttpMethod GetHttpMethodByAttribute(
            this MethodInfo method)
        {
            var attr = method.GetCustomAttribute<Method>();
            if (attr != null)
            {
                return attr.method;
            }

            throw new InvalidOperationException();
        }
        private static HttpMethod GetHttpMethodByName(
            this MethodInfo method)
        {
            var name = method.Name.ToLower();
            foreach (var item in deductionTable)
            {
                if (name.StartsWith(item.Key))
                    return item.Value;
            }

            throw new InvalidOperationException();
        }
        public static HttpMethod GetHttpMethod(
            this MethodInfo method)
        {
            try
            {
                if (method.DeclaringType.GetCustomAttribute<AutoHttpMethod>() != null)
                    return method.GetHttpMethodByName();

                return method.GetHttpMethodByAttribute();
            }
            catch (Exception)
            {
                return HttpMethod.Get;
            }
        }
    }
}
