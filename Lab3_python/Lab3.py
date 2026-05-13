from abc import ABC, abstractmethod
import time

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
      print(f"Barista {self.name} nie ma aktualnie żadnych zamówień.")
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
      print(f"Kucharz {self.name} nie ma aktualnie żadnych zamówień.")
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
    
def main():
  espresso = Drink("Espresso", 5, "1 shot z ekspresu kolbowego", temp = "hot", size = "S")
  americano = Drink("Americano", 8, "2 shoty z ekspresu kolbowego, więcej wody", temp = "hot", size = "M")
  ice_latte = Drink("Ice Latte", 15, "1 shot z ekspresu kolbowego, 3 kostki lodu, spienione mleko", temp = "cold", size = "M")

  ham_sandwich = Food("Kanapka z szynką", 14, "Bułka z serem, szynką, sałatą i pomidorem", type="śniadanie", is_vegan = False, prep_time_min = 3)
  scrambled_eggs = Food("Jajecznica z boczkiem", 18, "3 jajka, boczek i pieczywo", type="śniadanie", is_vegan = False, prep_time_min = 7)

  apple_pie = Food("Brownie", 15, "Czekoladowe ciasto", type="deser", is_vegan = False, prep_time_min = 0)

  barista = Barista("Anna", "Kowalska", "F", 5000)
  chef = Chef("Jan", "Nowak", "M", 6500)
  customer_1 = Customer("Robert", "Szczęsny", "M")
  customer_2 = Customer("Jadwiga", "Lewandowska", "F", 100)

  print("---Delegacja zadań---")
  barista.assign_task(espresso)
  barista.assign_task(americano)
  chef.assign_task(ham_sandwich)
  chef.assign_task(scrambled_eggs)

  print("---Przygotowanie zamówienia---")
  barista.perform_duties()
  chef.perform_duties()

if __name__ == "__main__":
  main()