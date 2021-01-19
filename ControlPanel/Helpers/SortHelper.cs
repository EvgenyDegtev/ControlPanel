using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using System.Web.Mvc.Routing;
using System.Web.Routing;

namespace ControlPanel.Helpers
{
    public static class SortHelper
    {
        public static MvcHtmlString Sort(this HtmlHelper html, string searchString, string page, string positionName, string prevSelectedEntityName, string prevSortOrder, int? selectedSkillId=null)
        {
            string controller = html.ViewContext.RouteData.Values["controller"].ToString();
            string hrefValue = GenerateSortUrl(controller,searchString,page, positionName, prevSelectedEntityName, prevSortOrder, selectedSkillId);

            TagBuilder aTag = new TagBuilder("a");
            aTag.MergeAttribute("href", hrefValue);

            TagBuilder iTag = new TagBuilder("i");

            iTag.MergeAttribute("class", GetClassAttribute(positionName,prevSelectedEntityName,prevSortOrder));
            aTag.InnerHtml += iTag.ToString();

            return new MvcHtmlString(aTag.ToString());
        }

        public static string GenerateSortUrl(string controller,string searchString, string page, string positionName, string prevSelectedEntityName, string prevSortOrder, int? selectedSkillId)
        {
            string sortOrder = GetSortOrder(positionName, prevSelectedEntityName, prevSortOrder);

            string hrefValue = UrlHelper.GenerateUrl(null, "Index", controller, new RouteValueDictionary
            {
                { "searchString", searchString },
                {"page", page },
                {"sortOrder", sortOrder },
                { "selectedSortProperty", positionName },
                {"skillId",selectedSkillId }
            },
            RouteTable.Routes,
            HttpContext.Current.Request.RequestContext,
            false);

            return hrefValue;
        }

        public static string GetSortOrder(string positionName, string prevSelectedEntityName, string prevSortOrder)
        {
            if (prevSelectedEntityName == positionName)
            {
                if (prevSortOrder == "asc")
                    return "desc";
                else
                    return "asc";
            }
            else
            {
                return "asc";
            }
        }

        public static string GetClassAttribute(string positionName, string prevSelectedEntityName, string prevSortOrder)
        {
            string classAttributeValue = "fa fa-sort";
            if (positionName == prevSelectedEntityName)
            {
                if (prevSortOrder == "asc")
                {
                    classAttributeValue="fa fa-sort-asc";
                }
                else
                {
                    classAttributeValue = "fa fa-sort-desc";
                }
            }
            return classAttributeValue;
        }
    }
}