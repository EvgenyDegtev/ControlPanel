using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
        public async Task<ActionResult> Index(string searchString, int? page)
        {
            logger.Info($"Action Start | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}| Input params: {nameof(searchString)}={searchString}, {nameof(page)}={page} ");

            int pageSize = 5;
            int pageNumber = page ?? 1;
            //var skills2 = repository.Skills.ToList();
            var skills = await repository.GetSkillsAsync();
            if(String.IsNullOrEmpty(searchString))
            {
                logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
                return View(skills.ToPagedList(pageNumber,pageSize));
            }
            skills = await repository.SearchSkillsAsync(searchString);

            logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
            return View(skills.ToPagedList(pageNumber,pageSize));
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
        public ActionResult Create([Bind] Skill skill)
        {
            logger.Info($"Action Start | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name} | Input params: {nameof(skill.Name)}={skill.Name}, {nameof(skill.Key)}={skill.Key}, {nameof(skill.Algorithm)}={skill.Algorithm}");

            if (ModelState.IsValid)
            {
                repository.Create(skill);
                repository.Save();

                logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
                return RedirectToAction("Index");
            }

            logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
            return View(skill);
        }

        [HttpGet]
        [ErrorLogger]
        public ActionResult Delete (int? id)
        {
            logger.Info($"Action Start | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name} | Input params: {nameof(id)}={id}");
            if (id==null)
            {
                logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Skill skill = repository.FindSkillById((int)id);
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
        public ActionResult Delete(int id)
        {
            logger.Info($"Action Start | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name} | Input params: {nameof(id)}={id}");
            repository.Delete(id);
            repository.Save();
            logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
            return RedirectToAction("Index");
        }

        [HttpGet]
        [ErrorLogger]
        public ActionResult Edit (int? id)
        {
            logger.Info($"Action Start | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name} | Input params: {nameof(id)}={id}");
            if (id==null)
            {
                logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Skill skill = repository.FindSkillById((int)id);
            //Skill skill = db.Skills.Find(id);
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
        public ActionResult Edit ([Bind] Skill skill)
        {
            logger.Info($"Action Start | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name} | Input params: {nameof(skill.Id)}={skill.Id}, {nameof(skill.Name)}={skill.Name}, {nameof(skill.Key)}={skill.Key}, {nameof(skill.Algorithm)}={skill.Algorithm}");
            if (ModelState.IsValid)
            {
                repository.Update(skill);
                repository.Save();

                logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
                return RedirectToAction("Index");
            }
            logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
            return View(skill);
        }

        [HttpGet]
        [ErrorLogger]
        public JsonResult CheckKeyUnique(string key, int? id)
        {
            logger.Info($"Action Start | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name} | Input params: {nameof(key)}={key}, {nameof(id)}={id}");
            //var skillsAlreadyInDb = db.Skills.Where(skill => skill.Key == key);
            var skillsAlreadyInDb=repository.FindSkillsByKey(key);

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
        public ActionResult SkillRoutes(int? id)
        {
            if(id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Skill skill = db.Skills.Include(sk => sk.Routes).FirstOrDefault(sk => sk.Id == id);
            Skill skill = repository.FindSkillByIdIncludeRoutes((int)id);
            if(skill==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            return View(skill);
        }

        [HttpGet]
        [ErrorLogger]
        public ActionResult RemoveRoute(int id, int routeId)
        {
            Skill skill = repository.FindSkillByIdIncludeRoutes(id);
            Route routeToRemove=skill.Routes.Find(route=>route.Id==routeId);
            skill.Routes.Remove(routeToRemove);            
            repository.Update(skill);
            repository.Save();
          
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