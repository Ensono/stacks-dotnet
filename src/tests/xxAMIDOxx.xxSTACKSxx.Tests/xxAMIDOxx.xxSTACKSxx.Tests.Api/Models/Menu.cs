﻿using System;
using System.Collections.Generic;
using System.Text;

namespace xxAMIDOxx.xxSTACKSxx.Tests.Api.Models
{
    public class Menu
    {
        public string id { get; set; }
        public string name { get; set; }
        public List<Category> categories { get; set; }
        public bool enabled { get; set; }
    }
}
