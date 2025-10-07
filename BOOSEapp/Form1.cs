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
        private Bitmap canvasBitmap;

        public Form1()
        {
            InitializeComponent();

            // Create bitmap canvas and assign to picture box
            canvasBitmap = new Bitmap(pictureBoxCanvas.Width, pictureBoxCanvas.Height);
            pictureBoxCanvas.Image = canvasBitmap;

            // Call About method and display
            string aboutText = AboutBOOSE.about();
            textBoxOutput.AppendText("About BOOSE:\r\n" + aboutText + "\r\n");
            Debug.WriteLine("About BOOSE:");
            Debug.WriteLine(aboutText);
        }

        private void buttonRun_Click(object sender, EventArgs e)
        {
            // Clear the canvas
            using (Graphics g = Graphics.FromImage(canvasBitmap))
            {
                g.Clear(Color.White);
            }
            pictureBoxCanvas.Invalidate();

            // Reset position and pen
            currentPos = new Point(0, 0);
            currentPen = new Pen(Color.Black, 2);

            // Clear output
            textBoxOutput.Clear();
            textBoxOutput.AppendText("Running BOOSE program...\r\n");

            // Run program line by line
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
            if (string.IsNullOrWhiteSpace(line)) return;
            if (line.StartsWith("*")) return; 

            string[] parts = line.Split(' ', 2);
            if (parts.Length < 2) return;

            string cmd = parts[0].ToLower();
            string args = parts[1];

            switch (cmd)
            {
                case "moveto":
                    {
                        var coords = args.Split(',');
                        if (coords.Length != 2) throw new ArgumentException("Invalid moveto arguments");
                        if (int.TryParse(coords[0], out int x) && int.TryParse(coords[1], out int y))
                        {
                            currentPos = new Point(x, y);
                        }
                        else
                        {
                            throw new ArgumentException("Invalid moveto coordinates");
                        }
                        break;
                    }
                case "pen":
                    {
                        var colors = args.Split(',');
                        if (colors.Length != 3) throw new ArgumentException("Invalid pen color arguments");
                        if (int.TryParse(colors[0], out int r) && int.TryParse(colors[1], out int gCol) && int.TryParse(colors[2], out int b))
                        {
                            currentPen.Color = Color.FromArgb(r, gCol, b);
                        }
                        else
                        {
                            throw new ArgumentException("Invalid pen color values");
                        }
                        break;
                    }
                case "circle":
                    {
                        if (int.TryParse(args, out int radius))
                        {
                            Rectangle rect = new Rectangle(currentPos.X - radius, currentPos.Y - radius, radius * 2, radius * 2);
                            g.DrawEllipse(currentPen, rect);
                        }
                        else
                        {
                            throw new ArgumentException("Invalid circle radius");
                        }
                        break;
                    }
                case "rect":
                    {
                        var sizes = args.Split(',');
                        if (sizes.Length != 2) throw new ArgumentException("Invalid rect arguments");
                        if (int.TryParse(sizes[0], out int width) && int.TryParse(sizes[1], out int height))
                        {
                            Rectangle rect = new Rectangle(currentPos.X, currentPos.Y, width, height);
                            g.DrawRectangle(currentPen, rect);
                        }
                        else
                        {
                            throw new ArgumentException("Invalid rect size");
                        }
                        break;
                    }
                case "write":
                    {
                        // write text on canvas at currentPos
                        string text = args;
                        using (Font font = new Font("Arial", 14))
                        using (Brush brush = new SolidBrush(currentPen.Color))
                        {
                            g.DrawString(text, font, brush, currentPos);
                        }
                        break;
                    }
                default:
                    // Unknown command
                    throw new ArgumentException($"Unknown command '{cmd}'");
            }
        }
    }
}
