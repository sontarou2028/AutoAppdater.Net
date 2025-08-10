using System.Data;
using AutoAppdater.Language;

namespace AutoAppdater.Log
{
    enum LogType
    {
        Internal,
        External,
        NoWindow,
    }
    public class Log
    {
        LogType logType;
        Language.Language? language;
        public bool AlwaysTranslate = false;
        internal Log(LogType type)
        {
            logType = type;
            language = null;
        }
        internal Log(LogType type, Language.Language language)
        {
            logType = type;
            this.language = language;
        }
        string? LanguageConverter(string message)
        {
            if (language != null) return language.Convert(message);
            else return null;
        }
        string TimeInsert(string message)
        {
            DateTime dt = DateTime.Now;
            return "[" + dt.ToString("yyyy-MM/dd HH:mm::ss") + "] " + message;
        }
        void Logging(string message)
        {
            
        }
        public void Info(string message, bool translate)
        {
            if (translate)
            {
                string? lang = LanguageConverter(message);
                if (lang != null) message = lang;
            }
            message = message.Insert(0, "[info]");
            message = TimeInsert(message);
            Logging(message);
        }
        public void Error(string message, bool translate)
        {
            if (translate)
            {
                string? lang = LanguageConverter(message);
                if (lang != null) message = lang;
            }
            message = message.Insert(0, "[error]");
            message = TimeInsert(message);
            Logging(message);
        }
        public void Guide(string message, bool translate)
        {
            if (translate)
            {
                string? lang = LanguageConverter(message);
                if (lang != null) message = lang;
            }
            message = message.Insert(0, "[guide]");
            message = TimeInsert(message);
            Logging(message);
        }
        public void Warning(string message, bool translate)
        {
            if (translate)
            {
                string? lang = LanguageConverter(message);
                if (lang != null) message = lang;
            }
            message = message.Insert(0, "[warn]");
            message = TimeInsert(message);
            Logging(message);
        }
        public void Info(string message)
        {
            if (AlwaysTranslate)
            {
                string? lang = LanguageConverter(message);
                if (lang != null) message = lang;
            }
            message = message.Insert(0, "[info]");
            message = TimeInsert(message);
            Logging(message);
        }
        public void Error(string message)
        {
            if (AlwaysTranslate)
            {
                string? lang = LanguageConverter(message);
                if (lang != null) message = lang;
            }
            message = message.Insert(0, "[error]");
            message = TimeInsert(message);
            Logging(message);
        }
        public void Guide(string message)
        {
            if (AlwaysTranslate)
            {
                string? lang = LanguageConverter(message);
                if (lang != null) message = lang;
            }
            message = message.Insert(0, "[guide]");
            message = TimeInsert(message);
            Logging(message);
        }
        public void Warning(string message)
        {
            if (AlwaysTranslate)
            {
                string? lang = LanguageConverter(message);
                if (lang != null) message = lang;
            }
            message = message.Insert(0, "[warn]");
            message = TimeInsert(message);
            Logging(message);
        }
    }
}