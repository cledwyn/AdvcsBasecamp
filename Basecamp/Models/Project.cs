using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.ComponentModel.DataAnnotations;

namespace Basecamp.Models
{
    public class Project
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public bool archived { get; set; }
        public bool is_client_project { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public bool trashed { get; set; }
        public string color { get; set; }
        public bool draft { get; set; }
        public bool template { get; set; }
        public DateTime last_event_at { get; set; }
        public bool starred { get; set; }
        public string url { get; set; }
        public string app_url { get; set; }
    }
}