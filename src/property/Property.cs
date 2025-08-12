using System.Xml;

namespace AutoAppdater.Property
{
    public static class PropertySelializer
    {
        public static PropertyGroup? PropertyDeselializer(string inline)
        {
            List<Property> properties = [];
            List<ReadOnlyProperty> readOnlies = [];
            inline = inline.Replace("\r", "");
            string nothing = "";
            string[] reverse = { "\n" };
            string[] ichole = { "=" };
            string[] space = { " ", "　" };
            string[] bool_true = { "True", "true" };
            string[] bool_false = { "False", "false" };
            string[] attribute_start = { "[" };
            string[] attribute_end = { "]" };
            string[] refact_ignore = { "#" };
            string[] refact_readonly = { "**" };
            string[] esc_return = { @"\\n" };
            string[] line = inline.Split(reverse[0].ToCharArray());
            string currendAttribute = nothing;
            foreach (string s in line)
            {
                string unspaced = s;
                foreach (string t in space)
                {
                    unspaced = unspaced.Replace(t, nothing);
                }
                if (unspaced.Length == 0) continue;
                string initial = unspaced.Substring(0, 1);
                foreach (string t in refact_ignore)
                {
                    if (t == initial)
                    {
                        continue;
                    }
                }
                bool readOnly = false;
                if (unspaced.Length >= 2)
                {
                    initial = unspaced.Substring(0, 2);
                    foreach (string t in refact_readonly)
                    {
                        if (t == initial)
                        {
                            readOnly = true;
                            break;
                        }
                    }
                }
                string attributecheck = unspaced;
                foreach (string t in attribute_start)
                {
                    if (attributecheck.Contains(t))
                    {
                        attributecheck.Replace(t, attribute_start[0]);
                    }
                }
                foreach (string t in attribute_end)
                {
                    if (attributecheck.Contains(t))
                    {
                        attributecheck.Replace(t, attribute_end[0]);
                    }
                }
                if (attributecheck.Length >= 2 &&
                    attributecheck.Substring(0, 1) == attribute_start[0] &&
                    attributecheck.Substring(attributecheck.Length - 1, 1) == attribute_end[0])
                {
                    currendAttribute = attributecheck.Substring(1, attributecheck.Length - 2);
                    continue;
                }
                char[] charArray = s.ToCharArray();
                int startLen = 0;
                bool icholeExist = false;
                int icholeLen = 0;
                foreach (char t in charArray)
                {
                    foreach (string u in ichole)
                    {
                        if (t.ToString() == u)
                        {
                            icholeExist = true;
                            icholeLen = u.Length;
                            break;
                        }
                    }
                    if (icholeExist) break;
                    startLen++;
                }
                if (icholeExist)
                {
                    Value val = new Value();
                    if (s.Length == startLen + icholeLen)
                    {
                        //nullvalue
                    }
                    else
                    {
                        unspaced = s.Substring(startLen + icholeLen);
                        foreach (string t in space)
                        {
                            unspaced = unspaced.Replace(t, nothing);
                        }
                        if (int.TryParse(unspaced, out int i))
                        {
                            val.IntValue = i;
                        }
                        else if (bool_true.Contains(unspaced))
                        {
                            val.BoolValue = true;
                        }
                        else if (bool_false.Contains(unspaced))
                        {
                            val.BoolValue = false;
                        }
                        else
                        {
                            unspaced = s.Substring(startLen + icholeLen);
                            foreach (string sr in esc_return)
                            {
                                unspaced = unspaced.Replace(sr, reverse[0]);
                            }
                            val.StrValue = unspaced;
                        }
                    }
                    string name = s.Substring(0, startLen);
                    foreach (string sr in esc_return)
                    {
                        name = name.Replace(sr, reverse[0]);
                    }
                    if (readOnly) readOnlies.Add(new ReadOnlyProperty(name, currendAttribute, val));
                    else properties.Add(new Property(name, currendAttribute, val));
                }
                else
                {
                    string name = s;
                    foreach (string sr in esc_return)
                    {
                        name = name.Replace(sr, reverse[0]);
                    }
                    if(readOnly) readOnlies.Add(new ReadOnlyProperty(name, currendAttribute, new Value()));
                    else properties.Add(new Property(name, currendAttribute, new Value()));
                }
            }
            return new PropertyGroup(properties.ToArray(),readOnlies.ToArray());
        }
        public static string PropertySerializer(PropertyGroup propertyGroup)
        {
            PropertyGroup group = propertyGroup.ToPropertyGroup();
            group.Sort(SortType.Attribute_ASC);
            string nothing = "";
            string ichole = "=";
            string att_start = "[";
            string att_end = "]";
            string chr_return = "\n";
            string chr_readonly = "**";
            string esc_return = @"\\n";
            string nullstr = nothing;
            string currentAttribute = nothing;
            string config = nothing;
            bool flag = false;
            List<string> attributes = [];
            foreach (Property p in group.Properties)
            {
                flag = true;
                if (p.Attribute == currentAttribute)
                {
                    config += p.Name.Replace(chr_return,esc_return) + ichole +
                    (p.Value.StrValue == null ? nullstr : p.Value.StrValue.Replace(chr_return,esc_return)) +
                    (p.Value.IntValue == null ? nullstr : p.Value.IntValue) +
                    (p.Value.BoolValue == null ? nullstr : p.Value.BoolValue) + chr_return;
                }
                else
                {
                    config += att_start + p.Attribute + att_end + chr_return +
                    p.Name.Replace(chr_return,esc_return) + ichole +
                    (p.Value.StrValue == null ? nullstr : p.Value.StrValue.Replace(chr_return,esc_return)) +
                    (p.Value.IntValue == null ? nullstr : p.Value.IntValue) +
                    (p.Value.BoolValue == null ? nullstr : p.Value.BoolValue) + chr_return;
                    attributes.Add(currentAttribute);
                    foreach (ReadOnlyProperty irp in group.GetAllReadOnlyPropertyByAttribute(currentAttribute))
                    {
                        config += chr_readonly.Replace(chr_return,esc_return) + irp.Name + ichole +
                        (irp.Value.StrValue == null ? nullstr : irp.Value.StrValue.Replace(chr_return,esc_return)) +
                        (irp.Value.IntValue == null ? nullstr : irp.Value.IntValue) +
                        (irp.Value.BoolValue == null ? nullstr : irp.Value.BoolValue) + chr_return;
                    }
                    currentAttribute = p.Attribute;
                }
            }
            nullstr = nothing;
            currentAttribute = nothing;
            config = nothing;
            flag = false;
            ReadOnlyProperty[] readOnlies = group.ReadOnlyProperties;
            for (int i = 0; i < readOnlies.Length; i++)
            {
                ReadOnlyProperty rp = readOnlies[i];
                if (attributes.Contains(rp.Attribute))
                {
                    string att = rp.Attribute;
                    for (int j = i + 1; j < readOnlies.Length; j++)
                    {
                        if (readOnlies[j].Attribute != att)
                        {
                            i = j;
                            break;
                        }
                    }
                }
                else
                {
                    config += att_start + rp.Attribute + att_end + chr_return;
                    string att = rp.Attribute;
                    for (int j = i; j < readOnlies.Length; j++)
                    {
                        if (rp.Attribute != att)
                        {
                            i = j;
                            break;
                        }
                        else
                        {
                            config += chr_readonly + rp.Name.Replace(chr_return,esc_return) + ichole +
                            (rp.Value.StrValue == null ? nullstr : rp.Value.StrValue.Replace(chr_return,esc_return)) +
                            (rp.Value.IntValue == null ? nullstr : rp.Value.IntValue) +
                            (rp.Value.BoolValue == null ? nullstr : rp.Value.BoolValue) + chr_return;
                        }
                    }
                }
            }
            if (flag) config = config.Substring(0, config.Length - chr_return.Length);
            return config;
        }
    }
    public enum SortType
    {
        Attribute_ASC,
        Attribute_DESC,
        Name_ASC,
        Name_DESC,
    }
    public class PropertyGroup
    {
        public Property[] Properties { get { return prop.ToArray(); } }
        public ReadOnlyProperty[] ReadOnlyProperties { get { return irProp.ToArray(); } }
        List<Property> prop = [];
        List<ReadOnlyProperty> irProp = [];
        public PropertyGroup ToPropertyGroup()
        {
            return new PropertyGroup(Properties, ReadOnlyProperties);
        }
        public PropertyGroup() { }
        public PropertyGroup(Property[] properties)
        {
            prop = properties.ToList();
        }
        public PropertyGroup(ReadOnlyProperty[] readOnlyProperties)
        {
            irProp = readOnlyProperties.ToList();
        }
        public PropertyGroup(Property[] properties, ReadOnlyProperty[] readOnlyProperties)
        {
            prop = properties.ToList();
            irProp = readOnlyProperties.ToList();
        }
        public void Sort(SortType type)
        {
            if (type == SortType.Attribute_ASC)
            {
                prop.Sort((a, b) => a.Attribute.CompareTo(b.Attribute));
                irProp.Sort((a, b) => a.Attribute.CompareTo(b.Attribute));
            }
            else if (type == SortType.Attribute_DESC)
            {
                prop.Sort((a, b) => b.Attribute.CompareTo(a.Attribute));
                irProp.Sort((a, b) => b.Attribute.CompareTo(a.Attribute));
            }
            else if (type == SortType.Name_ASC)
            {
                prop.Sort((a, b) => a.Name.CompareTo(b.Name));
                irProp.Sort((a, b) => a.Name.CompareTo(b.Name));
            }
            else if (type == SortType.Name_DESC)
            {
                prop.Sort((a, b) => b.Name.CompareTo(a.Name));
                irProp.Sort((a, b) => b.Name.CompareTo(a.Name));
            }
        }
        public int SetProperty(Property property)
        {
            prop.Add(property);
            return prop.Count - 1;
        }
        public int[] SetProperty(Property[] property)
        {
            int startIndex = prop.Count - 1;
            prop.AddRange(property);
            int[] count = new int[property.Length];
            for (int i = 0; i < property.Length; i++)
            {
                count[i] = startIndex + i;
            }
            return count;
        }
        public int SetReadOnlyProperty(ReadOnlyProperty property)
        {
            irProp.Add(property);
            return prop.Count - 1;
        }
        public int[] SetReadOnlyProperty(ReadOnlyProperty[] property)
        {
            int startIndex = irProp.Count - 1;
            irProp.AddRange(property);
            int[] count = new int[property.Length];
            for (int i = 0; i < property.Length; i++)
            {
                count[i] = startIndex + i;
            }
            return count;
        }
        public Property? GetPropertyByName(string name)
        {
            foreach (Property p in prop)
            {
                if (name == p.Name)
                {
                    return p;
                }
            }
            return null;
        }
        public ReadOnlyProperty? GetReadOnlyPropertyByName(string name)
        {
            foreach (ReadOnlyProperty p in irProp)
            {
                if (name == p.Name)
                {
                    return p;
                }
            }
            return null;
        }
        public Property? GetPropertyByValue(Value value)
        {
            foreach (Property p in prop)
            {
                if (value.Certificater(p.Value))
                {
                    return p;
                }
            }
            return null;
        }
        public ReadOnlyProperty? GetReadOnlyPropertyByValue(Value value)
        {
            foreach (ReadOnlyProperty p in irProp)
            {
                if (value.Certificater(p.Value))
                {
                    return p;
                }
            }
            return null;
        }
        public Property[] GetAllPropertyByAttribute(string attribute)
        {
            List<Property> list = new List<Property>();
            foreach (Property p in prop)
            {
                if (attribute == p.Attribute)
                {
                    list.Add(p);
                }
            }
            if (list.Count != 0)
            {
                return list.ToArray();
            }
            else return [];
        }
        public ReadOnlyProperty[] GetAllReadOnlyPropertyByAttribute(string attribute)
        {
            List<ReadOnlyProperty> list = new List<ReadOnlyProperty>();
            foreach (ReadOnlyProperty p in irProp)
            {
                if (attribute == p.Attribute)
                {
                    list.Add(p);
                }
            }
            if (list.Count != 0)
            {
                return list.ToArray();
            }
            else return [];
        }
        public Property[] GetAllPropertiesByName(string name)
        {
            List<Property> list = new List<Property>();
            foreach (Property p in prop)
            {
                if (name == p.Name)
                {
                    list.Add(p);
                }
            }
            if (list.Count != 0)
            {
                return list.ToArray();
            }
            else return [];
        }
        public ReadOnlyProperty[] GetAllReadOnlyPropertyByName(string name)
        {
            List<ReadOnlyProperty> list = new List<ReadOnlyProperty>();
            foreach (ReadOnlyProperty p in irProp)
            {
                if (name == p.Name)
                {
                    list.Add(p);
                }
            }
            if (list.Count != 0)
            {
                return list.ToArray();
            }
            else return [];
        }
        public Property[] GetAllPropertyByValue(Value value)
        {
            List<Property> list = new List<Property>();
            foreach (Property p in prop)
            {
                if (value.Certificater(p.Value))
                {
                    list.Add(p);
                }
            }
            if (list.Count != 0)
            {
                return list.ToArray();
            }
            return [];
        }
        public ReadOnlyProperty[] GetAllReadOnlyPropertyByValue(Value value)
        {
            List<ReadOnlyProperty> list = new List<ReadOnlyProperty>();
            foreach (ReadOnlyProperty p in irProp)
            {
                if (value.Certificater(p.Value))
                {
                    list.Add(p);
                }
            }
            if (list.Count != 0)
            {
                return list.ToArray();
            }
            return [];
        }
    }
    public class Value
    {
        public int? IntValue;
        public string? StrValue;
        public bool? BoolValue;
        public Value() { }
        public Value(int? intValue)
        {
            IntValue = intValue;
        }
        public Value(string? strValue)
        {
            StrValue = strValue;
        }
        public Value(bool? boolValue)
        {
            BoolValue = boolValue;
        }
        public Value(int? intValue, string? strValue)
        {
            IntValue = intValue;
            StrValue = strValue;
        }
        public Value(int? intValue, bool? boolValue)
        {
            IntValue = intValue;
            BoolValue = boolValue;
        }
        public Value(string? strValue, bool? boolValue)
        {
            StrValue = strValue;
            BoolValue = boolValue;
        }
        public Value(int? intValue, string? strValue, bool? boolValue)
        {
            IntValue = intValue;
            StrValue = strValue;
            BoolValue = boolValue;
        }
        public bool Certificater(Value value)
        {
            if (IntValue != value.IntValue) return false;
            if (StrValue != value.StrValue) return false;
            if (BoolValue != value.BoolValue) return false;
            return true;
        }
        public bool Certificater(int? intValue)
        {
            if (IntValue != intValue) return false;
            return true;
        }
        public bool Certificater(string? strValue)
        {
            if (StrValue != strValue) return false;
            return true;
        }
        public bool Certificater(bool? boolValue)
        {
            if (BoolValue != boolValue) return false;
            return true;
        }
        public bool Certificater(string? strValue, bool? boolValue)
        {
            if (StrValue != strValue) return false;
            if (BoolValue != boolValue) return false;
            return true;
        }
        public bool Certificater(int? intValue, bool? boolValue)
        {
            if (IntValue != intValue) return false;
            if (BoolValue != boolValue) return false;
            return true;
        }
        public bool Certificater(int? intValue, string? strValue)
        {
            if (IntValue != intValue) return false;
            if (StrValue != strValue) return false;
            return true;
        }
        public bool Certificater(int? intValue, string? strValue, bool? boolValue)
        {
            if (IntValue != intValue) return false;
            if (StrValue != strValue) return false;
            if (BoolValue != boolValue) return false;
            return true;
        }
        public Value GetInstance()
        {
            return new Value(IntValue, StrValue, BoolValue);
        }
    }
    public class Property
    {
        public string Name { get { return name; } }
        public string Attribute { get { return attribute; } }
        public Value Value { get { return value; } set { this.value = value; } }
        string name;
        string attribute;
        Value value = new Value();
        public Property(string name, string attribute)
        {
            this.name = name;
            this.attribute = attribute;
        }
        public Property(string name, string attribute, Value value)
        {
            this.name = name;
            this.attribute = attribute;
            this.value = value;
        }
        public Property ToProperty()
        {
            return new Property(Name, Attribute, Value);
        }
        public ReadOnlyProperty ToReadOnlyProperty()
        {
            return new ReadOnlyProperty(Name, Attribute, Value);
        }
    }
    public class ReadOnlyProperty
    {
        public string Name { get { return name; } }
        public string Attribute { get { return attribute; } }
        public Value Value { get { return value; } }
        string name;
        string attribute;
        Value value = new Value();
        public ReadOnlyProperty(string name, string attribute)
        {
            this.name = name;
            this.attribute = attribute;
        }
        public ReadOnlyProperty(string name, string attribute, Value value)
        {
            this.name = name;
            this.attribute = attribute;
            this.value = value;
        }
        public Property ToProperty()
        {
            return new Property(Name, Attribute, Value);
        }
        public ReadOnlyProperty ToReadOnlyProperty()
        {
            return new ReadOnlyProperty(Name, Attribute, Value);
        }
    }
}