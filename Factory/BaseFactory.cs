using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

//工厂基类 YMH

namespace WMS.Factory
{
  
    public class BaseFactory
    {
        public string asspath { get; set; }

        /// <summary>
        /// 创建一个实体
        /// </summary>
        /// <param name="ClassNamespace"></param>
        /// <param name="paramsStr"></param>
        /// <returns></returns>
        public static object CreateObject(string AssemblyPath, string ClassNamespace)
        {
            ClassNamespace = AssemblyPath + "." + ClassNamespace;
            object objtype = Assembly.Load(AssemblyPath).CreateInstance(ClassNamespace);
            return objtype;
        }
    }
}
