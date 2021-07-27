using UnityEngine;

namespace Bubbles.Patch
{
    internal static class Extensions
    {
        public static string Italic(this string self) => "<i>" + self + "</i>";
        public static string Bold(this string self) => "<b>" + self + "</b>";

        public static Color WithAlpha(this Color self, float alpha) => new Color(self.r, self.g, self.b, alpha);
        public static float ToPercentageFloat(this int self) => self / 100f;
    }
}
