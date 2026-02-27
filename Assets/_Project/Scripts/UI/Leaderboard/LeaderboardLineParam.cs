public readonly struct LeaderboardLineParam : IParam
{
    private readonly string _uniqueId;
    private readonly string _rank;
    private readonly string _nickname;
    private readonly string _score;
    private readonly bool _isCurrentPlayer;

    public LeaderboardLineParam(string uniqueId, int rank, string nickname, int score, bool isCurrentPlayer)
    {
        _uniqueId = uniqueId;
        _rank = rank.ToString();
        _nickname = nickname;
        _score = score.ToString();
        _isCurrentPlayer = isCurrentPlayer;
    }

    public string Rank => _rank;

    public string UniqueId => _uniqueId;

    public string Nickname => _nickname;

    public string Score => _score;

    public bool IsCurrentPlayer => _isCurrentPlayer;
}