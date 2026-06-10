using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using TechCorp_Vue.Server.Models;

namespace TechCorp_Vue.Server.Models
{

    public class FileMetadata
    {
        [Key]
        public int Id { get; set; }


        public string Filepath { get; set; } = null!;

        public DateTime Createdate { get; set; }
    }

    [ModelMetadataType(typeof(FileMetadata))]
    public partial class Filemodel
    {
        public static Filemodel? CreateFile(TechCorpContext db, IFormFile ifile)
        {
            if (ifile == null || ifile.Length == 0)
                return null;

            string originalFileName = Path.GetFileName(ifile.FileName);
            string newFileName = Guid.NewGuid().ToString() + Path.GetExtension(originalFileName);
            var uploadDir = Path.Combine("wwwroot", "uploads");
            if (!Directory.Exists(uploadDir))
                Directory.CreateDirectory(uploadDir);

            string filePath = Path.Combine(uploadDir, newFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                ifile.CopyTo(stream);
            }

            var fileEn = new Filemodel
            {
                Filepath = "/uploads/" + newFileName,
                Createby = "Admin",
                Updateby = "Admin",
                Createdate = DateTime.Now,
                Updatedate = DateTime.Now,
                Isdeleted = false
            };

            db.Filemodels.Add(fileEn);
            db.SaveChanges();

            return fileEn;
        }

        public static Filemodel? UpdateFile(TechCorpContext dbContext, int? oldFileId, List<IFormFile> files)
        {
            if (files == null || files.Count == 0) return null;

            IFormFile file = files.First();

            // ถ้ามีไฟล์เก่า → mark ลบ
            if (oldFileId.HasValue)
            {
                Filemodel? oldFile = dbContext.Filemodels.FirstOrDefault(f => f.Id == oldFileId.Value);
                if (oldFile != null)
                {
                    oldFile.Isdeleted = true;
                    oldFile.Updatedate = DateTime.Now;
                    dbContext.Filemodels.Update(oldFile);
                }
            }

            // สร้างไฟล์ใหม่
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string filePath = Path.Combine("wwwroot/uploads", fileName);
            Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            Filemodel newFile = new Filemodel
            {
                Filepath = "/uploads/" + fileName,
                Createby = "Admin",
                Updateby = "Admin",
                Createdate = DateTime.Now,
                Updatedate = DateTime.Now,
                Isdeleted = false
            };

            dbContext.Filemodels.Add(newFile);
            dbContext.SaveChanges(); // ให้ได้ newFile.Id กลับมาเลย
            return newFile;
        }

        public static void DeleteFilesOfEmployeeTree(Employee root, TechCorpContext db)
        {
            if (root == null) return;

            if (root.Idfile.HasValue)
            {
                Filemodel? file = db.Filemodels.FirstOrDefault(f => f.Id == root.Idfile.Value); //id ทีมีใน employee
                if (file != null)
                {
                    file.Isdeleted = true;
                    file.Updateby = "Admin";
                    file.Updatedate = DateTime.Now;
                }
            }

            if (root.InverseManager != null)
            {
                foreach (Employee sub in root.InverseManager)
                {
                    DeleteFilesOfEmployeeTree(sub, db);
                }
            }
            db.SaveChanges();
        }
    }
}
