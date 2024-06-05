using Microsoft.AspNetCore.Mvc;
using UploadResume.Services.NonServerlessResumeUploader.Services;
using UploadResume.Services;

namespace UploadResume.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApplicationController : Controller
    {

        private readonly IStorageService _storageService;
        private readonly IEmailService _emailService;

        public ApplicationController(IStorageService storageService, IEmailService emailService)
        {
            _storageService = storageService;
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<IActionResult> UploadResume()
        {
            // Copy our request body into a memory stream
            Request.EnableBuffering();
            using var fileStream = new MemoryStream();
            using var reader = new StreamReader(fileStream);
            await Request.Body.CopyToAsync(fileStream);

            // Upload the stream contents to cloud storage
            var storedFileUrl = await _storageService.Upload(fileStream);

            // Email recruitment team with a link to the file
            await _emailService.Send("enmanuelcruzdejesus@gmail.com",
                $"Somebody has uploaded a resume! Read it here: {storedFileUrl}");

            return Ok();
        }

    }
}
