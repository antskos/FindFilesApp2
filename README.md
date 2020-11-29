# FindFilesApp2
Программа для поиска текстовых файлов  в каталоге и подкаталогах

Тестовое задание от компании АО «ВНИИР Техно», по которому делается проект:
============
Написать консольное приложение на языке C#
команда find поиск текстовых файлов с параметрами 
формат команды
find опции шаблон1 шаблон2 ...

опции такие:
-R - рекурсивно по папками
-E regexp - выбрать файлы, текст в которых соответствует приведенному регулярному выражению
-T строка - найти 
-s order - Порядок сортировки (вид условаия - это строка из букв за каждой буквой знак + или -, если знака нет - подразумевается +)
	+ - сортировка по возрастанию, - по убыванию
	буквы сортировки: t - по времени последнего обновления, n - по имени, e - по расширению, s - по размеру файла
	Например так n-t+ - сортировать файлы сначала по имени лексикографически в обратном порядке, 
а файлы с одинаковыми именами выводить по возрастанию времени

Примеры обращения:
find -E "^\[\d+\]" *.log *.cs 
найти файлы с расширениями log и Cs В которых в начале строки имеется число в квадратных скобках, например 
	[2000] динозавров

результат команды - список файлов соответстувующих критерию выбора, перечисленных в указанном порядке.
имя каждого файла надо выводить на новой строке. Имя файла должно включать имя директории.

Задание содержит проблему кодировки файла.
На эту тему нужно провести исследования с помощью поисковика (Гугл), найти существующие варианты решения,
выбрать какой-то вариант. В комментарии к коду написать ссылку на решение, описать идею алгоритма и обосновать свой выбор.

Общие требования:
1) Комментарии к коду по правилам оформления самодокументированного кода 
2) Наличие тестов 
Дополнительные требования: 
1) использование стандартной функции сортировки
2) возможность прервать вывод программы клавишей Ctrl-C