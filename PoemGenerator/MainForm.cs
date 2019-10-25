using System;
using System.Linq;
using System.Windows.Forms;
using PoemGenerator.OntologyLoader.Model;

namespace PoemGenerator
{
	public partial class MainForm : Form
	{
		private readonly Ontology _ontology;
		
		public MainForm()
		{
			InitializeComponent();
			_ontology = OntologyLoader.OntologyLoader.LoadByPath(
				@"C:\Users\Антон Зыков\Documents\Учеба\PoemGenerator\PoemGenerator\black_poems.ont");
		}

		private void btGenerate_Click(object sender, EventArgs e)
		{
			var elements = _ontology.Nodes
				.Get("стихотворение")
				.To("a_part_of")
				.Get("1 строка")
				.To("a_part_of")
				.FirstOrDefault(x => x.From("order").Get("1") != null)
				.From("element")
				.FirstOrDefault()
				.To("is_a");
			var rnd = new Random();
			var index = rnd.Next(elements.Count);
			var element = elements.ElementAt(index);
			rtbPoem.Text = element.Name;
		}
	}
}
