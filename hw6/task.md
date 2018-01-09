# Простые числа 
Написать программу на WinForms или WPF, которая в интерфейсе пользователя имеет

 - форму ввода целого числа,
 - кнопку ок и
 - панель результатов

При введении числа х в форму ввода и нажатии Ок на панели результатов (которая представляет из себя скроллируемый список задач) появляется новая задача по поиску количества простых чисел от 0 до x.

У задачи 4 состояния:

1. ждет свободного потока,
2. выполняется
3. завершена
4. отменена

Визуально задача должна выглядеть как:

1. состояние
2. индикатор прогресса, если она в состоянии 2
3. кнопка отмены, если задача в состоянии 1 или 2
4. результат, если задача в состоянии 3.
Для решения нужно воспользоваться Тасками и /или конструкциями ```async/await```.

Дополнительное задание (на бонусные баллы): использовать не дефолтный scheduler, а написать собственный ```TaskScheduler``` на 3 потока (и использовать его для диспатчинга вычислений из UI-потока).