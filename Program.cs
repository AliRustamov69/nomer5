using System;

public class Weapon
{
    public string Name { get; private set; }
    public int Damage { get; private set; }

    public Weapon(string name, int damage)
    {
        Name = name;
        Damage = damage;
    }
}

public class Aid
{
    public string Name { get; private set; }
    public int HealthRestore { get; private set; }

    public Aid(string name, int healthRestore)
    {
        Name = name;
        HealthRestore = healthRestore;
    }
}

public class Player
{
    public string Name { get; private set; }
    public int CurrentHealth { get; set; }
    public int MaxHealth { get; private set; }
    public Weapon Weapon { get; private set; }
    public Aid Aid { get; private set; }

    public Player(string name, int currentHealth, int maxHealth, Weapon weapon, Aid aid)
    {
        Name = name;
        CurrentHealth = currentHealth;
        MaxHealth = maxHealth;
        Weapon = weapon;
        Aid = aid;
    }

    public void Attack(Enemy enemy)
    {
        Console.WriteLine($"{Name} ударил противника {enemy.Name}");
        enemy.CurrentHealth -= Weapon.Damage;
    }

    public void UseAid()
    {
        if (CurrentHealth < MaxHealth)
        {
            int healthRestored = Aid.HealthRestore;
            CurrentHealth = Math.Min(CurrentHealth + healthRestored, MaxHealth);
            Console.WriteLine($"{Name} использовал аптечку {Aid.Name}");
        }
        else
        {
            Console.WriteLine("Ваше здоровье уже полное!");
        }
    }
}

public class Enemy
{
    public string Name { get; private set; }
    public int CurrentHealth { get; set; }
    public Weapon Weapon { get; private set; }

    public Enemy(string name, int currentHealth, Weapon weapon)
    {
        Name = name;
        CurrentHealth = currentHealth;
        Weapon = weapon;
    }

    public void Attack(Player player)
    {
        Console.WriteLine($"Противник {Name} ударил вас!");
        player.CurrentHealth -= Weapon.Damage;
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Добро пожаловать, воин!");
        Console.Write("Назови себя: ");
        string playerName = Console.ReadLine();

        Weapon playerWeapon = new Weapon("Фламберг", 20);
        Aid playerAid = new Aid("Средняя аптечка", 10);
        Player player = new Player(playerName, 100, 100, playerWeapon, playerAid);

        string[] enemyNames = { "Варвар", "Огромный паук", "Темный маг" };
        int[] enemyHealths = { 50, 40, 60 };
        Weapon[] enemyWeapons = { new Weapon("Экскалибур", 10), new Weapon("Ядовитый укус", 5), new Weapon("Сила магии", 15) };

        for (int i = 0; i < enemyNames.Length; i++)
        {
            Enemy enemy = new Enemy(enemyNames[i], enemyHealths[i], enemyWeapons[i]);

            Console.WriteLine($"Ваше имя {player.Name}!");
            Console.WriteLine($"Вам был ниспослан меч {player.Weapon.Name} ({player.Weapon.Damage}), а также {player.Aid.Name} ({player.Aid.HealthRestore}hp).");
            Console.WriteLine($"У вас {player.CurrentHealth}hp.");

            while (player.CurrentHealth > 0 && enemy.CurrentHealth > 0)
            {
                Console.WriteLine($"\n{player.Name} встречает врага {enemy.Name} ({enemy.CurrentHealth}hp), у врага на поясе сияет оружие {enemy.Weapon.Name} ({enemy.Weapon.Damage})");
                Console.WriteLine("Что вы будете делать?");
                Console.WriteLine("1. Ударить");
                Console.WriteLine("2. Пропустить ход");
                Console.WriteLine("3. Использовать аптечку");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        player.Attack(enemy);
                        Console.WriteLine($"У противника {enemy.CurrentHealth}hp, у вас {player.CurrentHealth}hp");
                        break;
                    case "2":
                        Console.WriteLine($"{player.Name} пропустил ход.");
                        break;
                    case "3":
                        player.UseAid();
                        Console.WriteLine($"У противника {enemy.CurrentHealth}hp, у вас {player.CurrentHealth}hp");
                        break;
                    default:
                        Console.WriteLine("Неверный выбор. Пожалуйста, выберите снова.");
                        continue;
                }

                // Проверка состояния врага
                if (enemy.CurrentHealth > 0)
                {
                    enemy.Attack(player);
                    Console.WriteLine($"У противника {enemy.CurrentHealth}hp, у вас {player.CurrentHealth}hp");
                }
                else
                {
                    Console.WriteLine($"**{enemy.Name}** повержен!");
                    break;
                }

                // Проверка состояния игрока
                if (player.CurrentHealth <= 0)
                {
                    Console.WriteLine("Вы погибли. Игра окончена.");
                    return; // Завершить игру, если игрок погиб
                }
            }

            // После победы над врагом
            if (i < enemyNames.Length - 1)
            {
                Console.WriteLine("Вы победили врага! Подготовьтесь к следующему противнику.");
                player.CurrentHealth = Math.Min(player.CurrentHealth + 10, player.MaxHealth); // Восстановление здоровья
                Console.WriteLine($"Ваше текущее здоровье: {player.CurrentHealth}hp");
            }
            else
            {
                Console.WriteLine("Поздравляем! Вы победили всех врагов!");
            }
        }
    }
}