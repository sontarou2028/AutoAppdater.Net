using AutoAppdater.Language;

namespace AutoAppdater.Log
{
    enum LogOut
    {
        Internal,
        External,
        NoWindow,
    }
    public class Log
    {
        LogOut logOut;
        Language.Language? language;
        public bool AlwaysTranslate = false;
        internal Log()
        {
            language = null;
        }
        internal Log(Language.Language language)
        {
            this.language = language;
        }
        string? LanguageConverter(string message)
        {
            if (language != null) return language.Convert(message);
            else return null;
        }
        public void Info(string message,bool translate)
        {
            if (translate)
            {
                string? lang = LanguageConverter(message);
                if (lang != null) message = lang;
            }
        }
        public void Error(string message,bool translate)
        {
            if (translate)
            {
                string? lang = LanguageConverter(message);
                if (lang != null) message = lang;
            }
        }
        public void Guide(string message,bool translate)
        {
            if (translate)
            {
                string? lang = LanguageConverter(message);
                if (lang != null) message = lang;
            }
        }
        public void Warning(string message,bool translate)
        {
            if (translate)
            {
                string? lang = LanguageConverter(message);
                if (lang != null) message = lang;
            }
        }
        public void Info(string message)
        {
            if (AlwaysTranslate)
            {
                string? lang = LanguageConverter(message);
                if (lang != null) message = lang;
            }
        }
        public void Error(string message)
        {
            if (AlwaysTranslate)
            {
                string? lang = LanguageConverter(message);
                if (lang != null) message = lang;
            }
        }
        public void Guide(string message)
        {
            if (AlwaysTranslate)
            {
                string? lang = LanguageConverter(message);
                if (lang != null) message = lang;
            }
        }
        public void Warning(string message)
        {
            if (AlwaysTranslate)
            {
                string? lang = LanguageConverter(message);
                if (lang != null) message = lang;
            }
        }
    }
}