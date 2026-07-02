using System.Collections;
using System.Collections.Generic;
using CustomScienceContracts.Conditions;
using CustomScienceContracts.Core;
using CustomScienceContracts.Data;
using CustomScienceContracts.UI;
using UnityEngine;

namespace CustomScienceContracts.Persistence
{
    /// <summary>Lifecycle hook: loads the GameData catalog and save state, hosts the 1-second check
    /// loop, subscribes GameEvents and owns the UI. Runtime state is persisted in the save folder
    /// instead of the .sfs. Active in Science Mode.</summary>
    [KSPScenario(
        ScenarioCreationOptions.AddToNewScienceSandboxGames | ScenarioCreationOptions.AddToExistingScienceSandboxGames,
        GameScenes.SPACECENTER, GameScenes.EDITOR, GameScenes.FLIGHT, GameScenes.TRACKSTATION)]
    public class ContractsScenario : ScenarioModule
    {
        private static readonly List<Vessel> _noVessels = new List<Vessel>();
        private ContractManager _manager;
        private CscUI _ui;
        private bool _initialized;
        private Coroutine _loop;

        public override void OnLoad(ConfigNode node)
        {
            EnsureInitialized();
            // Runtime state lives in the save folder. Missing/broken file -> fresh seed.
            if (!SaveFolderStore.Load(_manager))
            {
                _manager.RecomputeAvailability();
                Debug.Log("[CSC] No state in save folder; seeded fresh.");
            }
        }

        public override void OnSave(ConfigNode node)
        {
            if (_manager != null) SaveFolderStore.Save(_manager);
            // Intentionally write nothing to the .sfs node; the save-folder file is authoritative.
        }

        private void Start()
        {
            EnsureInitialized();
            _manager.Events.Subscribe();
            if (_loop == null) _loop = StartCoroutine(CheckLoop());
        }

        private void EnsureInitialized()
        {
            if (_initialized) return;
            Log.Info($"{ModInfo.Name} v{ModInfo.Version} loaded.");
            BodyResolver.RebuildCache();
            Tuning.Load();

            _manager = new ContractManager();
            List<Model.MissionContract> catalog = CatalogLoader.LoadAll();
            _manager.Initialize(catalog);

            // Register the concrete evaluators here.
            //   _manager.Evaluators.Register(new OrbitEvaluator());
            ConditionRegistration.RegisterAll(_manager.Evaluators);

            _ui = new GameObject("CSC_UI").AddComponent<CscUI>();
            _ui.Bind(_manager);

            _initialized = true;
        }

        private IEnumerator CheckLoop()
        {
            var wait = new WaitForSeconds(Tuning.CheckIntervalSeconds);
            while (true)
            {
                yield return wait;
                if (_manager == null) continue;
                // Evaluate in all mod scenes so duration timers can complete everywhere. UT remains
                // authoritative: in VAB/Editor it does not advance, but Space Center/Tracking/Flight do.
                if (!(HighLogic.LoadedSceneIsFlight ||
                      HighLogic.LoadedScene == GameScenes.TRACKSTATION ||
                      HighLogic.LoadedScene == GameScenes.SPACECENTER ||
                      HighLogic.LoadedScene == GameScenes.EDITOR))
                    continue;

                try
                {
                    var ctx = new EvaluationContext
                    {
                        UniversalTime = Planetarium.GetUniversalTime(),
                        // Never null: evaluators iterate this without guards.
                        Vessels = (IReadOnlyList<Vessel>)FlightGlobals.Vessels ?? _noVessels,
                        Events = _manager.Events,
                        Stations = _manager.Stations
                    };
                    if (Tuning.VerboseLogging)
                    {
                        int active = 0;
                        foreach (var c in _manager.ActiveContracts()) active++;
                        Log.V($"Tick: active={active} scene={HighLogic.LoadedScene}");
                    }
                    _manager.Tick(ctx);
                }
                catch (System.Exception e) { Log.Ex("CheckLoop", e); }
            }
        }

        private void OnDestroy()
        {
            if (_loop != null) { StopCoroutine(_loop); _loop = null; }
            _manager?.Events.Unsubscribe();
            if (_ui != null) { Object.Destroy(_ui.gameObject); _ui = null; }
        }
    }
}
