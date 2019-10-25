using System.Collections.Generic;
using Newtonsoft.Json;

namespace PoemGenerator.OntologyLoader.JsonModel
{
    public class JsonOntology
    {
        [JsonProperty("nodes")]
        public List<JsonNode> Nodes { get; set; }
        
        [JsonProperty("relations")]
        public List<JsonRelation> Relations { get; set; }
    }
}