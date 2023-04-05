using Service.Models;
using Service.Models.DownloadService;
using System.Net.Http;

namespace Service
{
    public class SemaphoreDownloader
    {
        private readonly IHttpClientFactory _clientFactory;             
        private static readonly object LOCKError=new object();
        public SemaphoreDownloader(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }
        private void CreatHttpclient(int Count,ref HttpclientObj[] httpclientObjs)
        {
            httpclientObjs=
            Enumerable.Range(0, Count).Select(i => { return 
                new HttpclientObj() { client = _clientFactory.CreateClient(), IsUsed = false }; }).ToArray();
        }
        public DownloadRep Download(DownLoadReq req)
        {
            DownloadRep rep = new DownloadRep();
            HttpclientObj[] httpclientObjs = new HttpclientObj[req.ThreadCount];
            CreatHttpclient(req.ThreadCount, ref httpclientObjs);
            object LOCKChoose = new object();
            SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(initialCount: req.ThreadCount, maxCount: req.ThreadCount);
            Task.WaitAll(
            Enumerable.Range(req.Begin, req.End + 1).Select(async (i) =>
            {
                int k = i;
                // Console.WriteLine($"-----------------------------------------------------------------------{k}");                
                _semaphoreSlim.Wait();
                HttpclientObj clientObj = null;
                while (clientObj == null)
                {
                    lock (LOCKChoose)
                    {
                        clientObj = httpclientObjs.FirstOrDefault(p => p.IsUsed == false);
                    }
                }
                Uri uri = new Uri($"{req.Url}{k}.ts");
                string path = Path.Combine(req.Folder_path, $"index{i}.ts");
                await Downloader(clientObj, uri, path, rep.Errors);
                _semaphoreSlim.Release();
            }).ToArray());

            rep.Calculator(req.Begin, req.End);
            return rep;
        }        
        public DownloadRep Additional(AdditionalReq req)
        {
            DownloadRep rep = new DownloadRep();
            HttpclientObj[] httpclientObjs = new HttpclientObj[req.ThreadCount];
            CreatHttpclient(req.ThreadCount, ref httpclientObjs);
            object LOCKChoose = new object();
            SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(initialCount: req.ThreadCount, maxCount: req.ThreadCount);
            Task.WaitAll(
            req.Errors.Select(async (error) =>
            {
                _semaphoreSlim.Wait();
                HttpclientObj clientObj = null;
                while (clientObj == null)
                {
                    lock (LOCKChoose)
                    {
                        clientObj = httpclientObjs.FirstOrDefault(p => p.IsUsed == false);
                    }
                }
                await Downloader(clientObj, new Uri(error.url), error.filename, rep.Errors);
                _semaphoreSlim.Release();
            }).ToArray());
            rep.Calculator(req.Errors.Count);
            return rep;
        }
        private async Task Downloader(HttpclientObj httpclientObj,Uri url,string folder_path,List<Error> errors)
        {
            httpclientObj.IsUsed = true;
            HttpClient client = httpclientObj.client;
            try
            {
                HttpResponseMessage message =await client.GetAsync(url);
                if (message.IsSuccessStatusCode)
                {
                    Stream stream = await message.Content.ReadAsStreamAsync();
                    using (FileStream fs = new FileStream(folder_path, FileMode.CreateNew))
                    {
                        await stream.CopyToAsync(fs);
                    }
                }
                else
                {
                    lock (LOCKError)
                    {
                        errors.Add(new Error(url.OriginalString, folder_path));
                    }
                }
            }
            catch(Exception ex)
            {
                lock (LOCKError)
                {
                    errors.Add(new Error(url.OriginalString,folder_path));
                }
            }
            finally
            {
                httpclientObj.IsUsed = false;
            }
        }       
    }
}