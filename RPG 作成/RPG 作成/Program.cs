using System.Text.Json;
using System.Xml;

namespace RPG_作成 
{
    internal class program

    {
        class Item
        {
            public string Name { get; }
            public string Type { get; }
            public int Quantity { get; set; }

            public Item(string name, string type, int quantity = 1)
            {
                Name = name;
                Type = type;
                Quantity = quantity;
            }

            public override string ToString()
            {
                return $"{Name} ({Type}) x{Quantity}";
            }
        }

        class Inventory
        {
            private List<Item> items = new List<Item>();

            public void AddItem(Item item)
            {
                Item existingItem = items.Find(i => i.Name == item.Name);
                if (existingItem != null)
                {
                    existingItem.Quantity += item.Quantity;
                }
                else
                {
                    items.Add(item);
                }
            }

            public void RemoveItem(string itemName)
            {
                items.RemoveAll(i => i.Name == itemName);
            }

            public void ShowInventory()
            {
                Console.WriteLine("=== インベントリ ===");
                if (items.Count == 0)
                {
                    Console.WriteLine("Nothing");
                }
                else
                {
                    foreach (var item in items)
                    {
                        Console.WriteLine(item);
                    }
                }
            }
        }

        class Player
        {
            public string Name { get; }
            public string ClassType { get; }
            public int Health { get; private set; }
            public int Magic { get; private set; }
            public int Level { get; private set; }
            public int Experience { get; private set; }
            private Inventory inventory = new Inventory();

            public Player(string name, string classType, int health, int magic)
            {
                Name = name;
                ClassType = classType;
                Health = health;
                Magic = magic;
                Level = 1;
                Experience = 0;
            }

            public void UsePotion()
            {
                Console.WriteLine("I used the potion！");
                Health += 20;
            }

            public void EatFood()
            {
                Console.WriteLine("I ate food！");
                Health += 10;
                Magic += 5;
            }

            public void GainExperience(int exp)
            {
                Experience += exp;
                Console.WriteLine($"Exp +{exp}!");

                if (Experience >= Level * 100)
                {
                    LevelUp();
                }
            }

            private void LevelUp()
            {
                Level++;
                Experience = 0;
                Console.WriteLine($"level up！ New levels: {Level}");
            }

            public void ShowStatus()
            {
                Console.WriteLine($"player: {Name}, class: {ClassType}, HP: {Health}, MP: {Magic}, level: {Level}, Experience points: {Experience}");
            }
        }

        class Weapon
        {
            public string Name { get; }
            public int AttackPower { get; }

            public Weapon(string name, int attackPower)
            {
                Name = name;
                AttackPower = attackPower;
            }

            public void Attack()
            {
                Console.WriteLine($"{Name}attack！ damage: {AttackPower}");
            }
        }

        class Enemy
        {
            public string Name { get; }
            public int Health { get; private set; }
            public int AttackPower { get; }

            public Enemy(string name, int health, int attackPower)
            {
                Name = name;
                Health = health;
                AttackPower = attackPower;
            }

            public void TakeDamage(int damage)
            {
                Health -= damage;
                Console.WriteLine($"{Name}is{damage}Damaged！");
            }

            public void Attack(Player player)
            {
                Console.WriteLine($"{Name}attack！ damage: {AttackPower}");
            }
        }

        class Combat
        {
            private static Random rand = new Random();

            public static int CalculateDamage(int baseDamage)
            {
                bool isCritical = rand.Next(0, 100) < 20;
                return isCritical ? baseDamage * 2 : baseDamage;
            }
        }

        class SaveSystem
        {
            public static void SaveGame(Player player)
            {
                string jsonData = JsonSerializer.Serialize(player);
                File.WriteAllText("savegame.json", jsonData);
                Console.WriteLine("Game data saved！");
            }

            public static Player LoadGame()
            {
                if (File.Exists("savegame.json"))
                {
                    string jsonData = File.ReadAllText("savegame.json");
                    return JsonSerializer.Deserialize<Player>(jsonData);
                }
                else
                {
                    Console.WriteLine("No save data.");
                    return null;
                }
            }
        }

        class Program
        {  
            static void Main()
            {
                Player player = new Player("Hero", "Warior", 100, 50);
                player.ShowStatus();

                // Adding items
                Inventory inventory = new Inventory();
                inventory.AddItem(new Item("Potion", "Recovery", 3));
                inventory.AddItem(new Item("Sword", "Weapon", 1));

                // Battle
                Enemy enemy = new Enemy("Goblin", 50, 10);
                int damage = Combat.CalculateDamage(10);
                enemy.TakeDamage(damage);

                // Level up
                player.GainExperience(120);

                // Save game
                SaveSystem.SaveGame(player);
            }
        }
    
    }
}

