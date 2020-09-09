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

        IQueryable<Group> Groups { get; }

        IQueryable<Skill> Skills { get; }

        IQueryable<Agent> AgentsIncludeGroup { get; }

        Agent FindAgentById(int id);

        Skill FindSkillById(int skillId);

        Agent FindAgentByIdIncludeGroup(int id);

        Agent FindAgentByIdIncludeSkill(int id);

        IQueryable<AgentToSkill> FindAgentToSkillForAgentById(int agentId);


        IQueryable<Agent> FindAgentsByLogin(string login);

        void Save();

        void Create(Agent agent);

        void Update(Agent agent);

        void UpdateAgentToSkill(AgentToSkill agentToSkill);

        void Delete(int id);

        IQueryable<Agent> SearchAgent(string searchString);

        void CreateAgentToSkill(AgentToSkill agentToSkill);

        void DeleteAgentToSkill(int id);

        IQueryable<AgentToSkill> FindAgentToSkills(int agentId, int skillId);
    }
}
