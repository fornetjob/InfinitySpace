# InfinitySpace

Необходимо реализовать 2D игру про корабль, который летает в космосе. Платформы: PC, Mac, Android, iOS.

## Требования:
Космос это сетка из квадратных ячеек, корабль перемещается только по ячейкам через WASD сквозь все возможные препятствия. Передвижение мгновенное.
Ячейка заполняется либо ничем, либо планетой (планеты должны заполнять не менее 30% ячеек)
Космос бесконечен (возвращаясь на одно и тоже место каждый раз мы должны видеть те же самые объекты)
Каждой планете случайно присваивается “рейтинг” от 0 до 10 000, который отображается числом над планетой. При старте игры кораблю также присваивается рейтинг от 0 до 10 000
Минимальная область видимости NxN ячеек, где N = 5, возможен зум, который увеличивает до N = 10 000
Начиная с N = 10 включается особый режим отображения объектов, при котором отображается только P = 20 планет с самым близким к кораблю рейтингом в видимой области
В особом режиме объекты должны отображаться так, чтобы они всегда были видны на экране независимо от их реального размера.

## Требования к проекту:
Обратите внимание на производительность, старайтесь минимизировать лаги. Реализуйте с расчетом на то, что проект может работать и на мобильных платформах.
Стремитесь, чтобы архитектура и проект были расширяемыми для возможных дальнейших изменений.
Приветствуется краткое описание того, что и как было сделано плюс что можно было бы улучшить в будущем при развитии проекта и на что, возможно, не хватило времени (макс примерно 1-2 страницы).
Арт необязателен, но приветствуется.
Проект можно выложить на любой публичный хостинг, а также приложить ссылку на билд любой из указанных платформ.

## Реализация

### Генерация поля

Для генерации поля используются три реализации базового класса [NoiseGeneratorBase](https://github.com/fornetjob/InfinitySpace/blob/master/InfinitySpace/Assets/Game/Field/Generators/Base/NoiseGeneratorBase.cs). Если какая либо из платформ не поддерживается, необходимо добавить новую реализацию  [NoiseGeneratorBase](https://github.com/fornetjob/InfinitySpace/blob/master/InfinitySpace/Assets/Game/Field/Generators/Base/NoiseGeneratorBase.cs), либо использовать [CpuNoiseGenerator](https://github.com/fornetjob/InfinitySpace/blob/master/InfinitySpace/Assets/Game/Field/Generators/CpuNoiseGenerator.cs), у которого нет требований к платформе.
Для тестирования режимов необходимо переключить настройки, либо загрузить один из билдов под Windows из репозитория:

![](https://d.radikal.ru/d09/1808/6a/681c71264994.png)

#### [ComputedShaderNoiseGenerator](https://github.com/fornetjob/InfinitySpace/blob/master/InfinitySpace/Assets/Game/Field/Generators/ComputedShaderNoiseGenerator.cs)
Генерация рейтингов планет с помощью ComputedShader. Шейдер [тут](https://github.com/fornetjob/InfinitySpace/blob/master/InfinitySpace/Assets/Game/Shaders/ComputedShaders/CalculateCell.compute).

* **Поддерживаемые платформы**
    * Windows и Windows Store с DirectX 11 или DirectX 12 и Shader Model 5.0 GPU
    * macOS и iOS с использованием Metal graphics API
    * Android, Linux и Windows platforms с использованием Vulkan API
    * Современные платформы OpenGL (OpenGL 4.3 на Linux или Windows; OpenGL ES 3.1 на Android).
    * Современные консоли (Sony PS4 и Microsoft Xbox One)

#### [CustomRenderTextureNoiseGenerator](https://github.com/fornetjob/InfinitySpace/blob/master/InfinitySpace/Assets/Game/Field/Generators/CustomRenderTextureNoiseGenerator.cs)
Генерация рейтингов планет с помощью фрагментного шейдера для старых версий Android.  Шейдер [тут](https://github.com/fornetjob/InfinitySpace/blob/master/InfinitySpace/Assets/Game/Shaders/CalculateTextureShaders/CalculateCell.shader).

#### [CpuNoiseGenerator](https://github.com/fornetjob/InfinitySpace/blob/master/InfinitySpace/Assets/Game/Field/Generators/CpuNoiseGenerator.cs)
Генерация рейтингов планет "налету", платформонезависимая реализация.

### Организация поля

* **Настройки в [SettingsAccess](https://github.com/fornetjob/InfinitySpace/blob/master/InfinitySpace/Assets/Game/Access/SettingsAccess.cs)**
    * Размер ячейки в позициях: CellPxSize = 100
    * Размер поля в ячейках: FieldSize = 100
    * Полностью генирируемый размер ячеек вокруг игрока: FullGeneratedCellsRadiusSize = 3
    * Количество видимых игроку планет, в расширенном режиме: MaxAdvancedVisiblePlanet = 20
    * Режим отладки, отображется фпс, время генерации и поиска: IsDebugMode = true

* **Классы**
    * [CellCollection](https://github.com/fornetjob/InfinitySpace/blob/master/InfinitySpace/Assets/Game/Field/Cells/CellCollection.cs): содержит перезаписываемый массив размером 100 * 100 для хранения ячеек и перезаписываемый массив размером 3 * 3 для хранения рейтингов.
    * [CellInfo](https://github.com/fornetjob/InfinitySpace/blob/master/InfinitySpace/Assets/Game/Field/Cells/CellInfo.cs): содержит позицию ячейки, первых 20 ближайших к рейтингу игрока планет, и все рейтинги планет, если это полностью генерируемая ячейка.
    * [SortedCellsVisitor](https://github.com/fornetjob/InfinitySpace/blob/master/InfinitySpace/Assets/Game/Field/Cells/SortedCellsVisitor.cs): визитор, который считает первые 20 ближайших к рейтингу игрока планет на видимом вокруг игрока радиусе.
    * [FieldBehaviour](https://github.com/fornetjob/InfinitySpace/blob/master/InfinitySpace/Assets/Game/Field/FieldBehaviour.cs): считает разницу прямоугольных областей при движении игрока и посылает на генерацию, отображает планеты в различных режимах.
    * [ZoomControl](https://github.com/fornetjob/InfinitySpace/blob/master/InfinitySpace/Assets/Game/UI/Controls/ZoomControl.cs): отвечает за зум и отображение сетки на поле.
    * [RectIntTool](https://github.com/fornetjob/InfinitySpace/blob/master/InfinitySpace/Assets/Game/Tools/RectIntTool.cs): реализует вычитание прямоугольников. [Тесты тут](https://github.com/fornetjob/InfinitySpace/blob/master/InfinitySpace/Assets/Game/Editor/Tests/RectangleTest.cs).

* **Принцип работы**
    * Задаётся случайная позиция и рейтинг игрока на поле.
    * В процессе генерации с помощью одного из механизмов ([ComputedShaderNoiseGenerator](https://github.com/fornetjob/InfinitySpace/blob/master/InfinitySpace/Assets/Game/Field/Generators/ComputedShaderNoiseGenerator.cs) или [CustomRenderTextureNoiseGenerator](https://github.com/fornetjob/InfinitySpace/blob/master/InfinitySpace/Assets/Game/Field/Generators/CustomRenderTextureNoiseGenerator.cs)) создаётся массив ячеек 100х100 в каждой из которых находится 100х100 рейтингов планет. Но все рейтинги загружаются только в 9 ячеек вокруг игрока, а в остальные загружаются 20 самых близких к рейтингу игрока рейтингов планет.
    * Берём текущую область отображения с помощью [ZoomControl](https://github.com/fornetjob/InfinitySpace/blob/master/InfinitySpace/Assets/Game/UI/Controls/ZoomControl.cs).
    * Если это обычный режим отображения - показываем планеты вокруг игрока.
    * Если это особый режим отображения - вызываем визитор [SortedCellsVisitor](https://github.com/fornetjob/InfinitySpace/blob/master/InfinitySpace/Assets/Game/Field/Cells/SortedCellsVisitor.cs), который ходит по спирали вокруг игрока и вычисляет первые 20 ближайших по рейтингу планет.
    * В особом режиме отображения, если зум превысил 100х100 - происходит центрирование камеры по ячейкам и визитор принимает на вход не полностью загруженные рейтинги вокруг игрока на FullGeneratedCellsRadius радиусе ячеек, а сами ячейки, анализируя предгенерированные топ 20 рейтингов в ячейках.
    * При передвижении игрока, если была изменена текущая ячейка, создаются два задания: на генерацию всех данных в ячейках разницы между #4 и #3 прямоугольником и генерацию топ 20 рейтингов в ячейках разницы между #2 и #1 прямоугольником. Где #1 - старый прямоугольник поля 10000х10000 (100х100 ячеек), #2 - новый прямоугольник поля 10000х10000 (100х100 ячеек), #3 - старый прямоугольник 300х300 (3х3 ячейки) вокруг игрока, #4 - новый прямоугольник 300х300 (3х3 ячейки) вокруг игрока.

![Передвижение](https://c.radikal.ru/c30/1808/1d/a55483549034.png)

### Остальное
* **Классы**
    * [ResourcesAccess](https://github.com/fornetjob/InfinitySpace/blob/master/InfinitySpace/Assets/Game/Access/ResourcesAccess.cs): доступ к ресурсам проекта.
    * [PrefabsPoolingManager](https://github.com/fornetjob/InfinitySpace/blob/master/InfinitySpace/Assets/Game/Core/PoolingSystem/PrefabsPoolingManager.cs): Реализация пулинга префабов.
    * [MappingAttribute](https://github.com/fornetjob/InfinitySpace/blob/master/InfinitySpace/Assets/Game/Core/MappingAttribute.cs): Указание этого атрибута для поля автоматически подгружает его по имени из дочерних компонентов, либо по абсолютному пути. Механизм [тут](https://github.com/fornetjob/InfinitySpace/blob/master/InfinitySpace/Assets/Game/Access/Editor/SettingsAccessEditor.cs).
    * [KeyboardInput](https://github.com/fornetjob/InfinitySpace/blob/master/InfinitySpace/Assets/Game/Inputs/KeyboardInput.cs) и [MobileInput](https://github.com/fornetjob/InfinitySpace/blob/master/InfinitySpace/Assets/Game/Inputs/MobileInput.cs): для управления с клавиатуры или с кнопок на экране, если это мобильное устройство. Если нужно добавить новые способы управления, необходимо определить интерфейс [IPlayerInput](https://github.com/fornetjob/InfinitySpace/blob/master/InfinitySpace/Assets/Game/Inputs/Base/IPlayerInput.cs).
    * [SortCells](https://github.com/fornetjob/InfinitySpace/blob/master/InfinitySpace/Assets/Game/Editor/Tests/DataContracts/SortCells.cs): реализованный, но неиспользованный механизм сортировки на [шейдере](https://github.com/fornetjob/InfinitySpace/blob/master/InfinitySpace/Assets/Game/Shaders/ComputedShaders/SortCell.compute).
    * [EnumObjectDictionary](https://github.com/fornetjob/InfinitySpace/blob/master/InfinitySpace/Assets/Game/Core/Collections/EnumObjectDictionary.cs): Словарь, который получает на вход перечисление и список объектов. Предоставляет доступ к объекту по значению перечисления.
    * [DebugControl](https://github.com/fornetjob/InfinitySpace/blob/master/InfinitySpace/Assets/GameDebug/UI/DebugControl.cs): Используется для отладки.
 
### Тайминги

* **PC**
    * Процессор i7-5930
    * Видео-карта GTX 980 Ti
* **Phone**
   * [HTC One](https://www.htc.com/ru/smartphones/htc-one-m7/)

Генератор     |Платформа|Предварительная генерация всего поля|Генерация разницы при передвижении|% от ComputedShader PC|
--------------|---------|------------------------------------|----------------------------------|-------------------|
ComputedShader|PC       |~2800ms                             |~50ms                             |100%|
ComputedShader|Phone|-|-|-|
RenderTexture|PC|~7300ms|~150ms|~261%|
RenderTexture|Phone|~15200ms|~170ms|~542%|
CPU|PC|~5700ms|~30ms|~203%|
CPU|Phone|~37200ms|~50ms|~1328%|

### Что можно улучшить и на что не хватило времени

* Перепроектировать NoiseGeneratorBase в сторону билдера.
* Вынести лишние зависимости из FieldBehaviour, к примеру совместив поиск топ-20 рейтингов и процесс генерации.
* Подчистить функционал итераторов от несвойственных им функций.
* Упростить реализацию в классе CellCollection.
* Разнести генерируемые ошибки в отдельные классы.
* Если среди игроков будут преобладать платформы с поддержкой ComputedShader, можно доработать алгоритм генерации поля, чтобы выдавать сразу топ-20 рейтингов для ячеек, без необходимости выгружать данные и делать эту проверку на цпу.
* Добавить тесты на генераторы полей, для проверки различных способов оптимизации времени генерации (тесты были, но из за внесения изменения в реализацию, часть стала неактуальна и не была включена в конечную сборку).
* Добавить автоматический маппинг для ресурсов в класс ResourcesAccess.
* Перенести отладочную информацию для поля из FieldBehaviourEditor в DebugControl.
* Сделать прямоугольную камеру.
* Анимацию прогресса лучше сделать независимой от состояния прогресса.
* Необходимо детализировать комментарии к реализации, снабдив их примерами и отсылками к тестам.
* Подобрать цифры для лучших результатов генерации шума.
