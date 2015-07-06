using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace Vale.Parsing
{
    public class JsonParser
    {
        private static readonly JsonParser Singleton = new JsonParser();

        public static JsonParser Instance { get { return Singleton; } }

        /// <summary>
        /// Gets whether the UnitParser has finished parsing for unit information.
        /// </summary>
        public bool Busy { get; private set; }

        /// <summary>
        /// Parses all of the unit data from 'units.txt' and stores it into Resource.
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