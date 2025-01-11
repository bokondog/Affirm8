using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;

namespace Affirm8.Models
{
    public partial class Post
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Content { get; set; }
        public int UserId { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
