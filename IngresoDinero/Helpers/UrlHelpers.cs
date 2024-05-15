using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Hosting;
using System.Web.Mvc;

namespace IngresoDinero.Helpers
{
    public static class UrlHelpers
    {
        public static string AbsoluteAction(this UrlHelper url, string actionName, string controllerName, object routeValues = null)
        {
            var httpContext = url.RequestContext.HttpContext;
            string scheme = httpContext.Request.Url.Scheme;

            return url.Action(
                actionName,
                controllerName,
                routeValues,
                scheme
            );
        }

        public static string VersionedContent(this UrlHelper url, string rootRelativePath)
        {
            string absolute = HostingEnvironment.MapPath(rootRelativePath);
            DateTime date = File.GetLastWriteTime(absolute);
            return url.Content(rootRelativePath) + "?v=" + Convert.ToString(date.Ticks, 16);
        }
    }
}