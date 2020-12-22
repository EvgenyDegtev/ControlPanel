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
        public static MvcHtmlString Sort(this HtmlHelper html, string hrefValue,string positionName, string selectedEntityName, string sortOrder)
        {
            //< a href = @Url.Action("Delete", new { id = agent.Id }) >< i class="fa fa-close"></i></a>

            TagBuilder aTag = new TagBuilder("a");
            aTag.MergeAttribute("href", hrefValue);

            TagBuilder iTag = new TagBuilder("i");

            if(positionName==selectedEntityName)
            {
                if (sortOrder == "asc")
                {
                    iTag.MergeAttribute("class", "fa fa-sort-asc");
                }
                else
                {
                    iTag.MergeAttribute("class", "fa fa-sort-desc");
                }
            }
            iTag.MergeAttribute("class", "fa fa-sort");

            aTag.InnerHtml += iTag.ToString();

            return new MvcHtmlString(aTag.ToString());
        }

        public static string GetSortOrder(string prevSortOrder, string prevSelectedEntityName, string selectedEntityName)
        {
            if (prevSelectedEntityName == selectedEntityName)
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


        public static string GenerateSortUrl(string searchString,string page, string prevSortOrder, string prevSelectedEntityName, string selectedEntityName)
        {
            string sortOrder = GetSortOrder(prevSortOrder, prevSelectedEntityName, selectedEntityName);

            string hrefValue = UrlHelper.GenerateUrl(null, "Index", "Agents", new RouteValueDictionary 
            {
                { "searchString", searchString }, 
                {"page", page },
                {"sortOrder", sortOrder }, 
                { "selectedEntityName", selectedEntityName },
                { "prevSelectedEntityName",prevSelectedEntityName } 
            }, 
            RouteTable.Routes, 
            HttpContext.Current.Request.RequestContext, 
            false);

            return hrefValue;
        }
    }
}