using Api.RequestModel.Parameters;
using Api.RequestModel.ViewModels;
using Application.Services.Interface;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
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
        private readonly IMapper _mapper;

        public ImagesController(
            IImageService imageService,
            IMapper mapper
            )
        {
            this._imageService = imageService;
            this._mapper = mapper;
        }

        /// <summary>
        /// 根據圖檔Id取得圖片
        /// </summary>
        /// <param name="imageId"></param>
        /// <returns></returns>
        [HttpGet("{imageId}", Name = "GetImage")]
        public async Task<IActionResult> GetImage([FromRoute] int imageId)
        {
            var imageFileResponse = await this._imageService.GetImageAsync(imageId);
            return File(imageFileResponse.ImageBytes, "image/" + Path.GetExtension(imageFileResponse.Name).Replace(".", ""));
        }

        /// <summary>
        /// 上傳圖片
        /// </summary>
        /// <param name="imageUploadParameter"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> UploadImage([FromForm] ImageUploadParameter imageUploadParameter)
        {
            var imageResponses = await this._imageService.UploadImageAsync(imageUploadParameter.ImageFiles);
            var imageViewModels = this._mapper.Map<List<ImageViewModel>>(imageResponses);

            return this.Ok(imageViewModels);
        }


        /// <summary>
        /// 刪除圖片
        /// </summary>
        /// <param name="imageId"></param>
        /// <returns></returns>
        [HttpDelete("{imageId}")]
        public async Task<IActionResult> DeleteImage([FromRoute] int imageId)
        {
            await this._imageService.DeleteImageAsync(imageId);
            return this.Ok();
        }
    }
}
