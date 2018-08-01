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

Для генерации поля используются две реализации класса Assets.Game.Field.Generators.NoiseGeneratorBase. Если какая либо из платформ не поддерживается, необходимо добавить новую реализацию NoiseGeneratorBase.

#### ComputedShaderNoiseGenerator
Генерация рейтингов планет с помощью ComputedShader.

* **Поддерживаемые платформы**
    * Windows и Windows Store с DirectX 11 или DirectX 12 и Shader Model 5.0 GPU
    * macOS и iOS с использованием Metal graphics API
    * Android, Linux и Windows platforms с использованием Vulkan API
    * Современные платформы OpenGL (OpenGL 4.3 на Linux или Windows; OpenGL ES 3.1 на Android).
    * Современные консоли (Sony PS4 и Microsoft Xbox One)

#### CustomRenderTextureNoiseGenerator
Генерация рейтингов планет с помощью фрагментного шейдера для старых версий Android.

### Организация поля

* **Константы в [Assets.Game.Access.SettingsAccess](https://github.com/fornetjob/InfinitySpace/blob/master/InfinitySpace/Assets/Game/Access/SettingsAccess.cs)**
    * Размер ячейки в позициях: CellPxSize = 100
    * Размер поля в ячейках: FieldSize = 100
    * Полностью генирируемый размер ячеек вокруг игрока: FullGeneratedCellsRadiusSize = 3
    * Количество видимых игроку планет, в расширенном режиме: MaxAdvancedVisiblePlanet

* **Классы**
    * [Assets.Game.Field.Cells.CellCollection](https://github.com/fornetjob/InfinitySpace/blob/master/InfinitySpace/Assets/Game/Field/Cells/CellCollection.cs): содержит перезаписываемый массив размером CellPxSize * CellPxSize для хранения ячеек и перезаписываемый массив размером FullGeneratedCellsRadius * FullGeneratedCellsRadius для хранения рейтингов.
    * [Assets.Game.Field.Cells.CellInfo](https://github.com/fornetjob/InfinitySpace/blob/master/InfinitySpace/Assets/Game/Field/Cells/CellInfo.cs): содержит позицию ячейки, первых MaxAdvancedVisiblePlanet ближайших к рейтингу игрока планет, и все рейтинги планет, если это полностью генерируемая ячейка.
    * [Assets.Game.Field.Cells.SortedCellsVisitor](https://github.com/fornetjob/InfinitySpace/blob/master/InfinitySpace/Assets/Game/Field/Cells/SortedCellsVisitor.cs): визитор, который считает первые MaxAdvancedVisiblePlanet ближайших к рейтингу игрока планет на видимом вокруг игрока радиусе.
    * [Assets.Game.Field.FieldBehaviour](https://github.com/fornetjob/InfinitySpace/blob/master/InfinitySpace/Assets/Game/Field/FieldBehaviour.cs): считает разницу прямоугольных областей при движении игрока и посылает на генерацию, отображает планеты в различных режимах.
    * [Assets.Game.UI.Controls.ZoomControl](https://github.com/fornetjob/InfinitySpace/blob/master/InfinitySpace/Assets/Game/UI/Controls/ZoomControl.cs): отвечает за зум и отображение сетки на поле.
    * [Assets.Game.Tools.RectIntTool](https://github.com/fornetjob/InfinitySpace/blob/master/InfinitySpace/Assets/Game/Tools/RectIntTool.cs): реализует вычитание прямоугольников. [Тесты тут](https://github.com/fornetjob/InfinitySpace/blob/master/InfinitySpace/Assets/Game/Editor/Tests/RectangleTest.cs).

* **Принцип работы**
    * Задаётся случайная позиция и рейтинг игрока на поле.
    * В процессе генерации с помощью одного из механизмов (ComputedShaderNoiseGenerator или CustomRenderTextureNoiseGenerator) создаётся массив ячеек 100х100 в каждой из которых находится 100х100 рейтингов планет. Но все рейтинги загружаются только в 9 ячеек вокруг игрока, а в остальные загружаются 20 самых близких к рейтингу игрока рейтингов планет.
    * Берём текущую область отображения с помощью ZoomControl.
    * Если это обычный режим отображения - показываем планеты вокруг игрока.
    * Если это особый режим отображения - вызываем визитор SortedCellsVisitor, который ходит по спирали вокруг игрока и вычисляет первые 20 ближайших по рейтингу планет.
    * В особом режиме отображения, если зум превысил 100х100 - происходит центрирование камеры по ячейкам и визитор принимает на вход не полностью загруженные рейтинги вокруг игрока на FullGeneratedCellsRadius радиусе ячеек, а сами ячейки, анализируя предгенерированные топ 20 рейтингов в ячейках.
    * При передвижении игрока, если была изменена текущая ячейка, создаются два задания: на генерацию всех данных разницы между #4 и #3 прямоугольником и генерацию топ 20 рейтингов разницы между #2 и #1 прямоугольником.

![Передвижение](https://c.radikal.ru/c30/1808/1d/a55483549034.png)
