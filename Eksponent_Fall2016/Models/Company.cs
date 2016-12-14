using Eksponent_Fall2016.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Eksponent_Fall2016.Models
{
    public class Company
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        [Display(Name = "Description")]
        public string CompanyDescription { get; set; }
        public string CompanyLogo { get; set; }
        public string ApplicationUserId { get; set; }

        public virtual ICollection<Employee> IEmployee { get; set; }
        public virtual ICollection<Skill> ISkill { get; set; }

        public void SaveLogo(HttpPostedFileBase image,
        String serverPath, String pathToFile)
        {
            if (image == null) return;
            if (!String.IsNullOrEmpty(this.CompanyLogo))
            {
                // delete the old picture before adding new picture 
                ImageModel.DeleteFile(serverPath + CompanyLogo);
            }
            //ImageModel 
            Guid guid = Guid.NewGuid();
            ImageModel.ResizeAndSave(serverPath + pathToFile, guid.ToString(), image.InputStream, 200);
            CompanyLogo = pathToFile + guid.ToString() + ".jpg";
        }

    }
}