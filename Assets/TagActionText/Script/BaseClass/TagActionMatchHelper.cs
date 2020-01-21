
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;


namespace UGUITagActionText
{
	public class TagActionMatchHelper
	{

		StringBuilder auxiliarySB = new StringBuilder();
		Regex alphanumericRegex = new Regex(@"^[A-Za-z0-9\-_]+$", RegexOptions.Singleline);

		public void CreateTagMatchList(string originText, List<TagActionManager.TagData> tagDataList, List<TagMatch> tagMatchList)
		{
			if (originText == null || originText.Length <= 0)
				return;
			if (tagDataList == null || tagDataList.Count <= 0)
				return;

			//extract all match part all tag
			foreach (var data in tagDataList)
			{
				if (string.IsNullOrEmpty(data.tagString))
					continue;
				if (data.tagType == LinkActionTagType.enclosure)
				{
					if (alphanumericRegex.IsMatch(data.tagString) != true)
						continue;
					int tagLength = ((data.tagString.Length * 2) + 5);
					if (originText.Length <= tagLength)
						continue;
#if (NET_2_0 || NET_2_0_SUBSET)
					auxiliarySB.Remove(0, auxiliarySB.Length);
#else
					auxiliarySB.Clear(); //require higher .Net4.0 in Unity editor settings
#endif
					auxiliarySB.Append("<").Append(data.tagString).Append(">(.*?)</").Append(data.tagString).Append(">");
					//string linkPattern = @"<link>(.*?)</link>";
					MatchCollection matches = Regex.Matches(originText, auxiliarySB.ToString(), RegexOptions.Singleline);
					if (matches.Count > 0)
					{
						foreach (Match match in matches)
						{
							string argStr = match.Groups[1].Value.Replace("\r\n", string.Empty).Replace("\n", string.Empty).Replace("\r", string.Empty).Replace("\t", string.Empty);
							tagMatchList.Add(new TagMatch(data, argStr, match.Groups[1].Value, match.Index, match.Length, tagLength));
						}
					}
				}
				else if (data.tagType == LinkActionTagType.addition)
				{
					if (alphanumericRegex.IsMatch(data.tagString) != true)
						continue;
#if (NET_2_0 || NET_2_0_SUBSET)
					auxiliarySB.Remove(0, auxiliarySB.Length);
#else
					auxiliarySB.Clear(); //require higher .Net4.0 in Unity editor settings
#endif
					string pat = @"=""([^""\r\n\t]+)"">(.*?)</"; //pat = "=\"([^\"\r\n\t]+)\">(.*?)</";
					auxiliarySB.Append("<").Append(data.tagString).Append(pat).Append(data.tagString).Append(">");
					//string linkPattern = @"<url=([^>\r\n\t]+)>(.*?)</url>;
					//@"<a href=""([^""\r\n\t]+)"">(.*?)</a>";
					MatchCollection matches = Regex.Matches(originText, auxiliarySB.ToString(), RegexOptions.Singleline);
					if (matches.Count > 0)
					{
						foreach (Match match in matches)
						{
							int tagLength = ((data.tagString.Length * 2) + 8 + match.Groups[1].Value.Length);
							string argStr = match.Groups[1].Value.Replace("\r\n", string.Empty).Replace("\n", string.Empty).Replace("\r", string.Empty).Replace("\t", string.Empty);
							tagMatchList.Add(new TagMatch(data, argStr, match.Groups[2].Value, match.Index, match.Length, tagLength));
						}
					}
				}
				else if (data.tagType == LinkActionTagType.self)
				{
					int matchLength = data.tagString.Length;
					if (originText.Length < matchLength)
						continue;
					MatchCollection matches = Regex.Matches(originText, data.tagString, RegexOptions.Singleline);
					if (matches.Count > 0)
					{
						foreach (Match match in matches)
						{
							tagMatchList.Add(new TagMatch(data, data.tagString, data.tagString, match.Index, matchLength, 0));
						}
					}
				}
			}
			//sort by match index
			tagMatchList.Sort((a, b) => a.matchIndex - b.matchIndex);
			return;
		}


	}

	public class TagMatch
	{
		public TagActionManager.TagData tagData;
		public string argStr;
		public string taggedText;
		public int matchIndex;
		public int matchLength;
		public int tagLength;

		public TagMatch(TagActionManager.TagData tagData, string argStr, string taggedText, int matchIndex, int matchLength, int tagLength)
		{
			this.tagData = tagData;
			this.argStr = argStr;
			this.taggedText = taggedText;
			this.matchIndex = matchIndex;
			this.matchLength = matchLength;
			this.tagLength = tagLength;
		}
	}

}
