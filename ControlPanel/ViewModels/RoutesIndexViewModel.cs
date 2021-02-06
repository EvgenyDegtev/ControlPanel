using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ControlPanel.Models;
using PagedList;
using System.Web.Mvc;

namespace ControlPanel.ViewModels
{
    public class RoutesIndexViewModel
    {
        public IPagedList<Route> PagedRoutes { get; set; }

        public int? Page { get; set; }

        public string SearchString { get; set; }

        public string SortOrder { get; set; } = "asc";

        public string SelectedSortProperty { get; set; } = "Name";

        public SelectList Skills { get; set; }

        public int? SelectedSkillId { get; set; }
    }
}