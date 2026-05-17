using System;
using System.Threading;

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

      Console.WriteLine(espresso.DisplayInfo());
      Console.WriteLine(americano.DisplayInfo());
      Console.WriteLine(sandwich.DisplayInfo());
      Console.WriteLine(pancakes.DisplayInfo());

      lemonade.Prepare();
      brownie.Prepare();
    }
  }
}
