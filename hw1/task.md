# Бор:

  - Структура данных для эффективного хранения строк
  - Представляет из себя дерево с символами на ребрах
  - http://neerc.ifmo.ru/wiki/index.php?title=%D0%91%D0%BE%D1%80

Задача:
  - Необходимо создать класс Trie, реализующий следующие методы:
```csharp
    /// Expected complexity: O(|element|)
    /// Returns true if this set did not already contain the specified element
    bool Add(string element);

    /// Expected complexity: O(|element|)
    bool Contains(string element);

     /// Expected complexity: O(|element|)
     /// Returns true if this set contained the specified element
    bool Remove(string element);

    /// Expected complexity: O(1)
    int Size();

    /// Expected complexity: O(|prefix|)
    int HowManyStartsWithPrefix(string prefix);
```
Для того, чтобы сдать решение, заведите отдельную ветку в репозитории на гитхабе (заданий будет несколько, но не очень больших, поэтому одного репозитория на курс должно хватить), выложите туда решение задачи, сделайте пуллреквест к себе в репозиторий и скиньте ссылку на пуллреквест мне (на почту или через сайт CSC).

Выкладывать, если Visual Studio, надо:

  - все файлы .cs (даже те, что Вы не писали, например, Properties/AssemblyInfo.cs)
  - .sln
  - .csproj
  - App.config

 Не выкладывайте содержимое папок bin, obj и .vs. Вообще, на гитхабе есть правильный .gitignore для C#-проектов.

Если Rider, то можно пользоваться https://github.com/github/gitignore/blob/master/Global/JetBrains.gitignore

Если что-то другое, то в любом случае нужны сами исходники (.cs) и какой-то способ их собрать с минимумом ручных манипуляций.
