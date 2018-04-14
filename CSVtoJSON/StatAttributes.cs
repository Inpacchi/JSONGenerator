using System.ComponentModel;

namespace CSVtoJSON
{
    public class StatAttributes
    {
        [DefaultValue(0)] public int? Atk;
        [DefaultValue(0)] public int? Res;
        [DefaultValue(0)] public int? Mor;
        [DefaultValue(0)] public int? Ini;

        public StatAttributes()
        {
            Atk = 0;
            Res = 0;
            Mor = 0;
            Ini = 0;
        }

        public StatAttributes(int? res, int? mor)
        {
            Atk = null;
            Res = res;
            Mor = mor;
            Ini = null;
        }

        public StatAttributes(int atk, int res, int mor)
        {
            Atk = atk;
            Res = res;
            Mor = mor;
            Ini = null;
        }

        public StatAttributes(int ini)
        {
            Atk = null;
            Res = null;
            Mor = null;
            Ini = ini;
        }
    }
}