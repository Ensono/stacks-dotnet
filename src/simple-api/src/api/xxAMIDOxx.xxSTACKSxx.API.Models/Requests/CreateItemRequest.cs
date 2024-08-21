using System.ComponentModel.DataAnnotations;

namespace xxAMIDOxx.xxSTACKSxx.API.Models.Requests;

/// <summary>
/// Request model used by CreateItem api endpoint
/// </summary>
public class CreateItemRequest
{
    /// <example>Name of item created</example>
    [Required]
    public string Name { get; set; }

    /// <example>Description of item created</example>
    public string Description { get; set; }

    /// <example>1.50</example>
    [Required]
    public double Price { get; set; }

    /// <summary>
    /// Represents the status of the item. False if disabled
    /// </summary>
    [Required]
    public bool Available { get; set; }
}
