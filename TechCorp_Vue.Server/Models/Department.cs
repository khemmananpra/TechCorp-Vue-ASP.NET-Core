using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TechCorp_Vue.Server.Models;

[Table("Department")]
public partial class Department
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    public string? Name { get; set; }

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

    [InverseProperty("Department")]
    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
