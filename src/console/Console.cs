using System.Text.Json.Serialization;

namespace AutoAppdater.Consoles
{
    enum ChangeType
    {
        Add,
        Remove,
        Change,
    }
    internal class DisplayInfo
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
        public string? Text;
        public string? EndTitle;
        public string? EndTerop;
        public (int[] value, Func<int,bool>)? InitTeropSetConditions;
        public (int[] value, Func<int,bool>)? InitTitleSetConditions;
        public (int[] value, Func<int,bool>)? TextSetConditions;
        public (int[] value, Func<int,bool>)? EndTitleSetConditions;
        public (int[] value, Func<int,bool>)? EndTeropSetConditions;
        public ConsoleColor[]? InitTeropTextColor;
        public ConsoleColor[]? InitTitleTextColor;
        public ConsoleColor[]? TextColor;
        public ConsoleColor[]? EndTitleTextColor;
        public ConsoleColor[]? EndTeropTextColor;
        public ConsoleColor[]? InitTeropSceneColor;
        public ConsoleColor[]? InitTitleSceneColor;
        public ConsoleColor[]? TextSceneColor;
        public ConsoleColor[]? EndTitleSceneColor;
        public ConsoleColor[]? EndTeropSceneColor;
        public ConsoleColor[]? DefaultTextFillColorPattern;
        public ConsoleColor[]? DefaultSceneFillColorPattern;
    }
    public class Console
    {
        public int Priority { get; }
        public int ColumnCount { get; } = 0;
        public int AllColumnCount { get; } = 0;
        List<ColumnInfo> infos;
        internal Console()
        {

        }
        public void Insert(int index,string len,string insertValue)
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
        public (string text, ConsoleColor[] textColor, ConsoleColor[] sceneColor)? GetIndex(int index)
        {

        }
        public (string text,ConsoleColor[] textColor,ConsoleColor[] sceneColor)[] GetAll()
        {
            
        }
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
        void Display()
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