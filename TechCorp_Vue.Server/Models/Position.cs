using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TechCorp_Vue.Server.Models;

[Table("Position")]
public partial class Position
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("title")]
    public string? Title { get; set; }

    [Column("createby")]
    public string? Createby { get; set; }

    [Column("createdate")]
    public DateTime? Createdate { get; set; }

    [Column("updateby")]
    public string? Updateby { get; set; }

    [Column("updatedate")]
    public DateTime? Updatedate { get; set; }

    [Column("isdeleted")]
    public bool? Isdeleted { get; set; }

    [InverseProperty("Position")]
    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
