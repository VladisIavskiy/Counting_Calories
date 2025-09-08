# Counting_Calories - кроссплатформенное приложение для учёта калорий

<div align="center"> <img src="https://img.shields.io/badge/.NET%20MAUI-512BD4?logo=.net&logoColor=white"> <img src="https://img.shields.io/badge/C%23-239120?logo=c-sharp&logoColor=white"> <img src="https://img.shields.io/badge/XAML-0C54C2?logo=xaml&logoColor=white"> <img src="https://img.shields.io/badge/license-MIT-blue"> </div>

🍎 О проекте
Интеллектуальный помощник для контроля питания с расширенными возможностями:

Гибкое добавление — указание точной граммовки продуктов (100-500 г)
Избранное — мгновенный доступ к часто используемым позициям
Экспорт данных — сохранение статистики в .txt для анализа
📱 Основные функции
graph TD
  A[Главный экран] --> B1[Завтрак]
  A --> B2[Обед]
  A --> B3[Ужин]
  A --> B4[Настройки]
  
  B1 --> C1[Добавить продукт]
  B1 --> E1[★ Избранное]
  B2 --> C2[Добавить продукт]
  B2 --> E1[★ Избранное] 
  B3 --> C3[Добавить продукт]
  B3 --> E1[★ Избранное]
  
  C1 --> D1{Граммовка 100-500г}
  C2 --> D2{Граммовка 100-500г}
  C3 --> D3{Граммовка 100-500г}
  

  B1,B2,B3 --> E2[Сброс данных]
  B1,B2,B3 --> E3[Экспорт в .txt]
  
  E1 --> F[Список избранных продуктов]

🛠 Управление продуктами
Точный ввод — слайдер для выбора веса выбранного продукта с шагом 100 г
Избранное — звезда ★ сохраняет продукт в персональную коллекцию
Быстрый поиск — интеллектуальный поиск по 5000+ позициям из базы USDA
Экспорт — сохранение дневного рациона в текстовый файл с меткой времени
Сброс — кнопка удаления всех записей в текущем сеансе

🧩 Технологии
Компонент	Реализация
UI Framework	.NET MAUI 8.0 + XAML Hot Reload
Логика	MVVM (CommunityToolkit.MVVM)
Локальное хранилище	SQLite

🚀 Запуск
git clone https://github.com/yourusername/CalorieTracker
cd CalorieTracker/CalorieTracker
dotnet build -t:Run -f net8.0-android

<div align="center"> <sub>🚧 Проект в активной разработке — принимаются pull requests!</sub> </div>