**Architecture Notes**

- The solution uses:  .NET Core 2.1 and sqlite database
- The code is separated into layers: Controller, Service, Models (database).

---

**Design Notes**

- A work week belongs to a month if the week's Monday falls in the month.

---

**Testing Notes**

- To assist with testing, the database is seeded with data (in file: Models\TimeTrackingContext.cs)
