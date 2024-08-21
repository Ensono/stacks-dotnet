using System.ComponentModel.DataAnnotations;

namespace xxAMIDOxx.xxSTACKSxx.API.Models.Requests;

/// <summary>
/// Request model used by UpdateItem api endpoint
/// </summary>
public class UpdateItemRequest
{
    /// <example>Name of Item updated</example>
    [Required]
    public string Name { get; set; }

    /// <example>Description of Item updated</example>
    public string Description { get; set; }

    /// <example>1.50</example>
    [Required]
    public double Price { get; set; }

    /// <summary>
    /// Represents the status of the item. False if unavailable
    /// </summary>
    [Required]
    public bool Available { get; set; }
}
