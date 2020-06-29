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
        //the type of the room
        string roomType = "";
        //the special type
        string specialType = "";

        //tracks the amount of gold
        int gold;
        //the item currently selected by the player
        string item = "";
        //the number of rooms visited
        int roomsVisited = 1;

        //checks whether the room has been visited or not
        bool visitedFlag = false;
        //checks if the current enemy is still alive
        bool isEnemyAlive = false;
        //the variable acts as a switch between exploring rooms and buying items
        bool exploreMode = true;


        //flags for buying items

        //has left item been bought
        bool leftBougth = false;
        //has middle item been bought
        bool middleBougth = false;
        //has right item been bought
        bool rightBougth = false;

        //the damage that the player can deal
        int attackStrength = 5;




        //room naming convention: XYZ. Example: 1NE
        //X: number of corridors (after 3, evens: two corridors, odds: one or three corridors)
        //Y: type of room. N ormal (nothing in it), E nemy, M erchant
        //Z: type of type. There are different types of shops and enemies.
        //Example 1: 1NE (one corridoe, normal room, empty)
        //Example 2: 1EN (one corridor, enemy, normal type)

        //current number of screens: 4
        //current number of enemies: 1
        //current number of merchants: 0




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


            //uncomment only when you add new rooms

            //using (DungeonContext context = new DungeonContext())
            //{

            //    context.Rooms.Add(new Room() { RoomName = "3M3" });
            //    //context.Monsters.Add(new Monster() { MonsterName = "Putrid Sludge", MonsterAttack = 5, MonsterHealth = 10 });
            //    context.SaveChanges();
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



        

        private void Explore_Click(object sender, RoutedEventArgs e)
        {
            if (exploreMode == true)
            {
                //the game lets you only explore if you're not fighting an enemy
                if (isEnemyAlive == false)
                {
                    //generates a random index for the room
                    Random rand = new Random();
                    //the second int in the Next corresponds to total amount of rooms in the game -1
                    int nrand = rand.Next(1, 6);

                    //the query ruturns room name correspondin to the random index
                    using (var db = new DungeonContext())
                    {
                        var query = db.Rooms.Where(b => b.RoomId == nrand).Select(b => b.RoomName);
                        foreach (string name in query)
                        {
                            nameOfRoom = name;
                        }
                    }

                    //thwe X value of the naming conventiont corresponding to the number of corridors in the room is passed into the variable
                    roomNum = Int32.Parse(nameOfRoom.Substring(0, 1));
                    //the Y value of the naming convention is stored corresponding to the type of room
                    roomType = nameOfRoom.Substring(1, 1);
                    //the Z value of the naming convention is stored corresponding to the type of enemy
                    specialType = nameOfRoom.Substring(2, 1);

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

                                if (roomType == "M")
                                {
                                    exploreMode = false;

                                    ForwardB.Content = "BUY";
                                    LeftB.Content = "BUY";
                                    RightB.Content = "BUY";

                                    textCRWL.Text = "Welcome, welcome dear costumer! Are you intrested in my wares?";

                                    leftItem.Source = new BitmapImage(new Uri($"/items/HP Potion.png", UriKind.RelativeOrAbsolute));
                                    middleItem.Source = new BitmapImage(new Uri($"/items/HP Potion.png", UriKind.RelativeOrAbsolute));
                                    rightItem.Source = new BitmapImage(new Uri($"/items/HP Potion.png", UriKind.RelativeOrAbsolute));
                                }
                                break;
                            }
                    }

                    //resets the flag for visiting a room and updates the count of rooms visited
                    visitedFlag = false;
                    roomsVisited++;
                    textCRWL.Text = "The room is dark and filthy.";

                    enemyScreen.Source = null;
                }
                else
                {
                    textCRWL.Text = "You can't run away, you're currently in a fight!";
                    Enemy_Attack();
                }
            }
            //if the room has a merchant and the explore button is clicked again, the player explores a new room
            else
            {
                exploreMode = true;

                //the bought item flags reset themselves
                leftBougth = false;
                middleBougth = false;
                rightBougth = false;

                //the items get erased of the screen
                leftItem.Source = null;
                middleItem.Source = null;
                rightItem.Source = null;

                //the button names get set back to normal
                LeftB.Content = "LEFT";
                ForwardB.Content = "FORWARD";
                RightB.Content = "RIGHT";

                Explore_Click(sender, e);
            }
        }

        private void LeftB_Click(object sender, RoutedEventArgs e)
        {
            if(exploreMode == true)
                Explore_Click(sender, e);
            //buy mode
            else
            {
                if (leftBougth == false)
                {
                    //checks if player has enough gold to buy the item
                    if (gold >= 10)
                    {
                        leftItem.Source = null;

                        //the bought potion gets added to the inventory
                        using (DungeonContext context = new DungeonContext())
                        {
                            context.Inventories.Add(new Inventory() { InventoryName = "Health Potion" });
                            context.SaveChanges();

                            var query = context.Inventories.Select(b => b.InventoryName).ToList();

                            inventoryList.ItemsSource = query;
                        }

                        textCRWL.Text = "You bought a Health Potion.";

                        //bought gets set to true stopping from buying a non exsistant item
                        leftBougth = true;

                        //the display for gold changes and then the gold gets subtracted
                        Gold.Content = "Gold: " + (gold - 10);
                        gold -= 10;
                    }
                    else
                        textCRWL.Text = "You don't have enough gold to buy this item.";
                }
                //excutes if the item has been bought
                else
                    textCRWL.Text = "You've already bought this item.";
            }
        }

        private void ForwardB_Click(object sender, RoutedEventArgs e)
        {
            if (exploreMode == true)
                Explore_Click(sender, e);
            //buy mode
            else
            {
                if (middleBougth == false)
                {
                    //checks if player has enough gold to buy the item
                    if (gold >= 10)
                    {
                        middleItem.Source = null;

                        //the bought potion gets added to the inventory
                        using (DungeonContext context = new DungeonContext())
                        {
                            context.Inventories.Add(new Inventory() { InventoryName = "Health Potion" });
                            context.SaveChanges();

                            var query = context.Inventories.Select(b => b.InventoryName).ToList();

                            inventoryList.ItemsSource = query;
                        }

                        textCRWL.Text = "You bought a Health Potion.";

                        //bought gets set to true stopping from buying a non exsistant item
                        middleBougth = true;

                        //the display for gold changes and then the gold gets subtracted
                        Gold.Content = "Gold: " + (gold - 10);
                        gold -= 10;
                    }
                    else
                        textCRWL.Text = "You don't have enough gold to buy this item.";
                }
                //executes if the item has been bought
                else
                    textCRWL.Text = "You've already bought this item.";
            }
        }

        private void RightB_Click(object sender, RoutedEventArgs e)
        {
            if (exploreMode == true)
                Explore_Click(sender, e);
            //buy mode
            else
            {
                if (rightBougth == false)
                {
                    //checks if player has enough gold to buy the item
                    if (gold >= 10)
                    {
                        rightItem.Source = null;

                        //the bought potion gets added to the inventory
                        using (DungeonContext context = new DungeonContext())
                        {
                            context.Inventories.Add(new Inventory() { InventoryName = "Health Potion" });
                            context.SaveChanges();

                            var query = context.Inventories.Select(b => b.InventoryName).ToList();

                            inventoryList.ItemsSource = query;
                        }

                        textCRWL.Text = "You bought a Health Potion.";

                        //bought gets set to true stopping from buying a non exsistant item
                        rightBougth = true;

                        //the display for gold changes and then the gold gets subtracted
                        Gold.Content = "Gold: " + (gold - 10);
                        gold -= 10;
                    }
                    else
                        textCRWL.Text = "You don't have enough gold to buy this item.";
                }
                //excutes if the item has been bought
                else
                    textCRWL.Text = "You've already bought this item.";
            }
        }


        //Searches the room for treasure or enemies
        private void Search_Click(object sender, RoutedEventArgs e)
        {
            //room can only be searched once, afterwards the visited flag is switched on
            if (visitedFlag == false)
            {

                //enemies only found in 1 corridor rooms
                if (roomNum == 1)
                {
                    //check if it's an enemy room and if it's normal type
                    if (roomType == "E" && specialType == "N")
                    {
                        Random chance = new Random();
                        int tt = chance.Next(1, 4);
                        switch (tt)
                        {
                            case 1:
                                {
                                    textCRWL.Text = "You've encoutered Putrid Sludge. Prepare for battle!";
                                    isEnemyAlive = true;
                                    Beastiary.Putrid_Sludge();
                                    ForwardB.Visibility = Visibility.Hidden;
                                    LeftB.Visibility = Visibility.Hidden;
                                    RightB.Visibility = Visibility.Hidden;
                                    enemyScreen.Source = new BitmapImage(new Uri($"/monsters/Putrid Sludge.png", UriKind.RelativeOrAbsolute));
                                    break;
                                }
                            case 2:
                                {
                                    textCRWL.Text = "You've encoutered Putrid Sludge. Prepare for battle!";
                                    isEnemyAlive = true;
                                    Beastiary.Putrid_Sludge();
                                    ForwardB.Visibility = Visibility.Hidden;
                                    LeftB.Visibility = Visibility.Hidden;
                                    RightB.Visibility = Visibility.Hidden;
                                    enemyScreen.Source = new BitmapImage(new Uri($"/monsters/Putrid Sludge.png", UriKind.RelativeOrAbsolute));
                                    break;
                                }
                            case 3:
                                {
                                    Get_Items();
                                    break;
                                }
                        }
                        
                    }
                    else
                        Get_Items();


                }
                else
                {
                    Get_Items();
                }



                //once the room has been searched, the flag is set to true so they can't search the same room multiple times
                visitedFlag = true;
                
                
            }
            else
                //the room can't be searched more than once
                textCRWL.Text = "You have already searched this room.";
        }




        //attacks enemies
        private void Attack_Click(object sender, RoutedEventArgs e)
        {
            //if the enemy is alive you can attack them
            if (isEnemyAlive == true)
            {
                Beastiary.MonsterHealth -= attackStrength;
                textCRWL.Text = $"You strike the enemy dealing {attackStrength} damage to them!";
                Enemy_Attack();
            }
            else
                //if not...
                textCRWL.Text = "You swing at the air mindlessly.";
        }



        //use of items
        private void Use_Click(object sender, RoutedEventArgs e)
        {
            switch (item)
            {
                case "Health Potion":
                    {
                        //you can only use potions if you're below max health
                        if (healthBar.Value < 100)
                        {
                            //if drinking the potion will give you more than your max health it will just set your health to 100
                            if (healthBar.Value + 10 < 100)
                            {
                                healthBar.Value = healthBar.Value + 10;
                                IEditableCollectionView items = inventoryList.Items;
                                if (items.CanRemove)
                                {
                                    items.Remove(inventoryList.SelectedItem);
                                }
                            }
                            else
                            {
                                healthBar.Value = 100;
                                IEditableCollectionView items = inventoryList.Items;
                                if (items.CanRemove)
                                {
                                    items.Remove(inventoryList.SelectedItem);
                                }
                            }
                            //using items isn't a free action
                            if(isEnemyAlive == true)
                            {
                                Enemy_Attack();
                            }
                        }
                        break;
                    }
                case "Sludge":
                    {
                        if (healthBar.Value <= 100)
                        {
                            healthBar.Value = healthBar.Value - 5;
                            textCRWL.Text = "You feel nauseous...";
                            IEditableCollectionView items = inventoryList.Items;
                            if (items.CanRemove)
                            {
                                items.Remove(inventoryList.SelectedItem);
                            }

                            //if after eating the sludge the player's health drops to 0 the game will close
                            if(healthBar.Value == 0)
                            {
                                Close();
                            }

                            //using items isn't a free action
                            if (isEnemyAlive == true)
                            {
                                Enemy_Attack();
                            }
                        }
                        break;
                    }
            }
        }




        //sets the current held item based on selected item from inventoryList
        private void inventoryList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (inventoryList.SelectedItem != null)
                item = inventoryList.SelectedItem as string;
        }



        //the function executes per enemy attack
        public void Enemy_Attack()
        {
            //first checks if the enemy is alive
            if (Beastiary.MonsterHealth > 0)
            {
                //then the enemy attacks the player
                healthBar.Value -= Beastiary.MonsterAttack;
                textCRWL.Text += $" The enemy strikes you, dealing {Beastiary.MonsterAttack} damage to you!";

                //then it checks if the player has died. if they did the game is closed
                if (healthBar.Value == 0)
                {
                    Close();
                }
            }
            else
            //when the enemy dies, the player can progress again and is given an item
            {
                textCRWL.Text = "Victory!";
                enemyScreen.Source = null;
                //the enemy is declared dead allowing the player to explore once again
                isEnemyAlive = false;


                //variable to hold the item found
                string name = "";
                using (DungeonContext context = new DungeonContext())
                {
                    context.Inventories.Add(new Inventory() { InventoryName = "Sludge" });
                    context.SaveChanges();

                    var query = context.Inventories.Select(b => b.InventoryName).ToList();

                    inventoryList.ItemsSource = query;
                    foreach (var item in query)
                    {
                        name = item;
                    }

                    textCRWL.Text += $" You got {name}! It smells awful...";
                }

            }

        }



        //gives the player items
        public void Get_Items()
        {
            //generates a random amount of gold for the player to find in the room * [(the number of visited rooms/5), rounded up]
            int grand = (int)(new Random().Next(1, 11)) * (roomsVisited / 5);
            gold += grand;
            Gold.Content = "Gold: " + gold;
            textCRWL.Text = $"Searching around the room you find {grand} gold coins.";

            //variable to hold the item found
            string name = "";

            //adds new item to the player's inventory
            using (DungeonContext context = new DungeonContext())
            {
                context.Inventories.Add(new Inventory() { InventoryName = "Health Potion" });
                context.SaveChanges();

                var query = context.Inventories.Select(b => b.InventoryName).ToList();

                inventoryList.ItemsSource = query;
                foreach (var item in query)
                {
                    name = item;
                }

                textCRWL.Text += $" Additionally you've found {name}.";
            }
        }

        
    }
}
