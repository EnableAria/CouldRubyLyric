using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Text.RegularExpressions;

namespace CouldRubyLyric
{
    internal class LrcFile
    {
        public static bool notOutputRuby = false;
        public static bool notOutputTranslation = false;
        public static bool skipNotTR = true;

        /// <summary>保存歌词为Docx文件</summary>
        /// <param name="lrc">解析歌词</param>
        /// <param name="title">歌曲标题</param>
        /// <param name="subtitle">歌曲副标题</param>
        public static void SaveDocX(List<Lrc> lrc, string title, string? subtitle = null)
        {
            string filePath = $"{@"./"}{title}.docx";
            try
            {
                using (WordprocessingDocument doc = WordprocessingDocument.Create(filePath, WordprocessingDocumentType.Document)) //System.IO.IOException
                {
                    //添加主文档部件
                    MainDocumentPart mainPart = doc.AddMainDocumentPart();
                    mainPart.Document = new Document();
                    Body body = mainPart.Document.AppendChild(new Body());
                    body.AppendChild(new SectionProperties(
                        new PageSize() { Width = 6803U, Height = 30899U, Orient = PageOrientationValues.Portrait }, //定义宽高和方向
                        new PageMargin() { Top = 227, Bottom = 0, Left = 0, Right = 0, Header = 0})); //定义页边距

                    if(subtitle == null) { body.AppendChild(TitleParagraph(title)); } //添加单一主标题
                    else //添加主副标题
                    {
                        body.AppendChild(TitleParagraph(title, true));
                        body.AppendChild(TitleParagraph(subtitle, true, true));
                    }
                    bool hasTranslation = !lrc.All(l => string.IsNullOrEmpty(l.Translation)); //判断是否为单语歌词(无翻译歌词)
                    foreach (var lrc_item in lrc)
                    {
                        if (!(skipNotTR && string.IsNullOrEmpty(lrc_item.Roma) && string.IsNullOrEmpty(lrc_item.Translation)) || !hasTranslation) { body.AppendChild(LrcParagraph(lrc_item, hasTranslation ? lrc_item.Translation : null)); }
                    }
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show($"无法保存文件：目标文件可能正在被使用\n\n{ex}", "ERROR");
            }
        }

        /// <summary>Docx添加标题段落</summary>
        /// <param name="text">文本</param>
        /// <param name="doubleTitle">属于双标题格式</param>
        /// <param name="isSubtitle">属于副标题</param>
        /// <returns>标题段落</returns>
        private static Paragraph TitleParagraph(string text, bool doubleTitle = false, bool isSubtitle = false)
        {
            SpacingBetweenLines spacing;
            if (doubleTitle) //双标题格式
            {
                if (isSubtitle) { spacing = new SpacingBetweenLines() { After = "0" }; }
                else { spacing = new SpacingBetweenLines() { LineRule = LineSpacingRuleValues.Exact, Line = "500", Before = "240", After = "0" }; }
            }
            else { spacing = new SpacingBetweenLines() { Before = "240", After = "0" }; }
            return new Paragraph(
                new ParagraphProperties( //段落样式
                    spacing,
                    new Justification() { Val = JustificationValues.Center }),
                new Run(
                    new RunProperties( //文本样式
                        new RunFonts() { Hint = FontTypeHintValues.EastAsia },
                        new Bold(),
                        new FontSize() { Val = isSubtitle ? "18" : "28" }),
                    new Text(text))); //文本内容
        }

        /// <summary>Docx添加正文段落</summary>
        /// <param name="lrc">歌词文本</param>
        /// <param name="translation">属于翻译歌词</param>
        /// <returns>正文段落</returns>
        private static Paragraph LrcParagraph(Lrc lrc, string? translation = null)
        {
            Paragraph para = new Paragraph(
                new ParagraphProperties( //段落样式
                    new SpacingBetweenLines() { Before = "249", BeforeLines = 80, After = "0" },
                    new Justification() { Val = JustificationValues.Center }));
            if(translation != null) //双语歌词时
            {
                foreach(var item in lrc.Pronunciation)
                {
                    if (!notOutputRuby && item.IsKanji && !string.IsNullOrEmpty(item.Hira)) //汉字时
                    {
                        para.AppendChild(
                            new Run(
                                new RunProperties( //文本样式
                                    new RunFonts() { Hint = FontTypeHintValues.EastAsia },
                                    new FontSize() { Val = "21" }),
                                new Ruby(
                                    new RubyProperties( //文本样式
                                        new RubyAlign() { Val = RubyAlignValues.DistributeSpace },
                                        new PhoneticGuideTextFontSize() { Val = "14" }, //注音文本字号
                                        new PhoneticGuideRaise() { Val = 23 }, //注音文本偏移量
                                        new PhoneticGuideBaseTextSize() { Val = "28" }), //基底文本字号
                                    new RubyContent(
                                        new Run(
                                            new RunProperties( //注音文本样式
                                                new RunFonts() { Hint = FontTypeHintValues.EastAsia, EastAsia = "等线", HighAnsi = "等线", Ascii = "等线" },
                                                new FontSize() { Val = "14" }),
                                            new Text(item.Hira))), //注音文本内容(平假名)
                                    new RubyBase(
                                        new Run(
                                            new RunProperties(
                                                new RunFonts() { Hint = FontTypeHintValues.EastAsia }),
                                            new Text(item.Kanji)))))); //基底文本内容(汉字)
                    }
                    else //非汉字时
                    {
                        Text text;
                        if (!Converter.useFullSpace && Regex.IsMatch(item.Kanji, @"^[ ]+$")) { text = new Text(item.Kanji) { Space = SpaceProcessingModeValues.Preserve }; } //输出单半角空格
                        else { text = new Text(item.Kanji); }
                        para.AppendChild(
                            new Run(
                                new RunProperties( //文本样式
                                    new RunFonts() { Hint = FontTypeHintValues.EastAsia },
                                    new FontSize() { Val = "21" }),
                                text)); //文本内容
                    }   
                }
                if (!notOutputTranslation)
                {
                    para.Append(
                    new Run(
                        new RunProperties(
                            new RunFonts() { Hint = FontTypeHintValues.EastAsia },
                            new FontSize() { Val = "21" }),
                        new Break()), //换行符
                    new Run(
                        new RunProperties(
                            new RunFonts() { Hint = FontTypeHintValues.EastAsia },
                            new FontSize() { Val = "21" }),
                        new Text(translation))); //翻译文本
                }
            }
            else //单语歌词时
            {
                para.AppendChild(
                    new Run(
                        new RunProperties(
                            new RunFonts() { Hint = FontTypeHintValues.EastAsia },
                            new FontSize() { Val = "21" }),
                        new Text(lrc.Lyrics))); //歌词文本
            }
            return para;
        }
    }
}
