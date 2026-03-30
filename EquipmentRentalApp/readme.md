# How to install:

```
git clone https://github.com/pabloes949/APBD-Cw1-s29836.git
cd APBD-Cw1-s29836/EquipmentRentalApp
dotnet build
dotnet run
```

# Equipment Rental App

## Project Overview

**Equipment Rental App** is a console-based application designed to manage equipment rentals, clients, and rental history.

The system allows:

- registering clients and equipment
- renting and returning devices
- tracking delayed returns and payments
- browsing rental history

---

Design Decisions

The project structure was designed to:

- **separate UI from business logic**
- **keep classes small and focused**
- **improve readability and maintainability**
- **allow future extensions without major refactoring**

The application is divided into three main logical areas:

- **UI layer** — `TerminalHandler`
- **Business logic layer** — `RentalHandler`, `ClientHandler`
- **Domain layer** — `Client`, `Equipment`, `Rental`

This separation improves cohesion and reduces coupling between components.

# Project Structure

The project is divided into the following main components (and additional ones):

| Component             | Responsibility |
|-----------------------|----------------|
| **TerminalHandler**   | User interface and command parsing |
| **RentalHandler**     | Rental logic and equipment management |
| **ClientHandler**     | Client management and rental history |
| **Equipment** | Equipment domain model |
| **Client**   | Client domain model |


---

# Cohesion and Class Responsibilities

Each class has a **single responsibility**, which improves cohesion.

---

## TerminalHandler

**Responsibility:**

- parsing user commands
- interacting with console
- delegating logic to handlers

---

## RentalHandler

**Responsibility:**

- managing rentals
- tracking rental history
- equipment availability
- payment handling

---

## ClientHandler

**Responsibility:**

- registering clients
- client lookup
- tracking unpaid rentals
- client rental history

---

## Domain Classes

Domain classes represent business entities.

### Equipment hierarchy
`Equipment` abstract class
- ComputerHardware
- MultimediaDevice
- LabTool

### Client hierarchy
`Client` abstract class
- Student
- Employee
- Guest

# Coupling Examples

The project contains several examples of **low coupling** between components.

## 1. UI separated from business logic

`TerminalHandler` is responsible only for user interaction, while business logic is delegated to handler classes:

```csharp
RentalHandler.RentEquipment(...)
ClientHandler.RegisterNewClient(...)
```
## 2. Callback usage

The project uses callbacks to reduce dependencies between classes:
```csharp
public static void GetEquipmentList(Action<Equipment> callback){...}
public static int GetClientRentals(Client client, Action<string> callback){...}
```

Handlers do not know how the data will be used.
The calling layer decides whether the data should be displayed, logged, or processed further.

## 3. Object passing instead of identifiers

Objects are passed through methods instead of primitive identifiers:
```csharp
public static void RegisterNewRental(Equipment equipment, Client client, Rental rental){...}
public static Rental? HasClientUnpaidAsset(Client client, Equipment equipment){...}
```

# Scenarios

#### 1. Add equipment

Type `equipment-add` in the console and the program will guide you through the equipment registration form.

```bash
equipment-add                                                                   
Choose equipment type - type number:                                            
1 : ComputerHardware                                                            
2 : Multimedia                                                                  
3 : LabTools                                                                    
4 - [exit: give up adding new equipment]                                        
1                                                                               
Give a producer (optional) or type exit                                         
Lenovo                                                                          
Give a model or type exit                                                       
IdeaPad Slim 5                                                                  
Give serial number (optional) or type exit                                      
S4363456                                                                        
Choose state of equipment - type number:                                        
1 : Operable                                                                    
2 : ServiceNeed                                                                 
3 : RepairNeed                                                                  
4 - [exit: abort adding new equipment]                                          
1quipment e89c0 registered successfully.
Choose type of equipment - type number:
1 : Laptop
2 : Desktop
3 : Monitor
4 : Keyboard
5 : Mouse
6 : DockingStation
7 : ExternalDrive
8 : Tablet
9 : Webcam
10 : Headset
11 - [exit: abort adding new equipment]
1
Has equipment a charger - type number:
1 : yes
2 : no
3 - [exit: abort adding new equipment]
1
Is equipment portable - type number:
1 : yes
2 : no
3 - [exit: abort adding new equipment]
2
Equipment f7c3c registered successfully.
```

#### 2. Add client
Type `client-add` in the console and the program will guide you through the client registration form.

```bash
client-add
Choose user type - type number:
1 : Student
2 : Employee
3 : Guest
4 - [exit: give up adding new user]
1
Give a name or type exit
Julia
Give a surname or type exit
Nowak
Give an email address (optional) or type exit
nowak321@gmail.com
Give index number or type exit
s43535
Choose field of study - type number:
1 : ComputerScience
2 : CognitiveScience
3 : InformationManagement
4 : GraphicDesign
5 : MultimediaArts
6 : InteriorArchitecture
7 : JapaneseCulture
8 - [exit: abort adding new student]
1
Choose mode of study - type number:
1 : FullTime
2 : PartTime
3 - [exit: abort adding new student]
1
Type start year of studies or type exit
2022
Is student active - type number:
1 : yes
2 : no
3 - [exit: abort adding new student]
1
Client s43535 registered successfully.
```

# Example Equipment Rental Scenario

This scenario demonstrates the following functionalities:

1. Displaying all clients
2. Displaying all equipment
3. Renting equipment
4. Attempting to rent equipment that is already rented
5. Command validation examples (e.g. non-existing client, command without arguments)
6. Attempting to rent equipment by a client with outstanding payments
7. Returning equipment
8. Accepting payment for delayed return
9. Renting equipment again after settling outstanding payments

```bash
client-list                                                                     
[Student] Index number: s28473; Name: Anna; Surname: Jońska; Subject: ComputerScience; Mode: FullTime; Active: True
[Student] Index number: s48291; Name: Anna; Surname: Kowalczyk; Subject: CognitiveScience; Mode: FullTime; Active: False
[Student] Index number: s17364; Name: Michał; Surname: Nowak; Subject: ComputerScience; Mode: FullTime; Active: True
[...]
equipment-list                                                                  
[ComputerHardware] Id: 6a569; Type: Laptop; Producer: Lenovo; Model: ThinkPad E14; Serial: LN-E14-7K92X41; Status: Operable, Portable: True; Has charger: True
[ComputerHardware] Id: e76c6; Type: Desktop; Producer: Dell; Model: OptiPlex 7010; Serial: DL-7010-4M83Q29; Status: ServiceNeed, Portable: False; Has charger: True
[ComputerHardware] Id: b78fd; Type: Monitor; Producer: DL-7010-4M83Q29; Model: UltraWide 29WN600; Serial: LG-29WN-8P17T63; Status: Operable, Portable: False; Has charger: True
[...]
rent s28473 6a569 2026-02-10 2026-02-15
Client s28473 rented 6a569 equipment successfully.
rent s62754 6a569 2026-03-10 2026-03-15 
WARN: The equipment 6a569 is already rented for s62754 client.
rent s123 4d6ad 2026-04-15
WARN: The client with s123 id does not exist.
rent s28473 6a569 2026-02-10 2026-02-15
WARN: The equipment 6a569 is already rented for s28473 client.
rent s28473 6693e 2026-04-15
WARN: The client with s28473 id has outstanding payments for s28473 days and cannot rent any more equipment.
payment-accept
WARN: The payment-accept command expects client-id equipment-id arguments.
payment-accept s28473 6a569
WARN: The payment for 6a569 can be accepted only after a return.
return 
WARN: The command expects equipment-id argument.
return 6a569
Equipment 6a569 returned successfully.
payment-accept s28473 6a569
The payment accepted!
rent s28473 6693e 2026-04-15
Client s28473 rented 6693e equipment successfully.
```