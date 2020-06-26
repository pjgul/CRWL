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
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Xaml;
using System.ComponentModel;

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
        //the item currently selected by the player
        string item = "";


        //room naming convention: XYZ. Example: 1NE
        //X: number of corridors (after 3, evens: two corridors, odds: one or three corridors)
        //Y: type of room. N ormal (nothing in it), E nemy, M erchant
        //Z: type of type. There are different types of shops and enemies.
        //Example 1: 1NE (one corridoe, normal room, empty)
        //Example 2: 1ES (one corridor, enemy, type: sludge)
        


        //string lore = "";

        public MainWindow()
        {
            InitializeComponent();

            using (DungeonContext context = new DungeonContext())
            {
                context.Inventories.RemoveRange(context.Inventories);
                context.SaveChanges();
            }

            textCRWL.Text = "You venture into the dark depths of these decrepit catacombs. You search for treasure, but you may find more than you might have bargained for. For even though these Catacombs are Rampant With Loot, they also hold death.";
            
            

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
                //generates a random amount of gold for the player to find in the room
                int grand = (int)(new Random().Next(1, 101));
                gold += grand;
                Gold.Content = "Gold: " + gold;
                textCRWL.Text = $"Searching around the room you find {grand} gold coins.";

                using (DungeonContext context = new DungeonContext())
                {
                    context.Inventories.Add(new Inventory() { InventoryName = "Health Potion" });
                    context.SaveChanges();

                    var query = context.Inventories.Select(b => b.InventoryName).ToList();

                    inventoryList.ItemsSource = query;
                }

                //once the room has been searched, the flag is set to true so they can't search the same room multiple times
                visitedFlag = true;
            }
            else
                textCRWL.Text = "You have already searched this room.";
        }

        //attacks enemies
        private void Attack_Click(object sender, RoutedEventArgs e)
        {
            healthBar.Value = 50;
        }

        private void Use_Click(object sender, RoutedEventArgs e)
        {
            switch (item)
            {
                case "Health Potion":
                    {
                        if (healthBar.Value < 100)
                        {
                            healthBar.Value = healthBar.Value + 10;
                            IEditableCollectionView items = inventoryList.Items; 
                            if (items.CanRemove)
                            {
                                items.Remove(inventoryList.SelectedItem);
                            }
                        }
                        break;
                    }
            }
        }


        private void inventoryList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (inventoryList.SelectedItem != null)
                item = inventoryList.SelectedItem as string;
        }

        
    }
}
