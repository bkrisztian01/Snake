using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Drawing;

namespace Snake //.Utility ??
{
    public class Config //sealed?
    {
        Dictionary<string, string> cfg = new Dictionary<string, string>();
        public XmlSerializer serializer = new XmlSerializer(typeof(List<KeyValuePair<string, string>>));

        [Serializable]
        [XmlType(TypeName = "KeyValuePair")]
        public class KeyValuePair<K, V>
        {
            public K Key
            { get; set; }

            public V Value
            { get; set; }

            public KeyValuePair(K key, V value)
            {
                Key = key;
                Value = value;
            }

            public KeyValuePair()
            {

            }
        }

        private Config()
        {
            CreateFileIfDoesntExist();

            using (StreamReader r = new StreamReader("config.xml"))
            {
                var temp = serializer.Deserialize(r);
                cfg = listToDictionary((List<KeyValuePair<string, string>>)temp);
            }
        }

        private static Config instance = null;

        public static Config Instance
        {
            get //?
            {
                if (instance == null)
                {
                    instance = new Config();
                }
                return instance;
            }
        }

        private Dictionary<string, string> listToDictionary(List<KeyValuePair<string, string>> list)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            foreach (var item in list)
            {
                result.Add(item.Key, item.Value);
            }
            return result;
        }

        private List<KeyValuePair<string, string>> dictionaryToList(Dictionary<string, string> dictionary)
        {
            List<KeyValuePair<string, string>> result = new List<KeyValuePair<string, string>>();
            foreach (var item in dictionary)
            {
                result.Add(new KeyValuePair<string, string>(item.Key, item.Value));
            }

            return result;
        }

        private void CreateFileIfDoesntExist()
        {
            if (!File.Exists("config.xml") || new FileInfo("config.xml").Length == 0)
            {
                List<KeyValuePair<string, string>> cfg = new List<KeyValuePair<string, string>>();

                cfg.Add(new KeyValuePair<string, string>("PlayerCount", "2"));

                cfg.Add(new KeyValuePair<string, string>("Up1", "W"));
                cfg.Add(new KeyValuePair<string, string>("Down1", "S"));
                cfg.Add(new KeyValuePair<string, string>("Left1", "A"));
                cfg.Add(new KeyValuePair<string, string>("Right1", "D"));

                cfg.Add(new KeyValuePair<string, string>("Up2", "I"));
                cfg.Add(new KeyValuePair<string, string>("Down2", "K"));
                cfg.Add(new KeyValuePair<string, string>("Left2", "J"));
                cfg.Add(new KeyValuePair<string, string>("Right2", "L"));

                cfg.Add(new KeyValuePair<string, string>("Color1", Color.Blue.ToArgb().ToString()));
                cfg.Add(new KeyValuePair<string, string>("Color2", Color.Green.ToArgb().ToString()));

                cfg.Add(new KeyValuePair<string, string>("GrowAppleChance", "20"));
                cfg.Add(new KeyValuePair<string, string>("DoubleGrowAppleChance", "20"));
                cfg.Add(new KeyValuePair<string, string>("ChangeControlAppleChance", "20"));
                cfg.Add(new KeyValuePair<string, string>("SpeedBoostAppleChance", "20"));
                cfg.Add(new KeyValuePair<string, string>("BlackAppleChance", "20"));

                cfg.Add(new KeyValuePair<string, string>("AppleCount", "5"));

                using (StreamWriter writer = new StreamWriter("config.xml"))
                {
                    serializer.Serialize(writer, cfg);
                }
            }
        }

        public void StoreCfgFile()
        {
            File.WriteAllText("config.xml", String.Empty);
            using (StreamWriter w = new StreamWriter("config.xml"))
            {
                serializer.Serialize(w, dictionaryToList(cfg));
            }
        }

        public Dictionary<string, string> GetAll(string value)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            foreach (var i in cfg.Keys)
            {
                if (i.Contains(value))
                {
                    result.Add(i, cfg[i]);
                }
            }

            return result;
        }

        public bool IDExists(string id)
        {
            return cfg.Keys.Contains(id);
        }

        public string Get(string id)
        {
            return cfg[id];
        }

        public void Set(string id, string value)
        {
            cfg[id] = value;
        }

        public void NewID(string id, string value)
        {
            cfg.Add(id, value);
        }
    }
}
