using System;
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
    [ActionLogger]
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
        public async Task<ActionResult> Index([Bind] GroupsIndexViewModel groupsIndexModel)
        {
            logger.Info($"Action Start | Controller name: {nameof(GroupsController)} | Action name: {nameof(Index)}| Input params: {nameof(groupsIndexModel.SearchString)}={groupsIndexModel.SearchString}, " +
                $"{nameof(groupsIndexModel.Page)}={groupsIndexModel.Page}, {nameof(groupsIndexModel.Description)}={groupsIndexModel.Description}, " +
                $"{nameof(groupsIndexModel.SelectedSortProperty)}={groupsIndexModel.SelectedSortProperty}, {nameof(groupsIndexModel.SortOrder)}={groupsIndexModel.SortOrder} ");

            int pageSize = 5;
            int pageNumber = groupsIndexModel.Page ?? 1;
            var groups = await repository.GetGroupsAsync();

            groups = SortGroups(groups, groupsIndexModel.SortOrder, groupsIndexModel.SelectedSortProperty);

            GroupsIndexViewModel groupsIndexViewModel = new GroupsIndexViewModel
            {
                SearchString = groupsIndexModel.SearchString,
                SortOrder=groupsIndexModel.SortOrder,
                SelectedSortProperty=groupsIndexModel.SelectedSortProperty,
                Description=groupsIndexModel.Description
            };

            groups = FilterGroups(groups, groupsIndexModel.SearchString, groupsIndexModel.Description);

            groupsIndexViewModel.PagedGroups = groups.ToPagedList(pageNumber, pageSize);
            return View(groupsIndexViewModel);
        }

        public async Task<ActionResult> AutocompleteSearch(string term)
        {
            logger.Info($"Action Start | Controller name: {nameof(GroupsController)} | Action name: {nameof(AutocompleteSearch)}| Input params: {nameof(term)}={term}");
            var groups = await repository.SearchGroupsAsync(term);
            var logins = groups.Select(group => new { value = group.Name }).Take(5).OrderByDescending(row => row.value);
            return Json(logins, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> AutocompleteDescription(string term)
        {
            logger.Info($"Action Start | Controller name: {nameof(GroupsController)} | Action name: {nameof(AutocompleteDescription)}| Input params: {nameof(term)}={term}");
            var groups = await repository.SearchGroupsByDescriptionAsync(term);
            var descriptions = groups.Select(group => new { value = group.Description }).Take(5).OrderByDescending(row => row.value);
            return Json(descriptions, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [ErrorLogger]
        public ActionResult Create()
        {
            logger.Info($"Action Start | Controller name: {nameof(GroupsController)} | Action name: {nameof(Create)}");
            return View();
        }

        [HttpPost]
        [ErrorLogger]
        public async Task<ActionResult> Create([Bind] Group group)
        {
            logger.Info($"Action Start | Controller name: {nameof(GroupsController)} | Action name: {nameof(Create)} | Input params: {nameof(group.Name)}={group.Name}, {nameof(group.Description)}={group.Description}");

            if (ModelState.IsValid)
            {
                repository.Create(group);
                await repository.SaveAsync();
                return RedirectToAction("Index");
            }
            return View(group);
        }

        [HttpGet]
        [ErrorLogger]
        public async Task<ActionResult> Delete(int? id)
        {
            logger.Info($"Action Start | Controller name: {nameof(GroupsController)} | Action name: {nameof(Delete)} | Input params: {nameof(id)}={id}");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = await repository.FindGroupByIdAsync(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        [HttpPost]
        [ErrorLogger]
        public async Task<ActionResult> Delete(int id)
        {
            logger.Info($"Action Start | Controller name: {nameof(GroupsController)} | Action name: {nameof(Delete)} | Input params: {nameof(id)}={id}");
            await repository.DeleteAsync(id);
            await repository.SaveAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        [ErrorLogger]
        public async Task<ActionResult> Edit(int? id)
        {
            logger.Info($"Action Start | Controller name: {nameof(GroupsController)} | Action name: {nameof(Edit)} | Input params: {nameof(id)}={id}");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = await repository.FindGroupByIdAsync(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        [HttpPost]
        [ErrorLogger]
        public async Task<ActionResult> Edit([Bind] Group group)
        {
            logger.Info($"Action Start | Controller name: {nameof(GroupsController)} | Action name: {nameof(Edit)} | Input params: {nameof(group.Id)}={group.Id}, {nameof(group.Name)}={group.Name}, {nameof(group.Description)}={group.Description}");

            if (ModelState.IsValid)
            {
                repository.Update(group);
                await repository.SaveAsync();
                return RedirectToAction("Index");
            }
            return View(group);
        }

        [HttpGet]
        [ErrorLogger]
        public async Task<ActionResult> GroupAgents(int? id)
        {
            logger.Info($"Action Start | Controller name: {nameof(GroupsController)} | Action name: {nameof(GroupAgents)} | Input params: {nameof(id)}={id}");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = await repository.FindGroupByIdIncludeAgentsAsync(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        [HttpGet]
        [ErrorLogger]
        public async Task<JsonResult> CheckNameUnique (string name, int? id)
        {
            logger.Info($"Action Start | Controller name: {nameof(GroupsController)} | Action name: {nameof(CheckNameUnique)} | Input params: {nameof(name)}={name}, {nameof(id)}={id}");
            var groupsAlreadyInDb = await repository.FindGroupsByNameAsync(name);

            if (groupsAlreadyInDb.Count() <= 0)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var modifiedGroup = groupsAlreadyInDb.First();
                //check name corresponds id
                if (modifiedGroup.Id == id)
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json($"Группа с названием {name} уже существует", JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpGet]
        [ErrorLogger]
        public async Task<ActionResult> RemoveAgent (int groupId,int agentId)
        {
            logger.Info($"Action Start | Controller name: {nameof(GroupsController)} | Action name: {nameof(RemoveAgent)} | Input params: {nameof(groupId)}={groupId}, {nameof(agentId)}={agentId}");
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

        private static List<Group> FilterGroups(List<Group> groups, string searchString, string description)
        {
            List<Group> filtredGroups = groups;
            if (!String.IsNullOrEmpty(description))
            {
                var groupsWithDescription = filtredGroups.Where(group => group.Description != null).ToList();
                filtredGroups = groupsWithDescription.Where(group => group.Description.ToLower().Contains(description.ToLower())).ToList();
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                filtredGroups = filtredGroups.Where(group => group.Name.ToLower().Contains(searchString.ToLower())).ToList();
            }
            return filtredGroups;
        }
    }
}
