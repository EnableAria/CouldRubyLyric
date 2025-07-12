using System.Text.RegularExpressions;

namespace CouldRubyLyric
{
    internal struct Pronunciation
    {
        public string Kanji { get; set; }
        public string Hira { get; set; }
        public bool IsKanji { get; set; }
        public Pronunciation(string hira) : this()
        {
            Kanji = hira;
            Hira = hira;
            IsKanji = false;
        }
        public Pronunciation(string kanji, string hira) : this(hira)
        {
            Kanji = kanji;
            IsKanji = true;
        }
    }
    internal class Lrc : IComparable<Lrc>
    {
        private int Time;
        private string _time_tags = string.Empty;
        public string Time_tags {
            get { return _time_tags; }
            set {
                _time_tags = value;
                Time = int.Parse(Regex.Replace(Time_tags, @"[^\d]+", string.Empty));
            } 
        }
        public string Lyrics { get; set; }
        public string Translation { get; set; }
        public string Roma { get; set; }
        public string Hira { get; set; }
        public List<Pronunciation> Pronunciation { get; set; }

        public Lrc(string time_tags, string lyrics, string translation, string roma, string hira)
        {
            Time_tags = time_tags;
            Lyrics = lyrics;
            Translation = translation;
            Roma = roma;
            Hira = hira;
            Pronunciation = [];
        }
        public int CompareTo(Lrc? other) //列表排序接口实现
        {
            if (null == other) { return 1; }
            return Time.CompareTo(other?.Time);
        }

        public override string ToString()
        {
            return $"{Time_tags}\n{Lyrics}\n{Translation}\n{Roma}\n{Hira}";
        }
    }
}
