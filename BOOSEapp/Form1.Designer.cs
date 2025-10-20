namespace BOOSEapp
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TextBox textBoxProgram;
        private System.Windows.Forms.TextBox textBoxOutput;
        private System.Windows.Forms.Button buttonRun;
        private System.Windows.Forms.PictureBox pictureBoxCanvas;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            textBoxProgram = new System.Windows.Forms.TextBox();
            textBoxOutput = new System.Windows.Forms.TextBox();
            buttonRun = new System.Windows.Forms.Button();
            pictureBoxCanvas = new System.Windows.Forms.PictureBox();

            ((System.ComponentModel.ISupportInitialize)(pictureBoxCanvas)).BeginInit();
            SuspendLayout();

            // textBoxProgram
            textBoxProgram.Location = new System.Drawing.Point(12, 12);
            textBoxProgram.Multiline = true;
            textBoxProgram.Name = "textBoxProgram";
            textBoxProgram.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            textBoxProgram.Size = new System.Drawing.Size(350, 400);
            textBoxProgram.TabIndex = 0;
            textBoxProgram.Text =
                "moveto 100,100\r\npen 0,255,0\r\ncircle 50\r\npen 255,0,0\r\nmoveto 150,50\r\nrect 50,100";

            // textBoxOutput
            textBoxOutput.Location = new System.Drawing.Point(12, 420);
            textBoxOutput.Multiline = true;
            textBoxOutput.Name = "textBoxOutput";
            textBoxOutput.ReadOnly = true;
            textBoxOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            textBoxOutput.Size = new System.Drawing.Size(350, 100);
            textBoxOutput.TabIndex = 1;

            // buttonRun
            buttonRun.Location = new System.Drawing.Point(12, 530);
            buttonRun.Name = "buttonRun";
            buttonRun.Size = new System.Drawing.Size(350, 30);
            buttonRun.TabIndex = 2;
            buttonRun.Text = "Run BOOSE Program";
            buttonRun.UseVisualStyleBackColor = true;
            buttonRun.Click += new System.EventHandler(buttonRun_Click);

            // pictureBoxCanvas
            pictureBoxCanvas.BackColor = System.Drawing.Color.White;
            pictureBoxCanvas.Location = new System.Drawing.Point(370, 12);
            pictureBoxCanvas.Name = "pictureBoxCanvas";
            pictureBoxCanvas.Size = new System.Drawing.Size(600, 550);
            pictureBoxCanvas.TabIndex = 3;
            pictureBoxCanvas.TabStop = false;

            // Form1
            ClientSize = new System.Drawing.Size(984, 571);
            Controls.Add(pictureBoxCanvas);
            Controls.Add(buttonRun);
            Controls.Add(textBoxOutput);
            Controls.Add(textBoxProgram);
            Name = "Form1";
            Text = "BOOSE Interpreter";

            ((System.ComponentModel.ISupportInitialize)(pictureBoxCanvas)).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
