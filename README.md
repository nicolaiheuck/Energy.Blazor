![Build succeeded][build-shield]
![Test passing][test-shield]
[![Issues][issues-shield]][issues-url]
[![Issues][closed-shield]][issues-url]
[![Contributors][contributors-shield]][contributors-url]
[![Forks][forks-shield]][forks-url]
[![License][license-shield]][license-url]

# EGON
#### Energy Grid Optimization Network
<!-- TABLE OF CONTENTS -->
<details>
  <summary>Table of Contents</summary>

- [Case](#case)
- [Requirements](#requirements)
- [Architecture diagram](#architecture-diagram)
- [Roadmap](#roadmap)
- [Summary and rundown](#summary-and-rundown)
- [Getting started](#getting-started)
- [Libraries](#libraries)
- [Database Diagram](#database-diagram)
- [Flowcharts](#flowcharts)
- [License](#license)
- [Contact](#contact)
</details>

# Case
Inspireret af udenlandske eksempler ønsker IT-Center Syd at udvikle og implementere en række nye IT-services:
> Info ved lokaler: Display ved alle lokaler, der angiver aktivitet, periode og lærernavn.

> Energi- og Ressourceovervågning: En platform, der giver mulighed for overvågning og styring af skolernes energiforbrug og ressourceanvendelse. Dette kan inkludere intelligente målere og sensorer, der rapporterer forbrug i realtid og identificerer områder med potentiale for besparelser.
<p align="right">(<a href="#top">back to top</a>)</p>

# Requirements
- [X] Infodisplay ved alle lokaler
  - [X] Viser aktuel fag
  - [X] Viser periode for lokalet
  - [X] Viser lokale nummer/navn
  - [ ] Viser aktuel lærer
- [X] Energi- og ressourceovervågning
  - [x] Overvågning af energiforbrug i lokale/område
  - [x] Overvågning af temperatur og luftfugtighed
  - [x] Styring af strøm
  - [x] Styring af temperatur efter aktivitet i lokalet
  - [x] Alarmer for afvigelser
  - [x] Grafer/rapport over energiforbrug samt klima
<p align="right">(<a href="#top">back to top</a>)</p>

# Architecture diagram
![architecture diagram](/DOCS/EGON-Architecture-Diagram.drawio.png)
<p align="right">(<a href="#top">back to top</a>)</p>

# Roadmap
- [x] Gemme data i et overskueligt format
- [x] Gemme data i et overskueligt format
- [x] Hente data om lokaler fra ekstern API (IST Uddata)
  - [X] Vise aktuel fag i lokale
  - [x] Vise aktuel periode lokale er booket
  - [ ] Vise aktuel lærer
    * Dette var ikke muligt grundet begrænsninger ved ekstern API
<p align="right">(<a href="#top">back to top</a>)</p>

#  Summary and rundown
EGON er lavet som et proof of concept i to dele. IoT samt en kombineret back- og frontend (denne), hvor besøgende kan se info om lokalet, samt hvor ansatte i serviceafdelingen kan
se hvordan "status" er for et lokale.

EGON kan samtidig styre både temperatur, samt lys/strøm i lokalerne alt efter om et lokale står tomt eller er i brug.

Serviceafd. kan ændre et givent setpunkt for temperatur i et lokale efter ønske. 
Er et lokale booket vil displayet selv opdatere med relevant info (fag, lokale, periode, studie linie)

> Nicolai er i tvivl om hvor "Sibiren" er, på hans skema står der kun 51.244. 
> Nicolai kigger på displays rundt ved lokalerne og kan se både nummer, men også navn på lokalet. 
> 
> Nicolai finder `51.244 - Sibirien` og kan samtidig se at faget der kører er `Embedded II` på linien `Datatekniker Prog.`
> samt perioden hvor faget kører. Det fag stemmer overens med hans skema, og Nicolai har derved fundet det korrekte lokale.

> Serviceafdelingen vil gerne se om nogle lokaler bruger meget strøm i weekenden. 
> 
> De slår op på webinterfacet, og kan se at den ene fløj bruger meget strøm i weekenden - de vælger at "drill down" i systemet, og kan se
> at et bestemt lokale bruger meget strøm. De kan så undersøge om det er relevant, eller om strømmen skal slukkes automatisk i weekenden.
<p align="right">(<a href="#top">back to top</a>)</p>

# Getting started
EGON er bygget op i to dele - en IoT del samt en kombineret back- og frontend.

EGON backend styrer kald til ekstern API (IST Uddata) hvor den henter information om lokaler automatisk, baseret på lokale "nummer".
Den består af en MQTT klient der er koblet på den samme MQTT-broker som EGON IoT, og abonnerer på info for en given skole (i vores eksempel, EUC Syd)
For at komme igang med EGON kræver det fire steps:
1. Opsæt Azure CLI
2. Database opsætning
   1. Installer database jf. SQL script der findes under `/SETUP`
   2. Opret bruger og giv de korrekte privilegier
3. MQTT opsætning
   1. Brokerens adresse angives i `appsettings.json` under feltet "BaseUrl"
   ```json
    "Mqtt": {
            "BaseUrl": "10.131.15.57",
            "EnergyOnlineState": "energy/onlinestate"
        },
    ```
4. Front-/backend opsætning
   1. Kontroller forbindelse til databasen
   2. Inden opstart skal API forbindelse til IST opsættes
<p align="right">(<a href="#top">back to top</a>)</p>

# Libraries
## Energy.Blazor
| Name                                              | Version |
| :------------------------------------------------ | :------ |
| Azure.Extensions.AspNetCore.Configuration.Secrets | 1.0.0   |
| Azure.Identity                                    | 1.6.0   |
| Blazored.Modal                                    | 7.1.0   |
| Blazored.Toast                                    | 4.1.0   |
| Microsoft.AspNetCore.Authentication.OpenIdConnect | 7.0.11  |
| Microsoft.Identity.Web                            | 2.13.4  |
| Microsoft.Identity.Web.MicrosoftGraph             | 2.13.4  |
| Microsoft.Identity.Web.UI                         | 2.13.4  |
| Radzen.Blazor                                     | 4.15.14 |
| Seq.Extensions.Logging                            | 6.1.0   |
| Toolbelt.Blazor.HotKeys2                          | 3.0.0   |
| Toolbelt.Blazor.I18nText                          | 12.0.2  |

## Energy.Repositories
| Name                             | Version   |
| :------------------------------- | :-------- |
| Microsoft.Extensions.Options     | 7.0.0     |
| MQTTnet                          | 4.3.1.873 |
| MQTTnet.Extensions.ManagedClient | 4.3.1.873 |
| Microsoft.EntityFrameworkCore    | 7.0.11    |
| Pomelo.EntityFrameworkCore.MySql | 7.0.0     |

## Energy.Infrastructure
| Name                             | Version   |
| :------------------------------- | :-------- |
| Microsoft.Extensions.Options     | 7.0.1     |
| MQTTnet                          | 4.3.1.873 |
| MQTTnet.Extensions.ManagedClient | 4.3.1.873 |

## Energy.Services
| Name                                      | Version |
| :---------------------------------------- | :------ |
| Microsoft.Extensions.Hosting.Abstractions | 7.0.0   |
| Microsoft.Extensions.Options              | 7.0.0   |
<p align="right">(<a href="#top">back to top</a>)</p>

# Database Diagram
![DBDiagram.png](DOCS%2FDBDiagram.png)
<p align="right">(<a href="#top">back to top</a>)</p>

# Flowcharts
![alarm flowchart](/Docs/Alarm_Flowchart.png)
`/Docs/Alarm_Flowchart.png`
<p align="right">(<a href="#top">back to top</a>)</p>

# License
* Frontend: MIT
* Backend: MIT
<p align="right">(<a href="#top">back to top</a>)</p>

# Contact
- Peter Hymøller - peterhym21@gmail.com
  - [![Twitter][twitter-shield-ptr]][twitter-url-ptr]
- Nicolai Heuck - nicolaiheuck@gmail.com
- Jan Andreasen - jan@tved.it
  - [![Twitter][twitter-shield]][twitter-url]

Project Link: [https://github.com/nicolaiheuck/Energy.Blazor](https://github.com/nicolaiheuck/Energy.Blazor)
<p align="right">(<a href="#top">back to top</a>)</p>

<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->
[build-shield]: https://img.shields.io/badge/Build-passed-brightgreen.svg
[test-shield]: https://img.shields.io/badge/Tests-passed-brightgreen.svg
[contributors-shield]: https://img.shields.io/github/contributors/nicolaiheuck/Energy.Blazor.svg?style=badge
[contributors-url]: https://github.com/nicolaiheuck/Energy.Blazor/graphs/contributors
[forks-shield]: https://img.shields.io/github/forks/nicolaiheuck/Energy.Blazor.svg?style=badge
[forks-url]: https://github.com/nicolaiheuck/Energy.Blazor/network/members
[issues-shield]: https://img.shields.io/github/issues/nicolaiheuck/Energy.Blazor.svg?style=badge
[closed-shield]: https://img.shields.io/github/issues-closed/nicolaiheuck/Energy.Blazor?label=%20
[issues-url]: https://github.com/nicolaiheuck/Energy.Blazor/issues
[license-shield]: https://img.shields.io/github/license/nicolaiheuck/Energy.Blazor.svg?style=badge
[license-url]: https://github.com/nicolaiheuck/Energy.Blazor/blob/master/LICENSE
[twitter-shield]: https://img.shields.io/twitter/follow/andreasen_jan?style=social
[twitter-url]: https://twitter.com/andreasen_jan
[twitter-shield-ptr]: https://img.shields.io/twitter/follow/peter_hym?style=social
[twitter-url-ptr]: https://twitter.com/peter_hym
