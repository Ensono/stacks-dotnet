using System;
using System.Collections.Generic;
using System.Text;

namespace Amido.Stacks.E2e.Tests.Api.Models
{
    public class Menu
    {
        public string id { get; set; }
        public string name { get; set; }
        public List<Category> categories { get; set; }
        public bool enabled { get; set; }
    }
}
