using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ControlPanel.Abstract;
using ControlPanel.Models;
using ControlPanel.Controllers;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace ControlPanel.Concrete
{
    public class EFRouteRepository: IRouteRepository
    {
        private EFDbContext context = new EFDbContext();


        public IQueryable<Route> Routes
        {
            get 
            {
                var routes = context.Routes.AsQueryable();
                    return routes; 
            }
        }

        public async Task<List<Route>> GetRoutesAsync()
        {
            var routes = await context.Routes.ToListAsync();
            return routes;
        }

        public IQueryable<Skill> Skills
        {
            get
            {
                var skills = context.Skills.AsQueryable();
                return skills;
            }
        }

        public async Task<List<Skill>> GetSkillsAsync()
        {
            var skills = await context.Skills.ToListAsync();
            return skills;
        }

        public IQueryable<Route> RoutesIncludeSkills
        {
            get
            {
                var routes = context.Routes.Include(route => route.Skill).AsQueryable();
                return routes;
            }
        }

        public async Task<List<Route>> GetRoutesIncludeSkillsAsync()
        {
            var routes = await context.Routes.Include(route => route.Skill).ToListAsync();
            return routes;
        }

        public Route FindRouteById (int id)
        {
            var route = context.Routes.Find(id);
            return route;
        }

        public async Task<Route> FindRouteByIdAsync(int? id)
        {
            var route = await context.Routes.FindAsync(id);
            return route;
        }

        public async Task<Route> FindRouteByIdIncludeSkillAsync(int? id)
        {
            var route=await context.Routes.Include(rt => rt.Skill).FirstOrDefaultAsync(rt => rt.Id == id);
            return route;
        }

        public IQueryable<Route> FindRoutesByKey (string key)
        {
            var routes = context.Routes.Where(route => route.Key == key && route.IsActive == true);
            return routes;
        }

        public async Task<List<Route>> FindRoutesByKeyAsync(string key)
        {
            var routes = await context.Routes.Where(route => route.Key == key && route.IsActive == true).ToListAsync();
            return routes;
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public  async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }

        public void Create (Route route)
        {
            context.Routes.Add(route);
        }

        public void Update (Route route)
        {
            context.Entry(route).State = EntityState.Modified;
        }

        public void Delete (int id)
        {
            var route = context.Routes.Find(id);
            
            if(route!=null)
            {
                context.Routes.Remove(route);
            }
        }

        public async Task DeleteAsync(int id)
        {
            var route = await context.Routes.FindAsync(id);

            if (route != null)
            {
                context.Routes.Remove(route);
            }
        }
        public IQueryable<Route> SearchRoute (string searchString)
        {
            IQueryable<Route> routes = context.Routes.Where(route => route.Key.Contains(searchString) && route.IsActive == true);
            return routes;
        }

        public async Task<List<Route>> SearchRoutesAsync(string searchString)
        {
            List<Route> routes = await context.Routes.Where(route => route.Key.Contains(searchString) && route.IsActive == true).ToListAsync();
            return routes;
        }
    }
}