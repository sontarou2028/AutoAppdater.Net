using System.IO.Pipelines;
using System.Text.Encodings.Web;
using System.Collections.ObjectModel;
using AutoAppdater.ConsoleHosts;
using System.ComponentModel.DataAnnotations;

namespace AutoAppdater.Consoles
{
    enum ChangeType : byte
    {
        Add,
        Remove,
        Replace,
    }
    internal class ChangeInfo
    {
        internal ReadOnlyCollection<ColumnChangeInfo> Infos { get{ return infos.AsReadOnly(); } }
        ColumnChangeInfo[] infos;
        internal ChangeInfo(ColumnChangeInfo[] infos)
        {
            this.infos = infos;
        }
        internal void Add(ColumnChangeInfo info)
        {
            Array.Resize(ref infos, infos.Length + 1);
            infos[infos.Length - 1] = info;
        }
        internal void AddRange(ColumnChangeInfo[] infos)
        {
            int len = this.infos.Length;
            Array.Resize(ref this.infos, this.infos.Length + infos.Length);
            Array.Copy(infos, 0, this.infos, len, infos.Length);
        }
    }
    internal class ColumnChangeInfo
    {
        public int Priority;
        public string Sentences;
        //public string PastSentence;
        public int Column;
        public ConsoleColor[] TextColor;
        public ConsoleColor[] SceneColor;
        ///public ConsoleColor?[] DefaultTextFillColorPattern;
        //public ConsoleColor?[] DefaultSceneFillColorPattern;
        public (int start, int end) ChangeLen;
        public ChangeType ChangeTypes;
        public ColumnChangeInfo(int Priority, string Sentences, /*string PastSentence, */int Column,
        ConsoleColor[] TextColor, ConsoleColor[] SceneColor,
        /*ConsoleColor?[] DefaultTextFillColorPattern, ConsoleColor?[] DefaultSceneFillColorPattern,*/
        (int start, int end) ChangeLen, ChangeType ChangeTypes)
        {
            this.Priority = Priority;
            this.Sentences = Sentences;
            //this.PastSentence = PastSentence;
            this.Column = Column;
            this.TextColor = TextColor;
            this.SceneColor = SceneColor;
            //this.DefaultTextFillColorPattern = DefaultTextFillColorPattern;
            //this.DefaultSceneFillColorPattern = DefaultSceneFillColorPattern;
            this.ChangeLen = ChangeLen;
            this.ChangeTypes = ChangeTypes;
        }
    }
    class TeropColumnInfo
    {
        public string? InitTerop;
        public string? EndTerop;
        public (int[] value, Func<int, bool>)? InitTeropSetConditions;
        public (int[] value, Func<int, bool>)? EndTeropSetConditions;
        public ConsoleColor[]? InitTeropTextColor;
        public ConsoleColor[]? EndTeropTextColor;
        public ConsoleColor[]? InitTeropSceneColor;
        public ConsoleColor[]? EndTeropSceneColor;
    }
    class TitleColumnInfo
    {
        public string? InitTitle;
        public string? EndTitle;
        public (int[] value, Func<int, bool>)? InitTitleSetConditions;
        public (int[] value, Func<int, bool>)? EndTitleSetConditions;
        public ConsoleColor[]? InitTitleTextColor;
        public ConsoleColor[]? EndTitleTextColor;
        public ConsoleColor[]? InitTitleSceneColor;
        public ConsoleColor[]? EndTitleSceneColor;
    }
    class ColumnInfo
    {
        internal string Text { get { return text.ToString(); } }
        internal Func<int, int, int, bool>? TextSetConditions { get { return textSetConditions; } }
        internal ConsoleColor[] TextColor { get { return textColor.ToArray(); } }
        internal ConsoleColor[] TextSceneColor { get { return textSceneColor.ToArray(); } }
        internal bool SetCondition { get { return setCondition; } set { setCondition = value; } }
        internal (string Text, ConsoleColor[] TextColor, ConsoleColor[] TextSceneColor) GetLastChangeSentence{ get{ return getLCS; } }
        (string Text, ConsoleColor[] TextColor, ConsoleColor[] TextSceneColor) getLCS = ("",[],[]);
        string text = "";
        Func<int, int, int, bool>? textSetConditions = null;//title, terop, text, reesult
        bool setCondition = true;
        ConsoleColor[] textColor = [];
        ConsoleColor[] textSceneColor = [];
        public ColumnInfo() { }
        internal ColumnInfo(string text, Func<int, int, int, bool>? textSetConditions,
        bool setCondition, ConsoleColor[] textColor, ConsoleColor[] textSceneColor)
        {
            this.text = text;
            this.textSetConditions = textSetConditions;
            this.setCondition = setCondition;
            this.textColor = textColor;
            this.textSceneColor = textSceneColor;
        }
        internal bool? Func(int titleLen, int teropLen, int? textLen)
        {
            if (textSetConditions == null) return null;
            setCondition = textSetConditions(titleLen, teropLen, text.Length);
            return setCondition;
        }
        internal void ReplaceFunc(Func<int, int, int, bool> newContitions)
        {
            textSetConditions = newContitions;
        }
        internal void RemoveFunc()
        {
            textSetConditions = null;
        }
        internal bool Remove(int length, int count)
        {
            if (text.Length < length + count || length < 0 || count < 0) return false;
            ConsoleColor[] tc = new ConsoleColor[count];
            ConsoleColor[] tsc = new ConsoleColor[count];
            Array.Copy(textColor, length, tc, 0, count);
            Array.Copy(textSceneColor, length, tsc, 0, count);
            getLCS = (text.Substring(length, count), tc.ToArray(), tsc.ToArray());
            tc = new ConsoleColor[textColor.Length - length - count];
            tsc = new ConsoleColor[textSceneColor.Length - length - count];
            Array.Copy(textColor, length + count, tc, 0, textColor.Length - length - count);
            Array.Copy(textSceneColor, length + count, tc, 0, textSceneColor.Length - length - count);
            Array.Resize(ref textColor, textColor.Length - count);
            Array.Resize(ref textSceneColor, textSceneColor.Length - count);
            Array.Copy(tc, 0, textColor, length, textColor.Length - length - count);
            Array.Copy(tsc, 0, textSceneColor, length, textSceneColor.Length - length - count);
            text = text.Remove(length,count);
            return true;
        }
        internal void RemoveAll()
        {
            getLCS = (text.ToString(),textColor.ToArray(),textSceneColor.ToArray());
            text = "";
            textColor = [];
            textSceneColor = [];
        }
        internal void Replace(string text, ConsoleColor textFillColor, ConsoleColor textSceneFillColor)
        {
            ConsoleColor[] tc = new ConsoleColor[text.Length];
            ConsoleColor[] tsc = new ConsoleColor[text.Length];
            Array.Fill(tc, textFillColor);
            Array.Fill(tsc, textSceneFillColor);
            this.text = text;
            textColor = tc;
            textSceneColor = tsc;
            getLCS = (text.ToString(),tc,tsc);
        }
        internal bool Replace(string text, ConsoleColor[] textColor, ConsoleColor[] textSceneColor)
        {
            if (text.Length != textColor.Length || text.Length != textSceneColor.Length) return false;
            this.text = text;
            this.textColor = textColor;
            this.textSceneColor = textSceneColor;
            getLCS = (text.ToString(), textColor.ToArray(), textSceneColor.ToArray());
            return true;
        }
        internal void Add(string text, ConsoleColor textFillColor, ConsoleColor textSceneFillColor)
        {
            ConsoleColor[] tc = new ConsoleColor[text.Length];
            ConsoleColor[] tsc = new ConsoleColor[text.Length];
            Array.Fill(tc, textFillColor);
            Array.Fill(tsc, textSceneFillColor);
            int len = this.text.Length;
            this.text += text;
            Array.Resize(ref textColor, this.text.Length);
            Array.Resize(ref textSceneColor, this.text.Length);
            Array.Copy(tc, 0, textColor, len, tc.Length);
            Array.Copy(tsc, 0, textSceneColor, len, tsc.Length);
            getLCS = (text.ToString(), tc, tsc);
        }
        internal bool Add(string text, ConsoleColor[] textColor, ConsoleColor[] textSceneColor)
        {
            if (text.Length != textColor.Length || text.Length != textSceneColor.Length) return false;
            int len = this.text.Length;
            this.text += text;
            Array.Resize(ref this.textColor, this.text.Length);
            Array.Resize(ref this.textSceneColor, this.text.Length);
            Array.Copy(textColor, 0, this.textColor, len, textColor.Length);
            Array.Copy(textSceneColor, 0, this.textSceneColor, len, textSceneColor.Length);
            getLCS = (text.ToString(),textColor.ToArray(),textSceneColor.ToArray());
            return true;
        }
        internal bool Insert(string text, int length, ConsoleColor textFillColor, ConsoleColor textSceneFillColor)
        {
            if (text.Length < length || length < 0) return false;
            ConsoleColor[] tcf = new ConsoleColor[text.Length];
            ConsoleColor[] tscf = new ConsoleColor[text.Length];
            Array.Fill(tcf, textFillColor);
            Array.Fill(tscf, textSceneFillColor);
            getLCS = (text.ToString(), tcf.ToArray(), tscf.ToArray());
            this.text = this.text.Insert(length, text);
            ConsoleColor[] tc = new ConsoleColor[textColor.Length - length - text.Length];
            ConsoleColor[] tsc = new ConsoleColor[textSceneColor.Length - length - text.Length];
            Array.Copy(textColor, length, tc, 0, textColor.Length - length);
            Array.Copy(textSceneColor, length, tsc, 0, textSceneColor.Length - length);
            Array.Resize(ref textColor, this.text.Length);
            Array.Resize(ref textSceneColor, this.text.Length);
            Array.Copy(tcf, 0, textColor, length, tcf.Length);
            Array.Copy(tscf, 0, textSceneColor, length, tscf.Length);
            Array.Copy(tc, 0, textColor, length + text.Length, tc.Length);
            Array.Copy(tsc, 0, textSceneColor, length + text.Length, tsc.Length);
            return true;
        }
        internal bool Insert(string text, int length, ConsoleColor[] textColor, ConsoleColor[] textSceneColor)
        {
            if (text.Length != textColor.Length || text.Length != textSceneColor.Length ||
            text.Length < length || length < 0) return false;
            getLCS = (text.ToString(), this.textColor.ToArray(), this.textSceneColor.ToArray());
            this.text = this.text.Insert(length, text);
            ConsoleColor[] tc = new ConsoleColor[this.textColor.Length - length - text.Length];
            ConsoleColor[] tsc = new ConsoleColor[this.textSceneColor.Length - length - text.Length];
            Array.Copy(this.textColor, length, tc, 0, this.textColor.Length - length);
            Array.Copy(this.textSceneColor, length, tsc, 0, this.textSceneColor.Length - length);
            Array.Resize(ref this.textColor, this.text.Length);
            Array.Resize(ref this.textSceneColor, this.text.Length);
            Array.Copy(textColor, 0, this.textColor, length, textColor.Length);
            Array.Copy(textSceneColor, 0, this.textSceneColor, length, textSceneColor.Length);
            Array.Copy(tc, 0, this.textColor, length + text.Length, tc.Length);
            Array.Copy(tsc, 0, this.textSceneColor, length + text.Length, tsc.Length);
            return true;
        }
        public ColumnInfo ToColumnInfo()
        {
            return new ColumnInfo(text,textSetConditions,setCondition,textColor,textSceneColor);
        }
    }
    public class Console
    {
        struct DefaultValue
        {
            public const ConsoleColor color_text = ConsoleColor.White;
            public const ConsoleColor color_scene = ConsoleColor.Black;
        }
        public int Priority { get { return priority; } }
        internal int priority;
        public int ColumnCount { get; } = 0;
        public int AllColumnCount { get; } = 0;
        internal List<TeropColumnInfo> terops = [];
        internal List<TitleColumnInfo> titles = [];
        internal List<ColumnInfo> texts = [];//title>text<title
        internal ConsoleHost host;
        public ConsoleColor DefaultTextColor{ get{ return deft; } }
        ConsoleColor deft = DefaultValue.color_text;
        public ConsoleColor DefaultTextSceneColor{ get{ return defts; } }
        ConsoleColor defts = DefaultValue.color_scene;
        internal Console(ConsoleHost host, int priority)
        {
            this.host = host;
            this.priority = priority;
        }
        public bool TitleInsert(int index, int len, string insertValue)
        {
            if (index >= texts.Count) return false;
            if (len >= texts[index].Text.Length) return false;
            if (index < 0 || len < 0) return false;
            ColumnInfo info = texts[index].ToColumnInfo();
            if (texts[index].SetCondition)
            {
                if (!texts[index].Insert(insertValue, len, deft, defts))
                {
                    texts[index] = info;
                    return false;
                }
                bool? b = texts[index].Func(titles.Count, terops.Count, null);
                int code;
                if (b == null || b == true)
                {
                    (string Text, ConsoleColor[] TextColor, ConsoleColor[] TextSceneColor) gcd = texts[index].GetLastChangeSentence;
                    code = Write(new ColumnChangeInfo(priority, insertValue, index, gcd.TextColor, gcd.TextSceneColor, (len, insertValue.Length), ChangeType.Add));
                    if (code != 0)
                    {
                        texts[index] = info;
                        return false;
                    }
                    return true;
                }
                code = Write(new ColumnChangeInfo(priority, "", index, [], [], (0, texts[index].Text.Length), ChangeType.Remove));
                if (code != 0)
                {
                    texts[index] = info;
                    return false;
                }
                return true;
            }
        }
        public void InsertAt(int Index, string insertValue)
        {
            
        }
        public void Remove(int index, int startLen, int count)
        {

        }
        public void RemoveAt(int index)
        {

        }
        public void RemoveAll()
        {
            
        }
        public void Replace(int index, int startLen, int count, string newValue)
        {
            
        }
        public void Replace(int index, string oldValue, string newValue)
        {

        }
        public void ReplaceAt(int index, string newValue)
        {
            
        }
        //public (string text, ConsoleColor[] textColor, ConsoleColor[] sceneColor)? GetIndex(int index)
        //{

        //}
        //public (string text,ConsoleColor[] textColor,ConsoleColor[] sceneColor)[] GetAll()
        //{
            
        //}
        public void Add(string text)
        {
            
        }
        public void Add(string text, ConsoleColor textColor, ConsoleColor sceneColor)
        {

        }
        public void Add(string text, ConsoleColor[] textColor, ConsoleColor[] sceneColor)
        {
            
        }
        public void HideAt(int index)
        {

        }
        public void HideAll()
        {

        }
        public void ShowAt(int index)
        {
            
        }
        public void ShowAll()
        {
            
        }
        public class TeropMethots
        {
            
        }
        public class TitleMethots
        {
            
        }
        void ChangeOffset(int TopLen)
        {

        }
        int Write(ColumnChangeInfo info)
        {
            return host.SendInfo(info);
        }
        int Clear()
        {
            return host.FlushWindow();
        }
    }
}