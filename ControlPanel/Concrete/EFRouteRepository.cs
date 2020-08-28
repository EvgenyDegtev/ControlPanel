using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ControlPanel.Abstract;
using ControlPanel.Models;
using ControlPanel.Controllers;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace ControlPanel.Concrete
{
    public class EFRouteRepository: IRouteRepository
    {
        private EFDbContext context = new EFDbContext();


        public IQueryable<Route> Routes
        {
            get {
                var routes = context.Routes.AsQueryable();
                    return routes; }
        }

        public Route FindRouteById (int id)
        {
            var route = context.Routes.Find(id);
            return route;
        }

        public IQueryable<Route> FindRoutesByKey (string key)
        {
            var routes = context.Routes.Where(route => route.Key == key && route.IsActive == true);
            return routes;
        }

        public void Save()
        {
            context.SaveChanges();
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
        public IQueryable<Route> SearchRoute (string searchString)
        {
            IQueryable<Route> routes = context.Routes.Where(route => route.Key == searchString && route.IsActive == true);
            return routes;
        }
    }
}