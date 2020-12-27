﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using ControlPanel.Models;

namespace ControlPanel.ViewModels
{
    public class SkillsIndexViewModel
    {
        public IPagedList<Skill> PagedSkills { get; set; }

        public string SearchString { get; set; }
    }
}