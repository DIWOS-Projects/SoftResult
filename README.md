---

# 🎯 **SoftResult**

![logo.png](logo.png)

## 📋 **Информация о библиотеке**

- **Тип:**          Библиотека для API  
- **Фреймворк:**    .NET 8  
- **Зависимости:**  _Отсутствуют (легковесная библиотека)_  

---

## ⚙️ **Назначение библиотеки**  

SoftResult — это универсальная библиотека для формирования стандартных API-ответов в ASP.NET Core. Основные задачи:  
- 🔄 Формирование ответов в формате `IResult`, унаследованного от `IActionResult`.  
- 📦 Упрощение обработки HTTP-ответов в контроллерах, сервисах и медиаторе.  
- 🔧 Унификация API-ответов для обеспечения простоты и удобства взаимодействия.  

---

## 🚀 **Шаги для запуска**

1. 🛠️ Убедитесь, что у вас установлена среда разработки для **.NET 8**.  
2. Выполните следующие команды для сборки и публикации пакета:  

### **🔧 Сборка пакета**
```shell
dotnet build -c Release
```

### **📦 Создание локального источника пакетов**
```shell
mkdir /LocalNugget
```

### **📤 Публикация пакета в локальный источник**
```shell
dotnet nuget push .\soft-result\bin\Release\soft-result.1.4.8.1.nupkg --source C:\LocalNugget\
```

### **🌐 Публикация пакета в удаленный источник**
```shell
dotnet nuget push .\soft-result\bin\Release\soft-result.1.4.8.1.nupkg --api-key YOUR_API_KEY --source https://packages.salyk.kg/nuget
```

### **➕ Добавление локального источника в NuGet**
```shell
dotnet nuget add source C:\LocalNugget\ --name LocalNuggetSource
```

### **🔍 Проверка локальных источников**
```shell
dotnet nuget list source
```

### **📥 Добавление пакета в проект**
```shell
dotnet add package soft-result --source LocalNuggetSource
```

---

## 🌟 **Базовые сценарии использования**

1. **🔄 Универсальные API-ответы:**  
   Все ответы оборачиваются в `IResult`, позволяя легко обработать:  
   - 📝 Сообщение (`Message`),  
   - 📦 Полезную нагрузку (`Value`),  
   - ❗ Ошибки (`Errors`),  
   - 📋 HTTP-статус (`Status`).  

2. **📡 Работа с HTTP-запросами:**  
   Используется для формирования ответов внутри компонентов системы, а не только в контроллерах.  

3. **⚙️ Интеграция с контроллерами:**  
   Универсальный механизм возврата ответов через `IResult:IActionResult`.  

4. **📜 CQRS и MediatR:**  
   Формирование универсальных ответов в командах и запросах, которые возвращают `IResult`, упрощая работу с Mediator.  

5. **🔧 Поддержка сервисов:**  
   Методы сервисов возвращают `IResult`, обеспечивая гибкость обработки статуса выполнения и сообщений.  

---

## 💻 **Пример использования**

### **📜 Query, возвращающий `IResult`**
```csharp
using MediatR;
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities.Dictionary;
using WebApi.SoftResult.Response;
using IResult = WebApi.SoftResult.Interfaces.IResult;

namespace WebApi.Application.Mediatr.Dictionary.SystemType.Queries;

public record GetSystemTypesQuery(
    bool? IsActive = true
) : IRequest<IResult>;

public class GetSystemTypesQueryHandler(
    IApplicationDbContext context
) : IRequestHandler<GetSystemTypesQuery, IResult>
{
    public async Task<IResult> Handle(GetSystemTypesQuery query, CancellationToken cancellationToken)
    {
        var result = context.SystemTypesDictionary.ToList();
        if (query.IsActive is not null)
        {
            result = result.Where(x => x.IsActive == query.IsActive).ToList();
        }

        return result.Any()
            ? Result<List<SystemTypesDictionaryEntity>>.Ok("✅ Список систем получен", result)
            : Result<List<SystemTypesDictionaryEntity>>.NotFound("❌ Не удалось найти список систем");
    }
}
```

### **⚙️ Метод контроллера, возвращающий `IActionResult`**
```csharp
[HttpGet("get-system-types")]
[Auth([Policies.ForAllPositions])]
public async Task<IActionResult> GetSystemTypes([FromQuery] GetSystemTypesQuery query, CancellationToken cancellationToken)
{
    var result = await Mediator.Send(query, cancellationToken);
    return result;
}
```

**💡 Обратите внимание:**  
Контроллер напрямую возвращает результат из `Query`, так как `IResult` унаследован от `IActionResult`.  

---

## 📦 **Версионирование пакета**

- **soft-result: 1.4.8.1**  
  - **1:** Глобальная версия.  
  - **4:** Последняя цифра года — 2024.  
  - **8:** Версия .NET.  
  - **1:** Инкрементальная версия (увеличивается с каждым релизом).  

---

## 🎯 **Почему SoftResult?**

- 📋 **Единый формат ответов:** Простота обработки на клиентской стороне.  
- 🚀 **Удобство интеграции:** Подходит для работы с MediatR, контроллерами и сервисами.  
- 🔄 **Гибкость:** Легкая адаптация к требованиям проекта.  
- 📜 **Соответствие стандартам:** Поддержка RFC 7807 для описания ошибок.  

---

## 🌐 **Пакет в NuGet**
![nuggetScreenShot.png](nuggetScreenShot.png)

---

Попробуйте **SoftResult** и сделайте ваш API стандартизированным, простым и удобным для использования! 🎉
