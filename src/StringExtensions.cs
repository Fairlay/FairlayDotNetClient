using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FairlayDotNetClient
{
	public static class StringExtensions
	{
		public static string RemoveDiacritics(this string text)
		{
			if (text == null)
				return null;
			text = text.Replace("ı", "i").Replace("\"", "").Replace("ø", "o").Replace("đ", "d").
				Replace("?", "").Replace("Å", "A").Replace("ə", "e").Replace("\"", "").Replace("\\", "").
				Replace("ł", "l").Replace("Ł", "L").Replace("Đ", "D").Replace("Ð", "D").Replace("ộ", "o");
			string normalizedString = text.Normalize(NormalizationForm.FormD);
			var stringBuilder = new StringBuilder();
			foreach (char c in normalizedString)
				if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
					stringBuilder.Append(c);
			string ret = stringBuilder.ToString().Normalize(NormalizationForm.FormC);
			stringBuilder = new StringBuilder();
			foreach (char c in ret)
				if (Convert.ToInt32(c) < 130)
					stringBuilder.Append(c);
			return stringBuilder.ToString();
		}

		public static string ToText<T>(this IEnumerable<T> texts, string separator = ", ")
			=> texts == null ? "" : string.Join(separator, texts);
	}
}