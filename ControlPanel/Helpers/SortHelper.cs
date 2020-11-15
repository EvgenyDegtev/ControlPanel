using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ControlPanel.Helpers
{
    public static class SortHelper
    {
        public static MvcHtmlString Sort(this HtmlHelper html, string sortOrder="desc")
        {
            //< i class="sort fa fa-sort-amount-desc"></i>
            TagBuilder spanTag = new TagBuilder("span");
            TagBuilder iTag = new TagBuilder("i");
            
            if (sortOrder == "asc")
            {
                iTag.MergeAttribute("class", "fa fa-sort-asc");
            }
            else
            {
                iTag.MergeAttribute("class", "fa fa-sort-desc");
            }
            spanTag.InnerHtml += iTag.ToString();

            return new MvcHtmlString(spanTag.ToString());
        }
    }
}