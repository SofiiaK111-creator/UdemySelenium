using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace CSharpSelFramework.utilities
{

    public class JsonReader
    {
        public JsonReader()
        {

        }

        public string ExtractData(string tokenName)
        {
            string jsonString = File.ReadAllText("utilities/TestData.json");
            var jsonObject = JToken.Parse(jsonString);
            return jsonObject.SelectToken(tokenName)!.Value<string>()!;
        }

        public string[] ExtractDataArray(string tokenName)
        {
            string jsonString = File.ReadAllText("utilities/TestData.json");
            var jsonObject = JToken.Parse(jsonString);
            return jsonObject.SelectTokens(tokenName)!.Values<string>()!.ToArray()!;
        }
    }
}
