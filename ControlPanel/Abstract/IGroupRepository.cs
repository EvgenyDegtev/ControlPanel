using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ControlPanel.Models;

namespace ControlPanel.Abstract
{
    public interface IGroupRepository
    {
        IQueryable<Models.Group> Groups { get; }

        Task<List<Models.Group>> GetGroupsAsync();

        Models.Group FindGroupById(int id);

        Task<Models.Group> FindGroupByIdAsync(int? id);

        Models.Group FindGroupByIdIncludeAgents(int id);

        Task<Models.Group> FindGroupByIdIncludeAgentsAsync(int? id);

        IQueryable<Models.Group> FindGroupsByName(string name);

        Task<List<Models.Group>> FindGroupsByNameAsync(string name);

        void Create(Models.Group group);

        void Update(Models.Group group);

        void Delete(int id);

        Task DeleteAsync(int id);

        void Save();

        Task SaveAsync();

        void RemoveAgentFromGroup(int agentId);

        Task RemoveAgentFromGroupAsync(int agentId);

        IQueryable<Models.Group> SearchGroup(string searchString);

        Task<List<Models.Group>> SearchGroupsAsync(string searchString);


    }
}
