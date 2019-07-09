using System;
using System.Collections.Generic;
using System.Text;

namespace Amido.Stacks.Tests.Api.Models
{
    public class Category
    {
        public string id { get; set; }
        public string name { get; set; }
        public List<Item> items { get; set; }
    }
}
