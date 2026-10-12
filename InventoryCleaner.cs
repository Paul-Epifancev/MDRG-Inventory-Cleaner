using MelonLoader;
using UnityEngine;
using Il2CppInterop.Runtime.InteropTypes.Arrays;

[assembly: MelonInfo(typeof(InventoryCleaner.MainClass), "Inventory Cleaner", "1.0.0", "Paul.Epifancev #warlockforbugs (Discord: anon.jun")]
[assembly: MelonGame(null, null)]

namespace InventoryCleaner
{
    public class MainClass : MelonMod
    {
        private bool _initialized = false;
        
        // Кэшированные IL2CPP типы
        private Il2CppSystem.Type _gameVariablesType = null;
        private Il2CppSystem.Type _itemType = null;
        private Il2CppSystem.Type _itemManagerType = null;
        private Il2CppSystem.Type _gameItemContainerType = null;
        
        // Методы и поля
        private Il2CppSystem.Reflection.PropertyInfo _currentProperty = null;
        private Il2CppSystem.Reflection.FieldInfo _itemManagerField = null;
        private Il2CppSystem.Reflection.MethodInfo _removeItemMethod = null;
        private Il2CppSystem.Reflection.FieldInfo _prefabField = null;
        private Il2CppSystem.Reflection.FieldInfo _containerCurrentField = null;

        public override void OnInitializeMelon()
        {
            MelonLogger.Msg("[InventoryCleaner v4.0.0] Ready. Press F8 to clean inventory.");
        }

        public override void OnUpdate()
        {
            try
            {
                if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.F8))
                {
                    EnsureInitialized();
                    ExecuteCleanup();
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[InventoryCleaner] Fatal: {ex.Message}");
            }
        }

        // =====================================================================
        // ИНИЦИАЛИЗАЦИЯ
        // =====================================================================
        private void EnsureInitialized()
        {
            if (_initialized) return;
            
            try
            {
                var assemblies = Il2CppSystem.AppDomain.CurrentDomain.GetAssemblies();
                
                foreach (var asm in assemblies)
                {
                    try
                    {
                        if (_gameVariablesType == null)
                            _gameVariablesType = asm.GetType("GameVariables");
                        if (_itemType == null)
                            _itemType = asm.GetType("Item");
                        if (_gameItemContainerType == null)
                            _gameItemContainerType = asm.GetType("GameItemContainer");
                    }
                    catch { }
                }
                
                if (_itemType != null)
                {
                    try
                    {
                        var nested = _itemType.GetNestedTypes(
                            Il2CppSystem.Reflection.BindingFlags.Public |
                            Il2CppSystem.Reflection.BindingFlags.NonPublic);
                        
                        foreach (var nt in nested)
                        {
                            if (nt.Name == "ItemManager")
                            {
                                _itemManagerType = nt;
                                break;
                            }
                        }
                    }
                    catch { }
                }
                
                if (_gameVariablesType != null)
                {
                    _currentProperty = _gameVariablesType.GetProperty("Current",
                        Il2CppSystem.Reflection.BindingFlags.Public |
                        Il2CppSystem.Reflection.BindingFlags.Static);
                    
                    _itemManagerField = _gameVariablesType.GetField("itemManager",
                        Il2CppSystem.Reflection.BindingFlags.Public |
                        Il2CppSystem.Reflection.BindingFlags.Instance);
                }
                
                if (_itemManagerType != null)
                {
                    var methods = _itemManagerType.GetMethods(
                        Il2CppSystem.Reflection.BindingFlags.Public |
                        Il2CppSystem.Reflection.BindingFlags.Instance);
                    
                    foreach (var m in methods)
                    {
                        if (m.Name == "RemoveItem")
                        {
                            var pars = m.GetParameters();
                            if (pars.Length == 1)
                            {
                                _removeItemMethod = m;
                                break;
                            }
                        }
                    }
                }
                
                if (_itemType != null)
                {
                    _prefabField = _itemType.GetField("_prefab",
                        Il2CppSystem.Reflection.BindingFlags.NonPublic |
                        Il2CppSystem.Reflection.BindingFlags.Instance);
                }
                
                if (_gameItemContainerType != null)
                {
                    _containerCurrentField = _gameItemContainerType.GetField("_current",
                        Il2CppSystem.Reflection.BindingFlags.Public |
                        Il2CppSystem.Reflection.BindingFlags.NonPublic |
                        Il2CppSystem.Reflection.BindingFlags.Instance);
                }
                
                MelonLogger.Msg("[Init] Initialization complete:");
                MelonLogger.Msg($"  GameVariables: {(_gameVariablesType != null ? "OK" : "NULL")}");
                MelonLogger.Msg($"  Item: {(_itemType != null ? "OK" : "NULL")}");
                MelonLogger.Msg($"  ItemManager: {(_itemManagerType != null ? "OK" : "NULL")}");
                MelonLogger.Msg($"  GameItemContainer: {(_gameItemContainerType != null ? "OK" : "NULL")}");
                MelonLogger.Msg($"  RemoveItem: {(_removeItemMethod != null ? "OK" : "NULL")}");
                MelonLogger.Msg($"  _prefab: {(_prefabField != null ? "OK" : "NULL")}");
                MelonLogger.Msg($"  _current field: {(_containerCurrentField != null ? "OK" : "NULL")}");
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[Init] {ex.Message}");
            }
            
            _initialized = true;
        }

        // =====================================================================
        // ГЛАВНАЯ ФУНКЦИЯ: ОЧИСТКА ИНВЕНТАРЯ
        // =====================================================================
        private void ExecuteCleanup()
        {
            // Проверка инициализации
            if (_gameVariablesType == null || _currentProperty == null || 
                _itemManagerField == null || _removeItemMethod == null || 
                _prefabField == null || _gameItemContainerType == null || 
                _containerCurrentField == null)
            {
                MelonLogger.Warning("[InventoryCleaner] Required types/methods not found. Check init log.");
                return;
            }
            
            int totalRemoved = 0;
            int totalIterations = 0;
            
            try
            {
                // Получаем itemManager ОДИН раз
                object gameVars = _currentProperty.GetValue(null, null);
                if (gameVars == null)
                {
                    MelonLogger.Warning("[InventoryCleaner] GameVariables.Current is null. Load a save first!");
                    return;
                }
                
                object itemManager = _itemManagerField.GetValue((Il2CppSystem.Object)gameVars);
                if (itemManager == null)
                {
                    MelonLogger.Warning("[InventoryCleaner] itemManager is null.");
                    return;
                }
                
                MelonLogger.Msg("[InventoryCleaner] Starting cleanup...");
                
                // ГЛАВНЫЙ ЦИКЛ: повторяем очистку, пока есть Missing Items
                const int MAX_ITERATIONS = 10;
                
                for (int iteration = 1; iteration <= MAX_ITERATIONS; iteration++)
                {
                    totalIterations = iteration;
                    
                    var itemsToRemoveNow = new List<object>();
                    
                    // Сканируем все GameItemContainer в сцене
                    var allObjects = UnityEngine.Resources.FindObjectsOfTypeAll<GameObject>();
                    
                    foreach (var go in allObjects)
                    {
                        if (go == null || !go.activeInHierarchy) continue;
                        if (!go.name.Contains("GameItem(Clone)")) continue;
                        
                        var containerComp = go.GetComponent("GameItemContainer");
                        if (containerComp == null) continue;
                        
                        try
                        {
                            object itemObj = _containerCurrentField.GetValue(containerComp);
                            if (itemObj == null) continue;
                            
                            bool isMissing = false;
                            
                            if (_prefabField != null)
                            {
                                try
                                {
                                    var prefab = _prefabField.GetValue((Il2CppSystem.Object)itemObj);
                                    if (prefab == null)
                                    {
                                        isMissing = true;
                                    }
                                    else
                                    {
                                        string prefabStr = prefab.ToString() ?? "";
                                        
                                        if (prefabStr.Contains("IsMissingItem : True", StringComparison.OrdinalIgnoreCase) ||
                                            prefabStr.Contains("Name : Missing Item", StringComparison.OrdinalIgnoreCase))
                                        {
                                            isMissing = true;
                                        }
                                    }
                                }
                                catch { }
                            }
                            
                            if (isMissing)
                            {
                                itemsToRemoveNow.Add(itemObj);
                            }
                        }
                        catch { }
                    }
                    
                    // Если в этой итерации ничего не нашли — выходим
                    if (itemsToRemoveNow.Count == 0)
                    {
                        MelonLogger.Msg($"[InventoryCleaner] Iteration {iteration}: no missing items found. Clean!");
                        break;
                    }
                    
                    MelonLogger.Msg($"[InventoryCleaner] Iteration {iteration}: found {itemsToRemoveNow.Count} missing items, removing...");
                    
                    // Удаляем Missing Items
                    int removedNow = 0;
                    foreach (var item in itemsToRemoveNow)
                    {
                        try
                        {
                            var args = new Il2CppReferenceArray<Il2CppSystem.Object>(1);
                            args[0] = (Il2CppSystem.Object)item;
                            
                            _removeItemMethod.Invoke(
                                (Il2CppSystem.Object)itemManager,
                                args);
                            removedNow++;
                            totalRemoved++;
                        }
                        catch (Exception ex)
                        {
                            MelonLogger.Error($"[InventoryCleaner] RemoveItem failed: {ex.Message}");
                        }
                    }
                    
                    MelonLogger.Msg($"[InventoryCleaner] Iteration {iteration}: removed {removedNow} items.");
                    
                    // Если ничего не удалили — выходим, чтобы не зациклиться
                    if (removedNow == 0)
                    {
                        MelonLogger.Warning("[InventoryCleaner] No items were removed. Stopping to prevent infinite loop.");
                        break;
                    }
                }
                
                MelonLogger.Msg($"[InventoryCleaner] DONE! Total removed: {totalRemoved} missing items in {totalIterations} iteration(s).");
                MelonLogger.Msg("[InventoryCleaner] Save the game to persist changes.");
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[InventoryCleaner] Main loop error: {ex.Message}");
            }
        }
    }
}
