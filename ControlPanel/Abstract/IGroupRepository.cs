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

        void Create(Models.Group group);

        void Update(Models.Group group);

        void Delete(int id);

        void Save();

        IQueryable<Models.Group> SearchGroup(string searchString);


    }
}
