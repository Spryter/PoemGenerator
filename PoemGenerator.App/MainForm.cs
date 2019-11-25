using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PoemGenerator.GeneratorComponent;
using PoemGenerator.OntolisAdapter;
using PoemGenerator.OntologyModel;
using PoemGenerator.OntologyModel.Abstractions;

namespace PoemGenerator.App
{
	public partial class MainForm : Form
	{
		private readonly Ontolis _ontolis;
		
		private Generator _generator;

		private ToolStripMenuItem _editOntologyMenuItem;

		private Button _generateButton;
		
		private RichTextBox _poemTextBox;

		private SafeSituationGroupBox _safeSituationGroupBox;
		
		public MainForm()
		{
			InitializeComponent();
            InitializeMenu();
            InitializeForm();
            _ontolis = new Ontolis();
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
		        Dock = DockStyle.Fill
	        };

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

        private void InitializeForm()
        {
	        var table = new TableLayoutPanel
	        {
				Dock = DockStyle.Fill
	        };
	        table.RowStyles.Add(new RowStyle(SizeType.Absolute, 25));
	        table.RowStyles.Add(new RowStyle(SizeType.Percent, 20));
	        table.RowStyles.Add(new RowStyle(SizeType.Percent, 60));
	        table.RowStyles.Add(new RowStyle(SizeType.Percent, 20));
	        table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

	        table.Controls.Add(CreateSituationGroupBox(), 0, 1);
	        table.Controls.Add(CreatePoemTextBox(), 0, 2);
	        table.Controls.Add(CreateGenerateButton(), 0, 3);

	        Controls.Add(table);
        }

		private void ButtonGenerateClick(object sender, EventArgs e)
		{
			var child = _generator.GenerateChild();
			
			var builder = new StringBuilder();
			builder.Append($"{child} {_safeSituationGroupBox.ActionSelectedItem} {_safeSituationGroupBox.ObjectSelectedItem} {_safeSituationGroupBox.LocativeSelectedItem}");
			builder.Append(Environment.NewLine);
			builder.Append($"{child} {_generator.GenerateDangerSituation()}");
			
			_poemTextBox.Text = builder.ToString();
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
                    _generator = new Generator(ontology);
                    _generateButton.Enabled = true;
                    _editOntologyMenuItem.Enabled = true;
                    _safeSituationGroupBox.Generator = _generator;
                }
            }
        }
		
		private void EditOntologyClick(object sender, EventArgs e)
		{
			Visible = false;
			_ontolis.Open();
			Visible = true;
			var ontology = _ontolis.Reload();
			_generator = new Generator(ontology);
			_safeSituationGroupBox.Generator = _generator;
		}
	}
}
