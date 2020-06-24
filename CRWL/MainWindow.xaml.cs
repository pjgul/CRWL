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
using System.Data.SqlClient;
using System.IO;

namespace CRWL
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        //the full name of room
        string nameOfRoom = "text";
        //the numbers of corridors in the room
        int roomNum = 1;
        //tracks the amount of gold
        int gold;
        //checks whether the room has been visited or not
        bool visitedFlag = false;
        


        //string lore = "";

        public MainWindow()
        {
            InitializeComponent();

            //using (DungeonContext context = new DungeonContext())
            //{
            // context.Rooms.Add(new Room() { RoomName = "1NE" });
            //context.Rooms.Add(new Room() { RoomName = "2NE" });
            // context.Rooms.Add(new Room() { RoomName = "3NE" });
            // context.SaveChanges();
            //}


            //var path = Path.Combine(Directory.GetCurrentDirectory(), "\\images\\lorem.txt");
            //lore = System.IO.File.ReadAllText(path);
            //for(int i = 0; i < lore.Length; i++)
            //{
            //    textCRWL.Text = textCRWL.Text + lore[i];
            //}

            //using (DungeonContext context = new DungeonContext())
            //{

            //    context.Inventories.Remove(b);
            //}

        }



        private void ForwardB_Click(object sender, RoutedEventArgs e)
        {
            //generates a random index for the room
            Random rand = new Random();
            int nrand = rand.Next(1, 4);

            //the query ruturns room name correspondin to the random index
            using (var db = new DungeonContext())
            {
                var query = db.Rooms.Where(b => b.RoomId == nrand).Select(b => b.RoomName);
                foreach (string name in query)
                {
                    nameOfRoom = name;
                }
            }

            //the number of corridors in the room is passed into the variable
            roomNum = Int32.Parse(nameOfRoom.Substring(0, 1));

            //the screen of the room is set to the corresponding random room
            roomScreen.Source = new BitmapImage(new Uri($"/images/{nameOfRoom}.png", UriKind.RelativeOrAbsolute));

            //unneaded buttons get turned off
            switch (roomNum)
            {
                case 1:
                    {
                        ForwardB.Visibility = Visibility.Visible;
                        LeftB.Visibility = Visibility.Hidden;
                        RightB.Visibility = Visibility.Hidden;
                        break;
                    }
                case 2:
                    {
                        ForwardB.Visibility = Visibility.Hidden;
                        LeftB.Visibility = Visibility.Visible;
                        RightB.Visibility = Visibility.Visible;
                        break;
                    }
                case 3:
                    {
                        ForwardB.Visibility = Visibility.Visible;
                        LeftB.Visibility = Visibility.Visible;
                        RightB.Visibility = Visibility.Visible;
                        break;
                    }
            }
            //resets the flag for visiting a room
            visitedFlag = false;
        }

        private void Explore_Click(object sender, RoutedEventArgs e)
        {
            ForwardB_Click(sender, e);
        }

        private void LeftB_Click(object sender, RoutedEventArgs e)
        {
            ForwardB_Click(sender, e);
        }

        private void RightB_Click(object sender, RoutedEventArgs e)
        {
            ForwardB_Click(sender, e);
        }

        //Searches the room for treasure
        private void Search_Click(object sender, RoutedEventArgs e)
        {
            //room can only be searched once, afterwards the visited flag is switched on
            if (visitedFlag == false)
            {
                int grand = (int)(new Random().Next(1, 101));
                gold += grand;
                Gold.Content = "Gold: " + gold;

                using (DungeonContext context = new DungeonContext())
                {
                    context.Inventories.Add(new Inventory() { InventoryName = "Toothpick of destruction" });
                    context.SaveChanges();

                    var query = context.Inventories.Select(b => b.InventoryName).ToList();

                    inventoryList.ItemsSource = query;
                }

                visitedFlag = true;
            }
        }
    }
}
