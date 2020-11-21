using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ControlPanel.Helpers
{
    public static class SortHelper
    {
        public static MvcHtmlString Sort(this HtmlHelper html, string hrefValue,string sortOrder = "desc")
        {
            //< a href = @Url.Action("Delete", new { id = agent.Id }) >< i class="fa fa-close"></i></a>

            TagBuilder aTag = new TagBuilder("a");
            aTag.MergeAttribute("href", hrefValue);

            TagBuilder iTag = new TagBuilder("i");
            if (sortOrder == "asc")
            {
                iTag.MergeAttribute("class", "fa fa-sort-asc");
            }
            else
            {
                iTag.MergeAttribute("class", "fa fa-sort-desc");
            }

            aTag.InnerHtml += iTag.ToString();

            return new MvcHtmlString(aTag.ToString());
        }
    }
}