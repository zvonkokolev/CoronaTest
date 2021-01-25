# CoronaTest
[![Z|Kolev](https://github.com/zvonkokolev/CoronaTest/blob/master/crts.png)](https://github.com/zvonkokolev/CoronaTest/blob/master/crts.png)
## Aufgabenstellung
Setzen eine Web Seite mithilfe von Razor Pages mit folgenden Anforderungen:
> - Bereitstellung einer Web-Seite auf welcher sich Personen (Participant) registrieren können.
> - Nach der Registrierung kann sich die Person zu einer Testung (Examination) anmelden/einen Termin für einen freien Slot reservieren.>
> - Reservierungsbestätigung per SMS-Versand:
> o Identifikation-Nummer
> o Ausgewählter Zeitpunkt der Testung
> o Daten zum TestCenter
> - Eine Person kann eine angemeldete Testung auch adaptieren. Es kann ein anderes Test-Center ausgewählt werden bzw. ein anderer Termin gewählt werden.
> - Eine Person kann auf der Web-Seite ihre Stammdaten (Adresse, etc.) selbst verwalten.
> - Stellen Sie in der Anwendung Testdaten bereit (Stichwort „ImportConsole“).
> - Implementieren Sie die notwendigen Validierungen auf den Formularen

## Umsetzung (Teil 1)
26.01.2021:
  - Entitäten Modelstruktur samt Schnittstellen
  - Import console
  - .csv Dateien fü Teilnehmer und Testzentren erstellt
  - MsSql Datenbank erstellt und migriert nach "code first" Prinzip
  - "Unit of work" und "Repositories" mit CRUD Methoden
  - SMS Versand über Twilio.com integriert und auf Funktion getestet

#### Todos
 - Razor Seiten für WEB Applikation
 - Login Maske
