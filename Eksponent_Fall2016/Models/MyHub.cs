using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Owin;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Eksponent_Fall2016.Models
{
    public class MyHub : Hub
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public void Hello()
        { 

            //Fetching UserManager
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            //Get User from Database based on userId 
            var currentUser = userManager.FindById(Context.User.Identity.GetUserId());

            Company company = db.Companies.Where(x => x.ApplicationUserId == currentUser.Id).Single();

            Clients.Client(Context.ConnectionId).hello(new { Name = company.CompanyName, CId = Context.ConnectionId });
        }

        public void JoinGroup(string groupName)
        {

            Groups.Add(Context.ConnectionId, groupName);
            Clients.Caller.message("WELCOME TO -- " + groupName + " -- WORK GROUP!");
        }

        public void LeaveGroup(string groupName)
        {
            Groups.Remove(Context.ConnectionId, groupName);
            Clients.Client(Context.ConnectionId).message("WE ARE SORRY YOU ARE LEAVING -- " + groupName + " -- WORK GROUP!");

        }
    }
}