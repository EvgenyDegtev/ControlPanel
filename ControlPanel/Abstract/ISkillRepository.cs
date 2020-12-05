using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControlPanel.Models;

namespace ControlPanel.Abstract
{
    public interface ISkillRepository       
    {
        //IDispose
        IQueryable<Skill> Skills { get; }

        Task<List<Skill>> GetSkillsAsync();

        List<Skill> GetSkillsFromSqlQuery();

        Skill FindSkillById(int? id);

        Task<Skill> FindSkillByIdAsync(int? id);

        Skill FindSkillByIdIncludeRoutes(int? id);

        Task<Skill> FindSkillByIdIncludeRoutesAsync(int? id);

        IQueryable<Skill> FindSkillsByKey(string key);

        Task<List<Skill>> FindSkillsByKeyAsync(string key);

        void Save();

        Task SaveAsync();

        void Create(Skill skill);

        void Update(Skill skill);

        void Delete(int id);

        Task DeleteAsync(int id);

        IQueryable<Skill> SearchSkill(string searchString);

        Task<List<Skill>> SearchSkillsAsync(string searchString);

        //void SaveSkill(Skill skill);        
    }
}
