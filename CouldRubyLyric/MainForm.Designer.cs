using System.Diagnostics;
using System.Security.Policy;

namespace CouldRubyLyric
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            id = new TextBox();
            idLabel = new Label();
            subtitle = new TextBox();
            subLabel = new Label();
            start = new Button();
            cancel = new Button();
            advCheck = new CheckBox();
            advGroup = new GroupBox();
            useFullSpace = new CheckBox();
            skipNotTR = new CheckBox();
            notOutputTranslation = new CheckBox();
            notOutputRuby = new CheckBox();
            observeClipboard = new CheckBox();
            toolTip = new ToolTip(components);
            link = new LinkLabel();
            version = new Label();
            advGroup.SuspendLayout();
            SuspendLayout();
            // 
            // id
            // 
            id.Location = new Point(39, 12);
            id.Name = "id";
            id.Size = new Size(100, 23);
            id.TabIndex = 0;
            id.TextChanged += Id_TextChanged;
            // 
            // idLabel
            // 
            idLabel.AutoSize = true;
            idLabel.Location = new Point(12, 15);
            idLabel.Name = "idLabel";
            idLabel.Size = new Size(21, 17);
            idLabel.TabIndex = 1;
            idLabel.Text = "ID";
            // 
            // subtitle
            // 
            subtitle.Location = new Point(211, 12);
            subtitle.Name = "subtitle";
            subtitle.PlaceholderText = "可空";
            subtitle.Size = new Size(100, 23);
            subtitle.TabIndex = 2;
            // 
            // subLabel
            // 
            subLabel.AutoSize = true;
            subLabel.Location = new Point(161, 15);
            subLabel.Name = "subLabel";
            subLabel.Size = new Size(44, 17);
            subLabel.TabIndex = 3;
            subLabel.Text = "副标题";
            // 
            // start
            // 
            start.Enabled = false;
            start.Location = new Point(236, 51);
            start.Name = "start";
            start.Size = new Size(75, 23);
            start.TabIndex = 4;
            start.Text = "生成";
            start.UseVisualStyleBackColor = true;
            start.Click += Start_Click;
            // 
            // cancel
            // 
            cancel.Enabled = false;
            cancel.Location = new Point(236, 80);
            cancel.Name = "cancel";
            cancel.Size = new Size(75, 23);
            cancel.TabIndex = 5;
            cancel.Text = "取消";
            cancel.UseVisualStyleBackColor = true;
            cancel.Click += Cancel_Click;
            // 
            // advCheck
            // 
            advCheck.AutoSize = true;
            advCheck.Location = new Point(15, 51);
            advCheck.Name = "advCheck";
            advCheck.Size = new Size(75, 21);
            advCheck.TabIndex = 6;
            advCheck.Text = "高级设置";
            advCheck.UseVisualStyleBackColor = true;
            advCheck.CheckedChanged += AdvCheck_CheckedChanged;
            // 
            // advGroup
            // 
            advGroup.Controls.Add(useFullSpace);
            advGroup.Controls.Add(skipNotTR);
            advGroup.Controls.Add(notOutputTranslation);
            advGroup.Controls.Add(notOutputRuby);
            advGroup.Controls.Add(observeClipboard);
            advGroup.Enabled = false;
            advGroup.Location = new Point(12, 51);
            advGroup.Name = "advGroup";
            advGroup.Size = new Size(193, 157);
            advGroup.TabIndex = 7;
            advGroup.TabStop = false;
            advGroup.Text = "高级设置";
            advGroup.Enter += advGroup_Enter;
            // 
            // useFullSpace
            // 
            useFullSpace.AutoSize = true;
            useFullSpace.Checked = true;
            useFullSpace.CheckState = CheckState.Checked;
            useFullSpace.Location = new Point(6, 103);
            useFullSpace.Name = "useFullSpace";
            useFullSpace.Size = new Size(99, 21);
            useFullSpace.TabIndex = 4;
            useFullSpace.Text = "使用全角空格";
            toolTip.SetToolTip(useFullSpace, "替换半角空格为全角空格");
            useFullSpace.UseVisualStyleBackColor = true;
            useFullSpace.CheckedChanged += useFullSpace_CheckedChanged;
            // 
            // skipNotTR
            // 
            skipNotTR.AutoSize = true;
            skipNotTR.Checked = true;
            skipNotTR.CheckState = CheckState.Checked;
            skipNotTR.Location = new Point(6, 130);
            skipNotTR.Name = "skipNotTR";
            skipNotTR.Size = new Size(123, 21);
            skipNotTR.TabIndex = 3;
            skipNotTR.Text = "跳过无翻译无注音";
            toolTip.SetToolTip(skipNotTR, "针对双语歌词跳过部分空行和制作信息(可能误跳)");
            skipNotTR.UseVisualStyleBackColor = true;
            skipNotTR.CheckedChanged += SkipNotTR_CheckedChanged;
            // 
            // notOutputTranslation
            // 
            notOutputTranslation.AutoSize = true;
            notOutputTranslation.Location = new Point(6, 76);
            notOutputTranslation.Name = "notOutputTranslation";
            notOutputTranslation.Size = new Size(87, 21);
            notOutputTranslation.TabIndex = 2;
            notOutputTranslation.Text = "不输出翻译";
            toolTip.SetToolTip(notOutputTranslation, "不输出双语歌词中的翻译部分");
            notOutputTranslation.UseVisualStyleBackColor = true;
            notOutputTranslation.CheckedChanged += NotOutputTranslation_CheckedChanged;
            // 
            // notOutputRuby
            // 
            notOutputRuby.AutoSize = true;
            notOutputRuby.Location = new Point(6, 49);
            notOutputRuby.Name = "notOutputRuby";
            notOutputRuby.Size = new Size(87, 21);
            notOutputRuby.TabIndex = 1;
            notOutputRuby.Text = "不输出注音";
            toolTip.SetToolTip(notOutputRuby, "不对双语歌词注音");
            notOutputRuby.UseVisualStyleBackColor = true;
            notOutputRuby.CheckedChanged += NotOutputRuby_CheckedChanged;
            // 
            // observeClipboard
            // 
            observeClipboard.AutoSize = true;
            observeClipboard.Checked = true;
            observeClipboard.CheckState = CheckState.Checked;
            observeClipboard.Location = new Point(6, 22);
            observeClipboard.Name = "observeClipboard";
            observeClipboard.Size = new Size(87, 21);
            observeClipboard.TabIndex = 0;
            observeClipboard.Text = "检测剪贴板";
            toolTip.SetToolTip(observeClipboard, "检测剪贴板中的id");
            observeClipboard.UseVisualStyleBackColor = true;
            observeClipboard.CheckedChanged += ObserveClipboard_CheckedChanged;
            // 
            // link
            // 
            link.AutoSize = true;
            link.LinkBehavior = LinkBehavior.NeverUnderline;
            link.LinkColor = SystemColors.GrayText;
            link.Location = new Point(265, 177);
            link.Name = "link";
            link.Size = new Size(46, 17);
            link.TabIndex = 9;
            link.TabStop = true;
            link.Text = "Github";
            link.TextAlign = ContentAlignment.MiddleRight;
            toolTip.SetToolTip(link, "访问Github页面");
            link.VisitedLinkColor = SystemColors.GrayText;
            link.Click += Link_Click;
            // 
            // version
            // 
            version.AutoSize = true;
            version.ForeColor = SystemColors.ButtonShadow;
            version.Location = new Point(262, 194);
            version.Name = "version";
            version.Size = new Size(49, 17);
            version.TabIndex = 8;
            version.Text = "Ver 1.0";
            version.TextAlign = ContentAlignment.MiddleRight;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(323, 220);
            Controls.Add(link);
            Controls.Add(version);
            Controls.Add(advCheck);
            Controls.Add(cancel);
            Controls.Add(start);
            Controls.Add(subLabel);
            Controls.Add(subtitle);
            Controls.Add(idLabel);
            Controls.Add(id);
            Controls.Add(advGroup);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "CouldRubyLyric";
            Activated += Form_Activated;
            Load += Form_Load;
            advGroup.ResumeLayout(false);
            advGroup.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox id;
        private Label idLabel;
        private TextBox subtitle;
        private Label subLabel;
        private Button start;
        private Button cancel;
        private CheckBox advCheck;
        private GroupBox advGroup;
        private CheckBox skipNotTR;
        private CheckBox notOutputTranslation;
        private CheckBox notOutputRuby;
        private CheckBox observeClipboard;
        private ToolTip toolTip;
        private Label version;
        private LinkLabel link;
        private CheckBox useFullSpace;
    }
}
