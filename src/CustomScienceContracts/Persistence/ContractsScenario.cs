using System.Collections;
using System.Collections.Generic;
using CustomScienceContracts.Conditions;
using CustomScienceContracts.Core;
using CustomScienceContracts.Data;
using CustomScienceContracts.UI;
using UnityEngine;

namespace CustomScienceContracts.Persistence
{
    /// <summary>Lifecycle-Hook: laedt den Katalog (GameData) + Save-State, hostet den 1-s-Pruef-Loop,
    /// abonniert GameEvents und besitzt die UI. Persistiert den Laufzeit-State in den Save-Ordner
    /// (nicht ins .sfs). Aktiv im Science-Mode (Science-Sandbox).</summary>
    [KSPScenario(
        ScenarioCreationOptions.AddToNewScienceSandboxGames | ScenarioCreationOptions.AddToExistingScienceSandboxGames,
        GameScenes.SPACECENTER, GameScenes.EDITOR, GameScenes.FLIGHT, GameScenes.TRACKSTATION)]
    public class ContractsScenario : ScenarioModule
    {
        private ContractManager _manager;
        private CscUI _ui;
        private bool _initialized;
        private Coroutine _loop;

        public override void OnLoad(ConfigNode node)
        {
            EnsureInitialized();
            // Laufzeit-State liegt im Save-Ordner (editierbar). Fehlt/kaputt -> frisch seeden.
            if (!SaveFolderStore.Load(_manager))
            {
                _manager.RecomputeAvailability();
                Debug.Log("[CSC] Kein State im Save-Ordner — frisch geseedet.");
            }
        }

        public override void OnSave(ConfigNode node)
        {
            if (_manager != null) SaveFolderStore.Save(_manager);
            // bewusst NICHTS ins .sfs-node schreiben (Save-Ordner ist autoritativ).
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
            BodyResolver.RebuildCache();
            Tuning.Load();

            _manager = new ContractManager();
            List<Model.MissionContract> catalog = CatalogLoader.LoadAll();
            _manager.Initialize(catalog);

            // Schritt 4/5: hier die echten Evaluatoren registrieren, z.B.:
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
                // In allen Mod-Szenen auswerten, damit Dauer-Timer ueberall abschliessen koennen.
                // Ingame-Zeit bleibt massgeblich: im VAB/Editor steht die UT still -> der Timer waechst
                // dort nicht, schliesst aber ab, wo die UT fortschreitet (Space Center/Tracking/Flug).
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
                        Vessels = FlightGlobals.Vessels,
                        Events = _manager.Events,
                        Stations = _manager.Stations
                    };
                    int active = 0;
                    foreach (var c in _manager.ActiveContracts()) active++;
                    Log.V($"Tick: aktiv={active} szene={HighLogic.LoadedScene}");
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
