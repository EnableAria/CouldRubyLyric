using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CouldRubyLyric
{
    internal class GetLrc
    {
        private static readonly HttpClient client = new HttpClient();

        private static async Task<string> GetRequest(string uri)
        {
            try { return await client.GetStringAsync(uri); }
            catch (HttpRequestException e) //网络请求错误
            {
                Console.WriteLine($"请求错误: {e.Message}");
                return string.Empty;
            }
            catch (Exception e) //未知异常
            {
                Console.WriteLine($"发生错误: {e.Message}");
                return string.Empty;
            }
        }

        /// <summary>获取歌曲歌词</summary>
        /// <param name="id">歌曲id</param>
        /// <returns>歌词Json对象</returns>
        public static async Task<JObject> GetSongLrc(string id)
        {
            return JsonConvert.DeserializeObject<JObject>(await GetRequest($"{@"https://music.163.com/api/song/lyric?os=pc&id="}{id}&lv=-1&rv=-1&tv=-1")) ?? [];
        }

        /// <summary>获取歌曲名称</summary>
        /// <param name="id">歌曲id</param>
        /// <returns>歌曲名称字符串</returns>
        public static async Task<string> GetTitle(string id)
        {
            return JsonConvert.DeserializeObject<JObject>(await GetRequest($"{@"https://music.163.com/api/song/detail?ids=["}{id}]"))?["songs"]?[0]?["name"]?.ToString() ?? "Undefined";
        }
    }
}
