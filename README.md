# MDRG-Inventory-Cleaner
Safely removes corrupted "Missing Items" from MDRG inventory with F8 hotkey. MelonLoader mod.
# 🧹 MDRG Inventory Cleaner

> **Automatic removal of "Missing Items" from the MDRG inventory**
> *Автоматическое удаление "пропавших предметов" из инвентаря MDRG*

**Author / Автор:** Paul.Epifancev (`#warlockforbugs`)  
**Discord:** `anon.jun`  
**Version / Версия:** 1.0.0 (Stable Release / Стабильный релиз)  
**Compatibility / Совместимость:** PC (MelonLoader required / требуется MelonLoader)  
**Hotkey / Горячая клавиша:** `F8`

---

## 📖 About / О проекте

[EN]
**MDRG Inventory Cleaner** is a lightweight, open-source MelonLoader mod that automatically detects and removes corrupted "Missing Item" entries from the player's inventory in *My Dystopian Robot Girlfriend*. These ghost items appear when a save file references assets that no longer exist (e.g., after uninstalling other mods, updating the game, or save corruption).

The cleaner uses deep IL2CPP reflection to safely inspect every `GameItemContainer` in the active scene, verify the integrity of each item via its `_prefab` field, and remove only the broken entries — leaving valid items completely untouched.

[RU]
**MDRG Inventory Cleaner** — это лёгкий open-source мод на базе MelonLoader, который автоматически обнаруживает и удаляет повреждённые записи "Missing Item" из инвентаря игрока в *My Dystopian Robot Girlfriend*. Такие "призрачные" предметы появляются, когда файл сохранения ссылается на ассеты, которых больше не существует (например, после удаления других модов, обновления игры или повреждения сохранения).

Очиститель использует глубокую IL2CPP-рефлексию для безопасной проверки каждого `GameItemContainer` в активной сцене, верификации целостности каждого предмета через поле `_prefab` и удаления только битых записей — валидные предметы остаются полностью нетронутыми.

---

## 🎥 Video Demonstration / Видео-демонстрация

<div align="center">
  <a href="https://www.youtube.com/watch?v=QYsQayotf0Q" target="_blank">
    <img src="https://img.youtube.com/vi/QYsQayotf0Q/maxresdefault.jpg" alt="Inventory Cleaner Video Preview" width="600">
  </a>
  <br>
  <sub>🎬 <a href="https://www.youtube.com/watch?v=QYsQayotf0Q" target="_blank">Watch on YouTube / Смотреть на YouTube</a></sub>
</div>

---

## ✨ Key Features / Ключевые возможности

- 🎯 **One-key cleanup / Очистка одной клавишей:** Press `F8` in-game to instantly scan and clean the inventory / Нажмите `F8` в игре для мгновенного сканирования и очистки инвентаря
- 🔒 **100% safe / 100% безопасно:** Only removes items with `null` prefab or `IsMissingItem = True` flag / Удаляет только предметы с `null` префабом или флагом `IsMissingItem = True`
- 🔄 **Multi-iteration loop / Многоитерационный цикл:** Up to 10 cleanup passes to catch nested broken references / До 10 проходов очистки для перехвата вложенных битых ссылок
- 🛡️ **Valid items protection / Защита валидных предметов:** Verified items are never touched, even if they look suspicious / Проверенные предметы никогда не затрагиваются
- 📊 **Detailed console logging / Подробное логирование:** See exactly how many items were scanned and removed in each iteration / Видно, сколько предметов было удалено в каждой итерации
- 💾 **Save-compatible / Совместимо с сохранениями:** Just save the game after cleanup to persist changes / Просто сохраните игру после очистки, чтобы закрепить изменения

---

## 🧠 Connection to SuperParser / Связь с SuperParser

[EN]
This tool is the **logical successor** to the [MDRG Modding Toolkit](https://github.com/Paul-Epifancev/MDRG-Modding-Toolkit) and its flagship **SuperParser v6.5.1** diagnostic SDK.

While SuperParser **diagnoses** the problem (Folders 11, 13, and 14 dump all `GameItemContainer` instances and flag `IsMissingItem` entries), Inventory Cleaner **solves** it by safely removing these broken references from the live game state.

Together, they form a complete reverse-engineering workflow:
1. Run **SuperParser** (F5) → get a full report of all missing items.
2. Run **Inventory Cleaner** (F8) → remove them automatically.
3. Save the game → enjoy a clean inventory.

[RU]
Этот инструмент — **логическое продолжение** [MDRG Modding Toolkit](https://github.com/Paul-Epifancev/MDRG-Modding-Toolkit) и его флагманского диагностического SDK **SuperParser v6.5.1**.

Если SuperParser **диагностирует** проблему (папки 11, 13 и 14 выгружают все инстансы `GameItemContainer` и помечают записи `IsMissingItem`), то Inventory Cleaner **решает** её, безопасно удаляя эти битые ссылки из активного состояния игры.

Вместе они образуют полный рабочий процесс реверс-инжиниринга:
1. Запустите **SuperParser** (F5) → получите полный отчёт о всех пропавших предметах.
2. Запустите **Inventory Cleaner** (F8) → удалите их автоматически.
3. Сохраните игру → наслаждайтесь чистым инвентарём.

---

## 🚀 Quick Start / Быстрый старт

[EN]
1. Make sure you have [MelonLoader](https://github.com/LavaGang/MelonLoader) installed for MDRG.
2. Download `InventoryCleaner.dll` from the [Releases](../../releases) section.
3. Place it in your game's `Mods/` folder.
4. Launch the game and load any save.
5. Press **F8** to run the cleanup.
6. Watch the MelonLoader console for detailed logs.
7. **Save the game** to persist the changes.

[RU]
1. Убедитесь, что у вас установлен [MelonLoader](https://github.com/LavaGang/MelonLoader) для MDRG.
2. Скачайте `InventoryCleaner.dll` из раздела [Releases](../../releases).
3. Поместите его в папку `Mods/` в директории вашей игры.
4. Запустите игру и загрузите любое сохранение.
5. Нажмите **F8** для запуска очистки.
6. Следите за подробными логами в консоли MelonLoader.
7. **Сохраните игру**, чтобы закрепить изменения.

---

## ⚙️ How It Works / Как это работает

[EN]
Under the hood, the mod uses IL2CPP reflection to access the game's internal item management system:
1. **Type Discovery:** Scans assemblies to find `GameVariables`, `Item`, `ItemManager`, and `GameItemContainer`.
2. **Singleton Access:** Retrieves the live `ItemManager` via `GameVariables.Current.itemManager`.
3. **Scene Scan:** Iterates through all active `GameItemContainer` GameObjects.
4. **Integrity Check:** Reads the `_current` item and verifies if `_prefab` is `null` or contains `"IsMissingItem : True"`.
5. **Safe Removal:** Calls `ItemManager.RemoveItem()` only on verified broken entries.
6. **Iteration Loop:** Repeats up to 10 times to catch nested references.

[RU]
Под капотом мод использует IL2CPP-рефлексию для доступа к внутренней системе управления предметами:
1. **Обнаружение типов:** Сканирует сборки для поиска `GameVariables`, `Item`, `ItemManager` и `GameItemContainer`.
2. **Доступ к синглтону:** Получает живой инстанс `ItemManager` через `GameVariables.Current.itemManager`.
3. **Сканирование сцены:** Перебирает все активные `GameItemContainer`.
4. **Проверка целостности:** Читает предмет `_current` и проверяет, равен ли `_prefab` `null` или содержит `"IsMissingItem : True"`.
5. **Безопасное удаление:** Вызывает `ItemManager.RemoveItem()` только для проверенных битых записей.
6. **Цикл итераций:** Повторяет до 10 раз для перехвата вложенных ссылок.

---

## 🛡️ Safety Guarantees / Гарантии безопасности

✅ **Only broken items are removed** — verified via `_prefab` null check AND `IsMissingItem` flag detection  
✅ **Valid items are never touched** — even if they look suspicious, they pass the integrity check  
✅ **No save file modification** — works entirely in RAM; you must manually save to persist  
✅ **No original game files modified** — pure MelonLoader injection, 100% reversible  
✅ **Anti-infinite-loop protection** — stops if no items were removed in an iteration  
✅ **Max 10 iterations** — hard limit to prevent runaway loops  

---

## ⚠️ Known Limitations / Известные ограничения

- **Requires a loaded save:** The cleaner needs `GameVariables.Current` to be initialized. Press F8 only after loading a save file.
- **Console required for feedback:** All results are logged to the MelonLoader console.
- **No GUI:** This is a minimalist, key-driven tool. Press F8 and check the console.
- **PC only:** MelonLoader-based, not compatible with Android/mobile versions.

---

## 🔧 Tech Stack / Технологии

- **Language / Язык:** C# (.NET 6.0)
- **Framework / Фреймворк:** [MelonLoader](https://github.com/LavaGang/MelonLoader)
- **Target / Цель:** Unity IL2CPP (MDRG)
- **APIs:** IL2CPP Reflection (`Il2CppInterop.Runtime`), `UnityEngine.Resources`
- **Reverse Engineering / Реверс-инжиниринг:** Built on data from [SuperParser v6.5.1](https://github.com/Paul-Epifancev/MDRG-Modding-Toolkit)

---

## 💬 Community & Support / Сообщество и поддержка

🔗 **Discord Server / Discord-сервер:** [discord.gg/nwcvMUZgM](https://discord.gg/nwcvMUZgM)  
🔗 **Main Toolkit Repository / Репозиторий основного тулкита:** [MDRG-Modding-Toolkit](https://github.com/Paul-Epifancev/MDRG-Modding-Toolkit)

---

## 👤 About the Author / Об авторе

[EN]
This project is created by **Paul.Epifancev** (`#warlockforbugs`). My journey in game modification began with 5 years of deep engine analysis in World of Warcraft (2013–2018). Today, I apply the same analytical approach and reverse-engineering skills to Unity/IL2CPP environments.

[RU]
Этот проект создан **Paul.Epifancev** (`#warlockforbugs`). Мой путь в модификации игр начался с пяти лет глубокого анализа движка в World of Warcraft (2013–2018). Сегодня я применяю тот же аналитический подход и навыки реверс-инжиниринга к среде Unity/IL2CPP.

---

## 📄 License / Лицензия

This project is released under the MIT License. See [LICENSE](LICENSE) for details.

---

## ⚠️ Disclaimer / Отказ от ответственности

This mod is provided **as-is** for educational and maintenance purposes. It does not modify original game files on disk. All inventory changes are applied in RAM and must be manually saved to persist. **Always backup your save files before using inventory-modifying tools.**

---

**Made with ❤️ and 🐑 by Paul.Epifancev (`#warlockforbugs`)**  
*«Не пойман — не багер» ©*
