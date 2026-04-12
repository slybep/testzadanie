- Первым делом запустить BACKEND. 
- Миграции создаются автоматически / можно использовать docker-compose
- Запуск Backend(нажать кнопку запустить, проверить порты в консоли и в frontend) 
- В корне проекта(перейти в папку frontend/My-frontend) npm run dev (по адресу http://localhost:5173) 
- Возможно нужно будет установить tailwindcss (npm install tailwindcss)

1. Получить список всех ссылок
Метод: GET
Эндпоинт: /urls
Параметры: нет.
Ответ: массив объектов с полями id, shortCode, url, countClick.
Пример запроса:
GET http://localhost:5196/Link/urls
Пример ответа:

json
[
  {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "shortCode": "abc123",
    "url": "https://yandex.ru",
    "countClick": 5
  }
]

2. Создать новую короткую ссылку
Метод: POST
Эндпоинт: /
Тело запроса (JSON): { "url": "https://example.com" }
Ответ: созданный объект ссылки (с id, shortCode, url, countClick).
Пример запроса:

bash
curl -X POST http://localhost:5196/Link \
  -H "Content-Type: application/json" \
  -d '{"url":"https://yandex.ru"}'
Пример ответа:

json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "shortCode": "xyz789",
  "url": "https://yandex.ru",
  "countClick": 0
}

3. Перейти по короткой ссылке (редирект)
Метод: GET
Эндпоинт: /{shortCode} (где shortCode – уникальный код, например abc123)
Параметры: в пути.
Действие: выполняет HTTP-редирект (302) на оригинальный URL, если код найден. Если не найден – возвращает 404 Not Found.
Пример запроса в браузере:
http://localhost:5196/Link/abc123
После этого вы будете перенаправлены на https://yandex.ru.

4. Удалить ссылку
Метод: DELETE
Эндпоинт: /{id} (где id – GUID ссылки)
Параметры: в пути.
Ответ: 200 OK при успешном удалении, 404 Not Found если ссылка не существует.
Пример запроса:

bash
curl -X DELETE http://localhost:5196/Link/3fa85f64-5717-4562-b3fc-2c963f66afa6

5. Обновить оригинальный URL ссылки
Метод: PATCH
Эндпоинт: /{id}
Параметры: в пути id (GUID), в теле JSON – { "url": "https://new-url.com" }.
Ответ: обновлённый объект ссылки или 404 Not Found, если ссылка не найдена.
Пример запроса:

bash
curl -X PATCH http://localhost:5196/Link/3fa85f64-5717-4562-b3fc-2c963f66afa6 \
  -H "Content-Type: application/json" \
  -d '{"url":"https://new-example.com"}'
Пример ответа:

json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "shortCode": "abc123",
  "url": "https://new-example.com",
  "countClick": 5
}
