﻿using Newtonsoft.Json;

namespace PoemGenerator.OntologyLoader.JsonModel
{
    public class JsonNode
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}