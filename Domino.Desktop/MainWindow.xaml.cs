using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Domino.Lib;
using Domino.Lib.Brains;

namespace Domino.Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Brush> _bgColors = new List<Brush>
        {
            Brushes.LightSalmon, Brushes.LightSkyBlue, Brushes.LightSlateGray,
            Brushes.LightPink, Brushes.LightGreen, Brushes.Gold, Brushes.CadetBlue, Brushes.LightGray
        };

        private IBrain _brain;
        public MainWindow()
        {
            InitializeComponent();
        }

        protected List<List<int>> _input;

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var fn = _fileName.Text;
            _input = DominoReader.Read(fn);
            _grid.Children.Clear();
            _grid.ColumnDefinitions.Clear();
            _grid.RowDefinitions.Clear();

            if (!_input.Any()) return;

            DisplayInput();
            var board = await ParseBoard();
            DisplayBoard(board);
        }

        private async Task<Board> ParseBoard()
        {
            if (_brain == null) _brain = new CompositeBrain(_input);

            _brain.Parse();
            var board = _brain.Board;
            return board;
        }

        private void DisplayInput()
        {
            for (var x = 0; x < _input[0].Count; x++)
                _grid.ColumnDefinitions.Add(new ColumnDefinition());

            for (var y = 0; y < _input.Count; y++)
                _grid.RowDefinitions.Add(new RowDefinition());

            for (var y = 0; y < _input.Count; y++)
                for (var x = 0; x < _input[y].Count; x++)
                {
                    var lb = new Label
                    {
                        Content = _input[y][x].ToString(),
                        FontSize = 16,
                        Foreground = Brushes.SlateGray,
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center
                    };
                    Grid.SetRow(lb, y);
                    Grid.SetColumn(lb, x);
                    _grid.Children.Add(lb);
                }
        }

        private void DisplayBoard(Lib.Board board)
        {
            for (var ndx = 0; ndx < _grid.Children.Count; ndx++)
            {
                var x = ndx%board.Width;
                var y = ndx/board.Width;
                var c = board.CellAt(x, y);
                if (!c.IsOccupied) continue;

                var lb = _grid.Children[ndx] as Label;
                var color = _bgColors[c.Domino.Id % _bgColors.Count];
                lb.Content = c.Pips.ToString();
                lb.Foreground = Brushes.Black;
                lb.Background = color;
                lb.FontWeight = FontWeights.ExtraBold;
            }

            _coverLabel.Content = String.Format("Covered: {0}", board.CoveredCells);

        }

       private void _preferHorizontal_Selected(object sender, RoutedEventArgs e)
        {
            _brain = new SequentialHorizontalBrain(_input);
        }

        private void _preferVertical_Selected(object sender, RoutedEventArgs e)
        {
            _brain = new SequentialVerticalBrain(_input);
        }

        private void _spiral_Selected(object sender, RoutedEventArgs e)
        {
            _brain = new SpiralBrain(_input);
        }

        private void _composite_Selected(object sender, RoutedEventArgs e)
        {
            _brain = new CompositeBrain(_input);
        }
        private void _improvedHorizontal_Selected(object sender, RoutedEventArgs e)
        {
            _brain = new ImprovedHorizontalBrain(_input);
        }

        private void _improvedVertical_Selected(object sender, RoutedEventArgs e)
        {
            _brain = new ImprovedVerticalBrain(_input);
        }

        private void _fileName_TextChanged(object sender, TextChangedEventArgs e)
        {
            _brain = null;
            _grid.Children.Clear();
            _grid.ColumnDefinitions.Clear();
            _grid.RowDefinitions.Clear();
        }
    }
}
