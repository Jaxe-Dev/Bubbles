using UnityEngine;

namespace Bubbles.Patch
{
    internal static class Extensions
    {
        public static string Italic(this string self) => "<i>" + self + "</i>";
        public static string Bold(this string self) => "<b>" + self + "</b>";

        public static Color WithAlpha(this Color self, float alpha) => new Color(self.r, self.g, self.b, alpha);
        public static Rect ContractedBy(this Rect self, float x, float y) => new Rect(self.x + x, self.y + y, self.width - (x * 2f), self.height - (y * 2f));
        public static float ToPercentageFloat(this int self) => self / 100f;
    }
}
