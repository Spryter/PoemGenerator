using System;
using System.Windows.Forms;
using PoemGenerator.GeneratorComponent;
using PoemGenerator.OntolisAdapter;

namespace PoemGenerator.App
{
	public partial class MainForm : Form
	{
		private readonly Ontolis _ontolis;
		private Generator _generator;
		
		public MainForm()
		{
			InitializeComponent();
			_ontolis = new Ontolis();
		}

		private void btGenerate_Click(object sender, EventArgs e)
		{
			rtbPoem.Text = _generator.GeneratePoem();
		}

		private void ChooseOntology_Click(object sender, EventArgs e)
		{
			var openFileDialog = new OpenFileDialog
			{
				Filter = @"Ontology files (*.ont)|*.ont",
				Title = @"Open ontology file"
			};
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				var ontology = _ontolis.LoadByPath(openFileDialog.FileName);
				_generator = new Generator(ontology);
				btGenerate.Enabled = true;
				редактироватьToolStripMenuItem.Enabled = true;
			}
		}

		private void EditOntology_Click(object sender, EventArgs e)
		{
			Visible = false;
			_ontolis.Open();
			Visible = true;
			var ontology = _ontolis.Reload();
			_generator = new Generator(ontology);
		}
	}
}
