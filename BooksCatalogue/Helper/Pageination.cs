using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace BooksCatalogue.Helper
{
    public  static class Pageination
    {
        public static MvcHtmlString PageNext(this HtmlHelper Html, int page, int size, int count)
        {
            if (count - ((size * page) + 1) <= 0)
            {
                return Html.ActionLink("Next", "IndexGrid",
                new { page = page + 1},
                new { @class = "btn btn-primary", disabled = "disabled" });
            }
            else
            {
                return Html.ActionLink("Next", "IndexGrid",
                new { page = page + 1},
                new { @class = "btn btn-primary" });
            }
        }

        public static MvcHtmlString PagePrev(this HtmlHelper Html, int page, int size, int count)
        {
            if (page < 2)
            {
                return Html.ActionLink("Previous", "IndexGrid",
                new { page = page - 1 },
                new { @class = "btn btn-primary", disabled = "disabled" });
            }
            else
            {
                return Html.ActionLink("Previous", "IndexGrid",
                new { page = page - 1},
                new { @class = "btn btn-primary"});
            }
        }

       
    }
}