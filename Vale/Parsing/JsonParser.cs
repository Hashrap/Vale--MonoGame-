using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Vale.Parsing
{
   public class JsonParser
    {
        static readonly JsonParser singleton = new JsonParser();
        public static JsonParser Instance { get { return singleton; } }
        
        /// <summary>
        /// Gets whether the UnitParser has finished parsing for unit information.
        /// </summary>
        public bool Busy { get; private set; }
        
        /// <summary>
        /// Parses all of the unit data from 'units.txt' and stores it locally in a dictionary.
        /// </summary>
        public void ParseData<T>(string fileLocation)
        {
            Busy = true;

            var jsonReader = new JsonTextReader(new StreamReader(fileLocation));
            var jsonSerializer = new JsonSerializer();
            var parsedData = jsonSerializer.Deserialize<JObject>(jsonReader);
            
            foreach (var entryData in parsedData)
                Resource.Instance.AddInfo(entryData.Key, JsonConvert.DeserializeObject<T>(entryData.Value.ToString()));
            
            Busy = false;
        }
    }
}
