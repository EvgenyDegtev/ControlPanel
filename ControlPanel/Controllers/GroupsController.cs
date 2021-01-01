﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.SessionState;
using System.Web.Mvc;
using ControlPanel.Models;
using PagedList;
using ControlPanel.Filters;
using NLog;
using System.Reflection;
using ControlPanel.Abstract;
using System.Threading.Tasks;
using ControlPanel.ViewModels;

namespace ControlPanel.Controllers
{
    [Authorize]
    [SessionState(SessionStateBehavior.Disabled)]
    public class GroupsController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        IGroupRepository repository;

        public GroupsController(IGroupRepository groupRepository)
        {
            this.repository = groupRepository;
        }

        //Get and Post
        [ErrorLogger]
        public async Task<ActionResult> Index(string searchString, int? page, string selectedSortProperty = "Name", string sortOrder = "asc")
        {
            logger.Info($"Action Start | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}| Input params: {nameof(searchString)}={searchString}, {nameof(page)}={page} ");

            int pageSize = 5;
            int pageNumber = page ?? 1;
            var groups = await repository.GetGroupsAsync();

            groups = SortGroups(groups, sortOrder, selectedSortProperty);

            GroupsIndexViewModel groupsIndexViewModel = new GroupsIndexViewModel
            {
                PagedGroups = groups.ToPagedList(pageNumber, pageSize),
                SearchString = searchString,
                SortOrder=sortOrder,
                SelectedSortProperty=selectedSortProperty
            };

            if (String.IsNullOrEmpty(searchString))
            {
                logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
                return View(groupsIndexViewModel);
            }
            groups = groups.Where(group => group.Name.Contains(searchString)).ToList();

            groupsIndexViewModel.PagedGroups = groups.ToPagedList(pageNumber, pageSize);
            logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
            return View(groupsIndexViewModel);
        }

        public async Task<ActionResult> AutocompleteSearch(string term)
        {
            var groups = await repository.SearchGroupsAsync(term);
            var logins = groups.Select(group => new { value = group.Name }).Take(5).OrderByDescending(row => row.value);
            return Json(logins, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [ErrorLogger]
        public ActionResult Create()
        {
            logger.Info($"Action Start | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
            logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
            return View();
        }

        [HttpPost]
        [ErrorLogger]
        public async Task<ActionResult> Create([Bind] Group group)
        {
            logger.Info($"Action Start | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name} | Input params: {nameof(group.Name)}={group.Name}, {nameof(group.Description)}={group.Description}");

            if (ModelState.IsValid)
            {
                repository.Create(group);
                await repository.SaveAsync();

                logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
                return RedirectToAction("Index");
            }

            logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
            return View(group);
        }

        [HttpGet]
        [ErrorLogger]
        public async Task<ActionResult> Delete(int? id)
        {
            logger.Info($"Action Start | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name} | Input params: {nameof(id)}={id}");
            if (id == null)
            {
                logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = await repository.FindGroupByIdAsync(id);
            if (group == null)
            {
                logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
                return HttpNotFound();
            }
            logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
            return View(group);
        }

        [HttpPost]
        [ErrorLogger]
        public async Task<ActionResult> Delete(int id)
        {
            logger.Info($"Action Start | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name} | Input params: {nameof(id)}={id}");
            await repository.DeleteAsync(id);
            await repository.SaveAsync();

            logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
            return RedirectToAction("Index");
        }

        [HttpGet]
        [ErrorLogger]
        public async Task<ActionResult> Edit(int? id)
        {
            logger.Info($"Action Start | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name} | Input params: {nameof(id)}={id}");
            if (id == null)
            {
                logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = await repository.FindGroupByIdAsync(id);
            if (group == null)
            {
                logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
                return HttpNotFound();
            }

            logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
            return View(group);
        }

        [HttpPost]
        [ErrorLogger]
        public async Task<ActionResult> Edit([Bind] Group group)
        {
            logger.Info($"Action Start | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name} | Input params: {nameof(group.Id)}={group.Id}, {nameof(group.Name)}={group.Name}, {nameof(group.Description)}={group.Description}");

            if (ModelState.IsValid)
            {
                repository.Update(group);
                await repository.SaveAsync();

                logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
                return RedirectToAction("Index");
            }
            logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
            return View(group);
        }

        [HttpGet]
        [ErrorLogger]
        public async Task<ActionResult> GroupAgents(int? id)
        {
            logger.Info($"Action Start | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name} | Input params: {nameof(id)}={id}");
            if (id == null)
            {
                logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = await repository.FindGroupByIdIncludeAgentsAsync(id);
            if (group == null)
            {
                logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
                return HttpNotFound();
            }
            logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
            return View(group);
        }

        [HttpGet]
        [ErrorLogger]
        public async Task<JsonResult> CheckNameUnique (string name, int? id)
        {
            logger.Info($"Action Start | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name} | Input params: {nameof(name)}={name}, {nameof(id)}={id}");
            var groupsAlreadyInDb = await repository.FindGroupsByNameAsync(name);

            if (groupsAlreadyInDb.Count() <= 0)
            {
                logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var modifiedGroup = groupsAlreadyInDb.First();
                //check name corresponds id
                if (modifiedGroup.Id == id)
                {
                    logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
                    return Json($"Группа с названием {name} уже существует", JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpGet]
        [ErrorLogger]
        public async Task<ActionResult> RemoveAgent (int groupId,int agentId)
        {
            var group = await repository.FindGroupByIdAsync(groupId);
            await repository.RemoveAgentFromGroupAsync(agentId);
            await repository.SaveAsync();

            return RedirectToAction("GroupAgents",group);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //db.Dispose();
            }
            base.Dispose(disposing);
        }

        private static List<Group> SortGroups(List<Group> groups, string sortOrder, string selectedSortProperty)
        {
            List<Group> sortedGroups = groups;
            if (sortOrder == "desc" && selectedSortProperty == nameof(Group.Name))
                sortedGroups = sortedGroups.OrderByDescending(group => group.Name).ToList();
            else if (sortOrder == "asc" && selectedSortProperty == nameof(Group.Description))
                sortedGroups = sortedGroups.OrderBy(group => group.Description).ToList();
            else if (sortOrder == "desc" && selectedSortProperty == nameof(Group.Description))
                sortedGroups = sortedGroups.OrderByDescending(group => group.Description).ToList();
            else
                sortedGroups = sortedGroups.OrderBy(group => group.Name).ToList();
            return sortedGroups;
        }
    }
}
