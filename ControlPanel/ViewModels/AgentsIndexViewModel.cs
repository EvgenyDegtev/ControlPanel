using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using ControlPanel.Models;
using System.Web.Mvc;

namespace ControlPanel.ViewModels
{
    public class AgentsIndexViewModel
    {
        public IPagedList<Agent> PagedAgents { get; set; }

        public string SearchString { get; set; }

        public string SortOrder { get; set; }

        public string SelectedSortProperty { get; set; }

        public SelectList Groups { get; set; }

        public int? SelectedGroupId { get; set; }

        public SelectList Algorithms { get; set; }

        public int? SelectedAlgorithmId { get; set; }
    }
}