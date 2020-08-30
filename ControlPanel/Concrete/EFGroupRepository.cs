using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using ControlPanel.Abstract;
using ControlPanel.Models;

namespace ControlPanel.Concrete
{
    public class EFGroupRepository: IGroupRepository
    {
        private EFDbContext context = new EFDbContext();

        public IQueryable<Models.Group> Groups
        {
            get
            {
                var groups = context.Groups;
                return groups;
            }
        }

        public Models.Group FindGroupById (int id)
        {
            var group = context.Groups.Find(id);
            return group;
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Create (Models.Group group)
        {
            context.Groups.Add(group);
        }

        public void Update(Models.Group group)
        {
            context.Entry(group).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            var group = context.Groups.Find(id);
            if(group!=null)
            {
                context.Groups.Remove(group);
            }            
        }

        public IQueryable<Models.Group> SearchGroup(string searchString)
        {
            IQueryable<Models.Group> groups = context.Groups.Where(gr => gr.Name.Contains(searchString));
            return groups;
        }
    }
}