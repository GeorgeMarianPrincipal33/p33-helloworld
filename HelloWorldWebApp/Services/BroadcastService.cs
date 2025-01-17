﻿using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelloWorldWebApp.Services
{
    public class BroadcastService : IBroadcastService
    {
        private readonly IHubContext<MessageHub> messageHub;

        public BroadcastService(IHubContext<MessageHub> messageHub)
        {
            this.messageHub = messageHub;
        }

        public void NewTeamMemberAdded(string name, int id)
        {
            messageHub.Clients.All.SendAsync("NewTeamMemberAdded", name, id);
        }

        public void TeamMemberDeleted(int id)
        {
            messageHub.Clients.All.SendAsync("TeamMemberDeleted", id);
        }

        public void UpdateTeamMember(string name, int id)
        {
            messageHub.Clients.All.SendAsync("UpdateTeamMember", name, id);
        }
    }
}
