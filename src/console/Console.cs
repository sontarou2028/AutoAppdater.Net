using System.IO.Pipelines;
using System.Text.Encodings.Web;
using System.Collections.ObjectModel;
using AutoAppdater.ConsoleHosts;
using System.ComponentModel.DataAnnotations;

namespace AutoAppdater.Consoles
{
    enum ChangeType : byte
    {
        Insert,
        Remove,
        Replace,
        InsertAt,
        RemoveAt,
        ReplaceAt,
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
        public (int start, int count) ChangeLen;
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
        internal string InitTerop { get { return init.ToString(); } }
        internal string EndTerop{ get{ return end.ToString(); } }
        string init = "";
        string end = "";
        Func<int, int, int, bool>? initTeropSetConditions = null;//title, terop, text, reesult
        Func<TeropColumnInfo[], TitleColumnInfo[], TextColumnInfo[], int, bool>? initTeropSetConditions1 = null;
        Func<TitleColumnInfo[]?, TextColumnInfo, int, bool>? initTeropSetConditions2 = null;
        Func<int, int, int, bool>? endTeropSetConditions = null;//title, terop, text, reesult
        Func<TeropColumnInfo[], TitleColumnInfo[], TextColumnInfo[], int, bool>? endTeropSetConditions1 = null;
        Func<TitleColumnInfo[]?, TextColumnInfo, int, bool>? endTeropSetConditions2 = null;
        bool initTeropCondition = false;
        bool endTeropCondition = false;
        internal ConsoleColor[] InitTeropTextColor { get { return initTextColor; } }
        internal ConsoleColor[] InitTeropSceneColor{ get{ return initSceneColor; } }
        internal ConsoleColor[] EndTeropTextColor{ get{ return endTextColor; } }
        internal ConsoleColor[] EndTeropSceneColor{ get{ return endSceneColor; } }
        ConsoleColor[] initTextColor = [];
        ConsoleColor[] initSceneColor = [];
        ConsoleColor[] endTextColor = [];
        ConsoleColor[] endSceneColor = [];
        internal (string InitTerop, ConsoleColor[] InitTeropColor, ConsoleColor[] InitTeropSceneColor) GetLastChangeInitTerop{ get{ return getLcsi; } }
        internal (string EndTitle, ConsoleColor[] EndTitleColor, ConsoleColor[] EndTitleSceneColor) GetLastChangeEndTerop { get{ return getLcse; } }
        (string EndTerop, ConsoleColor[] EndTeropColor, ConsoleColor[] EndTitleSceneColor) getLcse;
        (string InitTitle, ConsoleColor[] InitTeropColor, ConsoleColor[] InitTeropSceneColor) getLcsi;
        internal TeropColumnInfo() { }
        TeropColumnInfo(string initTerop, string endTerop,
        Func<int, int, int, bool>? initTeropSetConditions,
        Func<TeropColumnInfo[], TitleColumnInfo[], TextColumnInfo[], int, bool>? initTeropSetConditions1,
        Func<TitleColumnInfo[]?, TextColumnInfo, int, bool>? initTeropSetConditions2,
        Func<int, int, int, bool>? endTeropSetConditions,
        Func<TeropColumnInfo[], TitleColumnInfo[], TextColumnInfo[], int, bool>? endTeropSetConditions1,
        Func<TitleColumnInfo[]?, TextColumnInfo, int, bool>? endTeropSetConditions2,
        bool initTeropCondition, bool endTeropCondition,
        ConsoleColor[] initTextColor, ConsoleColor[] initSceneColor,
        ConsoleColor[] endTextColor, ConsoleColor[] endSceneColor)
        {
            this.init = initTerop;
            this.end = endTerop;
            this.initTeropSetConditions = initTeropSetConditions;
            this.initTeropSetConditions1 = initTeropSetConditions1;
            this.initTeropSetConditions2 = initTeropSetConditions2;
            this.endTeropSetConditions = endTeropSetConditions;
            this.endTeropSetConditions1 = endTeropSetConditions1;
            this.endTeropSetConditions2 = endTeropSetConditions2;
            this.initTeropCondition = initTeropCondition;
            this.endTeropCondition = endTeropCondition;
            this.initTextColor = initTextColor;
            this.initSceneColor = initSceneColor;
            this.endTextColor = endTextColor;
            this.endSceneColor = endSceneColor;
        }/*
        internal bool? FuncInit(int titleLen, int teropLen,
        TeropColumnInfo[] teropCoumnInfos, TitleColumnInfo[] titleColumnInfos, TextColumnInfo[] columnInfos,
        TitleColumnInfo[]? activeTitleColumnInfos)
        {
            bool? b = Func(titleLen, teropLen, text.Length);
            bool? b1 = Func(teropCoumnInfos, titleColumnInfos, columnInfos, text.Length);
            bool? b2 = Func(activeTitleColumnInfos, ToTextColumnInfo(), text.Length);
            if (b == null && b1 == null && b2 == null) return null;
            else if (b == true || b1 == true || b2 == true)
            {
                setCondition = true;
                return true;
            }
            else
            {
                setCondition = false;
                return false;
            }
        }
        bool? FuncInit(int? titleLen, int teropLen, int textLen)
        {
            if (initTitleSetConditions == null) return null;
            return initTitleSetConditions(init.Length, teropLen, textLen);
        }
        bool? FuncInit(TeropColumnInfo[] teropCoumnInfos, TitleColumnInfo[] titleColumnInfos, TextColumnInfo[] columnInfos, int? titleLen)
        {
            if (initTitleSetConditions1 == null) return null;
            return initTitleSetConditions1(teropCoumnInfos,titleColumnInfos,columnInfos,init.Length);
        }
        bool? FuncInit(TitleColumnInfo[]? titleColumnInfo,TextColumnInfo columnInfo,int? titleLen)
        {
            if (initTitleSetConditions2 == null) return null;
            return initTitleSetConditions2([ToTitleColumnInfo()],columnInfo,init.Length);
        }
        bool? FuncEnd(int? titleLen, int teropLen, int textLen)
        {
            if (initTitleSetConditions == null) return null;
            return initTitleSetConditions(init.Length, teropLen, textLen);
        }
        bool? FuncEnd(TeropColumnInfo[] teropCoumnInfos, TitleColumnInfo[] titleColumnInfos, TextColumnInfo[] columnInfos, int? titleLen)
        {
            if (initTitleSetConditions1 == null) return null;
            return initTitleSetConditions1(teropCoumnInfos,titleColumnInfos,columnInfos,init.Length);
        }
        bool? FuncEnd(TitleColumnInfo[]? titleColumnInfo,TextColumnInfo columnInfo,int? titleLen)
        {
            if (initTitleSetConditions2 == null) return null;
            return initTitleSetConditions2([ToTitleColumnInfo()],columnInfo,init.Length);
        }*/
        internal void ReplaceFuncInit(Func<int, int, int, bool>? newContitions)
        {
            initTeropSetConditions = newContitions;
        }
        internal void ReplaceFuncInit(Func<TeropColumnInfo[], TitleColumnInfo[], TextColumnInfo[], int, bool>? newContitions)
        {
            initTeropSetConditions1 = newContitions;
        }
        internal void ReplaceFuncInit(Func<TitleColumnInfo[]?, TextColumnInfo, int, bool>? newContitions)
        {
            initTeropSetConditions2 = newContitions;
        }
        internal void ReplaceFuncEnd(Func<int, int, int, bool>? newContitions)
        {
            endTeropSetConditions = newContitions;
        }
        internal void ReplaceFuncEnd(Func<TeropColumnInfo[], TitleColumnInfo[], TextColumnInfo[], int, bool>? newContitions)
        {
            endTeropSetConditions1 = newContitions;
        }
        internal void ReplaceFuncEnd(Func<TitleColumnInfo[]?, TextColumnInfo, int, bool>? newContitions)
        {
            endTeropSetConditions2 = newContitions;
        }
        internal void RemoveFunc()
        {
            initTeropSetConditions = null;
            initTeropSetConditions1 = null;
            initTeropSetConditions2 = null;
            endTeropSetConditions = null;
            endTeropSetConditions1 = null;
            endTeropSetConditions2 = null;
        }
        internal bool RemoveInit(int length, int count)
        {
            if (init.Length < length + count || length < 0 || count < 0) return false;
            ConsoleColor[] tc = new ConsoleColor[count];
            ConsoleColor[] tsc = new ConsoleColor[count];
            Array.Copy(initTextColor, length, tc, 0, count);
            Array.Copy(initSceneColor, length, tsc, 0, count);
            getLcsi = (init.Substring(length, count), tc.ToArray(), tsc.ToArray());
            tc = new ConsoleColor[initTextColor.Length - length - count];
            tsc = new ConsoleColor[initSceneColor.Length - length - count];
            Array.Copy(initTextColor, length + count, tc, 0, initTextColor.Length - length - count);
            Array.Copy(initSceneColor, length + count, tc, 0, initSceneColor.Length - length - count);
            Array.Resize(ref initTextColor, initTextColor.Length - count);
            Array.Resize(ref initSceneColor, initSceneColor.Length - count);
            Array.Copy(tc, 0, initTextColor, length, initTextColor.Length - length - count);
            Array.Copy(tsc, 0, initSceneColor, length, initSceneColor.Length - length - count);
            init = init.Remove(length,count);
            return true;
        }
        internal bool RemoveEnd(int length, int count)
        {
            if (end.Length < length + count || length < 0 || count < 0) return false;
            ConsoleColor[] tc = new ConsoleColor[count];
            ConsoleColor[] tsc = new ConsoleColor[count];
            Array.Copy(endTextColor, length, tc, 0, count);
            Array.Copy(endSceneColor, length, tsc, 0, count);
            getLcse = (end.Substring(length, count), tc.ToArray(), tsc.ToArray());
            tc = new ConsoleColor[endTextColor.Length - length - count];
            tsc = new ConsoleColor[endSceneColor.Length - length - count];
            Array.Copy(endTextColor, length + count, tc, 0, endTextColor.Length - length - count);
            Array.Copy(endSceneColor, length + count, tc, 0, endSceneColor.Length - length - count);
            Array.Resize(ref endTextColor, endTextColor.Length - count);
            Array.Resize(ref endSceneColor, endSceneColor.Length - count);
            Array.Copy(tc, 0, endTextColor, length, endTextColor.Length - length - count);
            Array.Copy(tsc, 0, endSceneColor, length, endSceneColor.Length - length - count);
            end = end.Remove(length,count);
            return true;
        }
        internal void RemoveAllInit()
        {
            getLcsi = (init.ToString(),initTextColor.ToArray(),initSceneColor.ToArray());
            init = "";
            initTextColor = [];
            initSceneColor = [];
        }
        internal void RemoveAllEnd()
        {
            getLcse = (init.ToString(),initTextColor.ToArray(),initSceneColor.ToArray());
            end = "";
            endTextColor = [];
            endSceneColor = [];
        }
        internal void RemoveAll()
        {
            getLcsi = (init.ToString(),initTextColor.ToArray(),initSceneColor.ToArray());
            init = "";
            initTextColor = [];
            initSceneColor = [];
            getLcse = (init.ToString(),initTextColor.ToArray(),initSceneColor.ToArray());
            end = "";
            endTextColor = [];
            endSceneColor = [];
        }
        internal void ReplaceInit(string text, ConsoleColor textFillColor, ConsoleColor sceneFillColor)
        {
            ConsoleColor[] tc = new ConsoleColor[text.Length];
            ConsoleColor[] tsc = new ConsoleColor[text.Length];
            Array.Fill(tc, textFillColor);
            Array.Fill(tsc, sceneFillColor);
            init = text;
            initTextColor = tc;
            endSceneColor = tsc;
            getLcsi = (text.ToString(), tc, tsc);
        }
        internal bool ReplaceInit(string text, ConsoleColor[] textColor, ConsoleColor[] sceneColor)
        {
            if (text.Length != textColor.Length || text.Length != sceneColor.Length) return false;
            init = text;
            initTextColor = textColor;
            initSceneColor = sceneColor;
            getLcsi = (text.ToString(), textColor.ToArray(), sceneColor.ToArray());
            return true;
        }
        internal void ReplaceEnd(string text, ConsoleColor textFillColor, ConsoleColor sceneFillColor)
        {
            ConsoleColor[] tc = new ConsoleColor[text.Length];
            ConsoleColor[] tsc = new ConsoleColor[text.Length];
            Array.Fill(tc, textFillColor);
            Array.Fill(tsc, sceneFillColor);
            end = text;
            endTextColor = tc;
            endSceneColor = tsc;
            getLcse = (text.ToString(), tc, tsc);
        }
        internal bool ReplaceEnd(string text, ConsoleColor[] textColor, ConsoleColor[] sceneColor)
        {
            if (text.Length != textColor.Length || text.Length != sceneColor.Length) return false;
            end = text;
            endTextColor = textColor;
            endSceneColor = sceneColor;
            getLcse = (text.ToString(), textColor.ToArray(), sceneColor.ToArray());
            return true;
        }
        internal void AddInit(string text, ConsoleColor textFillColor, ConsoleColor sceneFillColor)
        {
            ConsoleColor[] tc = new ConsoleColor[text.Length];
            ConsoleColor[] tsc = new ConsoleColor[text.Length];
            Array.Fill(tc, textFillColor);
            Array.Fill(tsc, sceneFillColor);
            int len = init.Length;
            init += text;
            Array.Resize(ref initTextColor, init.Length);
            Array.Resize(ref initSceneColor, init.Length);
            Array.Copy(tc, 0, initTextColor, len, tc.Length);
            Array.Copy(tsc, 0, initSceneColor, len, tsc.Length);
            getLcsi = (text.ToString(), tc, tsc);
        }
        internal bool AddInit(string text, ConsoleColor[] textColor, ConsoleColor[] sceneColor)
        {
            if (text.Length != textColor.Length || text.Length != sceneColor.Length) return false;
            int len = init.Length;
            init += text;
            Array.Resize(ref this.initTextColor, init.Length);
            Array.Resize(ref initSceneColor, init.Length);
            Array.Copy(textColor, 0, initTextColor, len, textColor.Length);
            Array.Copy(sceneColor, 0, initSceneColor, len, sceneColor.Length);
            getLcsi = (text.ToString(),textColor.ToArray(),sceneColor.ToArray());
            return true;
        }
        internal void AddEnd(string text, ConsoleColor textFillColor, ConsoleColor sceneFillColor)
        {
            ConsoleColor[] tc = new ConsoleColor[text.Length];
            ConsoleColor[] tsc = new ConsoleColor[text.Length];
            Array.Fill(tc, textFillColor);
            Array.Fill(tsc, sceneFillColor);
            int len = end.Length;
            end += text;
            Array.Resize(ref endTextColor, end.Length);
            Array.Resize(ref endSceneColor, end.Length);
            Array.Copy(tc, 0, endTextColor, len, tc.Length);
            Array.Copy(tsc, 0, endSceneColor, len, tsc.Length);
            getLcse = (text.ToString(), tc, tsc);
        }
        internal bool AddEnd(string text, ConsoleColor[] textColor, ConsoleColor[] sceneColor)
        {
            if (text.Length != textColor.Length || text.Length != sceneColor.Length) return false;
            int len = end.Length;
            end += text;
            Array.Resize(ref endTextColor, end.Length);
            Array.Resize(ref endSceneColor, end.Length);
            Array.Copy(textColor, 0, endTextColor, len, textColor.Length);
            Array.Copy(sceneColor, 0, endSceneColor, len, sceneColor.Length);
            getLcse = (text.ToString(),textColor.ToArray(),sceneColor.ToArray());
            return true;
        }
        internal bool InsertInit(string text, int length, ConsoleColor textFillColor, ConsoleColor sceneFillColor)
        {
            if (text.Length < length || length < 0) return false;
            ConsoleColor[] tcf = new ConsoleColor[text.Length];
            ConsoleColor[] tscf = new ConsoleColor[text.Length];
            Array.Fill(tcf, textFillColor);
            Array.Fill(tscf, sceneFillColor);
            getLcsi = (text.ToString(), tcf.ToArray(), tscf.ToArray());
            init = init.Insert(length, text);
            ConsoleColor[] tc = new ConsoleColor[initTextColor.Length - length - text.Length];
            ConsoleColor[] tsc = new ConsoleColor[initSceneColor.Length - length - text.Length];
            Array.Copy(initTextColor, length, tc, 0, initTextColor.Length - length);
            Array.Copy(initSceneColor, length, tsc, 0, initSceneColor.Length - length);
            Array.Resize(ref initTextColor, init.Length);
            Array.Resize(ref initSceneColor, init.Length);
            Array.Copy(tcf, 0, initTextColor, length, tcf.Length);
            Array.Copy(tscf, 0, initSceneColor, length, tscf.Length);
            Array.Copy(tc, 0, initTextColor, length + text.Length, tc.Length);
            Array.Copy(tsc, 0, initSceneColor, length + text.Length, tsc.Length);
            return true;
        }
        internal bool InsertInit(string text, int length, ConsoleColor[] textColor, ConsoleColor[] sceneColor)
        {
            if (text.Length != textColor.Length || text.Length != sceneColor.Length ||
            text.Length < length || length < 0) return false;
            getLcsi = (text.ToString(), textColor.ToArray(), sceneColor.ToArray());
            init = init.Insert(length, text);
            ConsoleColor[] tc = new ConsoleColor[initTextColor.Length - length - text.Length];
            ConsoleColor[] tsc = new ConsoleColor[initSceneColor.Length - length - text.Length];
            Array.Copy(initTextColor, length, tc, 0, initTextColor.Length - length);
            Array.Copy(initSceneColor, length, tsc, 0, initSceneColor.Length - length);
            Array.Resize(ref initTextColor, init.Length);
            Array.Resize(ref initSceneColor, init.Length);
            Array.Copy(textColor, 0, initTextColor, length, textColor.Length);
            Array.Copy(sceneColor, 0, initSceneColor, length, sceneColor.Length);
            Array.Copy(tc, 0, initTextColor, length + text.Length, tc.Length);
            Array.Copy(tsc, 0, initSceneColor, length + text.Length, tsc.Length);
            return true;
        }
        internal bool InsertEnd(string text, int length, ConsoleColor textFillColor, ConsoleColor SceneFillColor)
        {
            if (text.Length < length || length < 0) return false;
            ConsoleColor[] tcf = new ConsoleColor[text.Length];
            ConsoleColor[] tscf = new ConsoleColor[text.Length];
            Array.Fill(tcf, textFillColor);
            Array.Fill(tscf, SceneFillColor);
            getLcse = (text.ToString(), tcf.ToArray(), tscf.ToArray());
            end = end.Insert(length, text);
            ConsoleColor[] tc = new ConsoleColor[endTextColor.Length - length - text.Length];
            ConsoleColor[] tsc = new ConsoleColor[endSceneColor.Length - length - text.Length];
            Array.Copy(endTextColor, length, tc, 0, endTextColor.Length - length);
            Array.Copy(endSceneColor, length, tsc, 0, endSceneColor.Length - length);
            Array.Resize(ref endTextColor, end.Length);
            Array.Resize(ref endSceneColor, end.Length);
            Array.Copy(tcf, 0, endTextColor, length, tcf.Length);
            Array.Copy(tscf, 0, endSceneColor, length, tscf.Length);
            Array.Copy(tc, 0, endTextColor, length + text.Length, tc.Length);
            Array.Copy(tsc, 0, endSceneColor, length + text.Length, tsc.Length);
            return true;
        }
        internal bool InsertEnd(string text, int length, ConsoleColor[] textColor, ConsoleColor[] sceneColor)
        {
            if (text.Length != textColor.Length || text.Length != sceneColor.Length ||
            text.Length < length || length < 0) return false;
            getLcse = (text.ToString(), textColor.ToArray(), sceneColor.ToArray());
            end = end.Insert(length, text);
            ConsoleColor[] tc = new ConsoleColor[endTextColor.Length - length - text.Length];
            ConsoleColor[] tsc = new ConsoleColor[endSceneColor.Length - length - text.Length];
            Array.Copy(endTextColor, length, tc, 0, endTextColor.Length - length);
            Array.Copy(endSceneColor, length, tsc, 0, endSceneColor.Length - length);
            Array.Resize(ref endTextColor, end.Length);
            Array.Resize(ref endSceneColor, end.Length);
            Array.Copy(textColor, 0, endTextColor, length, textColor.Length);
            Array.Copy(sceneColor, 0, endSceneColor, length, sceneColor.Length);
            Array.Copy(tc, 0, endTextColor, length + text.Length, tc.Length);
            Array.Copy(tsc, 0, endSceneColor, length + text.Length, tsc.Length);
            return true;
        }
        public TeropColumnInfo ToTeropColumnInfo()
        {
            return new TeropColumnInfo(init,end,
            initTeropSetConditions,initTeropSetConditions1,initTeropSetConditions2,
            endTeropSetConditions,endTeropSetConditions1,endTeropSetConditions2,
            initTeropCondition,endTeropCondition,
            initTextColor,initSceneColor,endTextColor,endSceneColor);
        }
    }
    class TitleColumnInfo
    {
        internal string InitTitle{ get{ return init.ToString(); } }
        internal string EndTitle{ get{ return end.ToString(); } }
        string init = "";
        string end = "";
        Func<int, int, int, bool>? initTitleSetConditions = null;//title, terop, text, reesult
        Func<TeropColumnInfo[], TitleColumnInfo[], TextColumnInfo[], int, bool>? initTitleSetConditions1 = null;
        Func<TitleColumnInfo[]?, TextColumnInfo, int, bool>? initTitleSetConditions2 = null;
        Func<int, int, int, bool>? endTitleSetConditions = null;//title, terop, text, reesult
        Func<TeropColumnInfo[], TitleColumnInfo[], TextColumnInfo[], int, bool>? endTitleSetConditions1 = null;
        Func<TitleColumnInfo[]?, TextColumnInfo, int, bool>? endTitleSetConditions2 = null;
        bool initTitleCondition = false;
        bool endTitleCondition = false;
        internal ConsoleColor[] InitTitleTextColor { get { return initTextColor; } }
        internal ConsoleColor[] InitTitleSceneColor{ get{ return initSceneColor; } }
        internal ConsoleColor[] EndTitleTextColor{ get{ return endTextColor; } }
        internal ConsoleColor[] EndTitleSceneColor{ get{ return endSceneColor; } }
        ConsoleColor[] initTextColor = [];
        ConsoleColor[] initSceneColor = [];
        ConsoleColor[] endTextColor = [];
        ConsoleColor[] endSceneColor = [];
        internal (string InitTitle, ConsoleColor[] InitTitleColor, ConsoleColor[] InitTitleSceneColor) GetLastChangeInitTitle{ get{ return getLcsi; } }
        internal (string EndTitle, ConsoleColor[] EndTitleColor, ConsoleColor[] EndTitleSceneColor) GetLastChangeEndTitle { get{ return getLcse; } }
        (string EndTitle, ConsoleColor[] EndTitleColor, ConsoleColor[] EndTitleSceneColor) getLcse;
        (string InitTitle, ConsoleColor[] InitTitleColor, ConsoleColor[] InitTitleSceneColor) getLcsi;
        internal TitleColumnInfo() { }
        TitleColumnInfo(string initTitle, string endTitle,
        Func<int, int, int, bool>? initTitleSetConditions,
        Func<TeropColumnInfo[], TitleColumnInfo[], TextColumnInfo[], int, bool>? initTitleSetConditions1,
        Func<TitleColumnInfo[]?, TextColumnInfo, int, bool>? initTitleSetConditions2,
        Func<int, int, int, bool>? endTitleSetConditions,
        Func<TeropColumnInfo[], TitleColumnInfo[], TextColumnInfo[], int, bool>? endTitleSetConditions1,
        Func<TitleColumnInfo[]?, TextColumnInfo, int, bool>? endTitleSetConditions2,
        bool initTitleCondition, bool endTitleCondition,
        ConsoleColor[] initTextColor, ConsoleColor[] initSceneColor,
        ConsoleColor[] endTextColor, ConsoleColor[] endSceneColor)
        {
            this.init = initTitle;
            this.end = endTitle;
            this.initTitleSetConditions = initTitleSetConditions;
            this.initTitleSetConditions1 = initTitleSetConditions1;
            this.initTitleSetConditions2 = initTitleSetConditions2;
            this.endTitleSetConditions = endTitleSetConditions;
            this.endTitleSetConditions1 = endTitleSetConditions1;
            this.endTitleSetConditions2 = endTitleSetConditions2;
            this.initTitleCondition = initTitleCondition;
            this.endTitleCondition = endTitleCondition;
            this.initTextColor = initTextColor;
            this.initSceneColor = initSceneColor;
            this.endTextColor = endTextColor;
            this.endSceneColor = endSceneColor;
        }/*
        internal bool? FuncInit(int titleLen, int teropLen,
        TeropColumnInfo[] teropCoumnInfos, TitleColumnInfo[] titleColumnInfos, TextColumnInfo[] columnInfos,
        TitleColumnInfo[]? activeTitleColumnInfos)
        {
            bool? b = Func(titleLen, teropLen, text.Length);
            bool? b1 = Func(teropCoumnInfos, titleColumnInfos, columnInfos, text.Length);
            bool? b2 = Func(activeTitleColumnInfos, ToTextColumnInfo(), text.Length);
            if (b == null && b1 == null && b2 == null) return null;
            else if (b == true || b1 == true || b2 == true)
            {
                setCondition = true;
                return true;
            }
            else
            {
                setCondition = false;
                return false;
            }
        }
        bool? FuncInit(int? titleLen, int teropLen, int textLen)
        {
            if (initTitleSetConditions == null) return null;
            return initTitleSetConditions(init.Length, teropLen, textLen);
        }
        bool? FuncInit(TeropColumnInfo[] teropCoumnInfos, TitleColumnInfo[] titleColumnInfos, TextColumnInfo[] columnInfos, int? titleLen)
        {
            if (initTitleSetConditions1 == null) return null;
            return initTitleSetConditions1(teropCoumnInfos,titleColumnInfos,columnInfos,init.Length);
        }
        bool? FuncInit(TitleColumnInfo[]? titleColumnInfo,TextColumnInfo columnInfo,int? titleLen)
        {
            if (initTitleSetConditions2 == null) return null;
            return initTitleSetConditions2([ToTitleColumnInfo()],columnInfo,init.Length);
        }
        bool? FuncEnd(int? titleLen, int teropLen, int textLen)
        {
            if (initTitleSetConditions == null) return null;
            return initTitleSetConditions(init.Length, teropLen, textLen);
        }
        bool? FuncEnd(TeropColumnInfo[] teropCoumnInfos, TitleColumnInfo[] titleColumnInfos, TextColumnInfo[] columnInfos, int? titleLen)
        {
            if (initTitleSetConditions1 == null) return null;
            return initTitleSetConditions1(teropCoumnInfos,titleColumnInfos,columnInfos,init.Length);
        }
        bool? FuncEnd(TitleColumnInfo[]? titleColumnInfo,TextColumnInfo columnInfo,int? titleLen)
        {
            if (initTitleSetConditions2 == null) return null;
            return initTitleSetConditions2([ToTitleColumnInfo()],columnInfo,init.Length);
        }*/
        internal void ReplaceFuncInit(Func<int, int, int, bool>? newContitions)
        {
            initTitleSetConditions = newContitions;
        }
        internal void ReplaceFuncInit(Func<TeropColumnInfo[], TitleColumnInfo[], TextColumnInfo[], int, bool>? newContitions)
        {
            initTitleSetConditions1 = newContitions;
        }
        internal void ReplaceFuncInit(Func<TitleColumnInfo[]?, TextColumnInfo, int, bool>? newContitions)
        {
            initTitleSetConditions2 = newContitions;
        }
        internal void ReplaceFuncEnd(Func<int, int, int, bool>? newContitions)
        {
            endTitleSetConditions = newContitions;
        }
        internal void ReplaceFuncEnd(Func<TeropColumnInfo[], TitleColumnInfo[], TextColumnInfo[], int, bool>? newContitions)
        {
            endTitleSetConditions1 = newContitions;
        }
        internal void ReplaceFuncEnd(Func<TitleColumnInfo[]?, TextColumnInfo, int, bool>? newContitions)
        {
            endTitleSetConditions2 = newContitions;
        }
        internal void RemoveFunc()
        {
            initTitleSetConditions = null;
            initTitleSetConditions1 = null;
            initTitleSetConditions2 = null;
            endTitleSetConditions = null;
            endTitleSetConditions1 = null;
            endTitleSetConditions2 = null;
        }
        internal bool RemoveInit(int length, int count)
        {
            if (init.Length < length + count || length < 0 || count < 0) return false;
            ConsoleColor[] tc = new ConsoleColor[count];
            ConsoleColor[] tsc = new ConsoleColor[count];
            Array.Copy(initTextColor, length, tc, 0, count);
            Array.Copy(initSceneColor, length, tsc, 0, count);
            getLcsi = (init.Substring(length, count), tc.ToArray(), tsc.ToArray());
            tc = new ConsoleColor[initTextColor.Length - length - count];
            tsc = new ConsoleColor[initSceneColor.Length - length - count];
            Array.Copy(initTextColor, length + count, tc, 0, initTextColor.Length - length - count);
            Array.Copy(initSceneColor, length + count, tc, 0, initSceneColor.Length - length - count);
            Array.Resize(ref initTextColor, initTextColor.Length - count);
            Array.Resize(ref initSceneColor, initSceneColor.Length - count);
            Array.Copy(tc, 0, initTextColor, length, initTextColor.Length - length - count);
            Array.Copy(tsc, 0, initSceneColor, length, initSceneColor.Length - length - count);
            init = init.Remove(length,count);
            return true;
        }
        internal bool RemoveEnd(int length, int count)
        {
            if (end.Length < length + count || length < 0 || count < 0) return false;
            ConsoleColor[] tc = new ConsoleColor[count];
            ConsoleColor[] tsc = new ConsoleColor[count];
            Array.Copy(endTextColor, length, tc, 0, count);
            Array.Copy(endSceneColor, length, tsc, 0, count);
            getLcse = (end.Substring(length, count), tc.ToArray(), tsc.ToArray());
            tc = new ConsoleColor[endTextColor.Length - length - count];
            tsc = new ConsoleColor[endSceneColor.Length - length - count];
            Array.Copy(endTextColor, length + count, tc, 0, endTextColor.Length - length - count);
            Array.Copy(endSceneColor, length + count, tc, 0, endSceneColor.Length - length - count);
            Array.Resize(ref endTextColor, endTextColor.Length - count);
            Array.Resize(ref endSceneColor, endSceneColor.Length - count);
            Array.Copy(tc, 0, endTextColor, length, endTextColor.Length - length - count);
            Array.Copy(tsc, 0, endSceneColor, length, endSceneColor.Length - length - count);
            end = end.Remove(length,count);
            return true;
        }
        internal void RemoveAllInit()
        {
            getLcsi = (init.ToString(),initTextColor.ToArray(),initSceneColor.ToArray());
            init = "";
            initTextColor = [];
            initSceneColor = [];
        }
        internal void RemoveAllEnd()
        {
            getLcse = (init.ToString(),initTextColor.ToArray(),initSceneColor.ToArray());
            end = "";
            endTextColor = [];
            endSceneColor = [];
        }
        internal void RemoveAll()
        {
            getLcsi = (init.ToString(),initTextColor.ToArray(),initSceneColor.ToArray());
            init = "";
            initTextColor = [];
            initSceneColor = [];
            getLcse = (init.ToString(),initTextColor.ToArray(),initSceneColor.ToArray());
            end = "";
            endTextColor = [];
            endSceneColor = [];
        }
        internal void ReplaceInit(string text, ConsoleColor textFillColor, ConsoleColor sceneFillColor)
        {
            ConsoleColor[] tc = new ConsoleColor[text.Length];
            ConsoleColor[] tsc = new ConsoleColor[text.Length];
            Array.Fill(tc, textFillColor);
            Array.Fill(tsc, sceneFillColor);
            init = text;
            initTextColor = tc;
            endSceneColor = tsc;
            getLcsi = (text.ToString(), tc, tsc);
        }
        internal bool ReplaceInit(string text, ConsoleColor[] textColor, ConsoleColor[] sceneColor)
        {
            if (text.Length != textColor.Length || text.Length != sceneColor.Length) return false;
            init = text;
            initTextColor = textColor;
            initSceneColor = sceneColor;
            getLcsi = (text.ToString(), textColor.ToArray(), sceneColor.ToArray());
            return true;
        }
        internal void ReplaceEnd(string text, ConsoleColor textFillColor, ConsoleColor sceneFillColor)
        {
            ConsoleColor[] tc = new ConsoleColor[text.Length];
            ConsoleColor[] tsc = new ConsoleColor[text.Length];
            Array.Fill(tc, textFillColor);
            Array.Fill(tsc, sceneFillColor);
            end = text;
            endTextColor = tc;
            endSceneColor = tsc;
            getLcse = (text.ToString(), tc, tsc);
        }
        internal bool ReplaceEnd(string text, ConsoleColor[] textColor, ConsoleColor[] sceneColor)
        {
            if (text.Length != textColor.Length || text.Length != sceneColor.Length) return false;
            end = text;
            endTextColor = textColor;
            endSceneColor = sceneColor;
            getLcse = (text.ToString(), textColor.ToArray(), sceneColor.ToArray());
            return true;
        }
        internal void AddInit(string text, ConsoleColor textFillColor, ConsoleColor sceneFillColor)
        {
            ConsoleColor[] tc = new ConsoleColor[text.Length];
            ConsoleColor[] tsc = new ConsoleColor[text.Length];
            Array.Fill(tc, textFillColor);
            Array.Fill(tsc, sceneFillColor);
            int len = init.Length;
            init += text;
            Array.Resize(ref initTextColor, init.Length);
            Array.Resize(ref initSceneColor, init.Length);
            Array.Copy(tc, 0, initTextColor, len, tc.Length);
            Array.Copy(tsc, 0, initSceneColor, len, tsc.Length);
            getLcsi = (text.ToString(), tc, tsc);
        }
        internal bool AddInit(string text, ConsoleColor[] textColor, ConsoleColor[] sceneColor)
        {
            if (text.Length != textColor.Length || text.Length != sceneColor.Length) return false;
            int len = init.Length;
            init += text;
            Array.Resize(ref this.initTextColor, init.Length);
            Array.Resize(ref initSceneColor, init.Length);
            Array.Copy(textColor, 0, initTextColor, len, textColor.Length);
            Array.Copy(sceneColor, 0, initSceneColor, len, sceneColor.Length);
            getLcsi = (text.ToString(),textColor.ToArray(),sceneColor.ToArray());
            return true;
        }
        internal void AddEnd(string text, ConsoleColor textFillColor, ConsoleColor sceneFillColor)
        {
            ConsoleColor[] tc = new ConsoleColor[text.Length];
            ConsoleColor[] tsc = new ConsoleColor[text.Length];
            Array.Fill(tc, textFillColor);
            Array.Fill(tsc, sceneFillColor);
            int len = end.Length;
            end += text;
            Array.Resize(ref endTextColor, end.Length);
            Array.Resize(ref endSceneColor, end.Length);
            Array.Copy(tc, 0, endTextColor, len, tc.Length);
            Array.Copy(tsc, 0, endSceneColor, len, tsc.Length);
            getLcse = (text.ToString(), tc, tsc);
        }
        internal bool AddEnd(string text, ConsoleColor[] textColor, ConsoleColor[] sceneColor)
        {
            if (text.Length != textColor.Length || text.Length != sceneColor.Length) return false;
            int len = end.Length;
            end += text;
            Array.Resize(ref endTextColor, end.Length);
            Array.Resize(ref endSceneColor, end.Length);
            Array.Copy(textColor, 0, endTextColor, len, textColor.Length);
            Array.Copy(sceneColor, 0, endSceneColor, len, sceneColor.Length);
            getLcse = (text.ToString(),textColor.ToArray(),sceneColor.ToArray());
            return true;
        }
        internal bool InsertInit(string text, int length, ConsoleColor textFillColor, ConsoleColor sceneFillColor)
        {
            if (text.Length < length || length < 0) return false;
            ConsoleColor[] tcf = new ConsoleColor[text.Length];
            ConsoleColor[] tscf = new ConsoleColor[text.Length];
            Array.Fill(tcf, textFillColor);
            Array.Fill(tscf, sceneFillColor);
            getLcsi = (text.ToString(), tcf.ToArray(), tscf.ToArray());
            init = init.Insert(length, text);
            ConsoleColor[] tc = new ConsoleColor[initTextColor.Length - length - text.Length];
            ConsoleColor[] tsc = new ConsoleColor[initSceneColor.Length - length - text.Length];
            Array.Copy(initTextColor, length, tc, 0, initTextColor.Length - length);
            Array.Copy(initSceneColor, length, tsc, 0, initSceneColor.Length - length);
            Array.Resize(ref initTextColor, init.Length);
            Array.Resize(ref initSceneColor, init.Length);
            Array.Copy(tcf, 0, initTextColor, length, tcf.Length);
            Array.Copy(tscf, 0, initSceneColor, length, tscf.Length);
            Array.Copy(tc, 0, initTextColor, length + text.Length, tc.Length);
            Array.Copy(tsc, 0, initSceneColor, length + text.Length, tsc.Length);
            return true;
        }
        internal bool InsertInit(string text, int length, ConsoleColor[] textColor, ConsoleColor[] sceneColor)
        {
            if (text.Length != textColor.Length || text.Length != sceneColor.Length ||
            text.Length < length || length < 0) return false;
            getLcsi = (text.ToString(), textColor.ToArray(), sceneColor.ToArray());
            init = init.Insert(length, text);
            ConsoleColor[] tc = new ConsoleColor[initTextColor.Length - length - text.Length];
            ConsoleColor[] tsc = new ConsoleColor[initSceneColor.Length - length - text.Length];
            Array.Copy(initTextColor, length, tc, 0, initTextColor.Length - length);
            Array.Copy(initSceneColor, length, tsc, 0, initSceneColor.Length - length);
            Array.Resize(ref initTextColor, init.Length);
            Array.Resize(ref initSceneColor, init.Length);
            Array.Copy(textColor, 0, initTextColor, length, textColor.Length);
            Array.Copy(sceneColor, 0, initSceneColor, length, sceneColor.Length);
            Array.Copy(tc, 0, initTextColor, length + text.Length, tc.Length);
            Array.Copy(tsc, 0, initSceneColor, length + text.Length, tsc.Length);
            return true;
        }
        internal bool InsertEnd(string text, int length, ConsoleColor textFillColor, ConsoleColor SceneFillColor)
        {
            if (text.Length < length || length < 0) return false;
            ConsoleColor[] tcf = new ConsoleColor[text.Length];
            ConsoleColor[] tscf = new ConsoleColor[text.Length];
            Array.Fill(tcf, textFillColor);
            Array.Fill(tscf, SceneFillColor);
            getLcse = (text.ToString(), tcf.ToArray(), tscf.ToArray());
            end = end.Insert(length, text);
            ConsoleColor[] tc = new ConsoleColor[endTextColor.Length - length - text.Length];
            ConsoleColor[] tsc = new ConsoleColor[endSceneColor.Length - length - text.Length];
            Array.Copy(endTextColor, length, tc, 0, endTextColor.Length - length);
            Array.Copy(endSceneColor, length, tsc, 0, endSceneColor.Length - length);
            Array.Resize(ref endTextColor, end.Length);
            Array.Resize(ref endSceneColor, end.Length);
            Array.Copy(tcf, 0, endTextColor, length, tcf.Length);
            Array.Copy(tscf, 0, endSceneColor, length, tscf.Length);
            Array.Copy(tc, 0, endTextColor, length + text.Length, tc.Length);
            Array.Copy(tsc, 0, endSceneColor, length + text.Length, tsc.Length);
            return true;
        }
        internal bool InsertEnd(string text, int length, ConsoleColor[] textColor, ConsoleColor[] sceneColor)
        {
            if (text.Length != textColor.Length || text.Length != sceneColor.Length ||
            text.Length < length || length < 0) return false;
            getLcse = (text.ToString(), textColor.ToArray(), sceneColor.ToArray());
            end = end.Insert(length, text);
            ConsoleColor[] tc = new ConsoleColor[endTextColor.Length - length - text.Length];
            ConsoleColor[] tsc = new ConsoleColor[endSceneColor.Length - length - text.Length];
            Array.Copy(endTextColor, length, tc, 0, endTextColor.Length - length);
            Array.Copy(endSceneColor, length, tsc, 0, endSceneColor.Length - length);
            Array.Resize(ref endTextColor, end.Length);
            Array.Resize(ref endSceneColor, end.Length);
            Array.Copy(textColor, 0, endTextColor, length, textColor.Length);
            Array.Copy(sceneColor, 0, endSceneColor, length, sceneColor.Length);
            Array.Copy(tc, 0, endTextColor, length + text.Length, tc.Length);
            Array.Copy(tsc, 0, endSceneColor, length + text.Length, tsc.Length);
            return true;
        }
        public TitleColumnInfo ToTitleColumnInfo()
        {
            return new TitleColumnInfo(init,end,
            initTitleSetConditions,initTitleSetConditions1,initTitleSetConditions2,
            endTitleSetConditions,endTitleSetConditions1,endTitleSetConditions2,
            initTitleCondition,endTitleCondition,
            initTextColor,initSceneColor,endTextColor,endSceneColor);
        }
    }
    class TextColumnInfo
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
        Func<TeropColumnInfo[], TitleColumnInfo[], TextColumnInfo[], int, bool>? textSetConditions1 = null;
        Func<TitleColumnInfo[]?, TextColumnInfo, int, bool>? textSetConditions2 = null;
        bool setCondition = true;
        ConsoleColor[] textColor = [];
        ConsoleColor[] textSceneColor = [];
        public TextColumnInfo() { }
        TextColumnInfo(string text,
        Func<int, int, int, bool>? textSetConditions,
        Func<TeropColumnInfo[], TitleColumnInfo[], TextColumnInfo[], int, bool>? textSetConditions1,
        Func<TitleColumnInfo[]?, TextColumnInfo, int, bool>? textSetConditions2, 
        bool setCondition, ConsoleColor[] textColor, ConsoleColor[] textSceneColor)
        {
            this.text = text;
            this.textSetConditions = textSetConditions;
            this.textSetConditions1 = textSetConditions1;
            this.textSetConditions2 = textSetConditions2;
            this.setCondition = setCondition;
            this.textColor = textColor;
            this.textSceneColor = textSceneColor;
        }
        internal bool? Func(int titleLen, int teropLen,
        TeropColumnInfo[] teropCoumnInfos, TitleColumnInfo[] titleColumnInfos, TextColumnInfo[] columnInfos,
        TitleColumnInfo[]? activeTitleColumnInfos)
        {
            bool? b = Func(titleLen, teropLen, text.Length);
            bool? b1 = Func(teropCoumnInfos, titleColumnInfos, columnInfos, text.Length);
            bool? b2 = Func(activeTitleColumnInfos, ToTextColumnInfo(), text.Length);
            if (b == null && b1 == null && b2 == null) return null;
            else if (b == true || b1 == true || b2 == true)
            {
                setCondition = true;
                return true;
            }
            else
            {
                setCondition = false;
                return false;
            }
        }
        bool? Func(int titleLen, int teropLen, int? textLen)
        {
            if (textSetConditions == null) return null;
            return textSetConditions(titleLen, teropLen, text.Length);
        }
        bool? Func(TeropColumnInfo[] teropCoumnInfos, TitleColumnInfo[] titleColumnInfos, TextColumnInfo[] columnInfos, int? textLen)
        {
            if (textSetConditions1 == null) return null;
            return textSetConditions1(teropCoumnInfos,titleColumnInfos,columnInfos,text.Length);
        }
        bool? Func(TitleColumnInfo[]? titleColumnInfo,TextColumnInfo? columnInfo,int? textLen)
        {
            if (textSetConditions2 == null) return null;
            return textSetConditions2(titleColumnInfo,ToTextColumnInfo(),text.Length);
        }
        internal void ReplaceFunc(Func<int, int, int, bool>? newContitions)
        {
            textSetConditions = newContitions;
        }
        internal void ReplaceFunc(Func<TeropColumnInfo[], TitleColumnInfo[], TextColumnInfo[], int, bool>? newContitions)
        {
            textSetConditions1 = newContitions;
        }
        internal void ReplaceFunc(Func<TitleColumnInfo[]?, TextColumnInfo, int, bool>? newContitions)
        {
            textSetConditions2 = newContitions;
        }
        internal void RemoveFunc()
        {
            textSetConditions = null;
            textSetConditions1 = null;
            textSetConditions2 = null;
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
        public TextColumnInfo ToTextColumnInfo()
        {
            return new TextColumnInfo(text,textSetConditions,textSetConditions1,textSetConditions2,setCondition,textColor,textSceneColor);
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
        internal List<TextColumnInfo> texts = [];//title>text<title
        internal ConsoleHost host;
        public ConsoleColor DefaultTextColor { get { return deft; } }
        ConsoleColor deft = DefaultValue.color_text;
        public ConsoleColor DefaultTextSceneColor { get { return defts; } }
        ConsoleColor defts = DefaultValue.color_scene;
        bool overTTRepair = false;
        internal Console(ConsoleHost host, int priority)
        {
            this.host = host;
            this.priority = priority;
        }/*
        bool TextAssets()
        {

        }
        bool TitleAssets()
        {

        }
        bool TeropAssets()
        {

        }
        public bool Insert(int index, int len, string insertValue)
        {
            if (index >= texts.Count) return false;
            if (len >= texts[index].Text.Length) return false;
            if (index < 0 || len < 0) return false;
            TextColumnInfo info = texts[index].ToTextColumnInfo();
            if (!texts[index].Insert(insertValue, len, deft, defts))
            {
                texts[index] = info;
                return false;
            }
            if (texts[index].SetCondition)
            {
                bool? b = texts[index].Func(titles.Count, terops.Count, terops.ToArray(), titles.ToArray(), texts.ToArray(),);
                int code;
                if (b == null || b == true)
                {
                    (string Text, ConsoleColor[] TextColor, ConsoleColor[] TextSceneColor) gcd = texts[index].GetLastChangeSentence;
                    code = Write(new ColumnChangeInfo(priority, insertValue, index, gcd.TextColor, gcd.TextSceneColor, (len, insertValue.Length), ChangeType.InsertAt));
                    if (code != 0)
                    {
                        texts[index] = info;
                        return false;
                    }
                    if (!TeropTitleRepair(texts[index], index))
                    {
                        overTTRepair = true;
                        //return false;
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
            else
            {
                bool? b = texts[index].Func(titles.Count, terops.Count, terops.ToArray(), titles.ToArray(), texts.ToArray(),);
                if (b == true)
                {
                    (string Text, ConsoleColor[] TextColor, ConsoleColor[] TextSceneColor) gcd = texts[index].GetLastChangeSentence;
                    int code = Write(new ColumnChangeInfo(priority, texts[index].Text, index, gcd.TextColor, gcd.TextSceneColor, (0, texts[index].Text.Length), ChangeType.Insert));
                    if (code != 0)
                    {
                        texts[index] = info;
                        return false;
                    }
                    if (!TeropTitleRepair(texts[index], index))
                    {
                        overTTRepair = true;
                        //return false;
                    }
                    return true;
                }
                return true;
            }
        }*/
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