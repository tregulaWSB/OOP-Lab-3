from abc import ABC, abstractmethod
import time
from datetime import datetime
import os

class MenuItem(ABC):
  def __init__(self, name: str, price: float, description: str):
    self.name = name
    self.price = price
    self.description = description

  @property
  def price(self) -> float:
    return self._price
  
  @price.setter
  def price(self, value: float):
    if value < 0:
      raise ValueError("Cena nie może być ujemna.")
    self._price = value

  def display_info(self) -> str:
    return f"{self.name} - {self.description} (Cena: {self.price:.2f} zł)"
  
  @abstractmethod
  def prepare(self):
    pass

class Drink(MenuItem):
  def __init__(self, name: str, price: float, description: str, temp: str, size: str):
    super().__init__(name, price, description)
    self.temp = temp
    self.size = size

  @property
  def temp(self) -> str:
    return self._temp

  @temp.setter
  def temp(self, value: str):
    if not value in ["hot", "cold"]: 
      raise ValueError("Nieprawidłowy typ napoju.")
    self._temp = value

  @property
  def size(self) -> str:
    return self._size

  @size.setter
  def size(self, value: str):
    if not value in ["S", "M", "L"]: 
      raise ValueError("Nieprawidłowy rozmiar napoju.")
    self._size = value

  def prepare(self):
    temp = "gorący" if self.temp else "zimny"
    print(f"Barista przygotowuje {temp} napój: {self.name}, rozmiar {self.size}.")
    match(self.size):
      case "S":
        time.sleep(2)
      case "M":
        time.sleep(4)
      case "L":
        time.sleep(6)
  
class Food(MenuItem):
  def __init__(self, name: str, price: float, description: str, type: str, is_vegan: bool, prep_time_min: int):
    super().__init__(name, price, description)
    self.type = type
    self.is_vegan = is_vegan
    self.prep_time_min = prep_time_min

  @property
  def type(self) -> str:
    return self._type

  @type.setter
  def type(self, value: str):
    if not value in ["śniadanie", "deser"]:
      ValueError("Nieprawidłowy typ jedzenia.")
    self._type = value
  
  @property
  def prep_time_min(self) -> int:
    return self._prep_time_min

  @prep_time_min.setter
  def prep_time_min(self, value: int):
    self._prep_time_min = value
      
  def prepare(self) -> str:
    diet = "[Wege] " if self.is_vegan else ""
    if self.prep_time_min > 0:
      print(f"Kucharz przygotowuje śniadanie: {diet}{self.name}. Czas oczekiwania: ~{self.prep_time_min} min.")
      time.sleep(self.prep_time_min)
    else:
      print(f"Kucharz wydaje {self.type}: {diet}{self.name}.")

class Person(ABC):
  def __init__(self, name: str, surname: str, gender: str):
    self.name = name
    self.surname = surname
    self.gender = gender

  @property
  def gender(self) -> str:
    return self._gender
  
  @gender.setter
  def gender(self, value: str):
    if not value in ["M", "F"]:
      raise ValueError("Nieprawidłowa płeć.")
    self._gender = value

  def display_info(self) -> str:
    gender = "mężczyzna" if self.gender == "M" else "kobieta"
    return f"Imię: {self.name}, nazwisko: {self.surname}, płeć: {gender}"
  
class Employee(Person, ABC):
  total_employees = 0

  def __init__(self, name: str, surname: str, gender: str, salary: float):
    Employee.total_employees += 1 
    super().__init__(name, surname, gender)
    self.salary = salary
    self.tasks = []

  @property
  def salary(self) -> float:
    return self._salary

  @salary.setter
  def salary(self, value: float):
    if value < 0:
      raise ValueError("Pensja nie może być ujemna.")
    self._salary = value

  @abstractmethod
  def assign_task(self, item):
    pass

  @abstractmethod
  def perform_duties(self):
    pass

class Barista(Employee):
  def assign_task(self, item):
    if not isinstance(item, Drink):
      raise ValueError("Barista może przygotowywać wyłącznie napoje.")
    self.tasks.append(item)
    if self.gender == "M":
      print(f"[Zlecenie] Barista {self.name} przyjął zamówienie na: {item.name}")
    else:
      print(f"[Zlecenie] Barista {self.name} przyjęła zamówienie na: {item.name}")

  def perform_duties(self):
    if not self.tasks:
      return  
        
    print(f"Barista {self.name} rozpoczyna przygotowywanie napojów...")
    for item in self.tasks:
      item.prepare()
      
    self.tasks.clear()
    if self.gender == "M":
      print(f"Barista {self.name} wydał wszystkie napoje.")
    else:
      print(f"Barista {self.name} wydała wszystkie napoje.")

class Chef(Employee):
  def assign_task(self, item):
    if not isinstance(item, Food):
      raise ValueError("Kucharz może przygotowywać wyłącznie jedzenie.")
    self.tasks.append(item)
    if self.gender == "M":
      print(f"[Zlecenie] Kucharz {self.name} przyjął zamówienie na: {item.name}")
    else:
      print(f"[Zlecenie] Kucharz {self.name} przyjęła zamówienie na: {item.name}")

  def perform_duties(self):
    if not self.tasks:
      return
      
    print(f"Kucharz {self.name} rozpoczyna przygotowywanie jedzenia...")
    for item in self.tasks:
      item.prepare()
        
    self.tasks.clear()
    if self.gender == "M":
      print(f"Kucharz {self.name} przygotował wszystkie dania.")
    else:
      print(f"Kucharz {self.name} przygotowała wszystkie dania.")

class Customer(Person):
  total_customers = 0

  def __init__(self, name: str, surname: str, gender: str, loyalty_points: int = 0):
    Customer.total_customers += 1 
    super().__init__(name, surname, gender)
    self.loyalty_points = loyalty_points

  @property
  def loyalty_points(self) -> int:
    return self._loyalty_points

  @loyalty_points.setter
  def loyalty_points(self, value: int):
    if value < 0:
      raise ValueError("Punkty lojalnościowe nie mogą być ujemne.")
    self._loyalty_points = value

  def add_loyalty_points(self, points: int):
    self.loyalty_points += points
    print(f"Klient {self.name} otrzymał {points} pkt. Razem: {self.loyalty_points} pkt.")

class Order:
  total_orders_count = 0
  total_revenue = 0.0

  def __init__(self, customer: Customer, items: list = []):
    self.customer = customer
    self.items = items
    self.status = "Nowe"
    
    Order.total_orders_count += 1
    self.order_id = Order.total_orders_count

  def add_item(self, item: MenuItem):
    self.items.append(item)

  def calculate_total(self) -> float:
    return sum(item.price for item in self.items)

  def __generate_bill(self):
    print(f"-----RACHUNEK DO ZAMÓWIENIA #{self.order_id}-----")
    print(f"Data: {datetime.now().strftime('%Y-%m-%d %H:%M:%S')}")
    print(f"Klient: {self.customer.name} {self.customer.surname}")
    
    for item in self.items:
      print(f" - {item.name} {item.price:.2f} zł")
    
    total = self.calculate_total()
    print(f"SUMA CAŁKOWITA: {total:.2f} zł")

  def finalize_order(self):
    self.__generate_bill()
    total = self.calculate_total()
    Order.total_revenue += total
    
    points = int(total // 10)
    self.customer.add_loyalty_points(points)
    self.status = "Opłacone"

def clear():
  os.system('cls' if os.name == 'nt' else 'clear')
    
def main():
  barista = Barista("Anna", "Kowalska", "F", 5000)
  chef = Chef("Jan", "Nowak", "M", 6500)

  menu = [
    Drink("Espresso", 7.0, "1 shot z ekspresu kolbowego", temp="hot", size="S"),
    Drink("Doppio", 9.0, "Podwójne espresso, 2 shoty z ekspresu kolbowego", temp="hot", size="S"),
    Drink("Americano M", 10.0, "Podwójne espresso z większą ilością wody", temp="hot", size="M"),
    Drink("Americano L", 12.0, "Podwójne espresso z większą ilością wody", temp="hot", size="L"),
    Drink("Cappuccino M", 13.0, "Espresso ze spienionym mlekiem", temp="hot", size="M"),
    Drink("Cappuccino L", 15.0, "Espresso ze spienionym mlekiem", temp="hot", size="L"),
    Drink("Flat White M", 14.0, "Podwójne espresso z delikatnie spienionym mlekiem", temp="hot", size="M"),
    Drink("Flat White L", 16.0, "Podwójne espresso z delikatnie spienionym mlekiem", temp="hot", size="L"),
    Drink("Herbata Czarna", 9.0, "Herbata liściasta Earl Grey", temp="hot", size="L"),
    Drink("Herbata Zimowa", 15.0, "Czarna herbata z pomarańczą, goździkami i miodem", temp="hot", size="L"),
    Drink("Ice Latte", 17.0, "Espresso, kostki lodu i zimne mleko", temp="cold", size="L"),
    Drink("Iced Caramel Macchiato", 18.0, "Kawa mrożona z syropem karmelowym", temp="cold", size="L"),
    Drink("Lemoniada", 13.0, "Gazowana woda z cytryną i świeżą miętą", temp="cold", size="L"),
    Drink("Sok pomarańczowy", 12.0, "Świeżo wyciskany sok z pomarańczy", temp="cold", size="L"),
    Food("Francuskie śniadanie", 11.0, "Maślany rogalik z dżemem truskawkowym", type="śniadanie", is_vegan=False, prep_time_min=1),
    Food("Kanapka z szynką", 14.0, "Ciabatta z żółtym serem, szynką parmeńską i pomidorem", type="śniadanie", is_vegan=False, prep_time_min=2),
    Food("Tosty z awokado", 22.0, "Dwa tosty, pasta z awokado", type="śniadanie", is_vegan=False, prep_time_min=5),
    Food("Jajecznica z boczkiem", 18.0, "Jajecznica z 3 jajek z cebulą i boczkiem, pieczywo", type="śniadanie", is_vegan=False, prep_time_min=6),
    Food("Pancakes", 21.0, "Naleśniki z syropem klonowym", type="śniadanie", is_vegan=False, prep_time_min=8),
    Food("Sernik z białą czekoladą", 17.0, "Kremowy sernik na kruchym spodzie", type="deser", is_vegan=False, prep_time_min=0),
    Food("Brownie", 15.0, "Mocno czekoladowe ciasto z orzechami włoskimi", type="deser", is_vegan=False, prep_time_min=0),
    Food("Szarlotka na gorąco", 16.0, "Z domowymi jabłkami i cynamonem, podawana na ciepło", type="deser", is_vegan=True, prep_time_min=2),
    Food("Wegańskie ciasto marchewkowe", 15.0, "Puszyste ciasto z kremem z orzechów nerkowca", type="deser", is_vegan=True, prep_time_min=0)
  ]

  clear()
  print("=" * 50)
  print("SYMULATOR KAWIARNI")
  print("=" * 50)

  print("\nProszę, załóż kartę stałego klienta:")
  name = ""
  while not name:
    name = input("Podaj imię: ").strip()
    if not name:
      print("Błąd: Podaj poprawne imię.")

  surname = ""
  while not surname:    
    surname = input("Podaj nazwisko: ").strip()
    if not surname:
      print("Błąd: Podaj poprawne nazwisko.")
      
  gender = ""
  while gender not in ["M", "F"]:
    gender = input("Podaj płeć (M/F): ").strip().upper()
    if gender not in ["M", "F"]:
      print("Błąd: Wybierz 'M' lub 'F'.")

  customer = Customer(name, surname, gender)
  print(f"\nKonto utworzone pomyślnie!\n")

  while True:
    print("-" * 30)
    print("MENU GŁÓWNE APLIKACJI")
    print("-" * 30)
    print("1. Przeglądaj menu kawiarni")
    print("2. Złóż nowe zamówienie")
    print("3. Moje konto i statystyki kawiarni")
    print("4. Wyjście z kawiarni")
    
    wybor = input("\nWybierz opcję (1-4): ").strip()

    match(wybor):
      case "1":
        clear()
        print("--- NASZE MENU ---")
        for idx, item in enumerate(menu, 1):
          print(f"{idx}. {item.display_info()}")

        input("\nWciśnij dowolny przycisk, aby wrócić do głównego menu: ")

      case "2":    
        clear()
        current_order = Order(customer, [])

        error = ""
        order = ""

        while True:
          print("--- SKŁADANIE ZAMÓWIENIA ---\n")
          if error:
            print(error + "\n")
          if order:
            print(f"Zamówienie: {order}\n")
          print("Wpisz numer pozycji z menu, aby dodać do zamówienia lub wciśnij Enter aby zakończyć.")
          for idx, item in enumerate(menu, 1):
            print(f"{idx}. {item.name} {item.price:.2f} zł")
          
          wybor_produktu = input("\nTwój wybór: ").strip()
          
          if wybor_produktu == "":
            break
          
          try:
            idx_produktu = int(wybor_produktu) - 1
            if 0 <= idx_produktu < len(menu):
              wybrany_produkt = menu[idx_produktu]
              current_order.add_item(wybrany_produkt)
              order += ", " if order else ""
              order += wybrany_produkt.name
              error = ""
            else:
              error = "Błąd: Brak wybranej pozycji w menu"
          except ValueError:
            error = "Błąd: Niepoprawny numer."

          clear()

        clear()
        if current_order.items:
          print("--- PODSUMOWANIE ZAMÓWIENIA ---\n")
          print(f"Wybrane pozycje z menu: " + order + ".\n")
          print(f"Dziękujemy za zamówienie. To będzie {current_order.calculate_total():.2f} zł")
          input("\nWciśnij dowolny przycik, aby zapłacić: ")

          clear()
          current_order.finalize_order()

          input("\nWciśnij dowolny przycik, aby zabrać rachunek: ")

          clear()
          print("Twoje zamówienie jest w trakcie realizacji...\n")
          for item in current_order.items:
            if isinstance(item, Drink):
              barista.assign_task(item)
            else:
              chef.assign_task(item)

          print("")
          barista.perform_duties()
          print("")
          chef.perform_duties()

        else:
            print("Anulowano: Zamówienie jest puste.")

        input("\nWciśnij dowolny przycisk, aby wrócić do głównego menu: ")

      case "3":
        clear()
        print("--- MOJE KONTO ---")
        print(customer.display_info())
        print(f"Zebrane punkty lojalnościowe: {customer.loyalty_points}")
        
        print("\n--- STATYSTYKI LOKALU ---")
        print(f"Całkowity obrót: {Order.total_revenue:.2f} zł")
        print(f"Zrealizowane zamówienia: {Order.total_orders_count}")
        print(f"Ilość zarejestrowanych klientów w systemie: {Customer.total_customers}")

        input("\nWciśnij dowolny przycisk, aby wrócić do głównego menu: ")

      case "4":
        print(f"\nDziękujemy za wizytę, {customer.name}! Do zobaczenia ponownie.")
        break
        
      case _:
        print("")

    clear()

if __name__ == "__main__":
  main()