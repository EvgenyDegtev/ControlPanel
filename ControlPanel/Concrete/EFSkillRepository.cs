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
            get { return context.Skills; }
        }

        public void SaveSkill (Skill skill)
        {
            if(skill.Id==0)
            {
                context.Skills.Add(skill);
                context.SaveChanges();
            }
            else
            {
                var dbEntry = context.Skills.Find(skill.Id);
                if(dbEntry!=null)
                {
                    context.Entry(skill).State = EntityState.Modified;
                }
                context.SaveChanges();
            }
        }
    }
}