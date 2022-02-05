using System;
using System.Collections.Generic;
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

namespace Astar
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int gx = 0;
        public bool obs_on = false;

        public Button[,] Buttons = new Button[4,4];
        public Button startNode, endNode,currentNode;

        List <Button> OpenList = new List <Button>();
        List <Button> ClosedList = new List <Button>();
        List <Button> Obstacles = new List <Button>();

        public MainWindow()
        {
            InitializeComponent();
            GenerateButtons();
        }

        public void GenerateButtons()
        {
            for (int i = 0; i < Buttons.GetLength(0); i++)
            {
                for (int j = 0; j < Buttons.GetLength(1); j++)
                {
                    Button bt = new Button();
                    bt.Content = "(" + i.ToString() + "," + j.ToString() + ")";
                    Buttons[i,j] = bt;
                    bt.MouseRightButtonUp += new MouseButtonEventHandler(Button_MouseRightButtonUp);
                    bt.Click += new RoutedEventHandler(Button_LeftClick);
                    mygrid.Children.Add(bt);
                    Grid.SetRow(bt, i);
                    Grid.SetColumn(bt, j);
                }
            }
        }


        public void Button_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            Button bt = (Button)sender;
            bt.Background = Brushes.Purple;
            endNode = bt;

        }

        public void Finding()
        {

            // Getting the cordinated of the current node
            currentNode = ClosedList[ClosedList.Count - 1];
            int row = FindRow(currentNode); 
            int column = FindColumn(currentNode);

            //TODO improve the for loop
            for (int i = 0; i < 4; i++)
            {
                try
                {

                    if (i == 0 && Buttons[row - 1,column] != startNode && !Obstacles.Contains(Buttons[row - 1, column]))
                    {
                        Up(row, column);

                    }
                    else if (i == 1 && Buttons[row,column + 1] != startNode && !Obstacles.Contains(Buttons[row, column + 1]))
                    {
                        Right(row, column);
                    }
                    else if (i == 2 && Buttons[row + 1,column] != startNode && !Obstacles.Contains(Buttons[row + 1, column]))
                    {
                        Bottom(row, column);
                    }
                    else if (i == 3 && Buttons[row,column - 1] != startNode && !Obstacles.Contains(Buttons[row, column - 1]))
                    {
                        Left1(row, column);
                    }




                }
                catch
                {
                    continue;
                }

            }

            if (OpenList.Contains(endNode))
            {
                Console.WriteLine("End node is one of the option");
            }


            // Going to the direction
            Button bt = FindMin();
            //bt.Background = Brushes.Brown;
            OpenList.Remove(bt);
            ClosedList.Add(bt);


            if (currentNode != endNode)
            {
                Finding();
            }


            Console.WriteLine(ClosedList.Count);


            
        }

        public Button FindMin()
        {
            int[] p = new int[OpenList.Count];
            for (int i = 0; i < p.Length; i++)
            {
                p[i] = 1000;
            }
            for (int i = 0; i < OpenList.Count; i++)
            {
                p[i] = Calc_Fn(OpenList[i]);
            }

            int min = int.MaxValue;
            int index = 0;
            for (int i = 1; i < p.Length; i++)
            {
                if (p[index] > p[i])
                {
                    index = i;
                }
            }

            return OpenList[index];


        }

        public int Calc_Fn(Button bt)
        { 
            int row = FindRow(bt);
            int column = FindColumn(bt);
            int row2 = FindRow(endNode);
            int column2 = FindColumn(endNode);

            int heuristic = Math.Abs(row - row2) + Math.Abs(column - column2);
            return (heuristic + gx);
        }


        private void Left1(int row, int column)
        {

                OpenList.Add(Buttons[row,column - 1]);
                if (Buttons[row, column - 1] != endNode)
                {
                    Buttons[row, column - 1].Background = Brushes.LightYellow;
                }
                
            
        }

        private void Bottom(int row, int column)
        {
            
                OpenList.Add(Buttons[row + 1,column]);
                if (Buttons[row + 1, column] != endNode)
                {
                    Buttons[row + 1, column].Background = Brushes.LightYellow;
                }

        }

        private void Right(int row, int column)
        {


                OpenList.Add(Buttons[row,column + 1]);
                if (Buttons[row, column + 1] != endNode)
                {
                    Buttons[row, column + 1].Background = Brushes.LightYellow;
                }


        }
        private void Up(int row, int column)
        {

                OpenList.Add(Buttons[row - 1,column]);
                if (Buttons[row - 1, column] != endNode)
                {
                    Buttons[row - 1, column].Background = Brushes.LightYellow;
                }

        }

        public int FindRow(Button bt) // Getting the row of a button
        {
            for (int i = 0; i < Buttons.GetLength(0); i++)
            {
                for (int j = 0; j < Buttons.GetLength(1); j++)
                {
                    if (Buttons[i,j] == bt)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }



        public int FindColumn(Button bt)
        {
            for (int i = 0; i < Buttons.GetLength(0); i++)
            {
                for (int j = 0; j < Buttons.GetLength(1); j++)
                {
                    if (Buttons[i,j] == bt)
                    {
                        return j;
                    }
                }
            }
            return -1;
        }
        private void Start_Click(object sender, RoutedEventArgs e)
        {
            ClosedList.Add(startNode);
            Finding();

            for (int i = 0; i < ClosedList.Count; i++)
            {
                if (ClosedList[i] != endNode && ClosedList[i] != startNode)
                {
                    ClosedList[i].Background = Brushes.Magenta;
                }
            }
        }

        public void Obs_Click(object sender, RoutedEventArgs e)
        {
            obs_on = !obs_on;
        }

        public void Button_LeftClick(object sender, RoutedEventArgs e)
        {
            Button bt = (Button)sender;
            if (!obs_on)
            {
                bt.Background = Brushes.SkyBlue;
                startNode = bt;
            }
            else
            {
                bt.Background= Brushes.Red;
                Obstacles.Add(bt);
            }
        }
    }
}
