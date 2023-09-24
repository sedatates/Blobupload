using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

public class BlobStorageHelper
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly string _containerName;

    public BlobStorageHelper(IConfiguration configuration)
    {
        // Azure Blob Storage bağlantı dizesini burada alın.
        string connectionString = configuration.GetConnectionString("AzureBlobStorage");

        // BlobServiceClient nesnesini oluşturun.
        _blobServiceClient = new BlobServiceClient(connectionString);

        // Blob Storage'da kullanılacak container adı
        _containerName = "gassbill2"; // Kendi container adınızı belirleyin
    }

    public async Task<string> UploadFileAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            throw new ArgumentException("Dosya seçilmedi veya boş.");
        }

        // Blob Storage'da dosyanın benzersiz adı
        string blobName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

        // Blob Container oluşturma (varsa oluşturmaz)
        BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        await containerClient.CreateIfNotExistsAsync(PublicAccessType.BlobContainer);

        // Blob'a dosyayı yükleme
        BlobClient blobClient = containerClient.GetBlobClient(blobName);
        using (var stream = file.OpenReadStream())
        {
            await blobClient.UploadAsync(stream, true);
        }

        return blobName;
    }

    public async Task<Stream> DownloadFileAsync(string blobName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        var blobClient = containerClient.GetBlobClient(blobName);
        var blobDownloadInfo = await blobClient.OpenReadAsync();
        return blobDownloadInfo;
    }
}
        