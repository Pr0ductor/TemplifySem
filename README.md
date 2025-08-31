Для начала необходимо склоннировать проект
После клоннирования проекта, создаём докер контейнер - docker run --name my-postgres  -e POSTGRES_USER=postgres  -e POSTGRES_PASSWORD=postgresmaster -p 5433:5432  -d postgres:latest
Дальше обновим базу данных dotnet ef database update -p Templify.Persistence -s Templify.mvc 
Дальше нам нужно использовать minio: 
### 1. Скачивание MinIO

1. Перейдите на официальный сайт MinIO: https://min.io/download
2. Скачайте версию для Windows (minio.exe)
3. Создайте папку `C:\minio\`
4. Поместите `minio.exe` в папку `C:\minio\`

### 2. Запуск MinIO сервера

1. Откройте PowerShell от имени администратора
2. Выполните команду:
```powershell
C:\minio\minio.exe server C:\minio\data --console-address ":9001"
```

3. Сервер MinIO будет доступен по адресу: http://localhost:9001
4. Консоль управления: http://localhost:9001

5. Если не получится запустить проект то возможно стоит поиграться немного с портами и полями endpoint для minio в Program.cs, appsettings.json, MinioLoggerProvider.cs

### 3. Запуск проекта
чтобы зарегаться на сайте нужна реальная почта а чтоб прошел пароль (фигня дотнетовская бывает) взять за 123Qwe!

чтобы зайти на админку авторизоваться через:
admin@templify.com / Admin123! (Admin)
manager1@templify.com / Manager1! (Manager)
manager2@templify.com / Manager2! (Manager))))))))
