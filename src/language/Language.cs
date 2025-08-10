using AutoAppdater.Property;

namespace AutoAppdater.Language
{
    public class Language
    {
        PropertyGroup group = new PropertyGroup();
        PropertyGroup? Attributed = null;
        public Language() { }
        public Language(PropertyGroup languageGroup)
        {
            group = languageGroup.ToPropertyGroup();
            group.Sort(SortType.Attribute_ASC);
        }
        object combinerLocker = new object();
        public string? Convert(string text)
        {
            lock (combinerLocker)
            {
                if (Attributed == null)
                {
                    Property.Property? prop = group.GetPropertyByName(text);
                    if (prop != null)
                    {
                        Task t = Task.Run(() => SetCurrentAttribute(prop.Attribute));
                        return prop.Value.StrValue;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    Property.Property? prop = Attributed.GetPropertyByName(text);
                    if (prop != null)
                    {
                        return prop.Value.StrValue;
                    }
                    else
                    {
                        prop = group.GetPropertyByName(text);
                        if (prop != null)
                        {
                            Task t = Task.Run(() => SetCurrentAttribute(prop.Attribute));
                            return prop.Value.StrValue;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }
        void SetCurrentAttribute(string attribute)
        {
            lock (combinerLocker)
            {
                Property.Property[]? properties = group.GetAllPropertyByAttribute(attribute);
                if (properties != null)
                {
                    Attributed = new PropertyGroup(properties);
                }                
            }
        }
    }
}