using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using ControlPanel.Models;
using System.Web.Mvc;

namespace ControlPanel.ViewModels
{
    public class SkillsIndexViewModel
    {
        public IPagedList<Skill> PagedSkills { get; set; }

        public int? Page { get; set; }

        public string SearchString { get; set; }

        public string SortOrder { get; set; } = "asc";

        public string SelectedSortProperty { get; set; } = "Name";

        public SelectList Algorithms { get; set; }

        public int? SelectedAlgorithmId { get; set; }
    }
}