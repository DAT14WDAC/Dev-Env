using Eksponent_Fall2016.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eksponent_Fall2016.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Profileimage { get; set; }
        public int CompanyId { get; set; }
        public string ApplicationUserId { get; set; }

        public virtual Company Company { get; set; }

        public virtual ICollection<SkillFocus> ISkillFocus { get; set; }
        public virtual ICollection<EmployeeSkill> IEmployeeSkill { get; set; }

        public void SaveImage(HttpPostedFileBase image,
          String serverPath, String pathToFile)
        {
            if (image == null) return;
            if (!String.IsNullOrEmpty(this.Profileimage))
            {
                // delete the old picture before adding new picture 
                ImageModel.DeleteFile(serverPath + Profileimage);
            }
            //ImageModel 
            Guid guid = Guid.NewGuid();
            ImageModel.ResizeAndSave(serverPath + pathToFile, guid.ToString(), image.InputStream, 200);
            Profileimage = pathToFile + guid.ToString() + ".jpg";
        }
    }
}