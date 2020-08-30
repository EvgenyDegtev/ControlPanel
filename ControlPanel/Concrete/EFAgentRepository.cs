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

        public Agent FindAgentById (int id)
        {
            var agent = context.Agents.Find(id);
            return agent;
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