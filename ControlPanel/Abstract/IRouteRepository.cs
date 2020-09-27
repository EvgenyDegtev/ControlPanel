using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ControlPanel.Models;
using System.Threading.Tasks;

namespace ControlPanel.Abstract
{
    public interface IRouteRepository
    {
        //Dispose

        IQueryable<Route> Routes { get; }

        Task<List<Route>> GetRoutesAsync();

        IQueryable<Skill> Skills { get; }

        Task<List<Skill>> GetSkillsAsync();

        IQueryable<Route> RoutesIncludeSkills { get; }

        Task<List<Route>> GetRoutesIncludeSkillsAsync();

        Route FindRouteById(int id);

        Task<Route> FindRouteByIdAsync(int? id);

        Task<Route> FindRouteByIdIncludeSkillAsync(int? id);

        IQueryable<Route> FindRoutesByKey(string key);

        Task<List<Route>> FindRoutesByKeyAsync(string key);

        void Save();

        Task SaveAsync();

        void Create(Route route);

        void Update(Route route);

        void Delete(int id);

        Task DeleteAsync(int id);

        IQueryable<Route> SearchRoute(string searchString);

        Task<List<Route>> SearchRoutesAsync(string searchString);
    }
}