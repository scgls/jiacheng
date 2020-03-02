using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;

//Service 层的工厂类 YMH
namespace WMS.Factory
{
    public class ServiceFactory:BaseFactory
    {

        /// <summary>
        /// 创建一个实体
        /// </summary>
        /// <param name="ClassNamespace"></param>
        /// <param name="paramsStr"></param>
        /// <returns></returns>
        public static object CreateObject(string ClassNamespace)
        {
            string AssemblyPath = "BILWeb";
            return CreateObject(AssemblyPath, ClassNamespace);
        }
    }
}
