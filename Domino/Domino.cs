namespace Domino.Lib
{
    public class Domino
    {
        private static int _globalIndex = 0;

        public Domino(int pip1, int pip2)
        {
            Pips = new int[2];
            Pips[0] = pip1;
            Pips[1] = pip2;
            Id = _globalIndex;
            _globalIndex++;
        }

        public int Id { get; set; }
        public int[] Pips { get; set; }
    }
}