using Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 棋子的信息
/// </summary>
public class ChessInfo
{
    public Postition pos;
    public ChessHeroInfo info;
}

/// <summary>
/// 队伍标识
/// </summary>
public enum Team
{
    /// <summary>
    /// 红队
    /// </summary>
    RedTeam,
    /// <summary>
    /// 蓝队
    /// </summary>
    BlueTeam
}

/// <summary>
/// 棋子坐标信息
/// </summary>
public struct Postition
{
    public Postition(int c, int r)
    {
        column = c;
        row = r;
    }
    public int column;
    public int row;
}
