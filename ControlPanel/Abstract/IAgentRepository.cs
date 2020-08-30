using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControlPanel.Models;

namespace ControlPanel.Abstract
{
    public interface IAgentRepository
    {
        IQueryable<Agent> Agents { get; }

        Agent FindAgentById(int id);

        IQueryable<Agent> FindAgentsByLogin(string login);

        void Save();

        void Create(Agent agent);

        void Update(Agent agent);

        void Delete(int id);

        IQueryable<Agent> SearchAgent(string searchString);
    }
}
