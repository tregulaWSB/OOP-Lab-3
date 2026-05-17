using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;

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

  class Order
  {
    public Customer Customer { get; set; }
    public List<MenuItem> Items { get; set; }
    public string Status { get; set; }
    public int OrderId { get; private set; }
    public static int TotalOrdersCount = 0;
    public static double TotalRevenue = 0.0;

    public Order(Customer customer, List<MenuItem> items)
    {
      this.Customer = customer;
      this.Items = items ?? new List<MenuItem>();
      this.Status = "Nowe";
      TotalOrdersCount++;
      this.OrderId = TotalOrdersCount;
    }

    public void AddItem(MenuItem item)
    {
      Items.Add(item);
    }

    public double CalculateTotal()
    {
      return Items.Sum(item => item.Price);
    }

    private void GenerateBill()
    {
      Console.WriteLine($"-----RACHUNEK DO ZAMÓWIENIA #{OrderId}-----");
      Console.WriteLine($"Data: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
      Console.WriteLine($"Klient: {Customer.Name} {Customer.Surname}");
      foreach (MenuItem item in Items)
      {
        Console.WriteLine($" - {item.Name} {item.Price:F2} zł");
      }
      double total = CalculateTotal();
      Console.WriteLine($"SUMA CAŁKOWITA: {total:F2} zł");
    }

    public void FinalizeOrder()
    {
      GenerateBill();
      double total = CalculateTotal();
      TotalRevenue += total;
      int points = (int)(total / 10);
      Customer.AddLoyaltyPoints(points);
      Status = "Opłacone";
    }
  }

  class Program
  {
    static void Main()
    {
      Barista barista = new Barista("Anna", "Kowalska", "F", 5000);
      Chef chef = new Chef("Jan", "Nowak", "M", 6500);

      var menu = new List<MenuItem>
      {
        new Drink("Espresso", 7.0, "1 shot z ekspresu kolbowego", "hot", "S"),
        new Drink("Doppio", 9.0, "Podwójne espresso, 2 shoty z ekspresu kolbowego", "hot", "S"),
        new Drink("Americano M", 10.0, "Podwójne espresso z większą ilością wody", "hot", "M"),
        new Drink("Americano L", 12.0, "Podwójne espresso z większą ilością wody", "hot", "L"),
        new Drink("Cappuccino M", 13.0, "Espresso ze spienionym mlekiem", "hot", "M"),
        new Drink("Cappuccino L", 15.0, "Espresso ze spienionym mlekiem", "hot", "L"),
        new Drink("Flat White M", 14.0, "Podwójne espresso z delikatnie spienionym mlekiem", "hot", "M"),
        new Drink("Flat White L", 16.0, "Podwójne espresso z delikatnie spienionym mlekiem", "hot", "L"),
        new Drink("Herbata Czarna", 9.0, "Herbata liściasta Earl Grey", "hot", "L"),
        new Drink("Herbata Zimowa", 15.0, "Czarna herbata z pomarańczą, goździkami i miodem", "hot", "L"),
        new Drink("Ice Latte", 17.0, "Espresso, kostki lodu i zimne mleko", "cold", "L"),
        new Drink("Iced Caramel Macchiato", 18.0, "Kawa mrożona z syropem karmelowym", "cold", "L"),
        new Drink("Lemoniada", 13.0, "Gazowana woda z cytryną i świeżą miętą", "cold", "L"),
        new Drink("Sok pomarańczowy", 12.0, "Świeżo wyciskany sok z pomarańczy", "cold", "L"),
        new Food("Francuskie śniadanie", 11.0, "Maślany rogalik z dżemem truskawkowym", "śniadanie", false, 1),
        new Food("Kanapka z szynką", 14.0, "Ciabatta z żółtym serem, szynką parmeńską i pomidorem", "śniadanie", false, 2),
        new Food("Tosty z awokado", 22.0, "Dwa tosty, pasta z awokado", "śniadanie", false, 5),
        new Food("Jajecznica z boczkiem", 18.0, "Jajecznica z 3 jajek z cebulą i boczkiem, pieczywo", "śniadanie", false, 6),
        new Food("Pancakes", 21.0, "Naleśniki z syropem klonowym", "śniadanie", false, 8),
        new Food("Sernik z białą czekoladą", 17.0, "Kremowy sernik na kruchym spodzie", "deser", false, 0),
        new Food("Brownie", 15.0, "Mocno czekoladowe ciasto z orzechami włoskimi", "deser", false, 0),
        new Food("Szarlotka na gorąco", 16.0, "Z domowymi jabłkami i cynamonem, podawana na ciepło", "deser", true, 2),
        new Food("Wegańskie ciasto marchewkowe", 15.0, "Puszyste ciasto z kremem z orzechów nerkowca", "deser", true, 0)
      };

      Console.Clear();
      Console.WriteLine(new string('=', 50));
      Console.WriteLine("SYMULATOR KAWIARNI");
      Console.WriteLine(new string('=', 50));

      Console.WriteLine("\nProszę, załóż kartę stałego klienta:");
      string name = "";
      while (string.IsNullOrWhiteSpace(name))
      {
        Console.Write("Podaj imię: ");
        name = Console.ReadLine()?.Trim();
        if (string.IsNullOrWhiteSpace(name))
        {
          Console.WriteLine("Błąd: Podaj poprawne imię.");
        }
      }

      string surname = "";
      while (string.IsNullOrWhiteSpace(surname))
      {
        Console.Write("Podaj nazwisko: ");
        surname = Console.ReadLine()?.Trim();
        if (string.IsNullOrWhiteSpace(surname))
        {
          Console.WriteLine("Błąd: Podaj poprawne nazwisko.");
        }
      }

      string gender = "";
      while (gender != "M" && gender != "F")
      {
        Console.Write("Podaj płeć (M/F): ");
        gender = Console.ReadLine()?.Trim().ToUpper();
        if (gender != "M" && gender != "F")
        {
          Console.WriteLine("Błąd: Wybierz 'M' lub 'F'.");
        }
      }

      Customer customer = new Customer(name, surname, gender);
      Console.WriteLine("\nKonto utworzone pomyślnie!\n");

      while (true)
      {
        Console.WriteLine(new string('-', 30));
        Console.WriteLine("MENU GŁÓWNE APLIKACJI");
        Console.WriteLine(new string('-', 30));
        Console.WriteLine("1. Przeglądaj menu kawiarni");
        Console.WriteLine("2. Złóż nowe zamówienie");
        Console.WriteLine("3. Moje konto i statystyki kawiarni");
        Console.WriteLine("4. Wyjście z kawiarni");

        Console.Write("\nWybierz opcję (1-4): ");
        string wybor = Console.ReadLine()?.Trim();

        switch (wybor)
        {
          case "1":
            Console.Clear();
            Console.WriteLine("--- NASZE MENU ---");
            for (int i = 0; i < menu.Count; i++)
            {
              Console.WriteLine($"{i + 1}. {menu[i].DisplayInfo()}");
            }
            Console.Write("\nWciśnij dowolny przycisk, aby wrócić do głównego menu: ");
            Console.ReadLine();
            break;

          case "2":
            Console.Clear();
            Order currentOrder = new Order(customer, new List<MenuItem>());
            string error = "";
            string orderString = "";

            while (true)
            {
              Console.WriteLine("--- SKŁADANIE ZAMÓWIENIA ---\n");
              if (!string.IsNullOrEmpty(error))
              {
                Console.WriteLine(error + "\n");
              }
              if (!string.IsNullOrEmpty(orderString))
              {
                Console.WriteLine($"Zamówienie: {orderString}\n");
              }
              
              Console.WriteLine("Wpisz numer pozycji z menu, aby dodać do zamówienia lub wciśnij Enter aby zakończyć.");
              for (int i = 0; i < menu.Count; i++)
              {
                Console.WriteLine($"{i + 1}. {menu[i].Name} {menu[i].Price:F2} zł");
              }

              Console.Write("\nTwój wybór: ");
              string wyborProduktu = Console.ReadLine()?.Trim();

              if (string.IsNullOrEmpty(wyborProduktu))
              {
                break;
              }

              try
              {
                int idxProduktu = int.Parse(wyborProduktu) - 1;
                if (idxProduktu >= 0 && idxProduktu < menu.Count)
                {
                  MenuItem wybranyProdukt = menu[idxProduktu];
                  currentOrder.AddItem(wybranyProdukt);
                  if (!string.IsNullOrEmpty(orderString)) orderString += ", ";
                  orderString += wybranyProdukt.Name;
                  error = "";
                }
                else
                {
                  error = "Błąd: Brak wybranej pozycji w menu";
                }
              }
              catch (FormatException)
              {
                error = "Błąd: Niepoprawny numer.";
              }

              Console.Clear();
            }

            Console.Clear();
            if (currentOrder.Items.Count > 0)
            {
              Console.WriteLine("--- PODSUMOWANIE ZAMÓWIENIA ---\n");
              Console.WriteLine($"Wybrane pozycje z menu: {orderString}.\n");
              Console.WriteLine($"Dziękujemy za zamówienie. To będzie {currentOrder.CalculateTotal():F2} zł");
              Console.Write("\nWciśnij dowolny przycisk, aby zapłacić: ");
              Console.ReadLine();

              Console.Clear();
              currentOrder.FinalizeOrder();

              Console.Write("\nWciśnij dowolny przycisk, aby zabrać rachunek: ");
              Console.ReadLine();

              Console.Clear();
              Console.WriteLine("Twoje zamówienie jest w trakcie realizacji...\n");
              
              foreach (MenuItem item in currentOrder.Items)
              {
                if (item is Drink)
                {
                  barista.AssignTask((IMenuItem)item);
                }
                else
                {
                  chef.AssignTask((IMenuItem)item);
                }
              }

              Console.WriteLine("");
              barista.PerformDuties();
              Console.WriteLine("");
              chef.PerformDuties();
            }
            else
            {
              Console.WriteLine("Anulowano: Zamówienie jest puste.");
            }

            Console.Write("\nWciśnij dowolny przycisk, aby wrócić do głównego menu: ");
            Console.ReadLine();
            break;

          case "3":
            Console.Clear();
            Console.WriteLine("--- MOJE KONTO ---");
            Console.WriteLine(customer.DisplayInfo());
            Console.WriteLine($"Zebrane punkty lojalnościowe: {customer.LoyaltyPoints}");

            Console.WriteLine("\n--- STATYSTYKI LOKALU ---");
            Console.WriteLine($"Całkowity obrót: {Order.TotalRevenue:F2} zł");
            Console.WriteLine($"Zrealizowane zamówienia: {Order.TotalOrdersCount}");
            Console.WriteLine($"Ilość zarejestrowanych klientów w systemie: {Customer.TotalCustomers}");

            Console.Write("\nWciśnij dowolny przycisk, aby wrócić do głównego menu: ");
            Console.ReadLine();
            break;

          case "4":
            Console.WriteLine($"\nDziękujemy za wizytę, {customer.Name}! Do zobaczenia ponownie.");
            return;

          default:
            Console.WriteLine("");
            break;
        }

        Console.Clear();
      }      
    }
  }
}
