## 🛠️ Как Запустить Проект

Нужно восстановить зависимости и накатить базу данных, так как весь код и миграции уже лежат в репозитории.

1) git clone https://github.com/username-nickname/dotNetProject.git
2) dotnet tool install --global dotnet-ef
3) dotnet ef database update --project Infrastructure --startup-project Project.Api
4) dotnet run --project Project.Api

## 🛠️ Архитектура и Разделение Ответственности

Проект строго разделён на четыре слоя. Зависимости всегда направлены внутрь

1) Domain: Ядро бизнеса. Содержит сущности (User), бизнес-правила (инварианты) и контракты (IUserRepository). Поля CreatedAt / UpdatedAt управляются автоматически.
2) Application: Слой сценариев. Содержит сервисы (UserService, AuthService), DTO и FluentValidation. Управляет бизнес-сценарием (оркестрация).
3) Infrastructure: Техническая реализация. Содержит AppDbContext (EF Core), репозитории, хешеры (BCrypt) и генератор токенов (JWT).
4) Project.Api: Слой представления. Контроллеры, настройка DI и Middleware.

## ⚙️ Основные Технологии и Концепции

1) ORM: EF Core. База данных (SQLServer) управляется миграциями, сущности загружаются и сохраняются через Change Tracking (без явных Update(), он сам отслеживает, когда сущность была изменена в памяти).
2) Аутентификация: JWT Bearer Token. Токен выдается при логине. Проверка и валидация происходят через Microsoft.AspNetCore.Authentication.JwtBearer автоматически.
3) Инвалидация Токена: Token Versioning (Middleware). При смене пароля увеличивается версия токена в БД в таблице users. Кастомный Middleware отклоняет старые токены (возвращает 401, если версия токена в БД у юзера ,больше чем в переданом токене)
4) Валидация: FluentValidation. Вся валидация (формат, уникальность Email) вынесена в DTO-валидаторы. (Слой Application , Validators)
5) Обработка Ошибок: Глобальный ApiExceptionFilter (Project.Api/Filters). Централизованно ловит все исключения и возвращает единый JSON-ответ в стиле ApiResponse<T> (400, 404, 500) (Application/DTO/Contracts).

## Сущности и Логика (Domain)
1) Сущности (User, Role): Это не просто структуры данных. Они содержат методы поведения (например, user.UpdateName(), user.ChangePassword()) и проверки инвариантов (например, имя не может быть пустым).
2) Аудит: Поля CreatedAt и UpdatedAt обновляются автоматически в AppDbContext (Infrastructure), а не вручную в сущностях.

## Точки Входа Controllers (Project.Api)
Контроллеры в проекте Project.Api максимально тонкие и выполняют две главные задачи:
1) Прием/Обработка HTTP: Принимают данные (DTO) из JSON и извлекают контекст безопасности (userId) из токена.
2) Делегирование: Вызывают соответствующие методы в Application Service.

Обработка Ошибок: Все блоки try-catch удалены. Ошибки (например, UserNotFoundException, ValidationException) перехватываются Глобальным Фильтром ApiExceptionFilter (Project.Api/Filters/), который конвертирует их в единый JSON-ответ в нашем стиле (ApiResponse).
*** Ответы от контроллеров вызывать рекомендуется через метода родительского класса ApiControllerBase (Например: return OkResponse("Register successfully");). Метод OkResponse лежит в ApiControllerBase.

## JWT и Аутентификация
1) Генерация Токена: В методе AuthService.Login(), после успешной проверки пароля, вызывается ITokenGenerator (который лежит в Infrastructure). Токен содержит ID пользователя и имя его роли (Admin или User) как Claims.
2) Проверка Токена: Middleware app.UseAuthentication() проверяет подпись токена.
3) Инвалидация Токена: Наш кастомный Middleware JwtTokenVersionMiddleware вставляется после аутентификации. Он проверяет версию токена (tver Claim) с актуальной версией из БД. Если пользователь сменил пароль (TokenVersion в БД вырос), старый токен отбрасывается (401 Unauthorized).

## Роуты и Защита
Маршруты организованы по принципу RESTful и защищены атрибутами:

1) Публичные: [AllowAnonymous] (например, /api/login, /api/register).
2) Защищенные: [Authorize] (требует любой валидный токен).
3) С Проверкой роли: [RoleAuthorize(RoleType.Student)] (требует токен с клеймом Role: Admin)


## API Endpoints

1) POST /api/register Регистрация нового пользователя. Публичный
2) POST /api/login	Аутентификация. Возвращает JWT Token.	Публичный
3) GET /api/user/{userId}	Получение деталей пользователя.	[Authorize(Roles = "Admin")]
4) PATCH /api/user/update	Обновление полей профиля.	[Authorize]
5) POST	/api/user/change-password	Смена пароля (инвалидирует все старые токены).	[Authorize]

## Responses
API всегда возвращает единый JSON-контракт в стиле ApiResponse<T>:

1) Успех (200 OK): { "success": true, "message": "...", "data": {...} }
2) Ошибка (400/404/500): { "success": false, "message": "...", "errors": [...] }

Это гарантирует, что клиент всегда получает предсказуемый ответ
