using System.Collections.Generic;

namespace xxENSONOxx.xxSTACKSxx.API.FunctionalTests.Models;

public class Category
{
    public string id { get; set; }
    public string name { get; set; }
        
    public string description { get; set; }
    public List<Item> items { get; set; }
}
