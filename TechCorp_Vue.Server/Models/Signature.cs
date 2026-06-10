using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TechCorp_Vue.Server.Models;

[Table("signature")]
public partial class Signature
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("signature")]
    public string? Signature1 { get; set; }

    [InverseProperty("Signature")]
    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
