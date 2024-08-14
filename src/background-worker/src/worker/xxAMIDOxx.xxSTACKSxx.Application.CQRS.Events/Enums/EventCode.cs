﻿namespace xxAMIDOxx.xxSTACKSxx.Application.CQRS.Events.Enums;

public enum EventCode
{
    // Menu operations
    MenuCreated = 101,
    MenuUpdated = 102,
    MenuDeleted = 103,

    // Categories Operations
    CategoryCreated = 201,
    CategoryUpdated = 202,
    CategoryDeleted = 203,

    // Items Operations
    MenuItemCreated = 301,
    MenuItemUpdated = 302,
    MenuItemDeleted = 303,

    // CosmosDB change feed operations
    EntityUpdated = 999
}
