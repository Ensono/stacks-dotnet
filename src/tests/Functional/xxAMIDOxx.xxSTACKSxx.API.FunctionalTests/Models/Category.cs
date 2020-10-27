using System;
using System.Collections.Generic;
using System.Text;

namespace xxAMIDOxx.xxSTACKSxx.API.FunctionalTests.Models
{
    public class Category
    {
        public string id { get; set; }
        public string name { get; set; }
        
        public string description { get; set; }
        public List<Item> items { get; set; }
    }
}
