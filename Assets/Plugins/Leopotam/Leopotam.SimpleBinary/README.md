<p align="center">
    <img src="./logo.png" alt="Logo">
</p>

# SimpleBinary - Легковесная замена protobuf/flatbuffers для C#
Данный формат - попытка создать простейший бинарный сериализатор, обладающий следующими свойствами:
* Легкая и простая в освоении схема описания данных.
* Возможность поддержки нескольких языков.
* Легковесность и отсутствие аллокаций в процессе работы.

> **ВАЖНО!** Не рекомендуется использовать сгенерированные типы в качестве компонентов ECS-фреймворков, либо любых других систем, использующих автоматический пулинг и переиспользование - сгенерированный код подразумевает использование своего собственного встроенного пулинга.


# Социальные ресурсы
Официальный блог: https://leopotam.ru


# Установка


## В виде исходников
Поддерживается установка в виде исходников из архива, который надо распаковать в проект.


## Прочие источники
Официальные версии выпускаются для активных подписчиков в виде ссылок на актуальные версии.


# Конфигурация

## Схема описания данных
Схема данных - это `INI`-файл, поддерживающий комментарии черезгде секции являются именами типов, а ключи-значения - полями этих типов:
```ini
; Это первый тип данных.
[user]
id=u32
position=point

; Это второй тип данных
[point]
x=f32
y=f32
z=f32
```


### Простые типы данных
Каждая секция (имя в квадратных скобках) - это отдельный пользовательский тип с вложенными полями,
значения которых означают типы данных.

Каждое поле пользовательского типа может быть одним из следующих "простых" типов:
* "i8" - 8-битное целое число со знаком (1 байт).
* "u8" - 8-битное целое число без знака (1 байт).
* "i16" - 16-битное целое число со знаком (2 байта).
* "u16" - 16-битное целое число без знака (2 байта).
* "i32" - 32-битное целое число со знаком (4 байта).
* "u32" - 32-битное целое число без знака (4 байта).
* "f32" - число с плавающей точкой одинарной точности (4 байта).
* "f64" - число с плавающей точкой двойной точности (8 байта).
* "s16" - utf8-строка с 16-битной длиной (64к байт).
    > **ВАЖНО!** Длины строки хранится в байтах, а не в символах, полезная длина строки может быть меньше в случае использования нелатинских символов! При сериализации строка принудительно обрезается до длины в 64к байт без учета кодировки - это может привести к некорректному отображению данных.

Любой пользовательский тип так же может быть использован в качестве типа поля (как тип `[point]` в примере выше).


### Коллекции в качестве типов данных
Поля могут быть представлены коллекциями простых или пользовательских типов:
```ini
; инвентарь.
[inventory]
; коллекция элементов инвентаря.
items=inventoryItem[]

; элемент инвентаря.
[inventoryItem]
id=u32
count=u16
```


### Перечисления в качестве типов данных
Поля могут быть перечислениями, для этого достаточно указать `@` перед именем типа:
```ini
[@status]
alive=
dead=

[user]
id=u32
status=@status
```
> **ВАЖНО!** Значения перечисления указываются отдельными полями без явного указания типа.

> **ВАЖНО!** Перечисления должны содержать от 1 до 256 элементов и упаковываются как `u8`-тип.


### Ограничения
* Количество пользовательских типов ограничено значением в 64k (`u16`).
* Количество элементов в каждой коллекции поля ограничено значением в 64k (`u16`).
* Опциональные поля не поддерживаются.
* Все строковые поля по умолчанию имеют значение `""`, а не `null`.
* Все числовые поля по умолчанию имеют значение 0.


## Конфигурация генератора.
Файл конфигурации представляет собой `INI`-файл, каждая секция которого управляет настройками, специфичными для определенного языка - файл может содержать конфигурации для нескольких языков одновременно.
Пример для `C#`:
```ini
[cs]
namespace=Client.Serialization.V1
prefix=// Сгенерировано автоматически, руками править не рекомендуется.
enumName=SB_PacketType
outFile=generated.cs
threads=false
```
Все поля опциональны и могут отсутствовать.

* `namespace` - пространство имен, в которое будет обернут сгенерированный код.
* `prefix` - строка (или набор строк, разделенных символами `"\n"`), которая будет добавлена в начало сгенерированного файла.
* `enumName` - имя перечисления, содержащего в себе все пользовательские типы в порядке их определения. Используется для определения типов при десериализации.
* `outFile` - путь до файла, в который будет сохранен результат генерации.
* `threads` - признак того, нужно генерировать потокобезопасный пулинг коллекций или нет (значения `true` или `false`). Потокобезопасный вариант медленнее, но позволяет использовать сгенерированный код в нескольких потоках.


# Генерация

## Через браузер
Самый простой способ, не требующий установки дополнительных утилит: достаточно открыть файл `index.html` в любом браузере локально, заполнить поля конфигурации и схемы данных и нажать кнопку "Сгенерировать". В случае ошибки будет показан ее текст. Если ошибок нет - файл будет доступен для скачивания с указанным в конфигурации именем.


## Через Node.JS
Требует установленной node.js (проверялось на `v20.5.0`):
```shell
node <путь до папки пакета>/ "cfg.ini" "schema.ini"
```
Параметры запуска (если запустить без них - появится подсказка о параметрах):
* `"cfg.ini"` - путь до файла конфигурации генератора.
* `"schema.ini"` - путь до файла схемы.


# Применение сгенерированного кода

## Схема данных
Пусть у нас есть инвентарь пользователя с элементами, которые имеют идентификатор и количество:
```ini
[item]
id=u32
count=u32

[inventory]
items=item[]
```


## Сериализация
```cs
using Leopotam.SimpleBinary;

// Создаем тестовый набор данных.
Inventory inv = Inventory.New ();
Item item = Item.New ();
item.Id = 1;
item.Count = 2;
inv.Items.Add (item);
// Сериализуем их.
byte[] buf = new byte[1024];
SimpleBinaryWriter sbw = new SimpleBinaryWriter (buf);
inv.Serialize (ref sbw);
// Очищаем экземпляр после сериализации.
inv.Recycle();
// Получаем актуальный буфер с сериализованными данными.
(byte[] serializedData, int len) = sbw.GetBuffer ();
```
> **ВАЖНО!** Переданный в `SimpleBinaryWriter` буфер будет автоматически расширяться по мере
> необходимости, поэтому его актуальные данные следует запрашивать через `GetBuffer()`.

> **ВАЖНО!** Сериализованные экземпляры данных следует сразу же очищать через `Recycle()` после
> использования, причем только для корневых типов - вложенные элементы будут очищены автоматически.
> Обращаться к полям экземпляра после очистки запрещено.


## Десериализация
```cs
using Leopotam.SimpleBinary;

SimpleBinarReader sbr = new SimpleBinarReader (serializedData);
(Inventory inv2, bool ok) = Inventory.Deserialize (ref sbr);
if (!ok) {
    // Некорректные данные (закончился поток байт, другой тип данных и т.п).
    return;
}
// Работаем с полученными данными.
// inv2.Items[0].id == 1
// inv2.Items[0].count == 2

// Очищаем.
inv2.Recycle();
```
> **ВАЖНО!** Десериализованные экземпляры данных следует сразу же очищать через `Recycle()` после использования, причем только для корневых типов - вложенные элементы будут очищены автоматически.

> **ВАЖНО!** Десериализация происходит без кидания исключений, возвращая успешность операции как результат вызова.


# Лицензия
Пакет выпускается под коммерческой лицензией, [подробности тут](./LICENSE.md).
