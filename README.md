![logo.png](logo.png)

# soft-result

## Информация о приложении

- Type:           API
- Framework:      .NET 8
- Dependencies:   

## Необходимые шаги для запуска приложения

- Среда разработки для .NET 8

## Назначение приложения

Проект библиотеки Nugget, которая предназначена для формирования универсальных ответов
от API. Возвращает IResult, который наследуется от IActionResult для работы с HTTP запросами.

## Публикация, сборка, использование пакета

Сборка пакета
```shell
dotnet build -c Release
```

Создаем локальную директорию пакетов (Если нет)
```shell
mkdir /LocalNugget
```

Публикация пакета в локальный источник
```shell
dotnet nuget push .\soft-result\bin\Release\soft-result.1.4.8.1.nupkg --source C:\LocalNugget\
```

Публикация пакета в удаленный источник 
```shell
dotnet nuget push .\soft-result\bin\Release\soft-result.1.4.8.1.nupkg --api-key YOUR_API_KEY --source https://packages.salyk.kg/nuget
```

Добавление локального источника в менеджер пакетов Nugget (Если нет)
```shell
dotnet nuget add source C:\LocalNugget\ --name LocalNuggetSource
```

Просмотр локальных источников
```shell
dotnet nuget list source
```

Добавление пакета в проект
```shell
dotnet add package soft-result --source LocalNuggetSource
```

Пакет в менеджере Nugget
![nuggetScreenShot.png](nuggetScreenShot.png)

## Версии пакетов Nugget

- soft-result: 1.4.8.1
- 1 - Глобальная версия
- 4 - Последняя цифра года - 2024
- 8 - Версия .NET
- 1 - Инкрементированная версия, добавляется +1 с каждым релизом

## Базовые случаи применения

1. Для формирования универсальных ответов от API - все ответы будут обернуты в IResult и для их обработки на клиенте нужен будет один универсальный механизм, который обрабатывает Message, Value, Errors, Status
2. Для работы с HTTP запросами - для формирования ответов от API внутри компонентов системы, а не в самом контроллере, с возможностью гибкой обработки статусов ответа
3. Для работы с контроллерами - для возвращения универсальных ответов от контроллеров типа IResult, который унаследован от IActionResult.
4. Для работы с медиатором - для возвращения универсальных ответов типа IResult из команд и запросов MediatR(CQRS). На пример чтобы все команды и запросы возвращали IResult, который потом можно вернуть из контроллеров и т.д.
5. Для работы с сервисами - для возвращения универсальных ответов типа IResult из сервисов. На пример чтобы все методы сервиса возвращали IResult, который потом можно универсально обработать, проверить статус выполнения, сообщения об ошибках и т.д.

## Пример использования

Query который возвращает IResult который наследуется от IActionResult
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
            ? Result<List<SystemTypesDictionaryEntity>>.Ok("Список систем получен", result)
            : Result<List<SystemTypesDictionaryEntity>>.NotFound($"Не удалось найти список систем");
    }
}
```

Метод в контроллере который возвращает IActionResult.
В контроллере вызывается Query, который возвращает IResult:IActionResult
```csharp
[HttpGet("get-system-types")]
[Auth([Policies.ForAllPositions])]
public async Task<IActionResult> GetSystemTypes([FromQuery]GetSystemTypesQuery query, CancellationToken cancellationToken)
{
    var result = await Mediator.Send(query, cancellationToken);
    return result;
}
```

Мы можем в контроллере вернуть результат из Query, потому как он возвращает IResult, который наследуется от IActionResult, и контроллер требует чтобы из него вернули IActionResult.