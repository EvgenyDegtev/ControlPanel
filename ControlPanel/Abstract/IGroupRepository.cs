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

        Models.Group FindGroupById(int id);

        Models.Group FindGroupByIdIncludeAgents(int id);

        IQueryable<Models.Group> FindGroupsByName(string name);

        void Create(Models.Group group);

        void Update(Models.Group group);

        void Delete(int id);

        void Save();

        void RemoveAgentFromGroup(int agentId);

        IQueryable<Models.Group> SearchGroup(string searchString);


    }
}
