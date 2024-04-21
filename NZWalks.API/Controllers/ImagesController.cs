using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        //POST: api/Images/Upload
        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Upload([FromForm] ImageUploadRequestDto requestDto)
        {
            ValidateFileUpload(requestDto);
            if(ModelState.IsValid)
            {
                //Convert DTO to Domain Model
                var imageDomainModel = new Image
                {
                    File = requestDto.File,
                    FileExtension = Path.GetExtension(requestDto.File.FileName),
                    FileName = requestDto.FileName,
                    FileSizeInBytes = requestDto.File.Length,
                    FileDescription = requestDto.FileDescription,

                };
                
                
                
                //User repository to upload image
            }
            return BadRequest(ModelState);
        }

        public void ValidateFileUpload(ImageUploadRequestDto requestDto)
        {
            var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };
            if (!allowedExtensions.Contains(Path.GetExtension(requestDto.File.FileName)))
            {
                ModelState.AddModelError("file", "Unsupported file extension");
            }

            if(requestDto.File.Length > 10485760)
            {
                ModelState.AddModelError("file", "File size morethan 10 MB< Please upload smaller size file.");
            }
        }
    }
}
