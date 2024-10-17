using System.Web;
using System.Web.Mvc;

namespace _23DH110152_Shopbanacc
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
