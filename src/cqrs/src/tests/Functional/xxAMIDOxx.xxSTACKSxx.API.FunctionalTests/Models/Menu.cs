using System.Collections.Generic;

namespace xxAMIDOxx.xxSTACKSxx.API.FunctionalTests.Models;

public class Menu
{
	public string id { get; set; }
	public string name { get; set; }
	public string description { get; set; }
	public List<Category> categories { get; set; }
	public bool enabled { get; set; }
}
