﻿using GroupSpace23.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Permissions;

namespace GroupSpace23.Models
{
    public class Group
    {
        public int Id { get; set; }

        [Display (Name="Naam")]
        public string Name { get; set; }

        [Display(Name = "Omschrijving")]
        public string Description { get; set; }

        [ForeignKey ("GroupSpace23User")]
        public string StartedById { get; set; } = Globals.DummyUser.Id;

        public GroupSpace23User? StartedBy { get; set; }

        [Display(Name = "Groep aangemaakt")]
        [DataType(DataType.Date)]
        public DateTime Started { get; set; } = DateTime.Now;

        [Display(Name = "Groep gestopt")]
        [DataType(DataType.Date)]
        public DateTime Ended { get; set; } = DateTime.MaxValue;
    }


    public class GroupMember
    {
        public int Id { get; set; }

        [ForeignKey("Group")]
        public int GroupId {  get; set; }
        public Group? Group { get; set; }

        [ForeignKey("GroupSpace23User")]
        public string MemberId { get; set; }
        public GroupSpace23User? Member { get; set; }

        [ForeignKey("GroupSpace23User")]
        public string AddedById { get; set; }
        public GroupSpace23User? AddedBy { get; set; }
        public DateTime Added { get; set; } = DateTime.Now;
        public DateTime Removed { get; set; } = DateTime.MaxValue;
        
        [ForeignKey("GroupSpace23User")]
        public string RemovedById { get; set; }

        public bool IsHost { get; set; } = false;
    }

}
