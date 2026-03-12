using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class EditUserViewModel
{
    public string Id { get; set; }

    [Required]
    [Display(Name = "First Name")]
    public string? FirstName { get; set; }

    [Required]
    [Display(Name = "Last Name")]
    public string? LastName { get; set; }

    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    [Required]
    public string? Role { get; set; }
    public IEnumerable<SelectListItem>? RoleList { get; set; }

    public int? StationId { get; set; }
    public IEnumerable<SelectListItem>? Stations { get; set; }
}