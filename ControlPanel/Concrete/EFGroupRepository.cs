using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using ControlPanel.Abstract;
using ControlPanel.Models;
using System.Threading.Tasks;

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

        public async Task<List<Models.Group>> GetGroupsAsync()
        {
            var groups = await context.Groups.ToListAsync();
            return groups;
        }

        public Models.Group FindGroupById (int id)
        {
            var group = context.Groups.Find(id);
            return group;
        }

        public async Task<Models.Group> FindGroupByIdAsync(int? id)
        {
            var group = await context.Groups.FindAsync(id);
            return group;
        }

        public Models.Group FindGroupByIdIncludeAgents (int id)
        {
            Models.Group group = context.Groups.Include(gr => gr.Agents).FirstOrDefault(gr => gr.Id == id);
            return group;
        }

        public async Task<Models.Group> FindGroupByIdIncludeAgentsAsync(int? id)
        {
            Models.Group group=await context.Groups.Include(gr => gr.Agents).FirstOrDefaultAsync(gr => gr.Id == id);
            return group;
        }

        public IQueryable<Models.Group> FindGroupsByName (string name)
        {
            IQueryable<Models.Group> groups = context.Groups.Where(gr => gr.Name == name);
            return groups;
        }

        public async Task<List<Models.Group>> FindGroupsByNameAsync (string name)
        {
            List<Models.Group> groups = await context.Groups.Where(gr => gr.Name == name).ToListAsync();
            return groups;
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
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

        public async Task DeleteAsync(int id)
        {
            var group = await context.Groups.FindAsync(id);
            if (group != null)
            {
                context.Groups.Remove(group);
            }
        }

        public void RemoveAgentFromGroup(int agentId)
        {
            Agent agent = context.Agents.Find(agentId);
            agent.GroupId = null;
            context.Entry(agent).State = EntityState.Modified;
        }

        public async Task RemoveAgentFromGroupAsync(int agentId)
        {
            Agent agent = await context.Agents.FindAsync(agentId);
            agent.GroupId = null;
            context.Entry(agent).State = EntityState.Modified;
        }

        public IQueryable<Models.Group> SearchGroup(string searchString)
        {
            IQueryable<Models.Group> groups = context.Groups.Where(gr => gr.Name.Contains(searchString));
            return groups;
        }

        public async Task<List<Models.Group>> SearchGroupsAsync(string searchString)
        {
            List<Models.Group> groups = await context.Groups.Where(gr => gr.Name.Contains(searchString)).ToListAsync();
            return groups;
        }
    }
}