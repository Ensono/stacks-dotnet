using System;
using Microsoft.Azure.ServiceBus;
using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Serializers;

namespace xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Extensions
{
    public static class MessageExtensions
    {
        public static string GetTypeNameWithAssembly(this Type type)
        {
            return $"{type.FullName}, {type.Assembly.GetName().Name}";
        }


        public static string GetEnclosedMessageType(this Message message)
        {
            message.UserProperties.TryGetValue($"{MessageProperties.EnclosedMessageType}", out var value);
            return value as string;
        }

        public static Message SetEnclosedMessageType(this Message message, Type type)
        {
            var key = $"{MessageProperties.EnclosedMessageType}";
            message.UserProperties[key] = type.GetTypeNameWithAssembly();
            return message;
        }

        public static string GetSerializerType(this Message message)
        {
            message.UserProperties.TryGetValue($"{MessageProperties.Serializer}", out var value);
            return value as string;
        }

        public static Message SetSerializerType(this Message message, Type type)
        {
            var key = $"{MessageProperties.Serializer}";
            message.UserProperties[key] = type.GetTypeNameWithAssembly();
            return message;
        }
        
        public static Message SetSessionId(this Message message, object body)
        {
            var ctx = body as ISessionContext;
            message.SessionId = ctx?.SessionId;
            return message;
        }
    }
}
