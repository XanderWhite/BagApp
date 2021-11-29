using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BagApp
{
    public partial class Form1 : Form
    {
        private const int ITEM_COUNT = 20;
        private static int maxWeight = 60;
        private static int maxCost = 0;
        private static Item[] items;
        private static HashSet<int> optimumList;
        
        public Form1()
        {
            InitializeComponent();
            InitTable();
            textBox1.Text = maxWeight.ToString();
            btnOptimum.Enabled = false;

            btnOptimum.Click += BtnOptimum_Click;
            btnRandom.Click += BtnRandom_Click;
            textBox1.TextChanged += TextBox1_TextChanged;
        }

        private void InitTable()
        {
            dataGridView1.RowCount = 3;
            dataGridView1.Columns[0].Width = 38;
            dataGridView1.Rows[0].Cells[0].Value = "Вес";
            dataGridView1.Rows[0].Cells[0].ReadOnly = true;
            dataGridView1.Rows[1].Cells[0].Value = "Цена";
            dataGridView1.Rows[1].Cells[0].ReadOnly = true;


            for (int i = 0; i < ITEM_COUNT; i++)
            {
                dataGridView1.Columns.Add(i.ToString(), (i + 1).ToString());
                dataGridView1.Columns[i.ToString()].Width = (dataGridView1.Width - 42) / ITEM_COUNT;
            }
        }

        private Item[] CreateRandomItems()
        {
            var arr = new Item[ITEM_COUNT];

            Random rand = new Random();

            for (int i = 0; i < ITEM_COUNT; i++)
            {
                arr[i] = new Item(rand.Next(1, 20), rand.Next(1, 10));
            }

            return arr;
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            var tb = sender as TextBox;

            if (tb == null) return;

            btnOptimum.Enabled = int.TryParse(tb.Text, out maxWeight);
        }

        private void BtnRandom_Click(object sender, EventArgs e)
        {
            items = CreateRandomItems();
            FillTable(items);

            btnOptimum.Enabled = true;
        }

        private void FillTable(Item[] items)
        {
            for (int i = 0; i < items.Length; i++)
            {
                dataGridView1[(i).ToString(), 0].Value = items[i].Weight;
                dataGridView1[(i).ToString(), 1].Value = items[i].Cost;
                dataGridView1[(i).ToString(), 2].Value = string.Empty;
            }
        }

        private void BtnOptimum_Click(object sender, EventArgs e)
        {
            optimumList = new HashSet<int>();
            maxCost = 0;

            CalculateOptimum();

            AddToTable(optimumList);
        }

        private void AddToTable(HashSet<int> optimumList)
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (optimumList.Contains(i))
                    dataGridView1[(i).ToString(), 2].Value = "V";
                else
                    dataGridView1[(i).ToString(), 2].Value = string.Empty;
            }
        }

        static void CalculateOptimum(int i = 0, int currentCost = 0, int currentWeight = 0, List<int> s = null)
        {
            for (int j = i; j < ITEM_COUNT; j++)
            {
                var list = new List<int>();

                if (s != null)
                    list.AddRange(s);

                if (items[j].Weight + currentWeight <= maxWeight)
                {
                    list.Add(j);

                    if (currentCost + items[j].Cost > maxCost)
                    {
                        maxCost = currentCost + items[j].Cost;
                        optimumList.Clear();
                        list.ForEach(item => optimumList.Add(item));
                    }

                    CalculateOptimum(j + 1, currentCost + items[j].Cost, currentWeight + items[j].Weight, list);

                    list.Remove(j);
                }
            }
        }

    }
}
