using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PoemGenerator
{
	public static class jsonExtensions
	{
		public static IEnumerable<JToken> GetNodesByForwardRelationName(this JObject ontology, JToken baseNode, string relationName)
		{
			var nodes = ((JArray)ontology["nodes"]).Children();
			var relations = ((JArray)ontology["relations"]).Children();
			var relation = relations.Where(r => r["source_node_id"].ToString() == baseNode["id"].ToString() && r["name"].ToString() == relationName).FirstOrDefault();
			var needRelationDestinations = relations.Where(r => r["source_node_id"].ToString() == baseNode["id"].ToString() && r["name"].ToString() == relationName).Select(r => r["destination_node_id"].ToString());
			return nodes.Where(n => needRelationDestinations.Contains(n["id"].ToString()));
		}
		public static IEnumerable<JToken> GetNodesByBackwardRelationName(this JObject ontology, JToken baseNode, string relationName)
		{
			var nodes = ((JArray)ontology["nodes"]).Children();
			var relations = ((JArray)ontology["relations"]).Children();
			var relation = relations.Where(r => r["source_node_id"].ToString() == baseNode["id"].ToString() && r["name"].ToString() == relationName).FirstOrDefault();
			var needRelationDestinations = relations.Where(r => r["destination_node_id"].ToString() == baseNode["id"].ToString() && r["name"].ToString() == relationName).Select(r => r["source_node_id"].ToString());
			return nodes.Where(n => needRelationDestinations.Contains(n["id"].ToString()));
		}
	}
}
