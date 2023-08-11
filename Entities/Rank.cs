namespace StreamerApi.Entities
{
	/// <summary>
	/// local ranks = {
	///['user'] = 1,
	///['player'] = 2,
	///['vip'] = 3,
	///['gefrajter'] = 4,
	///['rekrut'] = 5,
	///['operator'] = 6,
	///['moderator'] = 7,
	///['viceadmin'] = 8,
	///['admin'] = 9,
	///['Superadmin'] = 10,
	///['zarzad'] = 11,
	///['superadmin'] = 12
	/// </summary>
    public class Rank
	{
		public int Id { get; set; }
		public int Level { get; set; }
		public string Name { get; set; }
		public int MaxSeconds { get; set; }
    }
}


