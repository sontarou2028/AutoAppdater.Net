namespace AutoAppdater.Console
{
    enum ChangeType
    {
        Add,
        Remove,
        Change,
    }
    class DisplayInfo
    {
        public int Priority;
        public string[] Sentences;
        public string[] PastSentence;
        public int[] Column;
        public ConsoleColor[][] TextColor;
        public ConsoleColor[][] SceneColor;
        public ConsoleColor?[][] DefaultTextFillColorPattern;
        public ConsoleColor?[][] DefaultSceneFillColorPattern;
        public (int start, int end)[][] ChangeLen;
        public ChangeType[] ChangeTypes;
    }
    class ColumnInfo
    {
        public string? InitTerop;
        public string? InitTitle;
        public string Text;
        public string? EndTitle;
        public string? EndTerop;
        public (int[] value, Func<int,bool>)? InitTeropSetConditions;
        public (int[] value, Func<int,bool>)? InitTitleSetConditions;
        public (int[] value, Func<int,bool>)? TextSetConditions;
        public (int[] value, Func<int,bool>)? EndTitleSetConditions;
        public (int[] value, Func<int,bool>)? EndTeropSetConditions;
        public ConsoleColor[]? InitTeropTextColor;
        public ConsoleColor[]? InitTitleTextColor;
        public ConsoleColor[] TextColor;
        public ConsoleColor[]? EndTitleTextColor;
        public ConsoleColor[]? EndTeropTextColor;
        public ConsoleColor[]? InitTeropSceneColor;
        public ConsoleColor[]? InitTitleSceneColor;
        public ConsoleColor[] TextSceneColor;
        public ConsoleColor[]? EndTitleSceneColor;
        public ConsoleColor[]? EndTeropSceneColor;
        public ConsoleColor[]? DefaultTextFillColorPattern;
        public ConsoleColor[]? DefaultSceneFillColorPattern;
    }
    public class Console
    {
        public int Priority { get; }
        List<ColumnInfo> infos;
        internal Console()
        {
            
        }
    }
    static class Display
    {
        static void ChangeOffset(int TopLen)
        {

        }
        static void Write(DisplayInfo info)
        {

        }
        static void Clear(int priority)
        {
            
        }
        static void ClearAll()
        {

        }
    }
}