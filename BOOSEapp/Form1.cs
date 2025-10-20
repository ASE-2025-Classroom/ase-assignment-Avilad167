using BOOSE;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace BOOSEapp
{
    public partial class Form1 : Form
    {
        private Point currentPos = new Point(0, 0);
        private Pen currentPen = new Pen(Color.Black, 2);
        private Bitmap canvasBitmap = null!;

        public Form1()
        {
            InitializeComponent();

            // Set up the drawing surface
            canvasBitmap = new Bitmap(pictureBoxCanvas.Width, pictureBoxCanvas.Height);
            pictureBoxCanvas.Image = canvasBitmap;

            // Display library info in the output window
            string aboutText = AboutBOOSE.about();
            textBoxOutput.AppendText($"About BOOSE:\r\n{aboutText}\r\n");
            Debug.WriteLine($"About BOOSE:\r\n{aboutText}");
        }

        private void buttonRun_Click(object sender, EventArgs e)
        {
            // Clear and reset canvas
            using (Graphics g = Graphics.FromImage(canvasBitmap))
                g.Clear(Color.White);

            pictureBoxCanvas.Invalidate();
            currentPos = new Point(0, 0);
            currentPen = new Pen(Color.Black, 2);
            textBoxOutput.Clear();
            textBoxOutput.AppendText("Running BOOSE program...\r\n");

            // Execute commands line by line
            string[] lines = textBoxProgram.Text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            using (Graphics g = Graphics.FromImage(canvasBitmap))
            {
                foreach (var line in lines)
                {
                    try
                    {
                        ExecuteCommand(line.Trim(), g);
                        Debug.WriteLine("Executed: " + line.Trim());
                    }
                    catch (Exception ex)
                    {
                        string error = $"Error executing '{line}': {ex.Message}";
                        textBoxOutput.AppendText(error + "\r\n");
                        Debug.WriteLine(error);
                    }
                }
            }

            pictureBoxCanvas.Invalidate();
            textBoxOutput.AppendText("Program finished.\r\n");
        }

        private void ExecuteCommand(string line, Graphics g)
        {
            if (string.IsNullOrWhiteSpace(line) || line.StartsWith("*")) return;

            string[] parts = line.Split(' ', 2);
            if (parts.Length < 2) return;

            string cmd = parts[0].ToLower();
            string args = parts[1];

            switch (cmd)
            {
                case "moveto":
                    var coords = args.Split(',');
                    if (coords.Length == 2 &&
                        int.TryParse(coords[0], out int x) &&
                        int.TryParse(coords[1], out int y))
                    {
                        currentPos = new Point(x, y);
                    }
                    else throw new ArgumentException("Invalid moveto arguments");
                    break;

                case "pen":
                    var colors = args.Split(',');
                    if (colors.Length == 3 &&
                        int.TryParse(colors[0], out int r) &&
                        int.TryParse(colors[1], out int gCol) &&
                        int.TryParse(colors[2], out int b))
                    {
                        currentPen.Color = Color.FromArgb(r, gCol, b);
                    }
                    else throw new ArgumentException("Invalid pen color");
                    break;

                case "circle":
                    if (int.TryParse(args, out int radius))
                    {
                        var rect = new Rectangle(currentPos.X - radius, currentPos.Y - radius, radius * 2, radius * 2);
                        g.DrawEllipse(currentPen, rect);
                    }
                    else throw new ArgumentException("Invalid circle radius");
                    break;

                case "rect":
                    var size = args.Split(',');
                    if (size.Length == 2 &&
                        int.TryParse(size[0], out int width) &&
                        int.TryParse(size[1], out int height))
                    {
                        g.DrawRectangle(currentPen, new Rectangle(currentPos.X, currentPos.Y, width, height));
                    }
                    else throw new ArgumentException("Invalid rect arguments");
                    break;

                case "write":
                    using (Font font = new Font("Arial", 14))
                    using (Brush brush = new SolidBrush(currentPen.Color))
                        g.DrawString(args, font, brush, currentPos);
                    break;

                default:
                    throw new ArgumentException($"Unknown command '{cmd}'");
            }
        }
    }
}
