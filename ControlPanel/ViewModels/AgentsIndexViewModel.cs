using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using ControlPanel.Models;

namespace ControlPanel.ViewModels
{
    public class AgentsIndexViewModel
    {
        public IPagedList<Agent> PagedAgents { get; set; }

        public string SearchString { get; set; }

        public string SortOrder { get; set; }

        public string SelectedEntityName { get; set; }
    }
}