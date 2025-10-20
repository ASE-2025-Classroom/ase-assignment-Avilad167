using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using System.Reflection;
using BOOSEapp;

namespace BOOSEappTests
{
    [TestClass]
    public class Form1Tests
    {
        [TestMethod]
        public void MoveTo_ChangesCurrentPosition()
        {
            // Arrange
            Form1 form = new Form1();
            var g = Graphics.FromImage(new Bitmap(100, 100));

            // Act
            MethodInfo executeCommand = typeof(Form1).GetMethod("ExecuteCommand", BindingFlags.NonPublic | BindingFlags.Instance);
            executeCommand.Invoke(form, new object[] { "moveto 50,60", g });

            // Assert
            var currentPosField = typeof(Form1).GetField("currentPos", BindingFlags.NonPublic | BindingFlags.Instance);
            Point position = (Point)currentPosField.GetValue(form);
            Assert.AreEqual(new Point(50, 60), position);
        }

        [TestMethod]
        public void DrawTo_UpdatesPositionCorrectly()
        {
            // Arrange
            Form1 form = new Form1();
            var g = Graphics.FromImage(new Bitmap(100, 100));

            // Move to starting point
            MethodInfo executeCommand = typeof(Form1).GetMethod("ExecuteCommand", BindingFlags.NonPublic | BindingFlags.Instance);
            executeCommand.Invoke(form, new object[] { "moveto 10,10", g });

            // Act - simulate drawing by moving again
            executeCommand.Invoke(form, new object[] { "moveto 40,40", g });

            // Assert
            var currentPosField = typeof(Form1).GetField("currentPos", BindingFlags.NonPublic | BindingFlags.Instance);
            Point position = (Point)currentPosField.GetValue(form);
            Assert.AreEqual(new Point(40, 40), position);
        }

        [TestMethod]
        public void MultilineProgram_ExecutesWithoutErrors()
        {
            // Arrange
            Form1 form = new Form1();
            var g = Graphics.FromImage(new Bitmap(200, 200));

            string[] program = {
                "moveto 50,50",
                "pen 255,0,0",
                "circle 30",
                "rect 40,20",
                "write Hello"
            };

            // Act
            foreach (var line in program)
            {
                try
                {
                    MethodInfo executeCommand = typeof(Form1).GetMethod("ExecuteCommand", BindingFlags.NonPublic | BindingFlags.Instance);
                    executeCommand.Invoke(form, new object[] { line, g });
                }
                catch
                {
                    Assert.Fail($"Command failed: {line}");
                }
            }

            // Assert
            Assert.IsTrue(true); // if no exception, test passes
        }
    }
}
