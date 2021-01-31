using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Web.Mvc;
using ControlPanel.Models;
using System.Data.Entity;
using System.Net;
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
    public class SkillsController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        ISkillRepository repository;

        public SkillsController (ISkillRepository skillRepository)
        {
            this.repository = skillRepository;
        }

        //Get and Post
        [ErrorLogger]
        [ActionStart]
        public async Task<ActionResult> Index(string searchString, int? page, int? selectedAlgorithmId, string selectedSortProperty = "Name", string sortOrder = "asc")
        {
            logger.Info($"Action Start | Controller name: {nameof(SkillsController)} | Action name: {nameof(Index)}| Input params: {nameof(searchString)}={searchString}, {nameof(page)}={page}," +
                $"{nameof(selectedAlgorithmId)}={selectedAlgorithmId}, {nameof(selectedSortProperty)}={selectedSortProperty}, {nameof(sortOrder)}={sortOrder} ");

            int pageSize = 5;
            int pageNumber = page ?? 1;
            var skills = await repository.GetSkillsAsync();

            skills = SortSkills(skills, sortOrder, selectedSortProperty);

            SkillsIndexViewModel skillsIndexViewModel = new SkillsIndexViewModel
            {
                SearchString=searchString,
                SortOrder=sortOrder,
                SelectedSortProperty=selectedSortProperty,
                Algorithms = new SelectList(Skill.algorithmDictionary.Select(algo => new { Algorithm = algo.Key.ToString(), AlgorithmNAme = algo.Value }), "Algorithm", "AlgorithmName"),
                SelectedAlgorithmId =selectedAlgorithmId
            };

            if(!String.IsNullOrEmpty(searchString))
            {
                skills = skills.Where(skill => skill.Key.Contains(searchString)).ToList();
            }

            if(selectedAlgorithmId!=null)
            {
                skills = skills.Where(skill => skill.Algorithm == selectedAlgorithmId).ToList();
            }
            


            skillsIndexViewModel.PagedSkills = skills.ToPagedList(pageNumber, pageSize);
            logger.Info($"Action End | Controller name: {nameof(SkillsController)} | Action name: {nameof(Index)}");
            return View(skillsIndexViewModel);
        }

        public async Task<ActionResult> AutocompleteSearch(string term)
        {
            logger.Info($"Action Start | Controller name: {nameof(SkillsController)} | Action name: {nameof(AutocompleteSearch)}| Input params: {nameof(term)}={term}");
            var skills = await repository.SearchSkillsAsync(term);
            var logins = skills.Select(skill => new { value = skill.Key }).Take(5).OrderByDescending(row => row.value);
            logger.Info($"Action End | Controller name: {nameof(SkillsController)} | Action name: {nameof(AutocompleteSearch)}");
            return Json(logins, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [ErrorLogger]
        public ActionResult Create()
        {
            logger.Info($"Action Start | Controller name: {nameof(SkillsController)} | Action name: {nameof(Create)}");

            SkillCreateViewModel skillCreateViewModel = new SkillCreateViewModel
            {
                Algorithms = new SelectList(Skill.algorithmDictionary.Select(algo => new { Algorithm = algo.Key.ToString(), AlgorithmName = algo.Value }), "Algorithm", "AlgorithmName", 0)
            };

            logger.Info($"Action End | Controller name: {nameof(SkillsController)} | Action name: {nameof(Create)}");
            return View(skillCreateViewModel);
        }

        [HttpPost]
        [ErrorLogger]
        public async Task<ActionResult> Create([Bind] Skill skill)
        {
            logger.Info($"Action Start | Controller name: {nameof(SkillsController)} | Action name: {nameof(Create)} | Input params: {nameof(skill.Name)}={skill.Name}, {nameof(skill.Key)}={skill.Key}, {nameof(skill.Algorithm)}={skill.Algorithm}");

            if (ModelState.IsValid)
            {
                repository.Create(skill);
                await repository.SaveAsync();

                logger.Info($"Action End | Controller name: {nameof(SkillsController)} | Action name: {nameof(Create)}");
                return RedirectToAction("Index");
            }

            logger.Info($"Action End | Controller name: {nameof(SkillsController)} | Action name: {nameof(Create)}");
            return View(skill);
        }

        [HttpGet]
        [ErrorLogger]
        public async Task<ActionResult> Delete (int? id)
        {
            logger.Info($"Action Start | Controller name: {nameof(SkillsController)} | Action name: {nameof(Delete)} | Input params: {nameof(id)}={id}");
            if (id==null)
            {
                logger.Info($"Action End | Controller name: {nameof(SkillsController)} | Action name: {nameof(Delete)}");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Skill skill = await repository.FindSkillByIdAsync(id);
            if(skill==null)
            {
                logger.Info($"Action End | Controller name: {nameof(SkillsController)} | Action name: {nameof(Delete)}");
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            logger.Info($"Action End | Controller name: {nameof(SkillsController)} | Action name: {nameof(Delete)}");
            return View(skill);
        }

        [HttpPost]
        [ErrorLogger]
        public async Task<ActionResult> Delete(int id)
        {
            logger.Info($"Action Start | Controller name: {nameof(SkillsController)} | Action name: {nameof(Delete)} | Input params: {nameof(id)}={id}");
            await repository.DeleteAsync(id);
            await repository.SaveAsync();
            logger.Info($"Action End | Controller name: {nameof(SkillsController)} | Action name: {nameof(Delete)}");
            return RedirectToAction("Index");
        }

        [HttpGet]
        [ErrorLogger]
        public async Task<ActionResult> Edit (int? id)
        {
            logger.Info($"Action Start | Controller name: {nameof(SkillsController)} | Action name: {nameof(Edit)} | Input params: {nameof(id)}={id}");
            if (id==null)
            {
                logger.Info($"Action End | Controller name: {nameof(SkillsController)} | Action name: {nameof(Edit)}");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Skill skill = await repository.FindSkillByIdAsync(id);
            if(skill==null)
            {
                logger.Info($"Action End | Controller name: {nameof(SkillsController)} | Action name: {nameof(Edit)}");
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            SkillCreateViewModel skillCreateViewModel = new SkillCreateViewModel
            {
                Skill = skill,
                Algorithms = new SelectList(Skill.algorithmDictionary.Select(algo => new { Algorithm = algo.Key.ToString(), AlgorithmName = algo.Value }), "Algorithm", "AlgorithmName", skill.Algorithm)
            };

            logger.Info($"Action End | Controller name: {nameof(SkillsController)} | Action name: {nameof(Edit)}");
            return View(skillCreateViewModel);
        }

        [HttpPost]
        [ErrorLogger]
        public async Task<ActionResult> Edit ([Bind] Skill skill)
        {
            logger.Info($"Action Start | Controller name: {nameof(SkillsController)} | Action name: {nameof(Edit)} | Input params: {nameof(skill.Id)}={skill.Id}, {nameof(skill.Name)}={skill.Name}, {nameof(skill.Key)}={skill.Key}, {nameof(skill.Algorithm)}={skill.Algorithm}");
            if (ModelState.IsValid)
            {
                repository.Update(skill);
                await repository.SaveAsync();

                logger.Info($"Action End | Controller name: {nameof(SkillsController)} | Action name: {nameof(Edit)}");
                return RedirectToAction("Index");
            }
            logger.Info($"Action End | Controller name: {nameof(SkillsController)} | Action name: {nameof(Edit)}");
            return View(skill);
        }

        [HttpGet]
        [ErrorLogger]
        public async Task<JsonResult> CheckKeyUnique(string key, int? id)
        {
            logger.Info($"Action Start | Controller name: {nameof(SkillsController)} | Action name: {nameof(CheckKeyUnique)} | Input params: {nameof(key)}={key}, {nameof(id)}={id}");
            var skillsAlreadyInDb=await repository.FindSkillsByKeyAsync(key);

            if (skillsAlreadyInDb.Count() <= 0)
            {
                logger.Info($"Action End | Controller name: {nameof(SkillsController)} | Action name: {nameof(CheckKeyUnique)}");
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var modifiedSkill = skillsAlreadyInDb.First();
                //check login corresponds id
                if (modifiedSkill.Id == id)
                {
                    logger.Info($"Action End | Controller name: {nameof(SkillsController)} | Action name: {nameof(CheckKeyUnique)}");
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    logger.Info($"Action End | Controller name: {nameof(SkillsController)} | Action name: {nameof(CheckKeyUnique)}");
                    return Json($"Навык с названием {key} уже существует", JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpGet]
        [ErrorLogger]
        public async Task<ActionResult> SkillRoutes(int? id)
        {
            logger.Info($"Action Start | Controller name: {nameof(SkillsController)} | Action name: {nameof(SkillRoutes)} | Input params: {nameof(id)}={id}");
            if (id==null)
            {
                logger.Info($"Action End | Controller name: {nameof(SkillsController)} | Action name: {nameof(SkillRoutes)}");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Skill skill = await repository.FindSkillByIdIncludeRoutesAsync(id);
            if(skill==null)
            {
                logger.Info($"Action End | Controller name: {nameof(SkillsController)} | Action name: {nameof(SkillRoutes)}");
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            logger.Info($"Action End | Controller name: {nameof(SkillsController)} | Action name: {nameof(SkillRoutes)}");
            return View(skill);
        }

        [HttpGet]
        [ErrorLogger]
        public async Task<ActionResult> RemoveRoute(int skillId, int routeId)
        {
            logger.Info($"Action Start | Controller name: {nameof(SkillsController)} | Action name: {nameof(RemoveRoute)} | Input params: {nameof(skillId)}={skillId}, {nameof(routeId)}={routeId}");
            Skill skill = await repository.FindSkillByIdIncludeRoutesAsync(skillId);
            Route routeToRemove=skill.Routes.Find(route=>route.Id==routeId);
            skill.Routes.Remove(routeToRemove);            
            repository.Update(skill);
            await repository.SaveAsync();

            logger.Info($"Action End | Controller name: {nameof(SkillsController)} | Action name: {nameof(RemoveRoute)}");
            return RedirectToAction("SkillRoutes", skill);
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                //db.Dispose();
            }
            base.Dispose(disposing);
        }

        private static List<Skill> SortSkills(List<Skill> skills, string sortOrder, string selectedSortProperty)
        {
            List<Skill> sortedSkills = skills;
            if (sortOrder == "desc" && selectedSortProperty == nameof(Skill.Name))
                sortedSkills = sortedSkills.OrderByDescending(skill => skill.Name).ToList();
            else if (sortOrder == "asc" && selectedSortProperty == nameof(Skill.Key))
                sortedSkills = sortedSkills.OrderBy(skill => skill.Key).ToList();
            else if (sortOrder == "desc" && selectedSortProperty == nameof(Skill.Key))
                sortedSkills = sortedSkills.OrderByDescending(skill => skill.Key).ToList();
            else if (sortOrder == "asc" && selectedSortProperty == nameof(Skill.Algorithm))
                sortedSkills = sortedSkills.OrderBy(skill => skill.Algorithm).ToList();
            else if (sortOrder == "desc" && selectedSortProperty == nameof(Skill.Algorithm))
                sortedSkills = sortedSkills.OrderByDescending(skill => skill.Algorithm).ToList();
            else
                sortedSkills = sortedSkills.OrderBy(skill => skill.Name).ToList();
            return sortedSkills;
        }
    }
}