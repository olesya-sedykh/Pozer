﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Pozer
{

    public partial class Main : Form
    {
        Graphics graphics;
        private int NodeSize = 30;
        private int paintingZeroX = 0;
        private int paintingZeroY = 35;

        private Node Root;
        private int TreeHeight = 1;
   

        public Main()
        {
            InitializeComponent();
        }

        private void CreateNode(Node Parent = null)
        {
            int XCoordinate = this.paintingZeroX + this.Width / 2 - NodeSize;
            int YCoordinate = this.paintingZeroY;

            if (Parent == null)
            {
                this.Root = new Node(1);
                this.Root.SetPosition(XCoordinate, YCoordinate);
            } else
            {
                Parent.AddChild(
                    new Node(Parent.GetLevel() + 1, Parent)
                );

                bool IsNewRow = false;
                if (Parent.GetLevel() + 1 > this.TreeHeight)
                {
                    this.TreeHeight++;
                    IsNewRow = true;

                }

                int NumberOfChildren = Parent.CountChildren();
                List<Node> ParentChildren = Parent.GetChildren();

                int RestWidth = this.Width - NodeSize * NumberOfChildren;
                int SpaceBetweenHorizontal = RestWidth / (NumberOfChildren + 1);


                // Ставим X позицию для текущего ряда
                for (int i = 0; i < NumberOfChildren; i++)
                {
                    ParentChildren[i].SetX(SpaceBetweenHorizontal * (i + 1) + NodeSize * i);
                }
                
                // Ставим Y позицию для всех элементов кроме корня, если это новый ряд
                if (IsNewRow)
                {
                    int RestHeight = this.Height - this.paintingZeroY - NodeSize;
                    int SpaceBetweenVertical = RestHeight / this.Height;
                    for (int i = 0; i < this.Root.CountChildren(); i++)
                    {
                        Node node = Root.GetChildren()[i];
                        for (int j = 0; j < node.CountChildren(); j++)
                        {
                            node.SetY(SpaceBetweenVertical * (i + 1) + NodeSize * i);
                        }
                    }
                }
            }
        }

        private void DrawGraph()
        {
            graphics = CreateGraphics();
            foreach (Node node in this.Root.GetChildren())
            {
                foreach (Node child in node.GetChildren())
                {
                    graphics.DrawEllipse(
                        Pens.Black,
                        child.GetX(),
                        child.GetY(),
                        NodeSize, NodeSize
                    );
                    graphics.FillEllipse(
                        Brushes.Red,
                        child.GetX(),
                        child.GetY(),
                        NodeSize, NodeSize
                    );
                }
            }
        }

        // Обработка клика на кнопку "Справка"
        private void help_Click(object sender, EventArgs e)
        {

        }

        // Обработка клика на кнопку "Режим работы"
        private void wayOfWorking_Click(object sender, EventArgs e)
        {
            Work work = new Work();
            work.ShowDialog();
        }

        // Обработка хз чего в главном окне
        private void Main_Load(object sender, EventArgs e)
        {
            // ...
        }

        // Обработка клика на кнопку "Очистить поле"
        private void delete_Click(object sender, EventArgs e)
        {

        }

        // Обработка клика на кнопку "Начать решение"
        private void start_Click(object sender, EventArgs e)
        {
            
        }

        private void Main_Paint(object sender, PaintEventArgs e)
        {
            paintingZeroX = this.paintingZeroX + this.Width / 2 - NodeSize;
            this.CreateNode();

            //this.CreateNode(this.Root);
            //this.CreateNode(this.Root);
            //this.CreateNode(this.Root);

            //this.CreateNode(this.Root.GetChildren()[0]);

            this.DrawGraph();
        }
    }


    public class Node
    {
        private Node Parent;
        private int Level;  // определяем лейблы по правилу ((level + 1) % 2 == 0) ? "A" : "B"
        int[] Costs = new int[2];   // выигрыши А и В соответственно
        private List<Node> Children = new List<Node>();
        private int XCoordinate, YCoordinate;


        // Null Object Constructor
        public Node()
        {
            this.Parent = null;
            this.Children = null;
        }

        public Node(int level, Node parent = null)
        {
            this.Parent = parent;
            this.Level = level;
        }

        public int GetLevel()
        {
            return this.Level;
        }

        public int GetX()
        {
            return this.XCoordinate;
        }

        public int GetY()
        {
            return this.YCoordinate;
        }

        public void SetX(int x)
        {
            this.XCoordinate = x;
        }

        public void SetY(int y)
        {
            this.YCoordinate = y;
        }

        public int[] GetPosition()
        {
            return new int[] { this.XCoordinate, this.YCoordinate };
        }

        public void SetPosition(int XCoordinate, int YCoordinate)
        {
            this.XCoordinate = XCoordinate;
            this.YCoordinate = YCoordinate;
        }

        public void AddChild(Node child)
        {
            this.Children.Add(child);
        }

        public void DeleteChildren()
        {
            this.Children.Clear();
        }

        public List<Node> GetChildren()
        {
            return this.Children;
        }

        public int CountChildren()
        {
            return this.Children.Count;
        }

        public Node FindChild(int XCoordinate, int YCoordinate, int epsilon = 10)
        {
            return new Node();
        }

        public int[] FindBestCosts()
        {
            int label = (this.Level + 1) % 2; // 0 -> A, 1 -> B
            List<Node> TempChildren = new List<Node>(this.Children);
            TempChildren.Sort(
                (x, y) => x.Costs[label].CompareTo(y.Costs[label])
            );
            return TempChildren[TempChildren.Count - 1].Costs;
        }
    }
}
