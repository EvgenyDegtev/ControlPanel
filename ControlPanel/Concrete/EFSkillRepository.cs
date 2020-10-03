using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ControlPanel.Models;
using ControlPanel.Abstract;
using ControlPanel.Controllers;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace ControlPanel.Concrete
{
    public class EFSkillRepository : ISkillRepository
    {
        private EFDbContext context = new EFDbContext();

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
            List<Skill> skills = await context.Skills.ToListAsync();
            return skills;
        }

        public Skill FindSkillById(int? id)
        {
            var skill=context.Skills.Find(id);
            return skill;
        }

        public async Task<Skill> FindSkillByIdAsync(int? id)
        {
            var skill = await context.Skills.FindAsync(id);
            return skill;
        }

        public Skill FindSkillByIdIncludeRoutes(int? id)
        {
            Skill skill = context.Skills.Include(sk => sk.Routes).FirstOrDefault(sk => sk.Id == id);
            return skill;
        }

        public async Task<Skill> FindSkillByIdIncludeRoutesAsync(int? id)
        {
            Skill skill = await context.Skills.Include(sk => sk.Routes).FirstOrDefaultAsync(sk => sk.Id == id);
            return skill;
        }

        public IQueryable<Skill> FindSkillsByKey(string key)
        {
            IQueryable<Skill> skills = context.Skills.Where(skill => skill.Key == key);
            return skills;
        }

        public async Task<List<Skill>> FindSkillsByKeyAsync(string key)
        {
            List<Skill> skills = await context.Skills.Where(skill => skill.Key == key).ToListAsync();
            return skills;
        }

        public void Create (Skill skill)
        {
            context.Skills.Add(skill);
        }

        public void Update(Skill skill)
        {
            context.Entry(skill).State = EntityState.Modified;
        }

        public void Delete (int id)
        {
            var skill = context.Skills.Find(id);
            if(skill!=null)
            {
                context.Skills.Remove(skill);
            }
        }

        public async Task DeleteAsync(int id)
        {
            var skill = await context.Skills.FindAsync(id);
            if (skill != null)
            {
                context.Skills.Remove(skill);
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }

        public IQueryable<Skill> SearchSkill(string searchString)
        {
            IQueryable<Skill> skills = context.Skills.Where(skill => skill.Key.Contains(searchString));
            return skills;
        }

        public async Task<List<Skill>> SearchSkillsAsync(string searchString)
        {
            var skills = await context.Skills.Where(skill => skill.Key.Contains(searchString)).ToListAsync();
            return skills;
        }
























        //Dispose








        //public void SaveSkill (Skill skill)
        //{
        //    if(skill.Id==0)
        //    {
        //        context.Skills.Add(skill);
        //        context.SaveChanges();
        //    }
        //    else
        //    {
        //        var dbEntry = context.Skills.Find(skill.Id);
        //        if(dbEntry!=null)
        //        {
        //            context.Entry(skill).State = EntityState.Modified;
        //        }
        //        context.SaveChanges();
        //    }
        //}


    }
}