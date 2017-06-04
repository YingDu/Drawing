using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Drawing.Models
{
    public class Diagram
    {
        private static JsonSerializerSettings JsonSerializerSettings => new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };

        public Guid Id { get; set; }

        public string Name { get; set; }

        public List<ShapeBase> Shapes { get; set; }

        public string JsonValue
        {
            get
            {
                return JsonConvert.SerializeObject(Shapes, JsonSerializerSettings);
            }
        }

        public void Pase(string jsonValue)
        {
            Shapes = (List<ShapeBase>)JsonConvert.DeserializeObject(jsonValue, JsonSerializerSettings);
        }
    }
}
