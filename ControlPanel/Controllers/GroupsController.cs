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
        public async Task<ActionResult> Index(string searchString, int? page,string description, string selectedSortProperty = "Name", string sortOrder = "asc")
        {
            logger.Info($"Action Start | Controller name: {nameof(GroupsController)} | Action name: {nameof(Index)}| Input params: {nameof(searchString)}={searchString}, {nameof(page)}={page}," +
                $"{nameof(description)}={description}, {nameof(selectedSortProperty)}={selectedSortProperty}, {nameof(sortOrder)}={sortOrder} ");

            int pageSize = 5;
            int pageNumber = page ?? 1;
            var groups = await repository.GetGroupsAsync();

            groups = SortGroups(groups, sortOrder, selectedSortProperty);

            GroupsIndexViewModel groupsIndexViewModel = new GroupsIndexViewModel
            {
                SearchString = searchString,
                SortOrder=sortOrder,
                SelectedSortProperty=selectedSortProperty,
                Description=description
            };

            if (!String.IsNullOrEmpty(description))
            {
                var groupsWithDescription = groups.Where(group => group.Description != null).ToList();
                groups = groupsWithDescription.Where(group => group.Description.ToLower().Contains(description.ToLower())).ToList();
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                groups = groups.Where(group => group.Name.Contains(searchString)).ToList();
            }            

            groupsIndexViewModel.PagedGroups = groups.ToPagedList(pageNumber, pageSize);
            logger.Info($"Action End | Controller name: {nameof(GroupsController)} | Action name: {nameof(Index)}");
            return View(groupsIndexViewModel);
        }

        public async Task<ActionResult> AutocompleteSearch(string term)
        {
            logger.Info($"Action Start | Controller name: {nameof(GroupsController)} | Action name: {nameof(AutocompleteSearch)}| Input params: {nameof(term)}={term}");
            var groups = await repository.SearchGroupsAsync(term);
            var logins = groups.Select(group => new { value = group.Name }).Take(5).OrderByDescending(row => row.value);
            logger.Info($"Action End | Controller name: {nameof(GroupsController)} | Action name: {nameof(AutocompleteSearch)}");
            return Json(logins, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> AutocompleteDescription(string term)
        {
            logger.Info($"Action Start | Controller name: {nameof(GroupsController)} | Action name: {nameof(AutocompleteDescription)}| Input params: {nameof(term)}={term}");
            var groups = await repository.SearchGroupsByDescriptionAsync(term);
            var descriptions = groups.Select(group => new { value = group.Description }).Take(5).OrderByDescending(row => row.value);
            logger.Info($"Action End | Controller name: {nameof(GroupsController)} | Action name: {nameof(AutocompleteDescription)}");
            return Json(descriptions, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [ErrorLogger]
        public ActionResult Create()
        {
            logger.Info($"Action Start | Controller name: {nameof(GroupsController)} | Action name: {nameof(Create)}");
            logger.Info($"Action End | Controller name: {nameof(GroupsController)} | Action name: {nameof(Create)}");
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

                logger.Info($"Action End | Controller name: {nameof(GroupsController)} | Action name: {nameof(Create)}");
                return RedirectToAction("Index");
            }

            logger.Info($"Action End | Controller name: {nameof(GroupsController)} | Action name: {nameof(Create)}");
            return View(group);
        }

        [HttpGet]
        [ErrorLogger]
        public async Task<ActionResult> Delete(int? id)
        {
            logger.Info($"Action Start | Controller name: {nameof(GroupsController)} | Action name: {nameof(Delete)} | Input params: {nameof(id)}={id}");
            if (id == null)
            {
                logger.Info($"Action End | Controller name: {nameof(GroupsController)} | Action name: {nameof(Delete)}");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = await repository.FindGroupByIdAsync(id);
            if (group == null)
            {
                logger.Info($"Action End | Controller name: {nameof(GroupsController)} | Action name: {nameof(Delete)}");
                return HttpNotFound();
            }
            logger.Info($"Action End | Controller name: {nameof(GroupsController)} | Action name: {nameof(Delete)}");
            return View(group);
        }

        [HttpPost]
        [ErrorLogger]
        public async Task<ActionResult> Delete(int id)
        {
            logger.Info($"Action Start | Controller name: {nameof(GroupsController)} | Action name: {nameof(Delete)} | Input params: {nameof(id)}={id}");
            await repository.DeleteAsync(id);
            await repository.SaveAsync();

            logger.Info($"Action End | Controller name: {nameof(GroupsController)} | Action name: {nameof(Delete)}");
            return RedirectToAction("Index");
        }

        [HttpGet]
        [ErrorLogger]
        public async Task<ActionResult> Edit(int? id)
        {
            logger.Info($"Action Start | Controller name: {nameof(GroupsController)} | Action name: {nameof(Edit)} | Input params: {nameof(id)}={id}");
            if (id == null)
            {
                logger.Info($"Action End | Controller name: {nameof(GroupsController)} | Action name: {nameof(Edit)}");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = await repository.FindGroupByIdAsync(id);
            if (group == null)
            {
                logger.Info($"Action End | Controller name: {nameof(GroupsController)} | Action name: {nameof(Edit)}");
                return HttpNotFound();
            }

            logger.Info($"Action End | Controller name: {nameof(GroupsController)} | Action name: {nameof(Edit)}");
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

                logger.Info($"Action End | Controller name: {nameof(GroupsController)} | Action name: {nameof(Edit)}");
                return RedirectToAction("Index");
            }
            logger.Info($"Action End | Controller name: {nameof(GroupsController)} | Action name: {nameof(Edit)}");
            return View(group);
        }

        [HttpGet]
        [ErrorLogger]
        public async Task<ActionResult> GroupAgents(int? id)
        {
            logger.Info($"Action Start | Controller name: {nameof(GroupsController)} | Action name: {nameof(GroupAgents)} | Input params: {nameof(id)}={id}");
            if (id == null)
            {
                logger.Info($"Action End | Controller name: {nameof(GroupsController)} | Action name: {nameof(GroupAgents)}");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = await repository.FindGroupByIdIncludeAgentsAsync(id);
            if (group == null)
            {
                logger.Info($"Action End | Controller name: {nameof(GroupsController)}  | Action name:  {nameof(GroupAgents)}");
                return HttpNotFound();
            }
            logger.Info($"Action End | Controller name: {nameof(GroupsController)}  | Action name:  {nameof(GroupAgents)}");
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
                logger.Info($"Action End | Controller name: {nameof(GroupsController)} | Action name: {nameof(CheckNameUnique)}");
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var modifiedGroup = groupsAlreadyInDb.First();
                //check name corresponds id
                if (modifiedGroup.Id == id)
                {
                    logger.Info($"Action End | Controller name: {nameof(GroupsController)} | Action name: {nameof(CheckNameUnique)}");
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    logger.Info($"Action End | Controller name: {nameof(GroupsController)} | Action name: {nameof(CheckNameUnique)}");
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
            logger.Info($"Action End | Controller name: {nameof(GroupsController)} | Action name: {nameof(RemoveAgent)}");
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
