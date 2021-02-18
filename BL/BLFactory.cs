namespace BLApi
{
	public static class BlFactory
	{
		public static IBL GetBL()
		{
			return BL.BLIMP.Instance;
		}
	}
}
