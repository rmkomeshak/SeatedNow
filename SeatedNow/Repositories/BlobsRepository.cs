using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SeatedNow.Repositories
{
    public class BlobsRepository
    {

        string ConnectionString = "DefaultEndpointsProtocol=https;AccountName=seatednow;AccountKey=7grX562DYLZlCb7Xy7b3Zb13o5speCQJNJTUJT+gMJigTIpOuS+wj2x1lkNl7kvubHr/kTY5eSfliHDih8C7hw==;EndpointSuffix=core.windows.net";


        public CloudBlobContainer GetCloudBlobContainer()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConnectionString);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("seatednow");
            return container;
        }

        public List<string> ListBlobsNames()
        {
            List<string> blobNames = new List<string>();
            CloudBlobContainer container = GetCloudBlobContainer();
            var blobs = container.GetDirectoryReference("").ListBlobsSegmentedAsync(false, BlobListingDetails.Metadata, 100, null, null, null).Result;


            foreach (IListBlobItem item in blobs.Results)
            {
                if (item.GetType() == typeof(CloudBlockBlob))
                {
                    CloudBlockBlob blob = (CloudBlockBlob)item;
                    blobNames.Add(blob.Name);
                }
                else if (item.GetType() == typeof(CloudPageBlob))
                {
                    CloudPageBlob blob = (CloudPageBlob)item;
                    blobNames.Add(blob.Name);
                }
                else if (item.GetType() == typeof(CloudBlobDirectory))
                {
                    CloudBlobDirectory dir = (CloudBlobDirectory)item;
                    blobNames.Add(dir.Uri.ToString());
                }

            }

            return blobNames;
        }

        public async Task<bool> UploadBlobAsync(IFormFile File, string Name)
        {

            if (File == null)
            {
                return false;
            }

            Console.WriteLine("-------------------------------------------" + File.FileName);

            CloudBlobContainer container = GetCloudBlobContainer();
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(Name);
            blockBlob.Properties.ContentType = File.ContentType;

            using (var fileStream = File.OpenReadStream())
            {
                await blockBlob.UploadFromStreamAsync(fileStream);
            }

            return true;
        }

        public async Task<bool> DeleteBlobAsync(string Name)
        {
            CloudBlobContainer container = GetCloudBlobContainer();
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(Name);


            await blockBlob.DeleteIfExistsAsync();

            return true;
        }

        public async Task<bool> RenameBlobAsync(string NewName, string OldName)
        {
            CloudBlobContainer container = GetCloudBlobContainer();
            CloudBlockBlob blobCopy = container.GetBlockBlobReference(NewName);

            if (!await blobCopy.ExistsAsync())
            {
                CloudBlockBlob blob = container.GetBlockBlobReference(OldName);

                if (await blob.ExistsAsync())
                {
                    await blobCopy.StartCopyAsync(blob);
                    await blob.DeleteIfExistsAsync();
                    return true;
                }
            }

            return false;
        }

        public async Task<bool> DoesBlobExistAsync(string Name)
        {

            CloudBlobContainer container = GetCloudBlobContainer();
            CloudBlockBlob existingBlob = container.GetBlockBlobReference(Name);

            return await existingBlob.ExistsAsync();
        }
    }
}
