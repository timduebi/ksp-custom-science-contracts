using System.Collections.Generic;
using UnityEngine;

namespace CustomScienceContracts.Core
{
    /// <summary>Abonniert diskrete KSP-GameEvents (Andocken, SOI-Wechsel, Situationswechsel) und
    /// puffert sie. Die 1-s-Coroutine pollt Zustands-Bedingungen und liest diesen Puffer fuer
    /// ereignisbasierte Bedingungen (DOCK, FLYBY-SOI, ATMO_ENTRY) aus. Puffer wird pro Tick geleert.</summary>
    public class GameEventBridge
    {
        public struct DockEvent { public Vessel Vessel; }
        public struct SoiEvent { public Vessel Vessel; public CelestialBody From; public CelestialBody To; }
        public struct SituationEvent { public Vessel Vessel; public Vessel.Situations From; public Vessel.Situations To; }
        /// <summary>Zwei Vessel-IDs, die durch Andocken fusionieren — fuer das Umhaengen einer Timer-Bindung.</summary>
        public struct MergeEvent { public uint IdA; public uint IdB; }

        public readonly List<DockEvent> Dockings = new List<DockEvent>();
        public readonly List<SoiEvent> SoiChanges = new List<SoiEvent>();
        public readonly List<SituationEvent> SituationChanges = new List<SituationEvent>();
        public readonly List<MergeEvent> Merges = new List<MergeEvent>();

        private bool _subscribed;

        public void Subscribe()
        {
            if (_subscribed) return;
            GameEvents.onPartCouple.Add(OnPartCouple);
            GameEvents.onVesselSOIChanged.Add(OnSoiChanged);
            GameEvents.onVesselSituationChange.Add(OnSituationChange);
            _subscribed = true;
        }

        public void Unsubscribe()
        {
            if (!_subscribed) return;
            GameEvents.onPartCouple.Remove(OnPartCouple);
            GameEvents.onVesselSOIChanged.Remove(OnSoiChanged);
            GameEvents.onVesselSituationChange.Remove(OnSituationChange);
            _subscribed = false;
        }

        /// <summary>Nach jedem Pruef-Tick aufrufen.</summary>
        public void ClearFrameBuffer()
        {
            Dockings.Clear();
            SoiChanges.Clear();
            SituationChanges.Clear();
            Merges.Clear();
        }

        private void OnPartCouple(GameEvents.FromToAction<Part, Part> a)
        {
            Vessel v = a.to?.vessel ?? a.from?.vessel;
            if (v != null) Dockings.Add(new DockEvent { Vessel = v });
            // Beide beteiligten Vessel-IDs merken: beim Andocken geht eines im anderen auf, und eine
            // laufende Timer-Bindung muss aufs ueberlebende Schiff umgehaengt werden.
            uint idA = a.from?.vessel?.persistentId ?? 0u;
            uint idB = a.to?.vessel?.persistentId ?? 0u;
            if (idA != 0u && idB != 0u && idA != idB) Merges.Add(new MergeEvent { IdA = idA, IdB = idB });
        }

        private void OnSoiChanged(GameEvents.HostedFromToAction<Vessel, CelestialBody> a)
        {
            SoiChanges.Add(new SoiEvent { Vessel = a.host, From = a.from, To = a.to });
        }

        private void OnSituationChange(GameEvents.HostedFromToAction<Vessel, Vessel.Situations> a)
        {
            SituationChanges.Add(new SituationEvent { Vessel = a.host, From = a.from, To = a.to });
        }
    }
}
