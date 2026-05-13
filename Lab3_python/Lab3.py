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
      print(f"Obsługa wydaje {self.type}: {diet}{self.name}.")
      
def main():
  espresso = Drink("Espresso", 5, "1 shot z ekspresu kolbowego", temp = "hot", size = "S")
  americano = Drink("Americano", 8, "2 shoty z ekspresu kolbowego, więcej wody", temp = "hot", size = "M")
  ice_latte = Drink("Ice Latte", 15, "1 shot z ekspresu kolbowego, 3 kostki lodu, spienione mleko", temp = "cold", size = "M")

  ham_sandwich = Food("Kanapka z szynką", 14, "Bułka z serem, szynką, sałatą i pomidorem", type="śniadanie", is_vegan = False, prep_time_min = 3)
  scrambled_eggs = Food("Jajecznica z boczkiem", 18, "3 jajka, boczek i pieczywo", type="śniadanie", is_vegan = False, prep_time_min = 7)

  apple_pie = Food("Brownie", 15, "Czekoladowe ciasto", type="deser", is_vegan = False, prep_time_min = 0)

  cafe_menu = [espresso, americano, ice_latte, ham_sandwich, scrambled_eggs, apple_pie]
  
  for item in cafe_menu:
    print(item.display_info())
    item.prepare()

if __name__ == "__main__":
  main()