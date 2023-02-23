using Api.RequestModel.Parameters;
using Application.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Authorize]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [Route("api/images")]
    public class ImagesController : Controller
    {
        private readonly IImageService _imageService;

        public ImagesController(IImageService imageService)
        {
            this._imageService = imageService;
        }

        /// <summary>
        /// 根據圖檔名取得圖片
        /// </summary>
        /// <param name="imageName"></param>
        /// <returns></returns>
        [HttpGet("{imageName}", Name = "GetImage")]
        public async Task<IActionResult> GetImage([FromRoute] string imageName)
        {
            var result = await this._imageService.GetImageAsync(imageName);
            return File(result, "image/" + Path.GetExtension(imageName).Replace(".", ""));
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage([FromForm] ImageUploadParameter imageUploadParameter)
        {
            List<string> imageNames = await this._imageService.UploadImageAsync(imageUploadParameter.ImageFiles);
            var routeNames = string.Join(",", imageNames.Select(i => i.ToString()));
            return this.CreatedAtRoute(
                            "GetImage",
                            new { imageName = routeNames },
                            imageNames
                        );
        }


        [HttpDelete("{imageName}")]
        public async Task<IActionResult> DeleteImage([FromRoute] string imageName)
        {
            await this._imageService.DeleteImageAsync(imageName);
            return this.Ok();
        }
    }
}
