namespace PoemGenerator.App
{
	partial class MainForm
	{
		/// <summary>
		/// Обязательная переменная конструктора.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Освободить все используемые ресурсы.
		/// </summary>
		/// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Код, автоматически созданный конструктором форм Windows

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.rtbPoem = new System.Windows.Forms.RichTextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.btGenerate = new System.Windows.Forms.Button();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.ChooseOntologyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// rtbPoem
			// 
			this.rtbPoem.Location = new System.Drawing.Point(12, 51);
			this.rtbPoem.Name = "rtbPoem";
			this.rtbPoem.Size = new System.Drawing.Size(363, 164);
			this.rtbPoem.TabIndex = 0;
			this.rtbPoem.Text = "";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 33);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(191, 15);
			this.label1.TabIndex = 1;
			this.label1.Text = "Сгенерированное стихотворение";
			// 
			// btGenerate
			// 
			this.btGenerate.Enabled = false;
			this.btGenerate.Location = new System.Drawing.Point(10, 222);
			this.btGenerate.Name = "btGenerate";
			this.btGenerate.Size = new System.Drawing.Size(364, 38);
			this.btGenerate.TabIndex = 2;
			this.btGenerate.Text = "Сгенерировать стихотворение";
			this.btGenerate.UseVisualStyleBackColor = true;
			this.btGenerate.Click += new System.EventHandler(this.btGenerate_Click);
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {this.ChooseOntologyMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(392, 24);
			this.menuStrip1.TabIndex = 3;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// ChooseOntologyMenuItem
			// 
			this.ChooseOntologyMenuItem.Name = "ChooseOntologyMenuItem";
			this.ChooseOntologyMenuItem.Size = new System.Drawing.Size(131, 20);
			this.ChooseOntologyMenuItem.Text = "Выбрать онтологию";
			this.ChooseOntologyMenuItem.Click += new System.EventHandler(this.ChooseOntology_Click);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(392, 287);
			this.Controls.Add(this.btGenerate);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.rtbPoem);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "MainForm";
			this.Text = "Генератор стихотворений";
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();
		}

		#endregion

		private System.Windows.Forms.RichTextBox rtbPoem;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btGenerate;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem ChooseOntologyMenuItem;
	}
}

