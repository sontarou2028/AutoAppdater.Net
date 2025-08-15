using System.IO.Pipelines;
using AutoAppdater.ConsoleHosts;

namespace AutoAppdater.Consoles
{
    enum ChangeType : byte
    {
        Add,
        Remove,
        Replace,
    }
    internal class DisplayInfo
    {
        public int Priority;
        public string[] Sentences;
        //public string[] PastSentence;
        public int[] Column;
        public ConsoleColor[][] TextColor;
        public ConsoleColor[][] SceneColor;
        public ConsoleColor?[][] DefaultTextFillColorPattern;
        public ConsoleColor?[][] DefaultSceneFillColorPattern;
        public (int start, int end)[][] ChangeLen;
        public ChangeType[] ChangeTypes;
        public DisplayInfo(int Priority, string[] Sentences, /*string[] PastSentence, */int[] Column,
        ConsoleColor[][] TextColor, ConsoleColor[][] SceneColor,
        ConsoleColor?[][] DefaultTextFillColorPattern, ConsoleColor?[][] DefaultSceneFillColorPattern,
        (int start, int end)[][] ChangeLen, ChangeType[] ChangeTypes)
        {
            this.Priority = Priority;
            this.Sentences = Sentences;
            //this.PastSentence = PastSentence;
            this.Column = Column;
            this.TextColor = TextColor;
            this.SceneColor = SceneColor;
            this.DefaultTextFillColorPattern = DefaultTextFillColorPattern;
            this.DefaultSceneFillColorPattern = DefaultSceneFillColorPattern;
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
        public string? Text;
        public (int[] value, Func<int, bool>)? TextSetConditions;
        public ConsoleColor[]? TextColor;
        public ConsoleColor[]? TextSceneColor;
    }
    public class Console
    {
        public int Priority { get { return priority; } }
        int priority;
        public int ColumnCount { get; } = 0;
        public int AllColumnCount { get; } = 0;
        List<TeropColumnInfo> terops = [];
        List<TitleColumnInfo> titles = [];
        List<ColumnInfo> infos = [];
        ConsoleHost host;
        public TextMethots Text { get{ return text; } }
        TextMethots text;
        public TeropMethots Terop { get{ return terop; } }
        TeropMethots terop;
        public TitleMethots Title { get{ return title; } }
        TitleMethots title;
        internal Console(ConsoleHost host, int priority)
        {
            this.priority = priority;
            this.host = host;
            text = new TextMethots();
            terop = new TeropMethots();
            title = new TitleMethots();
        }
        public class TextMethots
        {
            public void Insert(int index, string len, string insertValue)
            {

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
        int Write(DisplayInfo info)
        {
            return host.SendInfo(info);
        }
        int Clear()
        {
            ChangeType[] t = new ChangeType[AllColumnCount];
            Array.Fill(t, ChangeType.Remove);
            DisplayInfo inf = new DisplayInfo(Priority, [], [], [], [], [], [], [], t);
            return host.SendInfo(inf);
        }
    }
}