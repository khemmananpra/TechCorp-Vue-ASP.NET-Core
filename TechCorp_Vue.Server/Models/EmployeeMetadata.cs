using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;  //--> for metadata
using Microsoft.EntityFrameworkCore;
using TechCorp_Vue.Server.Models;


namespace TechCorp_Vue.Server.Models
{
    public class EmployeeMetadata
    {
        [Required(ErrorMessage = "กรุณากรอกชื่อพนักงาน")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "กรุณาเลือกรูปภาพหน้าปก")]
        public int IdFile { get; set; }

        public ICollection<Employee> InverseManager { get; set; } = new List<Employee>();
    }


    [ModelMetadataType(typeof(EmployeeMetadata))]
    public partial class Employee
    {

        [NotMapped]
        public bool IsActivePhoto { get; set; } = false;

        [NotMapped]
        public IFormFile? CoverPhotoFile { get; set; }

        [NotMapped]
        public bool IsActiveDel { get; set; } = false;

        public Employee Create(TechCorpContext dbContext)
        {
            // กำหนดค่าของ employee หลัก
            this.Createby = "Admin";
            this.Createdate = DateTime.Now;
            this.Updateby = "Admin";
            this.Updatedate = DateTime.Now;
            this.Isdeleted = false;

            addParamAllSubordinates(this.InverseManager, this);

            dbContext.Employees.Add(this);
            dbContext.SaveChanges();
            return this;
        }

        private void addParamAllSubordinates(ICollection<Employee> subordinates, Employee manager)
        {
            if (subordinates == null) return;

            // ใช้ ToList() เพื่อสร้างสำเนา ไม่แก้ไขขณะวน
            foreach (var subordinate in subordinates.ToList())
            {
                if (subordinate.Isdeleted == true)
                {
                    subordinates.Remove(subordinate); // ลบจาก collection
                    continue;
                }

                subordinate.Managerid = manager.Id;
                subordinate.Departmentid = manager.Departmentid;
                subordinate.Createby = "Admin";
                subordinate.Createdate = DateTime.Now;
                subordinate.Updateby = "Admin";
                subordinate.Updatedate = DateTime.Now;
                subordinate.Isdeleted = false;

                addParamAllSubordinates(subordinate.InverseManager, subordinate);
            }
        }
        public Employee Update(TechCorpContext dbContext)
        {
            this.Updatedate = DateTime.Now;

            //EntityState.Modified จะ update เมื่อ savechanges(); แก้ปัญหา Parameter key
            //มันจะ update ตัวที่สนใจปัจุจบัน ไม่ยุ่งกับตัวอื่น
            //ถ้าใช้ update(this) มันอาจจะไป update ตัว mangerid ที่ยังไม่มีค่า parentid 
            dbContext.Entry(this).State = EntityState.Modified;

            addParamSubordinatesUpdate(this.InverseManager, this, dbContext);


            //ค่อย save ที่เดียว
            //dbContext.Update(this);
            dbContext.SaveChanges();

            return this;
        }

        private void addParamSubordinatesUpdate(ICollection<Employee> employees, Employee manager, TechCorpContext db)
        {
            foreach (var emp in employees.ToList())
            {
                if (emp.Isdeleted == true && emp.Id == 0)
                {
                    continue;
                }

                // set ค่าที่ต้องอัปเดตทุกครั้ง

                if (manager != null)
                {
                    emp.Departmentid = manager.Departmentid;
                    emp.Managerid = manager.Id;
                }
                
                
                emp.Updateby = "Admin";
                emp.Updatedate = DateTime.Now;



                //ปํญหา เพราะ id ที่ถูกสร้างที่เป็นแม่ map แล้ว แต่ยังไม่ได้ลง db ตัวลูกเลยไม่รู้ว่าใครเป็นแม่ เพราะ แม่เป็น id = 0;
                //ตอนแรกเอา id ไวนอกเช็ค สร้างใหม่ มันเลย error
                // parameter key error คือ มัน savechage หลายรอบมันเลย error เราต้องจัดเตรียม update โดยใช้ Entry(obj) เพื่อให้รู้ว่ามีการ update แล้วรอ savechange ทีเดียว
                if (emp.Id == 0 ) 
                {

                    emp.Createby = "Admin";
                    emp.Createdate = DateTime.Now;

                    db.Employees.Add(emp);
                    db.SaveChanges();
                }
                else 
                {
                    //db.Entry(emp) ใช้เก็ย metadata ของ emp
                    //EntityState.Modified จะ update เมื่อ savechanges();
                    db.Entry(emp).State = EntityState.Modified;
                }


                if (emp.InverseManager != null)
                {
                    addParamSubordinatesUpdate(emp.InverseManager, emp, db);
                }
            }
        }




        public void DeleteA(TechCorpContext dbContext)
        {

            MarkDeletedRecursive(this);
            dbContext.SaveChanges();

        }

        public static void MarkDeletedRecursive(Employee emp)
        {
            emp.Isdeleted = true;
            emp.Updateby = "Admin";
            emp.Updatedate = DateTime.Now;

            if (emp.InverseManager != null)
            {
                foreach (Employee sub in emp.InverseManager)
                {
                    MarkDeletedRecursive(sub);
                }
            }
        }



    }
}