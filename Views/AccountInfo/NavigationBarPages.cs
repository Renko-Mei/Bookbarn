using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;


namespace BookBarn.Views.AccountInfo
{
    public static class NavigationBarPages
    {
        public static string ActivePageKey => "ActivePage";

        public static string Index => "Index";

        public static string ChangePassword => "ChangePassword";
        
        public static string SalesVisualization => "SalesVisualization";

        public static string OrderHistory => "OrderHistory";

        public static string ChangeAddress => "ChangeAddress";

        public static string IndexNavClass(ViewContext viewContext) => PageNavClass(viewContext, Index);

        public static string ChangePasswordNavClass(ViewContext viewContext) => PageNavClass(viewContext, ChangePassword);

        public static string ChangeAddressNavClass(ViewContext viewContext)=> PageNavClass(viewContext, ChangeAddress);
        public static string SeeSaleHistoryNavClass(ViewContext viewContext) => PageNavClass(viewContext, SalesVisualization);   

        public static string OrderHistoryNavClass(ViewContext viewContext) => PageNavClass(viewContext, OrderHistory);

        public static string PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string;
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }

        public static void AddActivePage(this ViewDataDictionary viewData, string activePage) => viewData[ActivePageKey] = activePage;
    }
}
