using System;
using System.Management.Automation.Host;
using ConDep.Dsl.Logging;

namespace ConDep.Dsl.Remote
{
    internal class ConDepPsHostRawUI : PSHostRawUserInterface
    {
        public override KeyInfo ReadKey(ReadKeyOptions options)
        {
            throw new NotImplementedException();
        }

        public override void FlushInputBuffer()
        {
            throw new NotImplementedException();
        }

        public override void SetBufferContents(Coordinates origin, BufferCell[,] contents)
        {
            throw new NotImplementedException();
        }

        public override void SetBufferContents(Rectangle rectangle, BufferCell fill)
        {
            throw new NotImplementedException();
        }

        public override BufferCell[,] GetBufferContents(Rectangle rectangle)
        {
            throw new NotImplementedException();
        }

        public override void ScrollBufferContents(Rectangle source, Coordinates destination, Rectangle clip, BufferCell fill)
        {
            throw new NotImplementedException();
        }

        public override ConsoleColor ForegroundColor { get { return Logger.ForegroundColor; } set { Logger.ForegroundColor = value; } }
        public override ConsoleColor BackgroundColor { get { return Logger.BackgroundColor; } set { Logger.BackgroundColor = value; } }
        public override Coordinates CursorPosition { get; set; }
        public override Coordinates WindowPosition { get; set; }
        public override int CursorSize { get; set; }
        public override Size BufferSize { get {return new Size(9999, 9999);} set {} }
        public override Size WindowSize { get; set; }

        public override Size MaxWindowSize
        {
            get { return new Size(5000, 5000); }
        }

        public override Size MaxPhysicalWindowSize
        {
            get { return new Size(5000, 5000); }
        }

        public override bool KeyAvailable
        {
            get { return false; }
        }

        public override string WindowTitle { get; set; }
    }
}