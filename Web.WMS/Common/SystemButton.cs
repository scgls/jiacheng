using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Web.WMS.Util;
using WebGrease.Css.Extensions;

namespace Web.WMS.Common
{
    /// <summary>
    /// 系统按钮管理
    /// </summary>
    public static class SystemButton
    {
        static SystemButton()
        {
            InitAllBut();
        }

        private static List<But> allBut = new List<But>();
        private static void InitAllBut()
        {
            string filePath = PathHelper.GetAbsolutePath("~/Config/SystemButton.config");
            XElement xe = XElement.Load(filePath);
            List<But> Buts = new List<But>();
            xe.Elements("Controller")?.ForEach(aElement1 =>
            {
                But newBut1 = new But();
                newBut1.Text = aElement1.Attribute("Text").Value;
                newBut1.IsHave = Convert.ToBoolean(aElement1.Attribute("IsHave").Value);
                newBut1.IsChecked = Convert.ToBoolean(aElement1.Attribute("IsChecked").Value);
                Buts.Add(newBut1);
                newBut1.children = new List<But>();
                aElement1.Elements("Button")?.ForEach(aElement2 =>
                {
                    But newBut2 = new But();
                    newBut1.children.Add(newBut2);
                    newBut2.Text = aElement2.Attribute("Text").Value;
                    newBut2.IsHave = Convert.ToBoolean(aElement2.Attribute("IsHave").Value);
                    newBut2.IsChecked = Convert.ToBoolean(aElement2.Attribute("IsChecked").Value);
                    newBut2.children = new List<But>();
                });
            });
            allBut = Buts;
        }

        /// <summary>
        /// 获取系统所有按钮
        /// </summary>
        /// <returns></returns>
        public static List<But> GetAllSysBut()
        {
            //return allBut.DeepClone();
            return allBut;
        }
     
    }

    #region 数据模型
    public class But
    {
        public string Text { get; set; }
        public bool IsHave { get; set; }
        public bool IsChecked { get; set; }
        public List<But> children { get; set; }
    }
    #endregion

}

