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

        Task<List<Agent>> GetAgentsAsync();

        IQueryable<Group> Groups { get; }

        Task<List<Group>> GetGroupsAsync();

        IQueryable<Skill> Skills { get; }

        Task<List<Skill>> GetSkillsAsync();

        IQueryable<Agent> AgentsIncludeGroup { get; }

        Task<List<Agent>> GetAgentsIncludeGroupAsync();

        Agent FindAgentById(int id);

        Task<Agent> FindAgentByIdAsync(int? id);

        Skill FindSkillById(int skillId);

        Task<Skill> FindSkillByIdAsync(int? skillId);

        Agent FindAgentByIdIncludeGroup(int id);

        Task<Agent> FindAgentByIdIncludeGroupAsync(int? id);

        Agent FindAgentByIdIncludeSkill(int id);

        Task<Agent> FindAgentByIdIncludeSkillsAsync(int? id);

        IQueryable<AgentToSkill> FindAgentToSkillForAgentById(int agentId);

        Task<List<AgentToSkill>> FindAgentToSkillForAgentByIdAsync(int? agentId);

        IQueryable<Agent> FindAgentsByLogin(string login);

        Task<List<Agent>> FindAgentsByLoginAsync(string login);

        void Save();

        Task SaveAsync();

        void Create(Agent agent);

        void Update(Agent agent);

        void UpdateAgentToSkill(AgentToSkill agentToSkill);

        void Delete(int id);

        Task DeleteAsync(int id);

        IQueryable<Agent> SearchAgent(string searchString);

        Task<List<Agent>> SearchAgentsAsync(string searchString);

        void CreateAgentToSkill(AgentToSkill agentToSkill);

        void DeleteAgentToSkill(int id);

        Task DeleteAgentToSkillAsync(int id);

        IQueryable<AgentToSkill> FindAgentToSkills(int agentId, int skillId);

        Task<AgentToSkill> FindAgentToSkillAsync(int agentId, int skillId);
    }
}
