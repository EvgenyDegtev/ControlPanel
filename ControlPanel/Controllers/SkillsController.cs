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
        public async Task<ActionResult> Index(string searchString, int? page)
        {
            logger.Info($"Action Start | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}| Input params: {nameof(searchString)}={searchString}, {nameof(page)}={page} ");

            int pageSize = 5;
            int pageNumber = page ?? 1;
            var skills = await repository.GetSkillsAsync();
            if(String.IsNullOrEmpty(searchString))
            {
                logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
                return View(skills.ToPagedList(pageNumber,pageSize));
            }
            skills = await repository.SearchSkillsAsync(searchString);


            ViewBag.searchString = searchString;

            logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
            return View(skills.ToPagedList(pageNumber,pageSize));
        }

        [HttpGet]
        [ErrorLogger]
        public ActionResult Create()
        {
            logger.Info($"Action Start | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");

            var algorithm1 = new { Algorithm = 0, AlgorithmName = "Наиболее свободный без учета навыка" };
            var algorithm2 = new { Algorithm = 1, AlgorithmName = "Наиболее свободный с учетом навыка" };
            var algorithm3 = new { Algorithm = 2, AlgorithmName = "Наименее занятый без учета навыка" };
            var algorithm4 = new { Algorithm = 3, AlgorithmName = "Наименее занятый с учетом навыка" };
            var algorithms = new[] { algorithm1, algorithm2, algorithm3, algorithm4 };
            ViewBag.Algorithms = new SelectList(algorithms, "Algorithm", "AlgorithmName",0);

            logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
            return View();
        }

        [HttpPost]
        [ErrorLogger]
        public async Task<ActionResult> Create([Bind] Skill skill)
        {
            logger.Info($"Action Start | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name} | Input params: {nameof(skill.Name)}={skill.Name}, {nameof(skill.Key)}={skill.Key}, {nameof(skill.Algorithm)}={skill.Algorithm}");

            if (ModelState.IsValid)
            {
                repository.Create(skill);
                await repository.SaveAsync();

                logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
                return RedirectToAction("Index");
            }

            logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
            return View(skill);
        }

        [HttpGet]
        [ErrorLogger]
        public async Task<ActionResult> Delete (int? id)
        {
            logger.Info($"Action Start | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name} | Input params: {nameof(id)}={id}");
            if (id==null)
            {
                logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Skill skill = await repository.FindSkillByIdAsync(id);
            if(skill==null)
            {
                logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
            return View(skill);
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
        public async Task<ActionResult> Edit (int? id)
        {
            logger.Info($"Action Start | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name} | Input params: {nameof(id)}={id}");
            if (id==null)
            {
                logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Skill skill = await repository.FindSkillByIdAsync(id);
            //Skill skill = db.Skills.Find(id);
            if(skill==null)
            {
                logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            var algorithm1 = new { Algorithm = 0, AlgorithmName = "Наиболее свободный без учета навыка" };
            var algorithm2 = new { Algorithm = 1, AlgorithmName = "Наиболее свободный с учетом навыка" };
            var algorithm3 = new { Algorithm = 2, AlgorithmName = "Наименее занятый без учета навыка" };
            var algorithm4 = new { Algorithm = 3, AlgorithmName = "Наименее занятый с учетом навыка" };
            var algorithms = new[] { algorithm1, algorithm2, algorithm3, algorithm4 };
            ViewBag.Algorithms = new SelectList(algorithms, "Algorithm", "AlgorithmName", skill.Algorithm);

            logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
            return View(skill);
        }

        [HttpPost]
        [ErrorLogger]
        public async Task<ActionResult> Edit ([Bind] Skill skill)
        {
            logger.Info($"Action Start | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name} | Input params: {nameof(skill.Id)}={skill.Id}, {nameof(skill.Name)}={skill.Name}, {nameof(skill.Key)}={skill.Key}, {nameof(skill.Algorithm)}={skill.Algorithm}");
            if (ModelState.IsValid)
            {
                repository.Update(skill);
                await repository.SaveAsync();

                logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
                return RedirectToAction("Index");
            }
            logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
            return View(skill);
        }

        [HttpGet]
        [ErrorLogger]
        public async Task<JsonResult> CheckKeyUnique(string key, int? id)
        {
            logger.Info($"Action Start | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name} | Input params: {nameof(key)}={key}, {nameof(id)}={id}");
            var skillsAlreadyInDb=await repository.FindSkillsByKeyAsync(key);

            if (skillsAlreadyInDb.Count() <= 0)
            {
                logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var modifiedSkill = skillsAlreadyInDb.First();
                //check login corresponds id
                if (modifiedSkill.Id == id)
                {
                    logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
                    return Json($"Навык с названием {key} уже существует", JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpGet]
        [ErrorLogger]
        public async Task<ActionResult> SkillRoutes(int? id)
        {
            if(id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Skill skill = await repository.FindSkillByIdIncludeRoutesAsync(id);
            if(skill==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            return View(skill);
        }

        [HttpGet]
        [ErrorLogger]
        public async Task<ActionResult> RemoveRoute(int skillId, int routeId)
        {
            Skill skill = await repository.FindSkillByIdIncludeRoutesAsync(skillId);
            Route routeToRemove=skill.Routes.Find(route=>route.Id==routeId);
            skill.Routes.Remove(routeToRemove);            
            repository.Update(skill);
            await repository.SaveAsync();
          
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

    }
}