namespace CouldRubyLyric
{
    internal struct LyricLrc
    {
        public string lyric { get; set; }
    }

    internal class Lyric
    {
        private string? _lrc, _tlyric, _romalrc;
        public LyricLrc lrc { set { _lrc = value.lyric; } }
        public LyricLrc tlyric { set { _tlyric = value.lyric; } }
        public LyricLrc romalrc { set { _romalrc = value.lyric; } }
        public string? Lrc { get => _lrc; }
        public string? TLrc { get => _tlyric; }
        public string? RLrc { get => _romalrc; }
    }
}
