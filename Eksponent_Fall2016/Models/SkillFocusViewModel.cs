using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Eksponent_Fall2016.Models
{
    public class SkillFocusViewModel
    {
        public int SkillFocusId { get; set; }
        public int EmployeeId { get; set; }
        public int SkillId { get; set; }
        public int NumberOfFocus { get; set; }
        public DateTime Startdate { get; set; }
        public IEnumerable<SelectListItem> SkillList { get; set; }
    }
}