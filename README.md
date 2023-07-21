# MicroservicesArch
Проверочное задание для банка

Ссылка на [Postman CountService](https://elements.getpostman.com/redirect?entityId=27288696-02300d04-aa38-45df-8678-b13e22b79bf5&entityType=collection)

Ссылка на [Postman UsersService](https://elements.getpostman.com/redirect?entityId=27288696-57943c79-6bf0-410c-a9a1-825b345adfa3&entityType=collection)

Создано два сервиса:

+ Сервис счетов (Counts Service)

Данный сервис отвечает за работу со счетами и транзакциями

Для конвертирования валюты в сервисе использовался сайт [Exchangerates](https://exchangeratesapi.io). 
API для конвертирования валюты https://api.exchangeratesapi.io/v1/latest?access_key=API_TOKEN&from=RUB&to=ВАЛЮТА_ДЛЯ_ПРЕОБРАЗОВАНИЯ&amount=СУММА_СЧЕТА

В сервисе MicroserviceArch.CountsService в файле appsettings.json, в пункте "AccessKeyToExchangeratesapi" указывается токен для API [Exchangerates](https://exchangeratesapi.io), в строке запроса к API указан как API_TOKEN

В файле сервиса есть еще две строки:
1. Строка подключения к базе данных: VehicleQuotesContext
2. Строка подключения к RebbitMQ: RebbitMQServerHost (использовалась localhost, так как был использован контейнер Docker)

Сообщения в RabbitMQ сортируются для отправки клиентов по Queue. Запрос на внесение в RabbitMQ выглядит таким образом, что Queue выглядит так Transaction1, где
1. Transaction - тип сообщения (в данном случае транзакция)
2. 1 - ID клиента

Реализован простой пример на консольном приложении сервиса получения сообщения из RabbitMQ

Транзакции сохраняются в базу и реализован механизм постраничного вывода транзакций: https://localhost:5602/api/Transactions?CountId=2&startDate=10.10.2020&endDate=07.23.2023&IsSortStandart=2&PageNumber=1&PageSize=20

+ Сервис клиентов (Clients Service)

Данный сервис отвечает за работу пользователей и роли

В appsettings.json в секции VehicleQuotesContext указывается подключение к БД
