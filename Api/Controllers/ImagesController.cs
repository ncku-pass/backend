using Api.RequestModel.Parameters;
using Api.RequestModel.ViewModels;
using Application.Services.Interface;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
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
        [HttpGet("{imageId}", Name = "GetImageById")]
        public async Task<IActionResult> GetImage([FromRoute] int imageId)
        {

            try
            {
                var imageFileResponse = await this._imageService.GetImageAsync(imageId);
                return File(imageFileResponse.ImageBytes, "image/" + Path.GetExtension(imageFileResponse.Name).Replace(".", ""));
            }
            catch (FileNotFoundException ex)
            {
                return this.NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return this.NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return this.StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// 根據圖檔名稱取得圖片
        /// </summary>
        /// <param name="imageName"></param>
        /// <returns></returns>
        [HttpGet(Name = "GetImageByName")]
        public async Task<IActionResult> GetImageByName([FromQuery] string imageName)
        {
            try
            {
                var imageFileResponse = await this._imageService.GetImageAsync(imageName);
                return File(imageFileResponse.ImageBytes, "image/" + Path.GetExtension(imageFileResponse.Name).Replace(".", ""));
            }
            catch (FileNotFoundException ex)
            {
                return this.NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return this.StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// 上傳圖片
        /// </summary>
        /// <param name="imageUploadParameter"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> UploadImages([FromForm] ImageUploadParameter imageUploadParameter)
        {
            try
            {
                var imageResponses = await this._imageService.UploadImageAsync(imageUploadParameter.ImageFiles);
                var imageViewModels = this._mapper.Map<List<ImageViewModel>>(imageResponses);
                return this.Ok(imageViewModels);
            }
            catch (Exception ex)
            {
                return this.StatusCode(500, ex.Message);
            }
        }


        /// <summary>
        /// 刪除圖片
        /// </summary>
        /// <param name="imageId"></param>
        /// <returns></returns>
        [HttpDelete("{imageId}")]
        public async Task<IActionResult> DeleteImage([FromRoute] int imageId)
        {
            try
            {
                await this._imageService.DeleteImageAsync(imageId);
                return this.NoContent();
            }
            catch (FileNotFoundException ex)
            {
                return this.NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return this.StatusCode(500, ex.Message);
            }
        }
    }
}
