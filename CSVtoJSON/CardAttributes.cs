using System.Collections;
using System.ComponentModel;

namespace CSVtoJSON
{
    public class CardAttributes : StatAttributes
    {
        [DefaultValue("")] public string Name;
        [DefaultValue("")] public string Type;

        public ArrayList Stats = new ArrayList();

        [DefaultValue("")] public string Desc;
        [DefaultValue("")] public string Link;

        public CardAttributes(){}

        public CardAttributes(string name, string type, int atk, int res, int mor, string desc) // Troop or Commander Troop Card Type
        {
            Name = name;
            Type = type;
            Desc = desc;
            Link = null;
            
            Stats.Add(new StatAttributes(atk, res, mor));
        }

        public CardAttributes(string name, string type, int? res, int? mor, string desc) // Support Card Type
        {
            Name = name;
            Type = type;
            Desc = desc;
            Link = null;
            
            Stats.Add(new StatAttributes(res, mor));
        }

        public CardAttributes(string name, string type, string desc) // Officer Card Type
        {
            Name = name;
            Type = type;
            Desc = desc;
            Link = null;
        }

        public CardAttributes(string name, string type, int ini, string desc, string link) // Commander Card Type
        {
            Name = name;
            Type = type;
            Desc = desc;
            Link = link;
            
            Stats.Add(new StatAttributes(ini));
        }

        public new string ToString()
        {
            return Name + " " + Type;
        }
    }
}