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

        public int? Page { get; set; }

        public string SortOrder { get; set; } = "asc";

        public string SelectedSortProperty { get; set; } = "Name";

        public SelectList Groups { get; set; }

        public int? SelectedGroupId { get; set; }

        public SelectList Algorithms { get; set; }

        public int? SelectedAlgorithmId { get; set; }

        public SelectList IsServiceLevelList { get; set; }

        public bool? IsServiceLevel { get; set; }

        public static Dictionary<bool, string> IsServiceLevelListDictionary = new Dictionary<bool, string>
        {
            [true] = "Выбран",
            [false] = "Не выбран",
        };
    }
}