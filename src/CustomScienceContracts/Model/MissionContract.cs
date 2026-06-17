using System.Collections.Generic;
using UnityEngine;

namespace CustomScienceContracts.Model
{
    /// <summary>Eine Mission. Definition stammt aus dem .cfg-Katalog (GameData);
    /// der Laufzeit-State liegt editierbar im Save-Ordner.</summary>
    public class MissionContract
    {
        // --- Definition (aus dem Katalog, unveraenderlich zur Laufzeit) ---
        public string Id = "";
        public string Titel = "";
        public string Beschreibung = "";
        public Sparte HeimatSparte;            // Bemannt / UnbemannteErkundung / NetzwerkLogistik
        public string Unterkategorie = "";     // Koerper-Name (Luna=eigene Kat.; Monde unter Planet)
        /// <summary>Optionales, pro Mission gewaehltes UI-Icon (Dateiname in Icons/UI ohne Endung,
        /// z.B. "TrackingStation_ButtonMapLander"). Leer -> Fallback nach erstem Bedingungstyp.</summary>
        public string IconKey = "";
        public List<string> Voraussetzungen = new List<string>();  // IDs, muessen alle CompletedOnce sein
        public List<Condition> Bedingungen = new List<Condition>();
        public float ScienceReward = 0f;
        public bool Repeatable = false;
        /// <summary>Nur Erkundung/aeusseres System: ist diese Contract-ID CompletedOnce,
        /// werden alle Contracts der Unterkategorie sichtbar (4er-Limit faellt weg).</summary>
        public string RevealAllAfter = "";
        /// <summary>Ist gesetzt, merkt sich der Mod beim Erfuellen das erfuellende Vessel als "Station"
        /// unter diesem Schluessel. Versorgungsauftraege referenzieren ihn via CONDITION.stationKey.</summary>
        public string RecordStationKey = "";
        /// <summary>Verweist (nur fuer Anzeige) auf eine gemerkte Station/Basis, ohne sie zu erfassen —
        /// damit Folgeauftraege deren Namen in Titel/%station% zeigen koennen.</summary>
        public string StationRef = "";

        // --- Laufzeit-State (persistiert im Save-Ordner) ---
        public MissionStatus Status = MissionStatus.Locked;
        /// <summary>Fuer Repeatable-Cooldown: reset 0 beim Einloesen, +1 bei jedem anderen Abschluss,
        /// wieder annehmbar wenn &gt;= Tuning.RepeatableCooldown.</summary>
        public int CompletionsSinceLastClaim = 0;
        /// <summary>Wie oft insgesamt abgeschlossen (Statistik / UI).</summary>
        public int TotalCompletions = 0;
        /// <summary>Scratch-State der Condition-Evaluatoren (FLYBY-Phase, CREW_DURATION-Startzeit,
        /// gesetzte Marker-Waypoints ...). Nur fuer Active-Contracts relevant, wird mitpersistiert.</summary>
        public ConfigNode Progress = new ConfigNode("PROGRESS");

        public bool IsRepeatableInPool => Repeatable && TotalCompletions > 0;
    }
}
