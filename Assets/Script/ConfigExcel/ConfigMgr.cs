using System.Collections.Generic;
using Utils;

namespace Config
{
	public class ConfigMgr : Singleton<ConfigMgr>
	{
		/// <summary>
		///1棋子英雄表
		/// </summary>
		public List<ChessHeroInfo> ConfigChessHeroInfo;
		/// <summary>
		///2棋子站位布局表
		/// </summary>
		public List<ChessTeam> ConfigChessTeam;

		public void InitConfig()
		{
			string pathChessHeroInfo = "Assets/Script/ConfigExcel/Json/ChessHeroInfo.json";
			ConfigChessHeroInfo = ConfigUtil.ReadConfig<List<ChessHeroInfo>>(pathChessHeroInfo);

			string pathChessTeam = "Assets/Script/ConfigExcel/Json/ChessTeam.json";
			ConfigChessTeam = ConfigUtil.ReadConfig<List<ChessTeam>>(pathChessTeam);

		}
	}
}
