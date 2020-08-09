using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControlPanel.Models;

namespace ControlPanel.Abstract
{
    public interface ISkillRepository
    {
        IQueryable<Skill> Skills { get; }

        void SaveSkill(Skill skill);

    }
}
