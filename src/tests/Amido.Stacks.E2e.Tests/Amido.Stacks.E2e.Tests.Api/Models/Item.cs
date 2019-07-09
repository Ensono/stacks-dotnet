﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Amido.Stacks.E2e.Tests.Api.Models
{
    public class Item
    {
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string price { get; set; }
        public bool available { get; set; }
    }
}
