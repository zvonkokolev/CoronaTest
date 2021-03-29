[![Z.|Kolev](https://github.com/zvonkokolev/CoronaTest/blob/master/crts.png)](https://github.com/zvonkokolev/CoronaTest/blob/master/crts.png)
## Aufgabenstellung
Setzen eine Web Seite mithilfe von Razor Pages mit folgenden Anforderungen:
> - Bereitstellung einer Web-Seite auf welcher sich Personen (Participant) registrieren können.
> - Nach der Registrierung kann sich die Person zu einer Testung (Examination) anmelden/einen Termin für einen freien Slot reservieren.>
> - Reservierungsbestätigung per SMS-Versand:
>   - Identifikation-Nummer
>   - Ausgewählter Zeitpunkt der Testung
>   - Daten zum TestCenter
> - Eine Person kann eine angemeldete Testung auch adaptieren. Es kann ein anderes Test-Center ausgewählt werden bzw. ein anderer Termin gewählt werden.
> - Eine Person kann auf der Web-Seite ihre Stammdaten (Adresse, etc.) selbst verwalten.
> - Implementieren Sie die notwendigen Validierungen auf den Formularen

## Umsetzung (Teil 1)
26.01.2021:
  - Entitäten Modelstruktur samt Schnittstellen
  - Import console
  - .csv Dateien fü Teilnehmer und Testzentren erstellt
  - MsSql Datenbank erstellt und migriert nach "code first" Prinzip
  - "Unit of work" und "Repositories" mit CRUD Methoden
  - SMS Versand über Twilio.com integriert und auf Funktion getestet
  - Razor Seiten für WEB Applikation
  - Login Maske
  - Schönheitsfehler ausbessern
 
#### Todos
 - Test auf richtige Funktion

## Umsetzung (Teil 2)
02.02.2021:
  - WPF-Seiten und MVVM-Modelle erstellt 
  - Haupt-Fenster erstellt
  - WindowController sammt Schnittstelle erstellt
  - NotifyPropertyChanged und RelayCommand übernommen
  - nuGet Paket "MahApps.Metro" Version="2.4.3" installiert
  - Zusätzliche Methoden zum Datum Filtern

## Umsetzung (Teil 2 - Fortsetzung)
16.02.2021:
  - Kommunikation per SMS-Fenster erstellt
  - nuGet Paket "StringRandomizer" installiert
  - Methoden zum Identifikationsnummer erstellen
  
#### Todos
 - Datumsformat unterschied Datenbank--->WPF Form
 - Test auf richtige Funktion
 
## Umsetzung (Teil 3)
23.02.2021:
 - API Projekt erstellt
 - nuGet Pakete "Swashbuckle.AspNetCore" Version="6.0.7" installiert
 - nuGet Pakete "NSwag.AspNetCore" Version="13.10.6" installiert
 - Startup.cs Datei konfiguriert
 - Projekteigenschaften: Einstellungen angepasst
 
 ## Umsetzung (Teil 4)
05.03.2021:
 - API Projekt zur autentifizierung erstellt
 - nuGet Pakete "Microsoft.AspNetCore.Authentication.JwtBearer" Version="5.0.3" installiert
 - passwort hashen tool verwendet
 - Startup.cs Datei geändert und angepasst
 - Projekteigenschaften: Einstellungen angepasst
