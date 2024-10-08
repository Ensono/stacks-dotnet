﻿using System;
using System.Collections.Generic;
using System.Linq;
using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Extensions;
using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Serializers;

namespace xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Factories
{
    public class MessageBuilderFactory : IMessageBuilderFactory
    {
        private readonly IEnumerable<IMessageBuilder> builders;

        public MessageBuilderFactory(IEnumerable<IMessageBuilder> builders)
        {
            this.builders = builders;
        }

        public IMessageBuilder CreateMessageBuilder(string name)
        {
            if (name == null)
                throw new Exception("A serializer name must be provided");

            return builders
                .First(x =>
                    x.GetType().GetTypeNameWithAssembly() == name ||
                    x.GetType().Name == name ||
                    x.GetType().FullName == name);
        }
    }
}
