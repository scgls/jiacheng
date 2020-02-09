using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;
using System.Web.Mvc;
using System.Reflection;

namespace Web.WMS.Common
{
    public static class PageTag
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="RecordCounts">数据总条数</param>
        /// <param name="PagesCount">一页数据条数</param>
        /// <param name="currentPage">当前页</param>
        /// <param name="url">链接</param>
        /// <returns></returns>
        public static HtmlString ShowPageNavigate(this HtmlHelper htmlHelper, long RecordCounts, int PagesCount, int currentPage, String url)
        {
            
            var redirectTo = url;
            //var totalPages = RecordCounts==0?1:(Math.Max((RecordCounts + PagesCount - 1) / PagesCount, 1)); //总页数
            var totalPages = PagesCount;
            var output = new StringBuilder();
            if (totalPages > 1)
            {
                output.AppendFormat("<li><a href='{0}&CurrentPageNumber=1&RecordCounts={1}&PagesCount={2}'>首页</a><li> ", redirectTo, RecordCounts, PagesCount);
                if (currentPage > 1)
                {//处理上一页的连接
                    output.AppendFormat("<li><a  href='{0}&CurrentPageNumber={1}&RecordCounts={2}&PagesCount={3}'>上一页</a><li> ", redirectTo, currentPage - 1, RecordCounts, PagesCount);
                }
                else
                {
                    output.AppendFormat("<li><span>上一页</span><li> ");
                }

                output.Append(" ");
                int currint = 4;
                for (int i = 0; i <= 8; i++)
                {//一共最多显示9个页码，前面4个，后面4个
                    if ((currentPage + i - currint) >= 1 && (currentPage + i - currint) <= totalPages)
                    {
                        ////尾页4页的计算
                        //if ((totalPages - currentPage)<4)
                        //{

                        //}
                        //else
                        //{
                        if (currint == i)
                        {//当前页处理                           
                         //output.AppendFormat("<li><a href='{0}&CurrentPageNumber={1}&RecordCounts={2}&PagesCount={3}'>{4}</a><li> ", redirectTo, currentPage, RecordCounts, PagesCount, currentPage);
                            output.AppendFormat("<li><span style='background-color:#D1D1D1;'>{0}</span><li> ", currentPage);
                        }
                        else
                        {//一般页处理
                            output.AppendFormat("<li><a href='{0}&CurrentPageNumber={1}&RecordCounts={2}&PagesCount={3}'>{4}</a><li> ", redirectTo, currentPage + i - currint, RecordCounts, PagesCount, currentPage + i - currint);
                        }
                        //}
                    }
                    output.Append(" ");
                }
                if (currentPage < totalPages)
                {//处理下一页的链接
                    output.AppendFormat("<li><a  href='{0}&CurrentPageNumber={1}&RecordCounts={2}&PagesCount={3}'>下一页</a><li> ", redirectTo, currentPage + 1, RecordCounts, PagesCount);
                }
                else
                {
                    output.AppendFormat("<li><span>下一页</span><li> ");
                }
                output.Append(" ");
                if (currentPage != totalPages)
                {
                    output.AppendFormat("<li><a href='{0}&CurrentPageNumber={1}&RecordCounts={2}&PagesCount={3}'>末页</a><li> ", redirectTo, totalPages, RecordCounts, PagesCount);
                }
                else
                {
                    output.AppendFormat("<li><span>末页</span><li> ");
                }
                output.Append(" ");
            }
            output.AppendFormat("<li><span>第{0}页 / 共{1}页 (共{2}条数据)</span><li>", currentPage, totalPages, RecordCounts);//这个统计加不加都行

            HtmlString pagestring = new HtmlString(output.ToString());
            return pagestring;
        }



        public static string ModelToUriParam(object obj, string url = "")
        {
            PropertyInfo[] propertis = obj.GetType().GetProperties();
            StringBuilder sb = new StringBuilder();
            sb.Append(url);
            sb.Append("?");
            foreach (var p in propertis)
            {
                var v = p.GetValue(obj, null);
                if (v == null)
                    continue;
                sb.Append(p.Name);
                sb.Append("=");
                sb.Append(HttpUtility.UrlEncode(v.ToString()));
                sb.Append("&");
            }
            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();

        }


    }
}