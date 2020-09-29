using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using ControlPanel.Models;
using ControlPanel.Abstract;
using System.Threading.Tasks;

namespace ControlPanel.Concrete
{
    public class EFAgentRepository: IAgentRepository
    {
        private EFDbContext context = new EFDbContext();

        public IQueryable<Agent> Agents
        {
            get
            {
                var agents = context.Agents.AsQueryable();
                return agents;
            }

        }

        public async Task<List<Agent>> GetAgentsAsync()
        {
            var agents = await context.Agents.ToListAsync();
            return agents;
        }

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
            var skills = await context.Skills.ToListAsync();
            return skills;
        }

        public IQueryable<Group> Groups
        {
            get
            {
                var groups = context.Groups.AsQueryable();
                return groups;
            }
        }

        public async Task<List<Group>> GetGroupsAsync()
        {
            var groups = await context.Groups.ToListAsync();
            return groups;
        }

        public IQueryable<Agent> AgentsIncludeGroup
        {
            get
            {
                var agents = context.Agents.Include(agent => agent.Group).AsQueryable();
                return agents;
            }
        }

        public async Task<List<Agent>> GetAgentsIncludeGroupAsync()
        {
            var agents = await context.Agents.Include(agent => agent.Group).ToListAsync();
            return agents;
        }

        public Agent FindAgentById (int id)
        {
            var agent = context.Agents.Find(id);
            return agent;
        }

        public async Task<Agent> FindAgentByIdAsync(int? id)
        {
            var agent = await context.Agents.FindAsync(id);
            return agent;
        }

        public Skill FindSkillById(int skillId)
        {
            var skill = context.Skills.Find(skillId);
            return skill;
        }

        public async Task<Skill> FindSkillByIdAsync(int? skillId)
        {
            var skill = await context.Skills.FindAsync(skillId);
            return skill;
        }

        public Agent FindAgentByIdIncludeGroup(int id)
        {
            var agent = context.Agents.Include(agent => agent.Group).FirstOrDefault(agent => agent.Id == id);
            return agent;
        }
 
        public async Task<Agent> FindAgentByIdIncludeGroupAsync(int? id)
        {
            var agent = await context.Agents.Include(agent => agent.Group).FirstOrDefaultAsync(agent => agent.Id == id);
            return agent;
        }

        public Agent FindAgentByIdIncludeSkill(int id)
        {
            var agent = context.Agents.Include(agent => agent.AgentToSkills).FirstOrDefault(agent => agent.Id == id);
            return agent;
        }

        public async Task<Agent> FindAgentByIdIncludeSkillsAsync(int? id)
        {
            var agent = await context.Agents.Include(agent => agent.AgentToSkills).FirstOrDefaultAsync(agent => agent.Id == id);
            return agent;
        }

        public IQueryable<AgentToSkill> FindAgentToSkillForAgentById(int agentId)
        {
            IQueryable<AgentToSkill> agentToSkills = context.AgentToSkills.Include(agentToSkill => agentToSkill.Skill).Where(agentToSkill => agentToSkill.AgentId == agentId);
            return agentToSkills;
        }

        public async Task<List<AgentToSkill>> FindAgentToSkillForAgentByIdAsync(int? agentId)
        {
            List<AgentToSkill> agentToSkills = await context.AgentToSkills.Include(agentToSkill => agentToSkill.Skill).Where(agentToSkill => agentToSkill.AgentId == agentId).ToListAsync();
            return agentToSkills;
        }

        public IQueryable<Agent> FindAgentsByLogin(string login)
        {
            IQueryable<Agent> agents = context.Agents.Where(ag => ag.Login == login && ag.IsActive == true);
            return agents;
        }

        public async Task<List<Agent>> FindAgentsByLoginAsync(string login)
        {
            List<Agent> agents = await context.Agents.Where(ag => ag.Login == login && ag.IsActive == true).ToListAsync();
            return agents;
        }

        public void Create (Agent agent)
        {
            context.Agents.Add(agent);
        }

        public void Update(Agent agent)
        {
            context.Entry(agent).State = EntityState.Modified;
        }

        public void UpdateAgentToSkill(AgentToSkill agentToSkill)
        {
            context.Entry(agentToSkill).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            var agent = context.Agents.Find(id);
            if(agent!=null)
            {
                context.Agents.Remove(agent);
            }
        }

        public async Task DeleteAsync(int id)
        {
            var agent = await context.Agents.FindAsync(id);
            if(agent!=null)
            {
                context.Agents.Remove(agent);
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

        public IQueryable<Agent> SearchAgent(string searchString)
        {
            var agents = context.Agents.Where(ag => ag.Login.Contains(searchString) && ag.IsActive == true);
            return agents;
        }

        public async Task<List<Agent>> SearchAgentsAsync(string searchString)
        {
            var agents = await context.Agents.Where(ag => ag.Login.Contains(searchString) && ag.IsActive == true).ToListAsync();
            return agents;
        }

        public void CreateAgentToSkill(AgentToSkill agentToSkill)
        {
            context.AgentToSkills.Add(agentToSkill);
        }

        public void DeleteAgentToSkill(int id)
        {
            AgentToSkill agentToSkill = context.AgentToSkills.Find(id);
            if(agentToSkill!=null)
            {
                context.AgentToSkills.Remove(agentToSkill);
            }
        }

        public async Task DeleteAgentToSkillAsync(int id)
        {
            var agentToSkill = await context.AgentToSkills.FindAsync(id);
            if(agentToSkill!=null)
            {
                context.AgentToSkills.Remove(agentToSkill);
            }
        }

        public IQueryable<AgentToSkill> FindAgentToSkills(int agentId, int skillId)
        {
            IQueryable<AgentToSkill> agentToSkills = context.AgentToSkills.Where(agToSkill => agToSkill.AgentId == agentId && agToSkill.SkillId == skillId);
            return agentToSkills;
        }

        public async Task<List<AgentToSkill>> FindAgentToSkillsAsync(int agentId, int skillId)
        {
            List<AgentToSkill> agentToSkills = await context.AgentToSkills.Where(agToSkill => agToSkill.AgentId == agentId && agToSkill.SkillId == skillId).ToListAsync();
            return agentToSkills;
        }
    }
}