using System;

namespace xxAMIDOxx.xxSTACKSxx.CQRS.Commands.Models
{
    public class Group
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public Group(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
