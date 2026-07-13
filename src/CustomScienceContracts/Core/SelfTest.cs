using System.Collections.Generic;
using CustomScienceContracts.Model;

namespace CustomScienceContracts.Core
{
    /// <summary>Startup logic self-test (settings.cfg: selfTest = true): exercises the status
    /// flow, prerequisites and repeatable cooldowns on a throwaway manager with synthetic
    /// zero-reward contracts, logging PASS/FAIL lines. Never touches the real campaign state;
    /// toasts and sounds are suppressed while it runs.</summary>
    public static class SelfTest
    {
        private static int _failures;

        public static void Run()
        {
            bool prevToasts = Tuning.ShowToasts, prevSounds = Tuning.PlaySounds, prevUnlock = Tuning.UnlockAll;
            int prevCooldown = Tuning.RepeatableCooldown;
            Tuning.ShowToasts = false; Tuning.PlaySounds = false; Tuning.UnlockAll = false;
            Tuning.RepeatableCooldown = 2;
            _failures = 0;
            try
            {
                var mgr = new ContractManager();
                var a = Synth("t_a");
                var b = Synth("t_b", "t_a");
                var c = Synth("t_c");
                var r = Synth("t_r");
                r.Repeatable = true;
                mgr.Initialize(new List<MissionContract> { a, b, c, r });

                Check(a.Status == MissionStatus.Available, "A available at start");
                Check(b.Status == MissionStatus.Locked, "B locked behind A");

                Check(mgr.Accept("t_a"), "accept A");
                a.Status = MissionStatus.ReadyToClaim;
                Check(mgr.Claim("t_a"), "claim A");
                Check(a.Status == MissionStatus.CompletedOnce, "A completed once");
                Check(a.FirstCompletedUT >= 0.0, "A first-completion time recorded");
                Check(b.Status == MissionStatus.Available, "B unlocked by A");

                Check(mgr.Accept("t_r"), "accept repeatable");
                r.Status = MissionStatus.ReadyToClaim;
                Check(mgr.Claim("t_r"), "claim repeatable");
                Check(r.IsRepeatableInPool, "repeatable entered the pool");
                Check(!mgr.CanAccept(r), "repeatable on cooldown after claim");

                Check(mgr.Accept("t_b"), "accept B");
                b.Status = MissionStatus.ReadyToClaim;
                Check(mgr.Claim("t_b"), "claim B");
                Check(r.CompletionsSinceLastClaim == 1, "claim advanced the cooldown");
                Check(!mgr.CanAccept(r), "cooldown not finished after one completion");

                Check(mgr.Accept("t_c"), "accept C");
                Check(mgr.Skip("t_c"), "skip C");
                Check(r.CompletionsSinceLastClaim == 2, "skip advanced the cooldown");
                Check(mgr.CanAccept(r), "repeatable ready after two completions");

                Log.Info(_failures == 0
                    ? "SelfTest PASS — all checks green"
                    : $"SelfTest FAILED: {_failures} check(s), see log lines above");
            }
            catch (System.Exception e)
            {
                Log.Ex("SelfTest", e);
            }
            finally
            {
                Tuning.ShowToasts = prevToasts; Tuning.PlaySounds = prevSounds;
                Tuning.UnlockAll = prevUnlock; Tuning.RepeatableCooldown = prevCooldown;
            }
        }

        private static MissionContract Synth(string id, params string[] prereqs)
        {
            var m = new MissionContract { Id = id, Titel = id, HeimatSparte = Sparte.UnbemannteErkundung };
            m.Voraussetzungen.AddRange(prereqs);
            return m;
        }

        private static void Check(bool ok, string what)
        {
            if (ok) Log.V("SelfTest PASS: " + what);
            else { _failures++; Log.Error("SelfTest FAIL: " + what); }
        }
    }
}
