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
        public static MvcHtmlString PageNext(this HtmlHelper Html, int current, int size, int total)
        {
            if (total - ((size * current) + 1) <= 0)
            {
                return Html.ActionLink("Next", "IndexGrid",
                new { page = current + 1},
                new { @class = "btn btn-primary", disabled = "disabled" });
            }
            else
            {
                return Html.ActionLink("Next", "IndexGrid",
                new { page = current + 1},
                new { @class = "btn btn-primary" });
            }
        }

        public static MvcHtmlString PagePrev(this HtmlHelper Html, int current, int size, int total)
        {
            if (current < 2)
            {
                return Html.ActionLink("Previous", "IndexGrid",
                new { page = current - 1 },
                new { @class = "btn btn-primary", disabled = "disabled" });
            }
            else
            {
                return Html.ActionLink("Previous", "IndexGrid",
                new { page = current - 1},
                new { @class = "btn btn-primary" });
            }
        }

        // GET: Books/Details/5
       
    }
}