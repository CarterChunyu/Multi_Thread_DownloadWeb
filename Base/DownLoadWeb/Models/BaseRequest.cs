namespace DownLoadWeb.Models
{
    public class BaseRequest<T> where T : new()
    {
        public T data { get; set; }
    }
}
