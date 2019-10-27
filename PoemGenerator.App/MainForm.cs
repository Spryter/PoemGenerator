using System;
using System.Windows.Forms;
using PoemGenerator.GeneratorComponent;
using PoemGenerator.OntologyComponent.Loader;

namespace PoemGenerator.App
{
	public partial class MainForm : Form
	{
		private Generator _generator;
		
		public MainForm()
		{
			InitializeComponent();
		}

		private void btGenerate_Click(object sender, EventArgs e)
		{
			rtbPoem.Text = _generator.GeneratePoem();
		}

		private void ChooseOntology_Click(object sender, EventArgs e)
		{
			var openFileDialog = new OpenFileDialog
			{
				Filter = "Ontology files (*.ont)|*.ont",
				Title = "Open ontology file"
			};
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				var ontology = OntologyLoader.LoadByPath(openFileDialog.FileName);
				_generator = new Generator(ontology);
				btGenerate.Enabled = true;
			}
		}
	}
}
