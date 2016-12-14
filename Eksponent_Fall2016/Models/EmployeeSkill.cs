using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eksponent_Fall2016.Models
{
    public class EmployeeSkill
    {
        public int EmployeeSkillId { get; set; }
        public int Level { get; set; }
        public int SkillId { get; set; }
        public int EmployeeId { get; set; }
       

        public virtual Skill Skill { get; set; }
        public virtual Employee Employee { get; set; }

    }
}