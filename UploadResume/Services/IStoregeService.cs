namespace UploadResume.Services
{
    using Amazon;
    using Amazon.S3;
    using Amazon.S3.Model;
    using System;
    using System.IO;
    using System.Threading.Tasks;

    namespace NonServerlessResumeUploader.Services
    {
        public interface IStorageService
        {
            Task<string> Upload(Stream stream);
        }


        public class AwsS3StorageService : IStorageService
        {
            const string BucketName = "csharp-examples-bucket";

            public async Task<string> Upload(Stream stream)
            {
                // Create a unique S3 key name
                var fileName = Guid.NewGuid().ToString() + ".pdf";

                using var s3Client = new AmazonS3Client(RegionEndpoint.EUWest2);

                // Upload the file to S3
                await s3Client.PutObjectAsync(new PutObjectRequest()
                {
                    InputStream = stream,
                    BucketName = BucketName,
                    Key = fileName,
                });

                // Generate a presigned url pointing to our new file
                var url = s3Client.GetPreSignedURL(new GetPreSignedUrlRequest()
                {
                    BucketName = BucketName,
                    Key = fileName,
                    Expires = DateTime.UtcNow.AddMinutes(10)
                });

                return url;
            }
        }
    }
}
