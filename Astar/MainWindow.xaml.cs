using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        public int attempts = 0;
        public int count = 0;
        public bool obs_on = false;
        public bool found = false;
        

        public Button[,] Buttons = new Button[4,4];
        public Button startNode, endNode,currentNode;

        List <Button> OpenList = new List <Button>();
        List <Button> ClosedList = new List <Button>();
        List <Button> Obstacles = new List <Button>();
        List <Button> DeadEnd = new List <Button>();
        List <Button> Path = new List <Button>();

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

        public void Restart(object sender, RoutedEventArgs e)
        {
            OpenList.Clear();
            Obstacles.Clear();
            ClosedList.Clear();
            DeadEnd.Clear();
            found = false;
            GenerateButtons();
        }
        public void Finding()
        {
            attempts++;
            gx++;
            count = 0;
            // Getting the cordinated of the current node
            currentNode = ClosedList[ClosedList.Count - 1];
            int row = FindRow(currentNode);
            int column = FindColumn(currentNode);

            //TODO improve the for loop
            if (Dead(currentNode) == 0 && currentNode != endNode)
            {
                DeadEnd.Add(currentNode);
            }
            Direct(row, column);

            if (OpenList.Contains(endNode))
            {
                Console.WriteLine("End node is one of the option");
            }


            if (currentNode != endNode && attempts == 100)
            {
                MessageBox.Show("Not Found");
                Console.WriteLine("Not Found");
                found = true;
            }
            else if (currentNode != endNode)
            {


                // Going to the direction
                if (OpenList.Count != 0)
                {
                    Button bt = FindMin();
                    //bt.Background = Brushes.Brown;
                    OpenList.Remove(bt);
                    ClosedList.Add(bt);
                }

            }





            if (currentNode != endNode && !found)
            {
                Finding();
            }
            else
            {
                Console.WriteLine(ClosedList.Count);
            }


        }



        private void Direct(int row, int column)
        {
            for (int i = 0; i < 4; i++)
            {
                try
                {

                    if (i == 0 && Buttons[row - 1, column] != startNode && !Obstacles.Contains(Buttons[row - 1, column]) && !ClosedList.Contains(Buttons[row - 1, column]) && !DeadEnd.Contains(Buttons[row - 1, column]))
                    {
                        Up(row, column);

                    }
                    else if (i == 1 && Buttons[row, column + 1] != startNode && !Obstacles.Contains(Buttons[row, column + 1]) && !ClosedList.Contains(Buttons[row, column + 1]) && !DeadEnd.Contains(Buttons[row, column + 1]))
                    {
                        Right(row, column);
                    }
                    else if (i == 2 && Buttons[row + 1, column] != startNode && !Obstacles.Contains(Buttons[row + 1, column]) && !ClosedList.Contains(Buttons[row + 1, column]) && !DeadEnd.Contains(Buttons[row + 1, column]))
                    {
                        Bottom(row, column);
                    }
                    else if (i == 3 && Buttons[row, column - 1] != startNode && !Obstacles.Contains(Buttons[row, column - 1]) && !ClosedList.Contains(Buttons[row, column - 1]) && !DeadEnd.Contains(Buttons[row, column - 1]))
                    {
                        Left1(row, column);
                    }




                }
                catch
                {
                    continue;
                }

            }
        }


        
        private void GetPath()
        {

            for (int j = ClosedList.Count -1 ; j >= 0; j--)
            {
                Console.WriteLine(ClosedList[j] + " " + j);
            }


            ClosedList.Remove(startNode);
            int i;
            int row;
            int column;
            int row2;
            int column2;
            int sum = 0, sum2 = 0;
            List <int> sums2 = new List<int>();
            row2 = FindRow(ClosedList[ClosedList.Count -1]);
            column2 = FindColumn(ClosedList[ClosedList.Count -1]);
            sums2.Add(row2 + column2);
            for (i = ClosedList.Count -2; i >= 0; i--)
            {
                row = FindRow(ClosedList[i]);
                column = FindColumn(ClosedList[i]);
                row2 = FindRow(ClosedList[i + 1]);
                column2 = FindColumn(ClosedList[i + 1]);

                sum2 = sums2[sums2.Count - 1];
                sum = row + column;
                if (sum2 == sum + 1 || sum2 + 1 == sum)
                {
                    Path.Add(ClosedList[i]);
                    sums2.Add(row + column);
                }
                
            }
            //Path.Add(ClosedList[i]);
            Path.Add(startNode);
        }

        private int Dead(Button bt)
        {
            int row = FindRow(bt);
            int column = FindColumn(bt);
            for (int i = 0; i < 4; i++)
            {
                try
                {

                    if (i == 0 && Buttons[row - 1, column] != startNode  && !Obstacles.Contains(Buttons[row - 1, column]) && !ClosedList.Contains(Buttons[row - 1, column]))
                    {
                        count++;

                    }
                    else if (i == 1 && Buttons[row, column + 1] != startNode  && !Obstacles.Contains(Buttons[row, column + 1]) && !ClosedList.Contains(Buttons[row, column + 1]))
                    {
                        count++;
                    }
                    else if (i == 2 && Buttons[row + 1, column] != startNode && !Obstacles.Contains(Buttons[row + 1, column]) && !ClosedList.Contains(Buttons[row + 1, column]))
                    {
                        count++;
                    }
                    else if (i == 3 && Buttons[row, column - 1] != startNode && !Obstacles.Contains(Buttons[row, column - 1]) && !ClosedList.Contains(Buttons[row, column - 1]))
                    {
                        count++;
                    }




                }
                catch
                {
                    continue;
                }
               

            }
            return count;
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
            Console.WriteLine("Left");
            if (Buttons[row, column - 1] != endNode)
            {
                Buttons[row, column - 1].Background = Brushes.LightYellow;
            }
                
            
        }

        private void Bottom(int row, int column)
        {
            
            OpenList.Add(Buttons[row + 1,column]);
            Console.WriteLine("Down");
            if (Buttons[row + 1, column] != endNode)
            {
                Buttons[row + 1, column].Background = Brushes.LightYellow;
            }

        }

        private void Right(int row, int column)
        {


            OpenList.Add(Buttons[row,column + 1]);
            Console.WriteLine("Right");
            if (Buttons[row, column + 1] != endNode)
            {
                Buttons[row, column + 1].Background = Brushes.LightYellow;
            }


        }
        private void Up(int row, int column)
        {

            OpenList.Add(Buttons[row - 1,column]);
            Console.WriteLine("Up");
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

            if (!found)
            {
                for (int i = 0; i < ClosedList.Count; i++)
                {
                    if (ClosedList[i] != endNode && ClosedList[i] != startNode)
                    {
                        ClosedList[i].Background = Brushes.Magenta;
                    }
                }
                for (int i = 0; i < DeadEnd.Count; i++)
                {
                    DeadEnd[i].Background = Brushes.Black;
                }

                GetPath();
                for (int i = 0; i < Path.Count; i++)
                {
                    if (Path[i] != endNode && Path[i] != startNode)
                    {
                        Path[i].Background = Brushes.Green;
                        Console.WriteLine(Path[i]);
                    }
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
