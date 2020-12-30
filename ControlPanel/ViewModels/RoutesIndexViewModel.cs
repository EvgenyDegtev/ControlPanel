using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ControlPanel.Models;
using PagedList;

namespace ControlPanel.ViewModels
{
    public class RoutesIndexViewModel
    {
        public IPagedList<Route> PagedRoutes { get; set; }

        public string SearchString { get; set; }

        public string SortOrder { get; set; }

        public string SelectedSortProperty { get; set; }
    }
}