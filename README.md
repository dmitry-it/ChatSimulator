# ChatSimulator
      
   Задача:
   Реализовать симулятор чата со следующим функционалом:

Интерфейс
✦ Скролл-лист с сообщениями;
✦ Поле ввода сообщения;
✦ Кнопка “отправить сообщение”;
✦ Кнопка “удалить сообщения”;
✦ Кнопки “удалить сообщение” и “готово”.
✦ Если сообщения идут от одного автора, то аватар пользователя отображается только на последнем
✦ Сообщения масштабируются по вертикали и горизонтали, в зависимости от длинны текста сообщения

Конфигурируемые настройки:
✦ Список пользователей чата;
✦ Возможность указать одного из пользователей, как владельца чата
(сообщения с правой стороны).
Принцип работы:

Сценарий 1:
✦ Вводим текст в инпут и жмем кнопку “отправить”;
✦ Сообщение с заданным текстом появляется в скролл-панели от имени
автора, выбранного случайным образом из списка авторов.

Сценарий 2:
✦ Жмем кнопку “удалить сообщения”;
✦ Рядом с сообщениями владельца чата появляются кнопки “удалить
сообщение”;
✦ Жмем “удалить сообщение” и сообщение удаляется из списка;
✦ Жмем “готово” и возвращаемся к исходному состоянию.

Комментарии по реализации:
✦ Нежелательно использование архитектурных фреймворков;
✦ Сообщения должны появляться и пропадать с анимацией. Для
создания анимаций лучше использовать аниматор или DOTween.
✦ Реализация максимально приближена к макету

Ожидаемый результат
✦ Unity-проект
✦ Сопроводительный текст, если будет необходимость в
пояснениях
 