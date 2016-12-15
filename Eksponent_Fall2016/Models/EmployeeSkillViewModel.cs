using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Eksponent_Fall2016.Models
{
    public class EmployeeSkillViewModel

    {
        public int EmployeeSkillId { get; set; }
        public int Level { get; set; }
        public int SkillId { get; set; }
        public int EmployeeId { get; set; }
        public IEnumerable<SelectListItem> SkillList { get; set; }
        public IEnumerable<SelectListItem> LevelList { get; set; }
    }
}