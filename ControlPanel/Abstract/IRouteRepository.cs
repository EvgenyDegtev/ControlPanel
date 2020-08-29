using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ControlPanel.Models;

namespace ControlPanel.Abstract
{
    public interface IRouteRepository
    {
        //Dispose

        IQueryable<Route> Routes { get; }

        IQueryable<Skill> Skills { get; }

        IQueryable<Route> RoutesIncludeSkills { get; }

        Route FindRouteById(int id);

        //FindRouteInclude 

        IQueryable<Route> FindRoutesByKey(string key);

        void Save();

        void Create(Route route);

        void Update(Route route);

        void Delete(int id);

        IQueryable<Route> SearchRoute(string searchString);
    }
}