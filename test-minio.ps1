# PowerShell скрипт для тестирования MinIO
# Запустите этот скрипт от имени администратора

Write-Host "=== MinIO Test Script ===" -ForegroundColor Green
Write-Host ""

# Проверяем, запущен ли MinIO
Write-Host "1. Проверка статуса MinIO..." -ForegroundColor Yellow

try {
    $response = Invoke-WebRequest -Uri "http://localhost:9000/minio/health/live" -UseBasicParsing -TimeoutSec 5
    if ($response.StatusCode -eq 200) {
        Write-Host "✅ MinIO сервер запущен и доступен" -ForegroundColor Green
    }
} catch {
    Write-Host "❌ MinIO сервер не доступен по адресу http://localhost:9000" -ForegroundColor Red
    Write-Host "   Убедитесь, что MinIO запущен командой:" -ForegroundColor Yellow
    Write-Host "   C:\minio\minio.exe server C:\minio\data --console-address ':9001'" -ForegroundColor Cyan
    Write-Host ""
}

# Проверяем консоль управления
Write-Host "2. Проверка консоли управления..." -ForegroundColor Yellow

try {
    $response = Invoke-WebRequest -Uri "http://localhost:9001" -UseBasicParsing -TimeoutSec 5
    if ($response.StatusCode -eq 200) {
        Write-Host "✅ Консоль управления MinIO доступна по адресу http://localhost:9001" -ForegroundColor Green
    }
} catch {
    Write-Host "❌ Консоль управления MinIO недоступна" -ForegroundColor Red
    Write-Host "   Проверьте, что MinIO запущен с параметром --console-address ':9001'" -ForegroundColor Yellow
    Write-Host ""
}

# Проверяем health check приложения
Write-Host "3. Проверка health check приложения..." -ForegroundColor Yellow

try {
    $response = Invoke-WebRequest -Uri "http://localhost:5000/health" -UseBasicParsing -TimeoutSec 10
    if ($response.StatusCode -eq 200) {
        Write-Host "✅ Health check приложения доступен" -ForegroundColor Green
        
        # Парсим JSON ответ
        $healthData = $response.Content | ConvertFrom-Json
        Write-Host "   Статус: $($healthData.status)" -ForegroundColor Cyan
        
        foreach ($check in $healthData.checks) {
            $statusColor = if ($check.status -eq "Healthy") { "Green" } else { "Red" }
            Write-Host "   - $($check.name): $($check.status)" -ForegroundColor $statusColor
        }
    }
} catch {
    Write-Host "❌ Health check приложения недоступен" -ForegroundColor Red
    Write-Host "   Убедитесь, что приложение запущено" -ForegroundColor Yellow
    Write-Host ""
}

# Инструкции по настройке
Write-Host "4. Инструкции по настройке:" -ForegroundColor Yellow
Write-Host "   - Скачайте MinIO с https://min.io/download" -ForegroundColor Cyan
Write-Host "   - Создайте папку C:\minio\" -ForegroundColor Cyan
Write-Host "   - Поместите minio.exe в C:\minio\" -ForegroundColor Cyan
Write-Host "   - Запустите: C:\minio\minio.exe server C:\minio\data --console-address ':9001'" -ForegroundColor Cyan
Write-Host "   - Откройте консоль: http://localhost:9001" -ForegroundColor Cyan
Write-Host "   - Войдите с: minioadmin / minioadmin" -ForegroundColor Cyan
Write-Host ""

Write-Host "=== Тест завершен ===" -ForegroundColor Green

