using System;

namespace xxAMIDOxx.xxSTACKSxx.Domain.ValueObjects
{
    public class Cuisine
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public Cuisine(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
