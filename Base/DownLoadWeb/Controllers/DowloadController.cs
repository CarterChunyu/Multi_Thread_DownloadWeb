using DownLoadWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.Models;
using Service.Models.DownloadService;

namespace DownLoadWeb.Controllers
{
    /// <summary>
    /// 下載控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class DowloadController : ControllerBase
    {
        private readonly SemaphoreDownloader _downloader;
        public DowloadController(SemaphoreDownloader semaphoreDownloader)
        {
            _downloader= semaphoreDownloader;
        }
        /// <summary>
        /// 下載
        /// </summary>
        /// <param name="baseRequest"></param>
        /// <returns></returns>
        [HttpPost("Download")]
        public BaseResponse<DownloadRep> Download([FromBody]BaseRequest<DownLoadReq> baseRequest)
        {
            BaseResponse<DownloadRep> response=new BaseResponse<DownloadRep>();
            response.Data= _downloader.Download(baseRequest.data);
            return response;
        }
        /// <summary>
        /// 補充下載
        /// </summary>
        /// <param name="baseRequest"></param>
        /// <returns></returns>
        [HttpPost("Additional")]
        public BaseResponse<DownloadRep> Addtional([FromBody]BaseRequest<AdditionalReq> baseRequest)
        {
            BaseResponse<DownloadRep> response = new BaseResponse<DownloadRep>();
            response.Data = _downloader.Additional(baseRequest.data);
            return response;
        }
            
    }
}
