using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ControlPanel.Models;
using ControlPanel.Abstract;
using ControlPanel.Controllers;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;

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

        public Skill FindSkillById(int id)
        {
            var skill=context.Skills.Find(id);
            return skill;
        }

        public Skill FindSkillByIdIncludeRoutes(int id)
        {
            Skill skill = context.Skills.Include(sk => sk.Routes).FirstOrDefault(sk => sk.Id == id && sk.IsActive == true);
            return skill;
        }

        public IQueryable<Skill> FindSkillsByKey(string key)
        {
            IQueryable<Skill> skills = context.Skills.Where(skill => skill.Key == key && skill.IsActive == true);
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

        public void Save()
        {
            context.SaveChanges();
        }

        public IQueryable<Skill> SearchSkill(string searchString)
        {
            IQueryable<Skill> skills = context.Skills.Where(skill => skill.Key.Contains(searchString));
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