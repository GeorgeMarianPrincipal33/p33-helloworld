﻿using System.Collections.Generic;
using HelloWorldWebApp.Models;
using Microsoft.AspNetCore.SignalR;

namespace HelloWorldWebApp.Services
{
    public class TeamService : ITeamService
    {
        private readonly TeamInfo teamInfo;
        private readonly ITimeService timeService;
        private readonly IBroadcastService broadcastService;

        public TeamService(IBroadcastService broadcastService)
        {
            this.broadcastService = broadcastService;

            this.teamInfo = new TeamInfo
            {
                Name = "Team 3",
                TeamMembers = new List<TeamMember>(),
            };

            string[] teamMembersData = new string[]
           {
                "Sechei Radu",
                "Tanase Teona",
                "Duma Dragos",
                "Campean Leon",
                "Naghi Claudia",
                "Marian George",
           };

            foreach (string name in teamMembersData)
            {
                AddTeamMember(name);
            }
        }

        public TeamInfo GetTeamInfo()
        {
            return teamInfo;
        }

        public void RemoveMember(int memberId)
        {
            TeamMember member = GetMemberById(memberId);
            teamInfo.TeamMembers.Remove(member);
            broadcastService.TeamMemberDeleted(memberId);
        }

        public int AddTeamMember(string name)
        {
            TeamMember newMember = new TeamMember(name, timeService);
            teamInfo.TeamMembers.Add(newMember);

            broadcastService.NewTeamMemberAdded(newMember.Name, newMember.Id);

            return newMember.Id;
        }

        public void UpdateMemberName(int memberId, string name)
        {
            TeamMember member = GetMemberById(memberId);
            member.Name = name;

            broadcastService.UpdateTeamMember(name, memberId);
        }

        public TeamMember GetMemberById(int memberId)
        {
            return teamInfo.TeamMembers.Find(element => element.Id == memberId);
        }
    }
}
