using Microsoft.AspNetCore.Components;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServer.Pages
{
    public partial class Elastic 
    {
        [Inject]
        public Repo repo { get; set; }
        public string query;
    }
}
