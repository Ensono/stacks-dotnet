using System;
using System.Collections.Generic;
using System.Linq;
using xxAMIDOxx.xxSTACKSxx.CQRS.Queries.SearchMenu;

namespace xxAMIDOxx.xxSTACKSxx.API.Models.Responses;

/// <summary>
/// Response model used by SearchMenu api endpoint
/// </summary>
public class SearchMenuResponse
{
    /// <example>10</example>
    public int Size { get; private set; }

    /// <example>0</example>
    public int Offset { get; private set; }

    /// <summary>
    /// Contains the items returned from the search
    /// </summary>
    public List<SearchMenuResponseItem> Results { get; private set; }

    public static SearchMenuResponse FromMenuResultItem(SearchMenuResult results)
    {
        return new SearchMenuResponse()
        {
            Offset = (results?.PageNumber ?? 0) * (results?.PageSize ?? 0),
            Size = (results?.PageSize ?? 0),
            Results = results?.Results?.Select(SearchMenuResponseItem.FromSearchMenuResultItem).ToList()
        };
    }
}
