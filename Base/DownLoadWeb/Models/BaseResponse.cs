namespace DownLoadWeb.Models
{
    public class BaseResponse<T> where T : new()
    {
        public T Data { get; set; }
    }
}
