using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TechCorp_Vue.Server.Models;

[Table("Employee")]
public partial class Employee
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    public string? Name { get; set; }

    [Column("positionid")]
    public int? Positionid { get; set; }

    [Column("managerid")]
    public int? Managerid { get; set; }

    [Column("idfile")]
    public int? Idfile { get; set; }

    [Column("departmentid")]
    public int? Departmentid { get; set; }

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

    [Column("signatureid")]
    public int? Signatureid { get; set; }

    [ForeignKey("Departmentid")]
    [InverseProperty("Employees")]
    public virtual Department? Department { get; set; }

    [ForeignKey("Idfile")]
    [InverseProperty("Employees")]
    public virtual Filemodel? IdfileNavigation { get; set; }

    [InverseProperty("Manager")]
    public virtual ICollection<Employee> InverseManager { get; set; } = new List<Employee>();

    [ForeignKey("Managerid")]
    [InverseProperty("InverseManager")]
    public virtual Employee? Manager { get; set; }

    [ForeignKey("Positionid")]
    [InverseProperty("Employees")]
    public virtual Position? Position { get; set; }

    [ForeignKey("Signatureid")]
    [InverseProperty("Employees")]
    public virtual Signature? Signature { get; set; }
}
