using Newtonsoft.Json;

namespace PoemGenerator.OntolisAdapter.JsonModel
{
    public class JsonRelation
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("source_node_id")]
        public int SourceNodeId { get; set; }
        
        [JsonProperty("destination_node_id")]
        public int DestinationNodeId { get; set; }
    }
}