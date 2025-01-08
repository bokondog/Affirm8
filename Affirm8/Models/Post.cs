using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Affirm8.Models
{
    public partial class Post : ObservableObject
    {
        [ObservableProperty]
        public int postId;
        [ObservableProperty]
        public string content;
        [ObservableProperty]
        public int userId;
        [ObservableProperty]
        public DateTime timestamp;
    }
}
