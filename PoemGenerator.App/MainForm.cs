using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PoemGenerator.App.Controls;
using PoemGenerator.GeneratorComponent;
using PoemGenerator.GeneratorComponent.Situations;
using PoemGenerator.OntolisAdapter;
using PoemGenerator.OntologyModel;

namespace PoemGenerator.App
{
	public partial class MainForm : Form
	{
		private readonly Ontolis _ontolis;
		
		private readonly OntologyViewModel _ontologyViewModel;
		
		private Generator _generator;

		private ToolStripMenuItem _editOntologyMenuItem;

		private Button _generateButton;
		
		private RichTextBox _poemTextBox;

		private SafeSituationGroupBox _safeSituationGroupBox;

		private OntologyViewer _ontologyViewer;

		private List<Situation> _situations;
		
		public MainForm()
		{
			InitializeComponent();
			_ontologyViewModel = new OntologyViewModel();
			_ontolis = new Ontolis();
			InitializeMenu();
            InitializeForm();
		}

        private void InitializeMenu()
        {
            var openOntologyMenuItem = new ToolStripMenuItem("Открыть");
            openOntologyMenuItem.Click += OpenOntologyClick;

            _editOntologyMenuItem = new ToolStripMenuItem("Редактировать")
            {
                Enabled = false
            };
            _editOntologyMenuItem.Click += EditOntologyClick;

            var ontologyMenuItem = new ToolStripMenuItem("Онтология");
            ontologyMenuItem.DropDownItems.Add(openOntologyMenuItem);
            ontologyMenuItem.DropDownItems.Add(_editOntologyMenuItem);

            var menu = new MenuStrip();
            menu.Items.Add(ontologyMenuItem);

            Controls.Add(menu);
        }

        private Button CreateGenerateButton()
        {
	        _generateButton = new Button
	        {
		        Text = @"Сгенерировать стихотворение",
		        Enabled = false,
		        Dock = DockStyle.Fill
	        };
	        _generateButton.Click += ButtonGenerateClick;

	        return _generateButton;
        }

        private RichTextBox CreatePoemTextBox()
        {
	        _poemTextBox = new RichTextBox
	        {
		        Dock = DockStyle.Fill,
		        Font = new Font("Microsoft Sans Serif", 15f)
	        };
	        _poemTextBox.Click += PoemTextBoxClick;

	        return _poemTextBox;
        }

        private SafeSituationGroupBox CreateSituationGroupBox()
        {
	        _safeSituationGroupBox = new SafeSituationGroupBox
	        {
		        Dock = DockStyle.Fill
	        };

	        return _safeSituationGroupBox;
        }

        private TableLayoutPanel CreateGenerator()
        {
	        var table = new TableLayoutPanel
	        {
		        Dock = DockStyle.Fill
	        };
	        table.RowStyles.Add(new RowStyle(SizeType.Percent, 40));
	        table.RowStyles.Add(new RowStyle(SizeType.Percent, 40));
	        table.RowStyles.Add(new RowStyle(SizeType.Percent, 20));
	        table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

	        table.Controls.Add(CreateSituationGroupBox(), 0, 0);
	        table.Controls.Add(CreatePoemTextBox(), 0, 1);
	        table.Controls.Add(CreateGenerateButton(), 0, 2);

	        return table;
        }

        private OntologyViewer CreateOntologyViewer()
        {
	        _ontologyViewer = new OntologyViewer(_ontologyViewModel)
	        {
		        Dock = DockStyle.Fill
	        };
	        return _ontologyViewer;
        }

        private void InitializeForm()
        {
	        var table = new TableLayoutPanel
	        {
		        Dock = DockStyle.Fill
	        };
	        table.RowStyles.Add(new RowStyle(SizeType.Absolute, 18));
	        table.RowStyles.Add(new RowStyle(SizeType.Percent, 30));
	        table.RowStyles.Add(new RowStyle(SizeType.Percent, 70));

	        table.Controls.Add(CreateGenerator(), 0, 1);
	        table.Controls.Add(CreateOntologyViewer(), 0, 2);
	        
	        Controls.Add(table);
        }

        private void PoemTextBoxClick(object sender, EventArgs e)
        {
	        if (_poemTextBox.Lines.Length <= 0) return;
	        var firstCharIndex = _poemTextBox.GetFirstCharIndexOfCurrentLine();
			var currentLineIndex = _poemTextBox.GetLineFromCharIndex(firstCharIndex);

			var currentLineText = _poemTextBox.Lines[currentLineIndex];
			_poemTextBox.Select(firstCharIndex, currentLineText.Length);

			if (currentLineIndex >= _situations.Count) return;
			var situation = _situations[currentLineIndex];
			_ontologyViewModel.UpdateGraphColoring(situation.GetNodes(), situation.GetRelations());
        }

		private void ButtonGenerateClick(object sender, EventArgs e)
		{
			var safeSituation = _safeSituationGroupBox.GetSituation();
			var dangerousSituation = _generator.GenerateDangerousSituation(safeSituation);
			_situations = new List<Situation> {safeSituation, dangerousSituation};
			
			var builder = new StringBuilder();
			builder.Append($"{safeSituation.Agent} {safeSituation.Action} {safeSituation.Object} {safeSituation.Locative}");
			builder.Append(Environment.NewLine);
			builder.Append($"{dangerousSituation.Agent} {dangerousSituation.Action} {dangerousSituation.Object} {dangerousSituation.Locative}");
			
			_poemTextBox.Text = builder.ToString();
		}

		private void UpdateControlsWithOntology(Ontology ontology)
		{
			_generator = new Generator(ontology);
			_safeSituationGroupBox.Generator = _generator;
			_ontologyViewer.Ontology = ontology;
		}

		private void OpenOntologyClick(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog
            {
                Filter = @"Файлы онтологий (*.ont)|*.ont",
                Title = @"Открыть файл онтологии"
            })
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var ontology = _ontolis.LoadByPath(openFileDialog.FileName);
                    _generateButton.Enabled = true;
                    _editOntologyMenuItem.Enabled = true;
                    UpdateControlsWithOntology(ontology);
                }
            }
        }
		
		private void EditOntologyClick(object sender, EventArgs e)
		{
			Visible = false;
			_ontolis.Open();
			Visible = true;
			var ontology = _ontolis.Reload();
			UpdateControlsWithOntology(ontology);
		}
	}
}
