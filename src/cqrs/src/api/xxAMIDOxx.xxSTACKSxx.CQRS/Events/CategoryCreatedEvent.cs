﻿using System;
using xxAMIDOxx.xxSTACKSxx.Shared.Application.CQRS.ApplicationEvents;
using xxAMIDOxx.xxSTACKSxx.Shared.Core.Operations;
using Newtonsoft.Json;

namespace xxAMIDOxx.xxSTACKSxx.CQRS.ApplicationEvents;

public class CategoryCreatedEvent : IApplicationEvent
{
    [JsonConstructor]
    public CategoryCreatedEvent(int operationCode, Guid correlationId, Guid menuId, Guid categoryId)
    {
        OperationCode = operationCode;
        CorrelationId = correlationId;
        MenuId = menuId;
        CategoryId = categoryId;
    }

    public CategoryCreatedEvent(IOperationContext context, Guid menuId, Guid categoryId)
    {
        OperationCode = context.OperationCode;
        CorrelationId = context.CorrelationId;
        MenuId = menuId;
        CategoryId = categoryId;
    }

    public int EventCode => (int)Enums.EventCode.CategoryCreated;

    public int OperationCode { get; }

    public Guid CorrelationId { get; }

    public Guid MenuId { get; }

    public Guid CategoryId { get; }
}
