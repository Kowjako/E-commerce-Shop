## 🎨 KOWJAKO-COMMERCE

Aplikacja webowa przedstawiająca e-commerce sklep. W danym projekcie została zaimplementowana podstawowa rzecz każdego e-commerce sklepu: przykładowo lista produktów, zamówienia, system płatności, koszyk itd.
Projekt napisany przy użyciu kombo .NET 7 / Angular 15.

## Użyte narzędzia
- **Angular 15, HTML, SCSS, RxJS, Bootstrap, Typescript** - frontend  
- **.NET 7, ASP.NET Core Web API, ASP.NET Core Identity, Entity Framework Core, SQLite, Docker, Redis** - backend  

Do procesowania płatności został użyty serwis [Stripe](https://stripe.com/en-pl)  

## Architektura aplikacji
![Architecture](https://user-images.githubusercontent.com/19534189/220373316-a80b234b-dba3-42c0-8b96-06810d824f71.png)

## Zaimplementowane rzeczy  
- Autentykacja na podstawie JWT-tokenów
- Autoryzacja w powiązaniu z ASP.NET Core Identity (osobny DbContext)
- Tworzenie koszyków - są przechowywane w cache'u Redis
- Sortowanie, filtracja, paginacja produktów.
- Cache po stronie API - znów użyty Redis, kluczem cach'u jest queryString zapytania do pobrania produktów
- Generyczne repozytorium oraz Specification Pattern.
- Procesowanie płatności za pomocą serwisu Stripe, użycie stripe-js do implementacji komponentów karty bankowej.
- Cachowanie po stronie serwisów Angular'a
- Angular Route Guards do przeciwdziałania przypadkowym akcjom
- Angular Interceptors do wysłania zapytań z nagłówkiem autentykacji  
- Angular Lazy-loading moduły

I wiele innych :)

## Jak zainstalować
1️⃣ Pobrać kod źrodłowy backend + frontend  
2️⃣ Uruchomić API (automatycznie zrobi migrację bazy SQLite) oraz uruchomić kontenery mając zainstalowany Docker Desktop. W projekcie jest plik docker-compose.
Żeby wykreować strukturę kontenera wystarczy wydać polecenie: **docker-compose up -d**  
Kontener jest potrzebny do hostowania Redis'a oraz przedstawia GUI do podglądania dostępny na porcie: 8081. 
Również do przyjmowania płatności należy założyć konto na Stripe i podmienić klucze na swoje w **appsettings.development.json**  
3️⃣ Uruchomić stronę kliencką: ng serve, uwaga: w tym projekcie jest uruchamiany na podstawie SSL. W projekcie znajdują się przykładowe certyfikaty, aczkolwiek
gdy ktoś ma zamiar użyć swoje to można skorzystać z narzędzia [MKCert](https://github.com/FiloSottile/mkcert) 

## Screenshoty
![Screenshot_1](https://user-images.githubusercontent.com/19534189/220376599-ee86b6dd-4d7f-4109-b861-c1092b91ce95.png)
![Screenshot_4](https://user-images.githubusercontent.com/19534189/220376606-de279814-7917-4f9c-89ac-ef61a347d33d.png)
![Screenshot_3](https://user-images.githubusercontent.com/19534189/220376609-a07adea9-53b1-432a-8d21-80a21988260a.png)
![Screenshot_5](https://user-images.githubusercontent.com/19534189/220376611-7731aa39-8c16-48ec-b701-13e3fa03aaa2.png)
![Screenshot_9](https://user-images.githubusercontent.com/19534189/220376613-17d6db78-bf20-4b79-b342-f1b809610e35.png)
![Screenshot_6](https://user-images.githubusercontent.com/19534189/220376617-d11f6e43-10d7-4cbb-82bb-1e97db8c40fa.png)
![Screenshot_8](https://user-images.githubusercontent.com/19534189/220376621-1336ee00-8e95-4074-ad13-438c4aa5a7e3.png)
![Screenshot_7](https://user-images.githubusercontent.com/19534189/220376622-e2e160bf-155c-4375-a933-7c7618ff38d8.png)
![Screenshot_10](https://user-images.githubusercontent.com/19534189/220376628-03c65923-41cc-45c1-b4d0-bc9bf23c110f.png)
![Screenshot_12](https://user-images.githubusercontent.com/19534189/220376632-f7e4736e-293b-4a41-84a6-a2db89d84575.png)
![Screenshot_11](https://user-images.githubusercontent.com/19534189/220376636-a18adf24-c715-4f42-8e89-4bbb4fb6c086.png)
![Screenshot_13](https://user-images.githubusercontent.com/19534189/220376639-c923d88a-f278-4a8c-9bcd-77277185def7.png)
