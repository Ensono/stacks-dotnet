using System;

namespace Snyk.Fixes.CQRS.Commands
{
    public class Updatelocalhost : IlocalhostCommand
    {
        public int OperationCode => (int)Common.Operations.OperationCode.Updatelocalhost;

        public Guid CorrelationId { get; }

        public Guid localhostId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool Enabled { get; set; }
    }

}
