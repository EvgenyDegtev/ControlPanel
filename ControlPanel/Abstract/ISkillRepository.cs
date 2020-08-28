using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControlPanel.Models;

namespace ControlPanel.Abstract
{
    public interface ISkillRepository
        //IDispose
    {
        IQueryable<Skill> Skills { get; }

        Skill FindSkillById(int id);

        Skill FindSkillByIdIncludeRoutes(int id);

        IQueryable<Skill> FindSkillsByKey(string key);

        void Save();

        void Create(Skill skill);

        void Update(Skill skill);

        void Delete(int id);

        IQueryable<Skill> SearchSkill(string searchString);

        //void SaveSkill(Skill skill);        
    }
}
