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

        public string SearchString { get; set; }

        public string SortOrder { get; set; }

        public string SelectedSortProperty { get; set; }
    }
}