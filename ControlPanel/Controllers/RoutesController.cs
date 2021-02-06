using ControlPanel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Web.Mvc;
using System.Data.Entity;
using System.Net;
using PagedList;
using ControlPanel.Filters;
using NLog;
using System.Reflection;
using ControlPanel.Infastructure;
using ControlPanel.Abstract;
using System.Threading.Tasks;
using ControlPanel.ViewModels;

namespace ControlPanel.Controllers
{
    [Authorize]
    [SessionState(SessionStateBehavior.Disabled)]
    [ActionEnd]
    public class RoutesController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        IRouteRepository repository;

        public RoutesController(IRouteRepository routeRepository)
        {
            this.repository = routeRepository;
        }
        

        //Get and Post
        [ErrorLogger]
        public async Task<ActionResult> Index(/*string searchString, int? selectedSkillId, int? page, string selectedSortProperty = "Name", string sortOrder = "asc"*/ [Bind] RoutesIndexViewModel routesIndexView)
        {
            logger.Info($"Action Start | Controller name: {nameof(RoutesController)} | Action name: {nameof(Index)}| Input params: {nameof(routesIndexView.SearchString)}={routesIndexView.SearchString}, " +
                $"{nameof(routesIndexView.SelectedSkillId)}={routesIndexView.SelectedSkillId}, {nameof(routesIndexView.Page)}={routesIndexView.Page}, " +
                $"{nameof(routesIndexView.SelectedSortProperty)}={routesIndexView.SelectedSortProperty}, " +
                $"{nameof(routesIndexView.SortOrder)}={routesIndexView.SortOrder}");

            int pageSize = 5;
            int pageNumber = routesIndexView.Page ?? 1;
            var routes = await repository.GetRoutesIncludeSkillsAsync();

            routes = SortRoutes(routes, routesIndexView.SortOrder, routesIndexView.SelectedSortProperty);

            RoutesIndexViewModel routesIndexViewModel = new RoutesIndexViewModel
            {
                SearchString= routesIndexView.SearchString,
                Page= routesIndexView.Page,
                SortOrder= routesIndexView.SortOrder,
                SelectedSortProperty=routesIndexView.SelectedSortProperty,
                Skills= new SelectList(await repository.GetSkillsAsync(), "Id", "Name", routesIndexView.SelectedSkillId),
                SelectedSkillId= routesIndexView.SelectedSkillId
            };

            routes = FilterRoutes(routes, routesIndexView.SelectedSkillId, routesIndexView.SearchString);

            routesIndexViewModel.PagedRoutes = routes.ToPagedList(pageNumber, pageSize);
            return View(routesIndexViewModel);
        }

        public async Task<ActionResult> AutocompleteSearch(string term)
        {
            logger.Info($"Action Start | Controller name: {nameof(RoutesController)} | Action name: {nameof(AutocompleteSearch)}| Input params: {nameof(term)}={term}");
            var routes = await repository.SearchRoutesAsync(term);
            var logins = routes.Select(route => new { value = route.Key }).Take(5).OrderByDescending(row => row.value);
            return Json(logins, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [ErrorLogger]
        public async Task<ActionResult> Create ()
        {
            logger.Info($"Action Start | Controller name: {nameof(RoutesController)} | Action name: {nameof(Create)}");
           
            RouteCreateViewModel routeCreateViewModel = new RouteCreateViewModel
            {
                Skills = new SelectList(await repository.GetSkillsAsync(), "Id", "Name")
            };
            return View(routeCreateViewModel);
        }

        [HttpPost]
        [ErrorLogger]
        public async Task<ActionResult> Create( [Bind] Route route)
        {
            logger.Info($"Action Start | Controller name: {nameof(RoutesController)} | Action name: {nameof(Create)} | Input params: {nameof(route.Name)}={route.Name}, {nameof(route.Key)}={route.Key}, {nameof(route.SkillId)}={route.SkillId}");

            if (ModelState.IsValid)
            {
                repository.Create(route);
                await repository.SaveAsync();
                return RedirectToAction("Index");
            }
            return View(route);
        }
        
        [HttpGet]
        [ErrorLogger]
        public async Task<ActionResult> Delete(int? id)
        {
            logger.Info($"Action Start | Controller name: {nameof(RoutesController)} | Action name: {nameof(Delete)} | Input params: {nameof(id)}={id}");
            if (id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Route route = await repository.FindRouteByIdIncludeSkillAsync(id);
            if(route==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            return View(route);
        }

        [HttpPost]
        [ErrorLogger]
        public async Task<ActionResult> Delete(int id)
        {
            logger.Info($"Action Start | Controller name: {nameof(RoutesController)} | Action name: {nameof(Delete)} | Input params: {nameof(id)}={id}");

            await repository.DeleteAsync(id);
            await repository.SaveAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [ErrorLogger]
        public async Task<ActionResult> Edit(int? id)
        {
            logger.Info($"Action Start | Controller name: {nameof(RoutesController)} | Action name: {nameof(Edit)} | Input params: {nameof(id)}={id}");
            if (id==null)
            {
                logger.Info($"Action End | Controller name: {nameof(RoutesController)} | Action name: {nameof(Edit)}");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Route route = await repository.FindRouteByIdAsync(id);
            if(route==null)
            {
                logger.Info($"Action End | Controller name: {nameof(RoutesController)} | Action name: {nameof(Edit)}");
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            RouteCreateViewModel routeCreateViewModel = new RouteCreateViewModel
            {
                Route = route,
                Skills = new SelectList(await repository.GetSkillsAsync(), "Id", "Name")
            };
            return View(routeCreateViewModel);
        }

        [HttpPost]
        [ErrorLogger]
        public async Task<ActionResult> Edit([Bind] Route route)
        {
            logger.Info($"Action Start | Controller name: {nameof(RoutesController)} | Action name: {nameof(Edit)} | Input params: {nameof(route.Name)}={route.Name}, {nameof(route.Key)}={route.Key}, {nameof(route.SkillId)}={route.SkillId}");

            if (ModelState.IsValid)
            {
                repository.Update(route);
                await repository.SaveAsync();
                return RedirectToAction("Index");
            }
            return View(route);
        }

        [HttpGet]
        [ErrorLogger]
        public async Task<JsonResult> CheckKeyUnique (string key, int? id)
        {
            logger.Info($"Action Start | Controller name: {nameof(RoutesController)} | Action name: {nameof(CheckKeyUnique)} | Input params: {nameof(key)}={key}, {nameof(id)}={id}");
            var routesAlreadyInDb = await repository.FindRoutesByKeyAsync(key);

            if (routesAlreadyInDb.Count() <= 0)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var modifiedRoute = routesAlreadyInDb.First();
                //check key corresponds id
                if (modifiedRoute.Id == id)
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json($"Маршрут с названием {key} уже существует", JsonRequestBehavior.AllowGet);
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                //db.Dispose();
            }
            base.Dispose(disposing);
        }

        private static List<Route> SortRoutes(List<Route> routes, string sortOrder, string selectedSortProperty)
        {
            List<Route> sortedRoutes = routes;
            if (sortOrder == "desc" && selectedSortProperty == nameof(Route.Name))
                sortedRoutes = sortedRoutes.OrderByDescending(route => route.Name).ToList();
            else if (sortOrder == "asc" && selectedSortProperty == nameof(Route.Key))
                sortedRoutes = sortedRoutes.OrderBy(route => route.Key).ToList();
            else if (sortOrder == "desc" && selectedSortProperty == nameof(Route.Key))
                sortedRoutes = sortedRoutes.OrderByDescending(route => route.Key).ToList();
            else if (sortOrder == "asc" && selectedSortProperty == nameof(Route.Skill))
                sortedRoutes = sortedRoutes.OrderBy(route => route?.Skill?.Name ?? null).ToList();
            else if (sortOrder == "desc" && selectedSortProperty == nameof(Route.Skill))
                sortedRoutes = sortedRoutes.OrderByDescending(route => route?.Skill?.Name??null).ToList();
            else
                sortedRoutes = sortedRoutes.OrderBy(route => route.Name).ToList();
            return sortedRoutes;
        }

        private static List<Route> FilterRoutes(List<Route> routes, int? selectedSkillId, string searchString)
        {
            List<Route> filtredRoutes = routes;

            if (selectedSkillId != null)
            {
                filtredRoutes = filtredRoutes.Where(route => route.SkillId == selectedSkillId).ToList();
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                filtredRoutes = filtredRoutes.Where(route => route.Key.ToLower().Contains(searchString.ToLower())).ToList();
            }

            return filtredRoutes;
        }
    }
}