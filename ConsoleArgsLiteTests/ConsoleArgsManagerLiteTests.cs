using ConsoleArgsLite;
using System;
using Xunit;

namespace ConsoleArgsLiteTests
{
    public class ConsoleArgsManagerLiteTests
    {
        #region Simple Tests
        [Fact]
        public void ParsesSimpleArgument()
        {
            string[] args = new string[] { "output.mp4" };
            ConsoleArgsManagerLite manager = new ConsoleArgsManagerLite();
            manager.AddArgument("output");

            manager.ParseArguments(args);

            Assert.Equal("output.mp4", manager.GetValueFromArgument<string>("output"));
        }

        [Fact]
        public void ParsesSimpleOptionalArgument()
        {
            string[] args = new string[] { "output.mp4" };
            ConsoleArgsManagerLite manager = new ConsoleArgsManagerLite();
            manager.AddArgument("output", false);

            manager.ParseArguments(args);

            Assert.Equal("output.mp4", manager.GetValueFromArgument<string>("output"));
        }
        [Fact]
        public void ParsesSimpleMissingOptionalArgument()
        {
            string[] args = new string[0];
            ConsoleArgsManagerLite manager = new ConsoleArgsManagerLite();
            manager.AddArgument("output", false);

            manager.ParseArguments(args);
            Assert.True(true);
        }

        [Fact]
        public void ReturnsNullWhenGettingAnUnparsedArgument()
        {
            string[] args = new string[0];
            ConsoleArgsManagerLite manager = new ConsoleArgsManagerLite();
            manager.AddArgument("output", false);

            manager.ParseArguments(args);

            string argument = manager.GetValueFromArgument<string>("output");

            Assert.Null(argument);
        }
        #endregion
        
    }
}
