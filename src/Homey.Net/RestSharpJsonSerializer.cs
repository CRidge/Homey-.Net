using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp.Serializers;

namespace Homey.Net
{
    public class RestSharpJsonSerializer : ISerializer
    {
        private readonly Newtonsoft.Json.JsonSerializer _serializer;

        public RestSharpJsonSerializer()
        {
            ContentType = RestSharp.ContentType.Json;
            _serializer = new Newtonsoft.Json.JsonSerializer
            {
                MissingMemberHandling = MissingMemberHandling.Ignore,
                NullValueHandling = NullValueHandling.Include,
                DefaultValueHandling = DefaultValueHandling.Include,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
        }

        public RestSharpJsonSerializer(Newtonsoft.Json.JsonSerializer serializer)
        {
            ContentType = RestSharp.ContentType.Json;
            _serializer = serializer;
        }

        public string Serialize(object obj)
        {
            using (StringWriter stringWriter = new StringWriter())
            {
                using (JsonTextWriter jsonTextWriter = new JsonTextWriter(stringWriter))
                {
                    jsonTextWriter.Formatting = Formatting.Indented;
                    jsonTextWriter.QuoteChar = '"';

                    _serializer.Serialize(jsonTextWriter, obj);

                    string result = stringWriter.ToString();
                    return result;
                }
            }
        }

        public RestSharp.ContentType ContentType
        {
            get;
            set;
        }
    }
}
