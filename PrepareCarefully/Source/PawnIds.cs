using System.Threading;
using Verse;

namespace EdB.PrepareCarefully {
    public static class PawnIds {
        private static int serial;

        // Guid.NewGuid() in RimWorld's Mono has come back as the same value for every pawn,
        // so presets saved one id for the whole colony and every relationship loaded as that one person.
        public static string Create(Pawn pawn) {
            int n = Interlocked.Increment(ref serial);
            int thing = pawn?.thingIDNumber ?? 0;
            return "pc-" + thing + "-" + n + "-" + System.DateTime.UtcNow.Ticks;
        }
    }
}
