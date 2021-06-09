using System;

namespace Snyk.Fixes.CQRS.Commands
{
    public class Deletelocalhost : IlocalhostCommand
    {
        public int OperationCode => (int)Common.Operations.OperationCode.Deletelocalhost;

        public Guid CorrelationId { get; }

        public Guid localhostId { get; set; }

        public Deletelocalhost(Guid localhostId)
        {
            this.localhostId = localhostId;
        }
    }
}
