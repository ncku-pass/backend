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
        /// <param name="token"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{imageId}", Name = "GetImageById")]
        public async Task<IActionResult> GetImage([FromRoute] int imageId, [FromQuery] string token)
        {
            try
            {
                var imageFileResponse = await this._imageService.GetImageAsync(imageId, token);
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
        [AllowAnonymous]
        [HttpGet(Name = "GetImageByName")]
        public async Task<IActionResult> GetImageByName([FromQuery] string imageName, [FromQuery] string token)
        {
            try
            {
                var imageFileResponse = await this._imageService.GetImageAsync(imageName, token);
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
        public async Task<IActionResult> UploadImage([FromForm] ImageUploadParameter imageUploadParameter)
        {
            try
            {
                var imageResponse = await this._imageService.UploadImageAsync(imageUploadParameter.Image);
                var imageViewModel = this._mapper.Map<ImageViewModel>(imageResponse);
                return this.Ok(imageViewModel);
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