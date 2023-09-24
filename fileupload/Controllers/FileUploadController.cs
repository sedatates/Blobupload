using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using fileupload.Models;
using File = fileupload.Models.File;

[Route("api/[controller]")]
[ApiController]
public class FileController : ControllerBase
{
    private readonly FileService _fileService;
    private readonly BlobStorageHelper _blobStorageHelper;

    public FileController(FileService fileService, BlobStorageHelper blobStorageHelper)
    {
        _fileService = fileService;
        _blobStorageHelper = blobStorageHelper;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Dosya seçilmedi veya boş.");
            }

            // Azure Blob Storage'a dosya yükleme
            string blobName = await _blobStorageHelper.UploadFileAsync(file);

            // Veritabanına dosya bilgilerini kaydetme
            var uploadDate = DateTime.UtcNow;
            var uploadedFile = new File()
            {
                FileName = blobName,
                FileType = file.ContentType,
                UploadDate = uploadDate
                // Diğer dosya bilgilerini burada ekleyebilirsiniz...
            };
            

            //await _fileService.UploadFileAsync(uploadedFile);

            return Ok($"Dosya başarıyla yüklendi. Blob Adı: {blobName}");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Hata: {ex.Message}");
        }
    }

    [HttpGet("download/{blobName}")]
    public async Task<IActionResult> DownloadFile(string blobName)
    {
        try
        {
            // Azure Blob Storage'dan dosya indirme
            Stream fileStream = await _blobStorageHelper.DownloadFileAsync(blobName);

            if (fileStream == null)
            {
                return NotFound("Dosya bulunamadı.");
            }

            // Dosyayı kullanıcıya döndürme
            return File(fileStream, "application/octet-stream", blobName);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Hata: {ex.Message}");
        }
    }
}
