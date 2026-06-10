using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using TechCorp_Vue.Server.Models;

namespace TechCorp_Vue.Server.Models
{

    public class SignatureMetadata
    {
        [Key]
        public int Id { get; set; }


        public string Filepath { get; set; } = null!;

        public DateTime Createdate { get; set; }
    }

    [ModelMetadataType(typeof(SignatureMetadata))]
    public partial class Signature
    {
        public async Task CreateSign(TechCorpContext dbContext)
        {
            if (string.IsNullOrEmpty(this.Signature1))
                throw new ArgumentException("No signature received.");

            // แยก prefix ถ้ามี
            string base64Data = this.Signature1;
            if (base64Data.Contains(","))
                base64Data = base64Data.Split(',')[1];

            byte[] bytes;
            try
            {
                bytes = Convert.FromBase64String(base64Data);
            }
            catch (FormatException)
            {
                throw new ArgumentException("Invalid base64 data.");
            }

            // สร้าง folder ถ้าไม่มี
            var folder = Path.Combine("wwwroot", "signatures");
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            // สร้างไฟล์
            var fileName = $"{Guid.NewGuid()}.png";
            var filePath = Path.Combine(folder, fileName);
            await System.IO.File.WriteAllBytesAsync(filePath, bytes);

            // save path ลง DB
            this.Signature1 = $"/signatures/{fileName}"; // path สำหรับ frontend ใช้
            using var context = new TechCorpContext(); // หรือ inject context ผ่าน DI
            context.Signatures.Add(this);
            await context.SaveChangesAsync();
        }
    }
}
