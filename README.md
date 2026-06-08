# System Biblioteki Online

## Opis projektu

System Biblioteki Online to aplikacja webowa napisana w ASP.NET Core MVC, umożliwiająca zarządzanie biblioteką oraz wypożyczaniem książek. Użytkownicy mogą przeglądać katalog książek, wypożyczać i zwracać książki, natomiast administrator posiada dodatkowe funkcje zarządzania autorami, kategoriami, książkami oraz monitorowania wypożyczeń.

## Wykorzystane technologie

* ASP.NET Core MVC
* ASP.NET Core Identity
* Entity Framework Core
* Microsoft SQL Server
* Razor Views
* Bootstrap 5
* HTML5
* CSS3
* C#

## Wymagania

Przed uruchomieniem projektu należy zainstalować:

* .NET SDK 8.0 (lub wersję zgodną z projektem)
* SQL Server LocalDB lub Microsoft SQL Server
* Visual Studio 2022 z pakietem ASP.NET i web development

## Instrukcja uruchomienia

### 1. Sklonowanie repozytorium

```bash
git clone <adres_repozytorium>
```

### 2. Otworzenie projektu

Otwórz plik rozwiązania (.sln) w programie Visual Studio.

### 3. Konfiguracja bazy danych

Sprawdź połączenie z bazą danych w pliku:

```json
appsettings.json
```

Przykład:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=SystemBibliotekiOnline;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```

### 4. Utworzenie bazy danych

W konsoli Menedżera Pakietów wykonaj:

```powershell
Update-Database
```

lub z poziomu terminala:

```bash
dotnet ef database update
```

### 5. Uruchomienie aplikacji

W Visual Studio naciśnij:

```text
F5
```

lub uruchom z terminala:

```bash
dotnet run
```

### 6. Logowanie administratora

Podczas pierwszego uruchomienia tworzony jest domyślny administrator:

Email:

```text
admin@biblioteka.pl
```

Hasło:

```text
Admin123!
```

## Funkcjonalności

### Użytkownik

* Rejestracja i logowanie
* Przeglądanie katalogu książek
* Wyszukiwanie książek
* Filtrowanie po kategorii
* Wypożyczanie książek
* Zwrot książek
* Podgląd własnych wypożyczeń

### Administrator

* Zarządzanie autorami
* Zarządzanie kategoriami
* Zarządzanie książkami
* Dodawanie i usuwanie okładek książek
* Podgląd wszystkich wypożyczeń
* Panel administracyjny ze statystykami

## Autor

Projekt wykonany w ramach zajęć/programu studiów.
