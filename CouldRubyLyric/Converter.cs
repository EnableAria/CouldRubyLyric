using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace CouldRubyLyric
{
    internal struct SpHira
    {
        public string basic { get; set; }
        public string ambiguity { get; set; }
    }
    internal class Converter
    {
        public static bool useFullSpace = true;
        private static List<Lrc> lrc_list = [];
        private static readonly JObject hira;
        private static readonly SpHira[] spHira;
        static Converter()
        {
            JObject? json;
            Assembly assembly = Assembly.GetExecutingAssembly();
            using (Stream stream = assembly.GetManifestResourceStream("CouldRubyLyric.hira.json") ?? throw new Exception())
            using (StreamReader reader = new StreamReader(stream)) { json = JsonConvert.DeserializeObject<JObject>(reader.ReadToEnd()); }
            hira = JsonConvert.DeserializeObject<JObject>(json?["Match"]?.ToString() ?? string.Empty) ?? [];
            spHira = JsonConvert.DeserializeObject<SpHira[]>(json?["Ambiguity"]?.ToString() ?? string.Empty) ?? [];
        }

        /// <summary>生成注音歌词</summary>
        /// <param name="id">歌曲id</param>
        /// <param name="subtitle">副标题</param>
        public static async Task Run(string id, string? subtitle = null)
        {
            lrc_list = [];
            JObject json = await GetLrc.GetSongLrc(id);
            Conversion(json?["lrc"]?["lyric"]?.ToString() ?? string.Empty, json?["tlyric"]?["lyric"]?.ToString() ?? string.Empty, json?["romalrc"]?["lyric"]?.ToString() ?? string.Empty);
            LrcFile.SaveDocX(lrc_list, await GetLrc.GetTitle(id), subtitle);
        }

        /// <summary>格式转换(初始化)</summary>
        /// <param name="lrc">原歌词</param>
        /// <param name="tlrc">翻译歌词</param>
        /// <param name="rlrc">罗马歌词</param>
        private static void Conversion(string lrc, string tlrc, string rlrc)
        {
            //歌词导入列表
            Match match;
            tlrc += '\n'; rlrc += '\n'; //末尾添加换行保证正则匹配正确
            if (useFullSpace) { lrc = lrc.Replace(" ", "　"); tlrc = tlrc.Replace(" ", "　"); } //空格替换
            string[] lrc_arr = lrc.Split(['\n']);
            foreach (var item in lrc_arr)
            {
                match = Regex.Match(item, @"(?<=\[).+?(?=\])"); //匹配时间标签
                if (match.Success)
                {
                    //增添列表项
                    string time = match.Groups[0].Value;
                    Match tmatch = Regex.Match(tlrc, $"(?<=\\[{time}).+(?=\n)");
                    Match rmatch = Regex.Match(rlrc, $"(?<=\\[{time}).+(?=\n)");
                    string _tlrc = tmatch.Success ? tmatch.Groups[0].Value[(tmatch.Groups[0].Value.IndexOf(']') + 1)..] : string.Empty;
                    string _rlrc = rmatch.Success ? rmatch.Groups[0].Value[(rmatch.Groups[0].Value.IndexOf(']') + 1)..] : string.Empty;
                    lrc_list.Add(new Lrc(time, item[(time.Length + 2)..].Trim(), _tlrc.Trim(), _rlrc, RToHTranslator(_rlrc)));
                    lrc_list.Last().Pronunciation = LrcPronunciation(lrc_list.Last().Lyrics, lrc_list.Last().Hira);
                }
            }
            lrc_list.Sort(); //列表排序
        }

        /// <summary>罗马字转平假名</summary>
        /// <param name="roma">罗马歌词单句</param>
        /// <returns>平假名歌词</returns>
        private static string RToHTranslator(string roma) //罗马字转平假名实现
        {
            StringBuilder hiraStr = new();
            Regex headNotSpace = new Regex(@"(?<! )[^ A-Za-z]+");
            Regex tailNotSpace = new Regex(@"[^ A-Za-z]+(?! )");
            while (headNotSpace.IsMatch(roma)) { roma = roma.Insert(headNotSpace.Match(roma).Index, " "); }
            while (tailNotSpace.IsMatch(roma)) { roma = roma.Insert(tailNotSpace.Match(roma).Index + 1, " "); }
            roma = Regex.Replace(roma, @"\p{P}+", " "); //替换标点字符
            string[] roma_words = roma.Split([' ', '\n']); //分离罗马单音
            foreach (var word in roma_words)
            {
                if (hira?[word] != null) //直接匹配
                {
                    hiraStr.Append(hira?[word]);
                }
                else if (word.Length > 1 && (word[0] == word[1] || word[word.Length-2] == word[word.Length-1])) //促音长音匹配
                {
                    int flag = ((word[0] == word[1] && word.Length != 2) ? 2 : 0) + ((word[word.Length - 2] == word[word.Length - 1]) ? 1 : 0); //长度为2时优先长音
                    hiraStr.Append(flag >= 2 ? hira?["--"] : string.Empty); //促音
                    hiraStr.Append(hira?[word[(flag / 2)..(word.Length - (flag % 2))]]);
                    hiraStr.Append(flag % 2 == 1 ? hira?[word[word.Length - 1].ToString()] : string.Empty); //长音
                }
                else //无法匹配
                {
                    hiraStr.Append(word);
                }
            }
            return hiraStr.ToString();
        }

        /// <summary>汉字注音</summary>
        /// <param name="kanji">日文汉字</param>
        /// <param name="hira">片假名</param>
        /// <returns>汉字注音对照列表</returns>
        private static List<Pronunciation> KanjiPronunciation(string kanji, string hira)
        {
            List<Pronunciation> pronunciation = [];
            void AddKanji(string kanji, string hira) //汉字添加
            {
                //string rule = @" 　.,`'""()<>?!:;。，·—…‘’“”（）《》、？！：；・「」『』〔〕";
                string rule = @"\s\p{P}";
                if (Regex.IsMatch(kanji, $"^[{rule}]+$")) { pronunciation.Add(new Pronunciation(kanji)); } //仅标点空格时
                else if (kanji.Equals(hira)) { pronunciation.Add(new Pronunciation(kanji)); } //汉字与注音相等时
                else
                {
                    int flag = ((kanji.Length > 1) ? (Regex.IsMatch(kanji, $"^[{rule}]+.+$") ? 2 : 0) + (Regex.IsMatch(kanji, $"^.+[{rule}]+$") ? 1 : 0) : 0); //前后标点空格匹配
                    string[] match = Regex.Matches(kanji, $"[{rule}]+").Cast<Match>().Select(m => m.Value).ToArray();
                    int index = (match.Length > 0 && flag >= 2) ? match[0].Length : 0;
                    int last_index = (match.Length > 0 && flag % 2 == 1) ? kanji.Length - match[match.Length - 1].Length : kanji.Length;
                    if (flag >= 2) { pronunciation.Add(new Pronunciation(kanji[..index])); } //前标点空格
                    pronunciation.Add(new Pronunciation(kanji[index..last_index], hira));
                    if (flag % 2 == 1) { pronunciation.Add(new Pronunciation(kanji[last_index..])); } //后标点空格
                }
            }
            AddKanji(kanji.Replace('(', '（').Replace(')', '）'), hira);
            return pronunciation;
        }

        /// <summary>日文歌词注音(动态规划)</summary>
        /// <param name="lrc">日文歌词单句</param>
        /// <param name="hira">平假名歌词单句</param>
        /// <returns>汉字注音对照列表</returns>
        public static List<Pronunciation> LrcPronunciation(string lrc, string hira)
        {
            if (string.IsNullOrEmpty(hira)) { return new List<Pronunciation>([new Pronunciation(lrc)]); } //无平假名直接返回
            List<Pronunciation> pronunciation = [];
            int l_end = 0, h_end = 0, l_end_old = 0, h_end_old = 0 ;
            int[,] DP = new int[lrc.Length + 1, hira.Length + 1]; //动态规划数组
            for (int i = 1; i <= lrc.Length; i++)
            {
                for (int j = 1; j <= hira.Length; j++)
                {
                    bool hasAmbiguity = spHira.Any(sh => hira[j - 1].ToString() == sh.basic && lrc[i - 1].ToString() == sh.ambiguity); //判断是否为歧义字符
                    ///动态规划
                    ///X[i-1] != Y[j-1], DP[i, j] = 0
                    ///X[i-1] == Y[j-1], DP[i, j] = DP[i-1, j-1] + 1
                    ///特殊的存在歧义字符时，进入2
                    DP[i, j] = (lrc[i - 1] == hira[j - 1] || hasAmbiguity) ? (DP[i - 1, j - 1] + 1) : 0;
                    if (DP[i, j] != 0 && i >= l_end && j > h_end) //不为零且位于右下方时更新末位
                    {
                        h_end = (i == l_end && DP[i, j] <= DP[l_end, h_end]) ? h_end : j; //同行存在更大解更新为j
                        l_end = i;
                    }
                }
                if ((i == l_end + 1 && l_end != 0) || (i == lrc.Length && l_end == lrc.Length)) //本行对应位置无可行解或位于最末行时
                {
                    int l_start = (l_end - DP[l_end, h_end] < l_end_old) ? l_end_old : (l_end - DP[l_end, h_end]);
                    int h_start = (h_end - DP[l_end, h_end] < h_end_old) ? h_end_old : (h_end - DP[l_end, h_end]);
                    if (l_start != 0) { pronunciation.AddRange(KanjiPronunciation(lrc[l_end_old..l_start], hira[h_end_old..h_start])); } //汉字注音(首位为平假名时跳过本次)
                    pronunciation.Add(new Pronunciation(lrc[l_start..l_end])); //平假名
                    l_end_old = l_end; h_end_old = h_end; //保存为旧参数
                }
            }
            if (l_end != lrc.Length) { pronunciation.AddRange(KanjiPronunciation(lrc[l_end..], hira[h_end..]));} //末位汉字注音
            return pronunciation;
        }

        /// <summary>输出解析后歌词</summary>
        public static void WriteLrc()
        {
            foreach (var lrc_item in lrc_list)
            {
                foreach (var item in lrc_item.Pronunciation)
                {
                    //Console.Write(item.IsSpace ? "　" : item.IsKanji ? $"{item.Kanji}({item.Hira})" : $"{item.Hira}");
                    Console.Write(item.IsKanji ? $"[{item.Kanji}({item.Hira})]" : $"{item.Hira}");
                }
                Console.WriteLine($"\n{lrc_item.Translation}");
            }
        }
    }
}
