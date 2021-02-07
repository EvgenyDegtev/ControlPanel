using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using ControlPanel.Models;

namespace ControlPanel.ViewModels
{
    public class GroupsIndexViewModel
    {
        public IPagedList<Group> PagedGroups { get; set; }

        public int? Page { get; set; }

        public string SearchString { get; set; }

        public string SortOrder { get; set; } = "asc";

        public string SelectedSortProperty { get; set; } = "Name";

        public string Description { get; set; } 
    }
}