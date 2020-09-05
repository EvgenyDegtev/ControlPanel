using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using ControlPanel.Models;
using ControlPanel.Abstract;

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

        public IQueryable<Agent> AgentsIncludeGroup
        {
            get
            {
                var agents = context.Agents.Include(agent => agent.Group).AsQueryable();
                return agents;
            }
        }

        public Agent FindAgentById (int id)
        {
            var agent = context.Agents.Find(id);
            return agent;
        }

        public Agent FindAgentByIdIncludeGroup(int id)
        {
            var agent = context.Agents.Include(agent => agent.Group).FirstOrDefault(agent => agent.Id == id);
            return agent;
        }

        public Agent FindAgentByIdIncludeSkill(int id)
        {
            var agent = context.Agents.Include(agent => agent.AgentToSkills).FirstOrDefault(agent => agent.Id == id);
            return agent;
        }

        public IQueryable<AgentToSkill> FindAgentToSkillForAgentById(int agentId)
        {
            IQueryable<AgentToSkill> agentToSkills = context.AgentToSkills.Include(agentToSkill => agentToSkill.Skill).Where(agentToSkill => agentToSkill.AgentId == agentId);
            return agentToSkills;
        }

        public IQueryable<Agent> FindAgentsByLogin(string login)
        {
            IQueryable<Agent> agents = context.Agents.Where(ag => ag.Login == login && ag.IsActive == true);
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

        public void Delete(int id)
        {
            var agent = context.Agents.Find(id);
            if(agent!=null)
            {
                context.Agents.Remove(agent);
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public IQueryable<Agent> SearchAgent(string searchString)
        {
            var agents = context.Agents.Where(ag => ag.Login.Contains(searchString) && ag.IsActive == true);
            return agents;
        }
    }
}