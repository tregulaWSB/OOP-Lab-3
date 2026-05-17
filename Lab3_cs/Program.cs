using System;
using System.Threading;
using System.Collections.Generic;

namespace Lab3_cs
{
  interface IMenuItem
  {
    void Prepare();
  }

  class MenuItem
  {
    public string Name { get; set; }
    protected double price;
    public string Description { get; set; }

    public double Price
    {
      get { return price; }
      set
      {
        if (value < 0)
        {
          throw new ArgumentException("Cena nie może być ujemna.");
        }
        price = value;
      }
    }

    public MenuItem(string name, double price, string description)
    {
      this.Name = name;
      this.Price = price;
      this.Description = description;
    }

    public string DisplayInfo()
    {
      return $"{Name} - {Description} (Cena: {Price:F2} zł)";
    }
  }

  class Drink : MenuItem, IMenuItem
  {
    protected string temp;
    protected string size;

    public string Temp
    {
      get { return temp; }
      set
      {
        if (value != "hot" && value != "cold")
        {
          throw new ArgumentException("Nieprawidłowy typ napoju.");
        }
        temp = value;
      }
    }

    public string Size
    {
      get { return size; }
      set
      {
        if (value != "S" && value != "M" && value != "L")
        {
          throw new ArgumentException("Nieprawidłowy rozmiar napoju.");
        }
        size = value;
      }
    }

    public Drink(string name, double price, string description, string temp, string size) : base(name, price, description)
    {
      this.Temp = temp;
      this.Size = size;
    }

    public void Prepare()
    {
      string temp = Temp == "hot" ? "gorący" : "zimny";
      Console.WriteLine($"Barista przygotowuje {temp} napój: {Name}, rozmiar {Size}.");
      switch (Size)
      {
        case "S":
          Thread.Sleep(2000);
          break;
        case "M":
          Thread.Sleep(4000);
          break;
        case "L":
          Thread.Sleep(6000);
          break;
      }
    }
  }

  class Food : MenuItem, IMenuItem
  {
    protected string type;
    public bool IsVegan { get; set; }
    protected int prepTimeMin;

    public string Type
    {
      get { return type; }
      set
      {
        if (value != "śniadanie" && value != "deser")
        {
          throw new ArgumentException("Nieprawidłowy typ jedzenia.");
        }
        type = value;
      }
    }

    public int PrepTimeMin
    {
      get { return prepTimeMin; }
      set { prepTimeMin = value; }
    }

    public Food(string name, double price, string description, string type, bool isVegan, int prepTimeMin) : base(name, price, description)
    {
      this.Type = type;
      this.IsVegan = isVegan;
      this.PrepTimeMin = prepTimeMin;
    }

    public void Prepare()
    {
      string diet = IsVegan ? "[Wege] " : "";
      if (PrepTimeMin > 0)
      {
        Console.WriteLine($"Kucharz przygotowuje {Type}: {diet}{Name}. Czas oczekiwania: ~{PrepTimeMin} min.");
        Thread.Sleep(PrepTimeMin * 1000);
      }
      else
      {
        Console.WriteLine($"Kucharz wydaje {Type}: {diet}{Name}.");
      }
    }
  }

  class Person
  {
    public string Name { get; set; }
    public string Surname { get; set; }
    protected string gender;

    public string Gender
    {
      get { return gender; }
      set
      {
        if (value != "M" && value != "F")
        {
          throw new ArgumentException("Nieprawidłowa płeć.");
        }
        gender = value;
      }
    }

    public Person(string name, string surname, string gender)
    {
      this.Name = name;
      this.Surname = surname;
      this.Gender = gender;
    }

    public string DisplayInfo()
    {
      string gender = Gender == "M" ? "mężczyzna" : "kobieta";
      return $"Imię: {Name}, nazwisko: {Surname}, płeć: {gender}";
    }
  }

  interface IEmployee
  {
    void AssignTask(IMenuItem item);
    void PerformDuties();
  }

  class Employee : Person
  {
    protected double salary;
    public List<IMenuItem> Tasks { get; private set; }
    public static int TotalEmployees = 0;

    public double Salary
    {
      get { return salary; }
      set
      {
        if (value < 0)
        {
          throw new ArgumentException("Pensja nie może być ujemna.");
        }
        salary = value;
      }
    }

    public Employee(string name, string surname, string gender, double salary) : base(name, surname, gender)
    {
      this.Salary = salary;
      this.Tasks = new List<IMenuItem>();
      TotalEmployees++;
    }
  }

  class Barista : Employee, IEmployee
  {
    public Barista(string name, string surname, string gender, double salary) : base(name, surname, gender, salary) {}

    public void AssignTask(IMenuItem item)
    {
      if (!(item is Drink))
      {
        throw new ArgumentException("Barista może przygotowywać wyłącznie napoje.");
      }
      Tasks.Add(item);      
      string itemName = ((MenuItem)item).Name;
      if (Gender == "M")
      {
        Console.WriteLine($"[Zlecenie] Barista {Name} przyjął zamówienie na: {itemName}");
      }
      else
      {
        Console.WriteLine($"[Zlecenie] Barista {Name} przyjęła zamówienie na: {itemName}");
      }
    }

    public void PerformDuties()
    {
      if (Tasks.Count == 0) return;
      Console.WriteLine($"Barista {Name} rozpoczyna przygotowywanie napojów...");
      foreach (IMenuItem item in Tasks)
      {
        item.Prepare();
      }
      Tasks.Clear();
      if (Gender == "M")
      {
        Console.WriteLine($"Barista {Name} wydał wszystkie napoje.");
      }
      else
      {
        Console.WriteLine($"Barista {Name} wydała wszystkie napoje.");
      }
    }
  }

  class Chef : Employee, IEmployee
  {
    public Chef(string name, string surname, string gender, double salary) : base(name, surname, gender, salary) {}

    public void AssignTask(IMenuItem item)
    {
      if (!(item is Food))
      {
        throw new ArgumentException("Kucharz może przygotowywać wyłącznie jedzenie.");
      }
      Tasks.Add(item);
      string itemName = ((MenuItem)item).Name;
      if (Gender == "M")
      {
        Console.WriteLine($"[Zlecenie] Kucharz {Name} przyjął zamówienie na: {itemName}");
      }
      else
      {
        Console.WriteLine($"[Zlecenie] Kucharz {Name} przyjęła zamówienie na: {itemName}");
      }
    }

    public void PerformDuties()
    {
      if (Tasks.Count == 0) return;
      Console.WriteLine($"Kucharz {Name} rozpoczyna przygotowywanie jedzenia...");
      foreach (IMenuItem item in Tasks)
      {
        item.Prepare();
      }
      Tasks.Clear();
      if (Gender == "M")
      {
        Console.WriteLine($"Kucharz {Name} przygotował wszystkie dania.");
      }
      else
      {
        Console.WriteLine($"Kucharz {Name} przygotowała wszystkie dania.");
      }
    }
  }

  class Customer : Person
  {
    protected int loyaltyPoints;
    public static int TotalCustomers = 0;

    public int LoyaltyPoints
    {
      get { return loyaltyPoints; }
      set
      {
        if (value < 0)
        {
          throw new ArgumentException("Punkty lojalnościowe nie mogą być ujemne.");
        }
        loyaltyPoints = value;
      }
    }

    public Customer(string name, string surname, string gender, int loyaltyPoints = 0) : base(name, surname, gender)
    {
      this.LoyaltyPoints = loyaltyPoints;
      TotalCustomers++;
    }

    public void AddLoyaltyPoints(int points)
    {
      LoyaltyPoints += points;
      Console.WriteLine($"Klient {Name} otrzymał {points} pkt. Razem: {LoyaltyPoints} pkt.");
    }
  }

  class Program
  {
    static void Main()
    {
      Drink espresso = new Drink("Espresso", 7.0, "1 shot z ekspresu kolbowego", "hot", "S");
      Drink americano = new Drink("Americano M", 10.0, "Podwójne espresso z większą ilością wody", "hot", "M");
      Drink lemonade = new Drink("Lemoniada", 13.0, "Gazowana woda z cytryną i świeżą miętą", "cold", "L");

      Food sandwich = new Food("Kanapka z szynką", 14.0, "Ciabatta z żółtym serem, szynką parmeńską i pomidorem", "śniadanie", false, 2);
      Food pancakes = new Food("Pancakes", 21.0, "Naleśniki z syropem klonowym", "śniadanie", false, 8);
      Food brownie = new Food("Brownie", 15.0, "Mocno czekoladowe ciasto z orzechami włoskimi", "deser", false, 0);

      Barista barista = new Barista("Anna", "Kowalska", "F", 5000);
      Chef chef = new Chef("Jan", "Nowak", "M", 6500);
      Customer customer = new Customer("Robert", "Szczęsny", "M", loyaltyPoints : 100);

      Console.WriteLine(barista.DisplayInfo());
      Console.WriteLine(chef.DisplayInfo());
      Console.WriteLine(customer.DisplayInfo());

      barista.AssignTask(espresso);
      barista.AssignTask(lemonade);

      chef.AssignTask(sandwich);
      chef.AssignTask(brownie);

      barista.PerformDuties();
      chef.PerformDuties();
      customer.AddLoyaltyPoints(1000);
    }
  }
}
