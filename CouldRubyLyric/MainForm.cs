using System.Diagnostics;
using System.Text.RegularExpressions;

namespace CouldRubyLyric
{
    public partial class MainForm : Form
    {
        private bool _notRuby, _notTranslation, _useFullSpace, _skipNotTR;
        private bool ObserveClipboard { get; set; }
        private bool NotRuby
        {
            get { return _notRuby; }
            set
            {
                _notRuby = value;
                LrcFile.notOutputRuby = value;
            }
        }
        private bool NotTranslation
        {
            get { return _notRuby; }
            set
            {
                _notTranslation = value;
                LrcFile.notOutputTranslation = value;
            }
        }
        private bool UseFullSpace
        {
            get { return _useFullSpace; }
            set
            {
                _useFullSpace = value;
                Converter.useFullSpace = value;
            }
        }
        private bool SkipNotTR
        {
            get { return _skipNotTR; }
            set
            {
                _skipNotTR = value;
                LrcFile.skipNotTR = value;
            }
        }

        public MainForm()
        {
            InitializeComponent();
        }

        private void Form_Load(object sender, EventArgs e)
        {
            ObserveClipboard = true;
            NotRuby = false;
            NotTranslation = false;
            UseFullSpace = true;
            SkipNotTR = true;
        }

        private void Form_Activated(object sender, EventArgs e)
        {
            //窗口活动时检测剪贴板
            if (ObserveClipboard)
            {
                IDataObject? dataObject = Clipboard.GetDataObject();
                if (dataObject?.GetDataPresent(DataFormats.Text) ?? false)
                {
                    string text = (string)(dataObject.GetData(DataFormats.Text) ?? string.Empty);
                    string idText = Regex.Matches(text, @"\d+").Cast<Match>().Select(m => m.Value).ToList().OrderByDescending(s => s.Length).FirstOrDefault() ?? string.Empty;
                    if (!string.IsNullOrEmpty(idText)) { id.Text = idText; }
                }
            }
        }
        private void Id_TextChanged(object sender, EventArgs e)
        {
            //ID文本框改变响应
            start.Enabled = (!string.IsNullOrEmpty(id.Text) && !Regex.Match(id.Text, @"\D+").Success);
        }

        private async void Start_Click(object sender, EventArgs e)
        {
            id.Enabled = false; subtitle.Enabled = false; start.Enabled = false; cancel.Enabled = true;
            await Converter.Run(id.Text, string.IsNullOrEmpty(subtitle.Text) ? null : subtitle.Text);
            id.Enabled = true; subtitle.Enabled = true; start.Enabled = true; cancel.Enabled = false;
        }

        private void Cancel_Click(object sender, EventArgs e)
        {

        }

        private void AdvCheck_CheckedChanged(object sender, EventArgs e)
        {
            advGroup.Enabled = advCheck.Checked;
        }

        private void ObserveClipboard_CheckedChanged(object sender, EventArgs e)
        {
            ObserveClipboard = observeClipboard.Checked;
        }

        private void NotOutputRuby_CheckedChanged(object sender, EventArgs e)
        {
            NotRuby = notOutputRuby.Checked;
        }

        private void NotOutputTranslation_CheckedChanged(object sender, EventArgs e)
        {
            NotTranslation = notOutputTranslation.Checked;
        }

        private void useFullSpace_CheckedChanged(object sender, EventArgs e)
        {
            UseFullSpace = useFullSpace.Checked;
        }

        private void SkipNotTR_CheckedChanged(object sender, EventArgs e)
        {
            SkipNotTR = skipNotTR.Checked;
        }

        private void Link_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo(Program.GithubUrl) { UseShellExecute = true });
        }

        private void advGroup_Enter(object sender, EventArgs e)
        {

        }
    }
}