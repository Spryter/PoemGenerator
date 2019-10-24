using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PoemGenerator
{
	public partial class MainForm : Form
	{
		JObject ontology;
		JEnumerable<JToken> nodes;
		JEnumerable<JToken> relations;
		public MainForm()
		{
			InitializeComponent();
			ontology = JObject.Parse(File.ReadAllText(@"C:\Users\Fox\source\repos\PoemGenerator\PoemGenerator\black_poems.ont"));
			nodes = ((JArray)ontology["nodes"]).Children();
			relations = ((JArray)ontology["relations"]).Children();
		}

		private void btGenerate_Click(object sender, EventArgs e)
		{
			var poemNode = nodes.Where(n => n["name"].ToString() == "стихотворение").FirstOrDefault();
			var strings = ontology.GetNodesByBackwardRelationName(poemNode, "a_part_of");
			var string1 = strings.Where(s => s["name"].ToString() == "1 строка").FirstOrDefault();
			var tokens = ontology.GetNodesByBackwardRelationName(string1, "a_part_of");
			var token1 = tokens.Where(t => ontology.GetNodesByForwardRelationName(t, "order").FirstOrDefault()["name"].ToString() == "1").FirstOrDefault();
			var elementOfToken1 = ontology.GetNodesByForwardRelationName(token1, "element").FirstOrDefault();
			var concreteElements = ontology.GetNodesByBackwardRelationName(elementOfToken1, "is_a");
			Random rnd = new Random();
			int index = rnd.Next(concreteElements.Count());
			var concreteElement = concreteElements.ElementAt(index);
			rtbPoem.Text = concreteElement["name"].ToString();
		}
	}
}
