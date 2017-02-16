namespace Domino.Lib.Brains
{
    public interface IBrain
    {
        Board Board { get; }
        void Parse();
    }
}